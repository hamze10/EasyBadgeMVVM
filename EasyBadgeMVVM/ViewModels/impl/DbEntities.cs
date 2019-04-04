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
        private const string EVENTFIELD = "eventfield";
        private const string EVENTFIELDUSER = "eventfielduser";

        private EasyModelContext _dbContext = new EasyModelContext();
        private int _idEvent;
        private IRepostitoryFactory _repostitoryFactory = new RepostitoryFactory();
        private IDictionary<string, List<object>> _myUsers;
        public HashSet<string> FieldsToShow { get; set; }

        public DbEntities()
        {
            this._dbContext.Configuration.AutoDetectChangesEnabled = false;
            this._myUsers = new Dictionary<string, List<object>>();
            this.FieldsToShow = new HashSet<string>();
        }

        public void SetIdEvent(int idEvent)
        {
            this._idEvent = idEvent;
        }

        /*********************************************************************************************************************************************************************/
        /*********** GETTERS *************/
        /*********************************************************************************************************************************************************************/

        public ObservableCollection<EventFieldUser> GetAllUsers()
        {
            return new ObservableCollection<EventFieldUser>(
                this._repostitoryFactory.GetEventFieldUserRepository(this._dbContext).SearchFor(x => x.EventFieldEventID_Event == this._idEvent)
            );
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

        public ObservableCollection<Field> GetAllFields()
        {
            return new ObservableCollection<Field>(this._repostitoryFactory.GetFieldRepository(this._dbContext).GetAll());
        }

        /*public List<UserEvent> GetUserEventByDTO(UserEventDTO dto)
        {
            return this._repostitoryFactory.GetUserEventRepository(this._dbContext).SearchFor(ue => ue.User.Barcode.Equals(dto.Barcode)).ToList();
        }

        public List<UserEvent> GetAllFieldsOfEvent(int idEvent)
        {
            return this._repostitoryFactory.GetUserEventRepository(this._dbContext).SearchFor(ue => ue.EventID_Event == idEvent)
                .GroupBy(ue1 => ue1.FieldUser.Field.Name).Select(g => g.FirstOrDefault()).ToList();
        }*/

        public ObservableCollection<EventField> GetEventFieldByEvent(int idEvent)
        {
            return new ObservableCollection<EventField>(this._repostitoryFactory.GetEventFieldRepository(this._dbContext).SearchFor(eu => eu.EventID_Event == idEvent));
        }


        /*********************************************************************************************************************************************************************/
        /*********** INSERT *************/
        /*********************************************************************************************************************************************************************/
        
        //TODO CORRIGER (PERFORMANCE + QD USER > 2000)
        public bool CheckIfAlreadyExists(List<string> allFields, HashSet<string> fieldToShow, string datas)
        {
            string test = datas.Split(',')[0];

            var firstQuery = from efuu in this._dbContext.EventFieldUserSet
                             where efuu.EventField.EventID_Event == this._idEvent && efuu.Value.Equals(test)
                             select efuu.UserID_User;

            var defQuery = (from efu in this._dbContext.EventFieldUserSet
                           where firstQuery.Contains(efu.UserID_User)
                           select efu).ToList();

            ObservableCollection<EventFieldUser> toCheck = new ObservableCollection<EventFieldUser>(defQuery);

            int i = 1;
            HashSet<bool> toContains = new HashSet<bool>();
            List<string> myDatas = new List<string>(datas.Split(','));

            foreach(EventFieldUser uu in toCheck)
            {
                if (!fieldToShow.Contains(uu.EventField.Field.Name)) continue;

                if (i == FieldsToShow.Count)
                {
                    if (toContains.Count == 1 && toContains.ElementAt(0) == true)
                    {
                        return true;
                    }
                    else
                    {
                        toContains.Clear();
                    }
                    i = 1;
                }
                else
                {
                    toContains.Add(myDatas.Contains(uu.Value));
                    i++;
                }

            }

            return toContains.Count == 1 && toContains.ElementAt(0) == true;
        }

        public void InsertNewField(string field)
        {
            Field f = new Field();
            f.Name = field;
            InsertInFieldTable(f);
        }

        public void InsertNewUser(int index, string field, string data, bool visibility)
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

            //INSERT IN EVENTFIELD
            Event ev = this._repostitoryFactory.GetEventRepository(this._dbContext).GetById(this._idEvent);

            bool inDico = this._myUsers.ContainsKey(FIELD);
            Field fieldDb = inDico 
                ? this._myUsers[FIELD].Cast<Field>().Where(f2 => f2.Name.ToLower().Equals(field.ToLower())).FirstOrDefault()
                        ?? this._repostitoryFactory.GetFieldRepository(this._dbContext).SearchFor(f => f.Name.ToLower().Equals(field.ToLower())).FirstOrDefault()
                : this._repostitoryFactory.GetFieldRepository(this._dbContext).SearchFor(f => f.Name.ToLower().Equals(field.ToLower())).FirstOrDefault();

            EventField evf = this._repostitoryFactory.GetEventFieldRepository(this._dbContext)
                                .SearchFor(e => e.Event.Name.Equals(ev.Name) && e.Field.Name.Equals(fieldDb.Name)).FirstOrDefault();
            if (evf == null)
            {
                if (this._myUsers.ContainsKey(EVENTFIELD))
                {
                    evf =  this._myUsers[EVENTFIELD].Cast<EventField>().Where(e => e.Event.Name.Equals(ev.Name) && e.Field.Name.Equals(fieldDb.Name)).FirstOrDefault();
                    
                }
            }

            if (evf == null)
            {
                evf = new EventField();
                evf.Event = ev;
                evf.Field = fieldDb;
                evf.Visibility = visibility;
                evf.Unique = false;
            }

            InsertInEventFieldTable(evf);

            EventFieldUser evfu = new EventFieldUser();
            evfu.User = user;
            evfu.EventField = evf;
            evfu.Value = data;

            InsertInEventFieldUserTable(evfu);
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

        private bool InsertInEventFieldTable(EventField eventField)
        {
            return CheckBeforeInsert(
                EVENTFIELD,
                ef => ef.Event.Name.ToLower().Equals(eventField.Event.Name.ToLower()) && ef.Field.Name.ToLower().Equals(eventField.Field.Name.ToLower()),
                ef => ef.Event.Name.ToLower().Equals(eventField.Event.Name.ToLower()) && ef.Field.Name.ToLower().Equals(eventField.Field.Name.ToLower()),
                eventField,
                this._repostitoryFactory.GetEventFieldRepository(this._dbContext));
        }

        private bool InsertInEventFieldUserTable(EventFieldUser eventFieldUser)
        {
            return CheckBeforeInsert(EVENTFIELDUSER, null, null, eventFieldUser, this._repostitoryFactory.GetEventFieldUserRepository(this._dbContext));
        }

        private bool CheckBeforeInsert<T>(string key, Func<T, bool> predicate, Expression<Func<T, bool>> expression, T fieldToInsert, IRepository<T> baseRepository)
        {
            if (!this._myUsers.ContainsKey(key))
            {
                this._myUsers.Add(key, new List<object>());
            }
            else
            {
                if (predicate != null)
                {
                    bool isExist = this._myUsers[key].Cast<T>().Where(predicate).Count() != 0;
                    if (isExist) return false;
                }

            }

            if (expression != null)
            {
                bool isExistInDb = baseRepository.SearchFor(expression).Count() != 0;
                if (isExistInDb) return false;
            }

            this._myUsers[key].Add(fieldToInsert);
            baseRepository.Insert(fieldToInsert);
            return true;
        }

        /*********************************************************************************************************************************************************************/
        /*********** OTHER *************/
        /*********************************************************************************************************************************************************************/

        public void SaveAllChanges()
        {
            /*try {
                this._repostitoryFactory.GetFieldRepository(this._dbContext).SaveChanges();
                this._repostitoryFactory.GetUserRepository(this._dbContext).SaveChanges();
                this._repostitoryFactory.GetEventRepository(this._dbContext).SaveChanges();
            }
            catch(Exception e)
            {
                Console.WriteLine("Error while save changes : {0}", e);
            }*/

            this._repostitoryFactory.GetFieldRepository(this._dbContext).SaveChanges();
            this._repostitoryFactory.GetUserRepository(this._dbContext).SaveChanges();
            this._repostitoryFactory.GetEventRepository(this._dbContext).SaveChanges();

        }

        public void Clear()
        {
            this._myUsers.Clear();
        }

    }
}