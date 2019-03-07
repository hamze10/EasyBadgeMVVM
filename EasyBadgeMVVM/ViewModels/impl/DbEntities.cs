using EasyBadgeMVVM.DataAccess;
using EasyBadgeMVVM.Models;
using EasyBadgeMVVM.Views;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;

namespace EasyBadgeMVVM.ViewModels
{
    public class DbEntities : IDbEntities
    {
        private const string USER = "user";
        private const string EVENT = "event";
        private const string FIELD = "field";
        private const string FIELDUSER = "fielduser";
        private const string USEREVENT = "userevent";

        private EasyModelContext _dbContext = new EasyModelContext();
        private int _idEvent;
        private IRepostitoryFactory _repostitoryFactory = new RepostitoryFactory();
        private IDictionary<string, List<object>> _myUsers;

        public DbEntities()
        {
            this._dbContext.Configuration.AutoDetectChangesEnabled = false;
            this._myUsers = new Dictionary<string, List<object>>();
        }

        public void SetIdEvent(int idEvent)
        {
            this._idEvent = idEvent;
        }

        /*********************************************************************************************************************************************************************/
        /*********** GETTERS *************/
        /*********************************************************************************************************************************************************************/

        public ObservableCollection<UserEventDTO> GetAllUsers()
        {
            IList<UserEvent> lst = this._repostitoryFactory.GetUserEventRepository(this._dbContext)
                .SearchFor(us => us.User.Active == true && us.EventID_Event == this._idEvent).ToList();
            ObservableCollection<UserEventDTO> localCollection = new ObservableCollection<UserEventDTO>();

            UserEventDTO dto = new UserEventDTO();
            foreach (UserEvent ue in lst)
            {
                string nameTrim = Util.TrimWord(ue.FieldUser.Field.Name.ToLower());

                switch (nameTrim)
                {
                    case "lastname":
                        if (AddIfNecessary(localCollection, dto, dto.LastName)) dto = new UserEventDTO();
                        dto.LastName = ue.FieldUser.Value;
                        break;
                    case "firstname":
                        if (AddIfNecessary(localCollection, dto, dto.FirstName)) dto = new UserEventDTO();
                        dto.FirstName = ue.FieldUser.Value;
                        break;
                    case "company":
                        if (AddIfNecessary(localCollection, dto, dto.Company)) dto = new UserEventDTO();
                        dto.Company = ue.FieldUser.Value;
                        break;
                    case "printbadge":
                        if (AddIfNecessary(localCollection, dto, dto.PrintBadge)) dto = new UserEventDTO();
                        dto.PrintBadge = DateTime.Parse(ue.FieldUser.Value);
                        break;
                    default:
                        break;
                }
                dto.Barcode = ue.FieldUser.User.Barcode;
            }
            return localCollection;
        }

        private bool AddIfNecessary(ObservableCollection<UserEventDTO> coll, UserEventDTO dto, Object obj)
        {
            if (obj != null)
            {
                coll.Add(dto);
                return true;
            }
            return false;
        }

        public ObservableCollection<Event> GetEvents()
        {
            return new ObservableCollection<Event>(this._repostitoryFactory.GetEventRepository(this._dbContext).GetAll());
        }

        public Event GetEventById(int idEvent)
        {
            return this._repostitoryFactory.GetEventRepository(this._dbContext).GetById(idEvent);
        }

        public Event SearchFor(Expression<Func<Event, bool>> predicate)
        {
            return this._repostitoryFactory.GetEventRepository(this._dbContext).SearchFor(predicate).FirstOrDefault();
        }

        public List<UserEvent> GetUserEventByDTO(UserEventDTO dto)
        {
            return this._repostitoryFactory.GetUserEventRepository(this._dbContext).SearchFor(ue => ue.User.Barcode.Equals(dto.Barcode)).ToList();
        }

        public List<UserEvent> GetAllFieldsOfEvent(int idEvent)
        {
            return this._repostitoryFactory.GetUserEventRepository(this._dbContext).SearchFor(ue => ue.EventID_Event == idEvent)
                .GroupBy(ue1 => ue1.FieldUser.Field.Name).Select(g => g.FirstOrDefault()).ToList();
        }


        /*********************************************************************************************************************************************************************/
        /*********** INSERT *************/
        /*********************************************************************************************************************************************************************/

