using EasyBadgeMVVM.DataAccess;
using EasyBadgeMVVM.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;


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
            IList<UserEvent> lst = this._repostitoryFactory.GetUserEventRepository(this._dbContext).SearchFor(us => us.User.Active == true && us.EventID_Event == this._idEvent).ToList();
            ObservableCollection<UserEventDTO> localCollection = new ObservableCollection<UserEventDTO>();

            UserEventDTO dto = new UserEventDTO();
            foreach (UserEvent ue in lst)
            {
                switch (ue.FieldUser.Field.Name)
                {
                    case "LastName":
                        if (AddIfNecessary(localCollection, dto, dto.LastName)) dto = new UserEventDTO();
                        dto.LastName = ue.FieldUser.Value;
                        break;
                    case "FirstName":
                        if (AddIfNecessary(localCollection, dto, dto.FirstName)) dto = new UserEventDTO();
                        dto.FirstName = ue.FieldUser.Value;
                        break;
                    case "Company":
                        if (AddIfNecessary(localCollection, dto, dto.Company)) dto = new UserEventDTO();
                        dto.Company = ue.FieldUser.Value;
                        break;
                    case "PrintBadge":
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

        public void InsertNewField(string field)
        {
            Field f = new Field();
            f.Name = field.Trim();
            InsertInFieldTable(f);
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
            Field fff = this._myUsers[FIELD].Cast<Field>().Where(f2 => f2.Name.Equals(field)).FirstOrDefault();
            Field field2 = fff ?? this._repostitoryFactory.GetFieldRepository(this._dbContext).SearchFor(f => f.Name.ToLower().Equals(field.ToLower())).FirstOrDefault()
;

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


        public bool CheckIfAlreadyExists(string lastName, string firstName)
        {
            IEnumerable<UserEvent> test;
            if (this._myUsers.ContainsKey(USEREVENT))
            {
                test = this._myUsers[USEREVENT].Cast<UserEvent>().Where(ue =>
                                                 ue.EventID_Event == this._idEvent &&
                                                 (ue.FieldUser.Field.Name.ToLower().Equals("lastname") || ue.FieldUser.Field.Name.ToLower().Equals("firstname")) &&
                                                 (ue.FieldUser.Value.Equals(lastName) || ue.FieldUser.Value.Equals(firstName)));
            }
            else
            {
                test = this._repostitoryFactory.GetUserEventRepository(this._dbContext).SearchFor(ue1 =>
                ue1.EventID_Event == this._idEvent &&
                (ue1.FieldUser.Field.Name.ToLower().Equals("lastname") || ue1.FieldUser.Field.Name.ToLower().Equals("firstname")) &&
                (ue1.FieldUser.Value.Equals(lastName) || ue1.FieldUser.Value.Equals(firstName)));
            }

            int i = 0;

            foreach (UserEvent ue in test)
            {
                switch (ue.FieldUser.Field.Name.ToLower())
                {
                    case "lastname":
                        if (ue.FieldUser.Value.ToLower().Equals(lastName.ToLower()))
                        {
                            if (i != 2) i++;
                        }
                        else
                        {
                            if (i == 1) i = 0;
                        }
                        break;
                    case "firstname":
                        if (ue.FieldUser.Value.ToLower().Equals(firstName.ToLower()))
                        {
                            if (i != 2) i++;
                        }
                        else
                        {
                            if (i == 1) i = 0;
                        }
                        break;
                    default:
                        break;
                }

                if (i == 2) return true;
            }

            return false;
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