        //A CORRIGER CAR L'UNICITE N'EST PAS TOP TOP (nom, prenom, company) + dans SEARCHIFUNIQUE le i < 3 est du harcodage
        public bool CheckIfAlreadyExists(string lastName, string firstName, string company)
        {
            IEnumerable<UserEvent> testInDb = null;
            IEnumerable<UserEvent> testInDico = null;
            if (this._myUsers.ContainsKey(USEREVENT))
            {
                testInDico = this._myUsers[USEREVENT].Cast<UserEvent>().Where(ue => ue.EventID_Event == this._idEvent);
            }

            testInDb = this._repostitoryFactory.GetUserEventRepository(this._dbContext).SearchFor(ue1 => ue1.EventID_Event == this._idEvent);


            bool exists = SearchIfUnique(testInDb, lastName, firstName, company);
            if (exists) return true;

            if (testInDico != null)
            {
                exists = SearchIfUnique(testInDico, lastName, firstName, company);
                if (exists) return true;
            }

            return false;
        }

        private bool SearchIfUnique(IEnumerable<UserEvent> list, string lastName, string firstName, string company)
        {
            IEnumerable<IGrouping<int, UserEvent>> myGrouping = list.GroupBy(u => u.UserID_User);
            foreach (IGrouping<int, UserEvent> uu in myGrouping)
            {
                int i = 0;
                foreach (UserEvent uv in uu)
                {
                    string nameTrim = Util.TrimWord(uv.FieldUser.Field.Name);

                    switch (nameTrim)
                    {
                        case "lastname":
                            if (uv.FieldUser.Value.ToLower().Equals(lastName.ToLower()))
                            {
                                if (i < 3) i++;
                            }
                            else
                            {
                                if (i < 3) i = 0;
                            }
                            break;
                        case "firstname":
                            if (uv.FieldUser.Value.ToLower().Equals(firstName.ToLower()))
                            {
                                if (i < 3) i++;
                            }
                            else
                            {
                                if (i < 3) i = 0;
                            }
                            break;
                        case "company":
                            if (uv.FieldUser.Value.ToLower().Equals(company.ToLower()))
                            {
                                if (i < 3) i++;
                            }
                            else
                            {
                                if (i < 3) i = 0;
                            }
                            break;
                        default:
                            break;
                    }

                    if (i == 3) return true;
                }
            }
            return false;
        }

        //Field == oldfieldname with trim
        //return false if similar field name is not used
        public bool InsertNewField(string field,string oldfieldname)
        {
            Field similar = this._repostitoryFactory.GetFieldRepository(this._dbContext).CheckSimilarField(oldfieldname);
            if (similar == null)
            {
                similar = this._repostitoryFactory.GetFieldRepository(this._dbContext).CheckSimilarField(field);
            }
            Field f = new Field();
            f.Name = oldfieldname;
            if (similar != null && !similar.Name.Equals(oldfieldname))
            {
                Application.Current.Dispatcher.Invoke((Action)delegate {
                    FieldMatching fieldMatching = new FieldMatching();
                    fieldMatching.FieldImported = oldfieldname;
                    fieldMatching.FieldInDb = similar.Name;
                    fieldMatching.CreateMessages();
                    Nullable<Boolean> waiting = fieldMatching.ShowDialog();
                    if (waiting == true)
                    {
                        f.Name = similar.Name;
                    }
                });
            }
            InsertInFieldTable(f);
            return similar != null && f.Name.Equals(similar.Name);
        }

        public void InsertNewUser(int index, string field, string data)
        {
            //INSERT IN USER TABLE
            User user = new User();
            if (index == 0)
            {
                user.Active = true;
                user.CreationDate = DateTime.Now;

                if (field.ToLower().Equals("barcode"))
                {
                    user.Barcode = data;
                }
                else
                {
                    //CHECK IN THE DB THE LASTBARCODE
                    User userDb = this._repostitoryFactory.GetUserRepository(this._dbContext).GetLastUser();
                    if (userDb == null)
                    {
                        //IF THE DB IS EMPTY AND THE MAP DOESN'T CONTAINS SOMEONE, START WITH 100000
                        user.Barcode = !this._myUsers.ContainsKey(USER)
                                       ? "100000"
                                       : (Int32.Parse(this._myUsers[USER].Cast<User>().OrderByDescending(u => u.Barcode).FirstOrDefault().Barcode) + 1).ToString();
                    }
                    else
                    {
                        
                        user.Barcode = this._myUsers.ContainsKey(USER) 
                                       ? (Int32.Parse(this._myUsers[USER].Cast<User>().OrderByDescending(u => u.Barcode).FirstOrDefault().Barcode) + 1).ToString() 
                                       : (Int32.Parse(userDb.Barcode) + 1).ToString();
                    }
                }

                InsertInUserTable(user);
            }
            else
            {
                user = this._myUsers[USER].Cast<User>().OrderByDescending(u => u.Barcode).FirstOrDefault();
            }

            //INSERT IN FIELDUSER TABLE
            Field fff = this._myUsers[FIELD].Cast<Field>().Where(f2 => f2.Name.ToLower().Equals(field.ToLower())).FirstOrDefault();
            Field field2 = fff ?? this._repostitoryFactory.GetFieldRepository(this._dbContext).SearchFor(f => f.Name.ToLower().Equals(field.ToLower())).FirstOrDefault();

            FieldUser fieldUser = new FieldUser();
            fieldUser.Field = field2;
            fieldUser.User = user;
            fieldUser.Value = data;

            InsertInFieldUserTable(fieldUser);

            //INSERT IN USEREVENT
            Event ev = this._repostitoryFactory.GetEventRepository(this._dbContext).GetById(this._idEvent);
            UserEvent userEvent = new UserEvent();
            userEvent.Event = ev;
            userEvent.User = user;
            userEvent.FieldUser = fieldUser;

            InsertInUserEventTable(userEvent);
        }

        private bool InsertInUserTable(User user)
        {
            return CheckBeforeInsert(
                USER, 
                u => u.Barcode.ToLower().Equals(user.Barcode.ToLower()), 
                u => u.Barcode.ToLower().Equals(user.Barcode.ToLower()), 
                user,
                this._repostitoryFactory.GetUserRepository(this._dbContext));
        }

        public bool InsertInEventTable(Event ev)
        {
            if (CheckBeforeInsert(
                EVENT, 
                e => e.Name.ToLower().Equals(ev.Name.ToLower()), 
                e => e.Name.ToLower().Equals(ev.Name.ToLower()), 
                ev,
                this._repostitoryFactory.GetEventRepository(this._dbContext)))
            {
                this._repostitoryFactory.GetEventRepository(this._dbContext).SaveChanges();
                return true;
            }
            return false;
        }

        private bool InsertInFieldTable(Field field)
        {
            return CheckBeforeInsert(
                FIELD, 
                f => f.Name.ToLower().Equals(field.Name.ToLower()), 
                f => f.Name.ToLower().Equals(field.Name.ToLower()), 
                field,
                this._repostitoryFactory.GetFieldRepository(this._dbContext));
        }

        private bool InsertInFieldUserTable(FieldUser fieldUser)
        {
            return CheckBeforeInsert(
                FIELDUSER, 
                fu => fu.UserID_User == fieldUser.UserID_User && fu.FieldID_Field == fieldUser.FieldID_Field,
                fu => fu.UserID_User == fieldUser.UserID_User && fu.FieldID_Field == fieldUser.FieldID_Field, 
                fieldUser, 
                this._repostitoryFactory.GetFieldUserRepository(this._dbContext));
        }

        private void InsertInUserEventTable(UserEvent userEvent)
        {
            this._repostitoryFactory.GetUserEventRepository(this._dbContext).Insert(userEvent);
        }

        private bool CheckBeforeInsert<T>(string key, Func<T, bool> predicate, Expression<Func<T, bool>> expression, T fieldToInsert, IRepository<T> baseRepository)
        {
            if (!this._myUsers.ContainsKey(key))
            {
                this._myUsers.Add(key, new List<object>());
            }
            else
            {
                bool isExist = this._myUsers[key].Cast<T>().Where(predicate).Count() != 0;
                if (isExist) return false;
            }

            bool isExistInDb = baseRepository.SearchFor(expression).Count() != 0;
            if (isExistInDb) return false;

            this._myUsers[key].Add(fieldToInsert);
            baseRepository.Insert(fieldToInsert);
            return true;
        }

        /*********************************************************************************************************************************************************************/
        /*********** OTHER *************/
        /*********************************************************************************************************************************************************************/

        public void SaveAllChanges()
        {
            this._repostitoryFactory.GetFieldRepository(this._dbContext).SaveChanges();
            this._repostitoryFactory.GetUserRepository(this._dbContext).SaveChanges();
            this._repostitoryFactory.GetEventRepository(this._dbContext).SaveChanges();
            this._repostitoryFactory.GetFieldUserRepository(this._dbContext).SaveChanges();
            this._repostitoryFactory.GetUserEventRepository(this._dbContext).SaveChanges();
        }

        public void Clear()
        {
            this._myUsers.Clear();
        }

    }
}