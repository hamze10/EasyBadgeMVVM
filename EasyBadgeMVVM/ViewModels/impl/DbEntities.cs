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

        private EasyBadgeModelContext _dbContext = new EasyBadgeModelContext();
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

        public ObservableCollection<EventFieldUserSet> GetAllUsers()
        {
            return new ObservableCollection<EventFieldUserSet>(
                this._repostitoryFactory.GetEventFieldUserRepository(this._dbContext).SearchFor(x => x.EventFieldEventID_Event == this._idEvent)
            );
        }

        public ObservableCollection<EventSet> GetEvents()
        {
            return new ObservableCollection<EventSet>(this._repostitoryFactory.GetEventRepository(this._dbContext).GetAll());
        }

        public EventSet GetEventById(int idEvent)
        {
            return this._repostitoryFactory.GetEventRepository(this._dbContext).GetById(idEvent);
        }

        public EventSet SearchFor(Expression<Func<EventSet, bool>> predicate)
        {
            return this._repostitoryFactory.GetEventRepository(this._dbContext).SearchFor(predicate).FirstOrDefault();
        }

        public ObservableCollection<FieldSet> GetAllFields()
        {
            return new ObservableCollection<FieldSet>(this._repostitoryFactory.GetFieldRepository(this._dbContext).GetAll());
        }

        public List<EventFieldUserSet> GetEventFieldUserByValues(List<string> values)
        {
            Dictionary<int, int> usersWithVal = new Dictionary<int, int>();

            foreach(string val in values)
            {
                var users = from efu in this._dbContext.EventFieldUserSets
                            where efu.EventFieldEventID_Event == this._idEvent && efu.Value.Equals(val)
                            select efu.UserID_User;

                foreach(var user in users)
                {
                    if (usersWithVal.ContainsKey(user))
                    {
                        usersWithVal[user] = usersWithVal[user] + 1;
                    }
                    else
                    {
                        usersWithVal.Add(user, 1);
                    }
                    
                }
            }

            int userOk = usersWithVal.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

            List<EventFieldUserSet> toSend = (from efu in this._dbContext.EventFieldUserSets
                                           where efu.EventFieldEventID_Event == this._idEvent && efu.UserID_User == userOk
                                           select efu).ToList();

            return toSend;
        }

        public List<EventFieldUserSet> GetAllFieldsOfEvent(int idEvent)
        {
            return this._repostitoryFactory.GetEventFieldUserRepository(this._dbContext)
                        .SearchFor(efu => efu.EventFieldEventID_Event == idEvent)
                        .GroupBy(efu => efu.EventFieldSet.FieldSet.Name)
                        .Select(efu => efu.FirstOrDefault())
                        .ToList();
        }

        public ObservableCollection<EventFieldSet> GetEventFieldByEvent(int idEvent)
        {
            return new ObservableCollection<EventFieldSet>(this._repostitoryFactory.GetEventFieldRepository(this._dbContext).SearchFor(eu => eu.EventID_Event == idEvent));
        }

        public ObservableCollection<BadgeSet> GetAllBadges()
        {
            return new ObservableCollection<BadgeSet>(this._repostitoryFactory.GetBadgeRepository(this._dbContext).GetAll());
        }

        public ObservableCollection<BadgeEventSet> GetAllBadgeEvent()
        {
            return new ObservableCollection<BadgeEventSet>(
                this._repostitoryFactory.GetBadgeEventRepository(this._dbContext)
                .SearchFor(be => be.EventID_Event == this._idEvent)
            );
        }

        public ObservableCollection<BadgeEventSet> GetAllBadgeEvent(int eventId)
        {
            ObservableCollection<BadgeEventSet> all = new ObservableCollection<BadgeEventSet>(this._repostitoryFactory.GetBadgeEventRepository(this._dbContext).GetAll());
            List<BadgeEventSet> result = all.Where(be => be.EventID_Event == eventId).ToList();
            ObservableCollection<BadgeEventSet> toReturn = new ObservableCollection<BadgeEventSet>();
            foreach (BadgeEventSet item in result)
            {
                toReturn.Add(item);
            }
            return toReturn;
        }

        public BadgeEventSet GetBadgeEvent(int idBadge, int idEvent)
        {
            return this._repostitoryFactory.GetBadgeEventRepository(this._dbContext).SearchFor(be => be.BadgeID_Badge == idBadge && be.EventID_Event == idEvent).FirstOrDefault();
        }

        public List<PositionSet> GetPositions(int idBadge, int idEvent, string templateName)
        {
            return this._repostitoryFactory
                        .GetPositionRepository(this._dbContext)
                        .SearchFor(p => p.BadgeEventSet.BadgeID_Badge == idBadge && p.BadgeEventSet.EventID_Event == idEvent && p.BadgeEventSet.Name.Equals(templateName))
                        .ToList();
        }

        public BadgeEventSet GetBadgeEventById(int idBadgeEvent)
        {
            return this._repostitoryFactory
                        .GetBadgeEventRepository(this._dbContext)
                        .GetById(idBadgeEvent);
        }

        public BadgeEventSet GetDefaultBadgeEvent()
        {
            return this._repostitoryFactory
                        .GetBadgeEventRepository(this._dbContext)
                        .SearchFor(be => be.EventID_Event == this._idEvent && be.DefaultPrint == true)
                        .SingleOrDefault();
        }

        public bool GetVisibilityField(string field)
        {
            return this._repostitoryFactory.GetEventFieldRepository(this._dbContext)
                    .SearchFor(ef => ef.EventID_Event == this._idEvent && ef.FieldSet.Name.Equals(field))
                    .FirstOrDefault().Visibility;

        }


        public List<PrintBadgeSet> GetAllPrintBadge()
        {
            return this._repostitoryFactory.GetPrintBadgeRepository(this._dbContext).SearchFor(p => p.EventID_Event == this._idEvent).ToList();
        }

        public ObservableCollection<FilterSet> GetAllFilters()
        {
            return new ObservableCollection<FilterSet>(this._repostitoryFactory.GetFilterRepository(this._dbContext).GetAll());
        }

        public ObservableCollection<FilterSet> GetAllFilters(int eventId)
        {
            ObservableCollection<FilterSet> all = new ObservableCollection<FilterSet>(this._repostitoryFactory.GetFilterRepository(this._dbContext).GetAll());
            List<FilterSet> result = all.Where(f => f.EventFieldEventID_Event == eventId).ToList();
            ObservableCollection<FilterSet> toReturn = new ObservableCollection<FilterSet>();
            foreach(FilterSet item in result)
            {
                toReturn.Add(item);
            }
            return toReturn;
        }

        public ObservableCollection<RuleSet> GetAllRules(int filterId)
        {
            ObservableCollection<RuleSet> all = new ObservableCollection<RuleSet>(this._repostitoryFactory.GetRuleRepository(this._dbContext).GetAll());
            List<RuleSet> result = all.Where(r => r.FilterID_Filter == filterId).ToList();
            ObservableCollection<RuleSet> toReturn = new ObservableCollection<RuleSet>();
            foreach (RuleSet item in result)
            {
                toReturn.Add(item);
            }
            return toReturn;
        }

        public ObservableCollection<TargetSet> GetAllTargets()
        {
            return new ObservableCollection<TargetSet>(this._repostitoryFactory.GetTargetRepository(this._dbContext).GetAll());
        }

        /*********************************************************************************************************************************************************************/
        /*********** INSERT *************/
        /*********************************************************************************************************************************************************************/

        //TODO CORRIGER (PERFORMANCE + QD USER > 2000)
        public bool CheckIfAlreadyExists(List<string> allFields, HashSet<string> fieldToShow, string datas)
        {
            string test = datas.Split(',')[0];

            var firstQuery = from efuu in this._dbContext.EventFieldUserSets
                             where efuu.EventFieldSet.EventID_Event == this._idEvent && efuu.Value.Equals(test)
                             select efuu.UserID_User;

            var defQuery = (from efu in this._dbContext.EventFieldUserSets
                           where firstQuery.Contains(efu.UserID_User)
                           select efu).ToList();

            ObservableCollection<EventFieldUserSet> toCheck = new ObservableCollection<EventFieldUserSet>(defQuery);

            int i = 1;
            HashSet<bool> toContains = new HashSet<bool>();
            List<string> myDatas = new List<string>(datas.Split(','));

            foreach(EventFieldUserSet uu in toCheck)
            {
                if (!fieldToShow.Contains(uu.EventFieldSet.FieldSet.Name)) continue;

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
            FieldSet f = new FieldSet();
            f.Name = field;
            InsertInFieldTable(f);
        }

        public void InsertNewUser(int index, string field, string data, bool visibility)
        {
            //INSERT IN USER TABLE
            UserSet user = new UserSet();
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
                    UserSet userDb = this._repostitoryFactory.GetUserRepository(this._dbContext).GetLastUser();
                    if (userDb == null)
                    {
                        //IF THE DB IS EMPTY AND THE MAP DOESN'T CONTAINS SOMEONE, START WITH 100000
                        user.Barcode = !this._myUsers.ContainsKey(USER)
                                       ? "100000"
                                       : (Int32.Parse(this._myUsers[USER].Cast<UserSet>().OrderByDescending(u => u.Barcode).FirstOrDefault().Barcode) + 1).ToString();
                    }
                    else
                    {
                        
                        user.Barcode = this._myUsers.ContainsKey(USER) 
                                       ? (Int32.Parse(this._myUsers[USER].Cast<UserSet>().OrderByDescending(u => u.Barcode).FirstOrDefault().Barcode) + 1).ToString() 
                                       : (Int32.Parse(userDb.Barcode) + 1).ToString();
                    }
                }

                InsertInUserTable(user);
                
            }
            else
            {
                user = this._myUsers[USER].Cast<UserSet>().OrderByDescending(u => u.Barcode).FirstOrDefault();
            }

            //INSERT IN EVENTFIELD
            EventSet ev = this._repostitoryFactory.GetEventRepository(this._dbContext).GetById(this._idEvent);

            bool inDico = this._myUsers.ContainsKey(FIELD);
            FieldSet fieldDb = inDico 
                ? this._myUsers[FIELD].Cast<FieldSet>().Where(f2 => f2.Name.ToLower().Equals(field.ToLower())).FirstOrDefault()
                        ?? this._repostitoryFactory.GetFieldRepository(this._dbContext).SearchFor(f => f.Name.ToLower().Equals(field.ToLower())).FirstOrDefault()
                : this._repostitoryFactory.GetFieldRepository(this._dbContext).SearchFor(f => f.Name.ToLower().Equals(field.ToLower())).FirstOrDefault();

            EventFieldSet evf = this._repostitoryFactory.GetEventFieldRepository(this._dbContext)
                                .SearchFor(e => e.EventSet.Name.Equals(ev.Name) && e.FieldSet.Name.Equals(fieldDb.Name)).FirstOrDefault();
            if (evf == null)
            {
                if (this._myUsers.ContainsKey(EVENTFIELD))
                {
                    evf =  this._myUsers[EVENTFIELD].Cast<EventFieldSet>().Where(e => e.EventSet.Name.Equals(ev.Name) && e.FieldSet.Name.Equals(fieldDb.Name)).FirstOrDefault();
                    
                }
            }

            if (evf == null)
            {
                evf = new EventFieldSet();
                evf.EventSet = ev;
                evf.FieldSet = fieldDb;
                evf.Visibility = visibility;
                evf.Unique = false;
            }

            bool isInserted = InsertInEventFieldTable(evf);
            if (!isInserted) this._repostitoryFactory.GetEventFieldRepository(this._dbContext).Update(evf);

            EventFieldUserSet evfu = new EventFieldUserSet();
            evfu.UserSet = user;
            evfu.EventFieldSet = evf;
            evfu.Value = data;

            InsertInEventFieldUserTable(evfu);
        }

        private bool InsertInUserTable(UserSet user)
        {
            return CheckBeforeInsert(
                USER, 
                u => u.Barcode.ToLower().Equals(user.Barcode.ToLower()), 
                u => u.Barcode.ToLower().Equals(user.Barcode.ToLower()), 
                user,
                this._repostitoryFactory.GetUserRepository(this._dbContext));
        }

        public bool InsertInEventTable(EventSet ev)
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

        private bool InsertInFieldTable(FieldSet field)
        {
            return CheckBeforeInsert(
                FIELD, 
                f => f.Name.ToLower().Equals(field.Name.ToLower()), 
                f => f.Name.ToLower().Equals(field.Name.ToLower()), 
                field,
                this._repostitoryFactory.GetFieldRepository(this._dbContext));
        }

        private bool InsertInEventFieldTable(EventFieldSet eventField)
        {
            return CheckBeforeInsert(
                EVENTFIELD,
                ef => ef.EventSet.Name.ToLower().Equals(eventField.EventSet.Name.ToLower()) && ef.FieldSet.Name.ToLower().Equals(eventField.FieldSet.Name.ToLower()),
                ef => ef.EventSet.Name.ToLower().Equals(eventField.EventSet.Name.ToLower()) && ef.FieldSet.Name.ToLower().Equals(eventField.FieldSet.Name.ToLower()),
                eventField,
                this._repostitoryFactory.GetEventFieldRepository(this._dbContext));
        }

        private bool InsertInEventFieldUserTable(EventFieldUserSet eventFieldUser)
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

        public BadgeEventSet InsertInBadgeEvent(int idBadge, int idEvent, string templateName)
        {
            var repo = this._repostitoryFactory.GetBadgeEventRepository(this._dbContext);
            var badgeEventFound = repo.SearchFor(b => b.BadgeSet.ID_Badge == idBadge && b.EventSet.ID_Event == idEvent && b.Name.Equals(templateName)).FirstOrDefault();

            if (badgeEventFound != null)
            {
                return badgeEventFound;
            }

            BadgeEventSet be = new BadgeEventSet();
            be.BadgeSet = this._repostitoryFactory.GetBadgeRepository(this._dbContext).SearchFor(badg => badg.ID_Badge == idBadge).FirstOrDefault();
            be.EventSet = this._repostitoryFactory.GetEventRepository(this._dbContext).SearchFor(eve => eve.ID_Event == idEvent).FirstOrDefault();
            be.Name = templateName;
            repo.Insert(be);
            repo.SaveChanges();

            
            return repo.SearchFor(b => b.BadgeSet.ID_Badge == idBadge && b.EventSet.ID_Event == idEvent && b.Name.Equals(templateName)).FirstOrDefault();
        }

        public void InsertInPosition(BadgeEventSet be, FieldSet f, double posX, double posY, string fontFamily, int fontSize)
        {
            PositionSet p = new PositionSet();
            p.BadgeEventSet = be;
            p.FieldSet = f;
            p.Position_X = posX;
            p.Position_Y = posY;
            p.FontFamily = fontFamily;
            p.FontSize = fontSize;
            p.FontStyle = System.Drawing.FontStyle.Regular.ToString();

            var repo = this._repostitoryFactory.GetPositionRepository(this._dbContext);
            repo.Insert(p);
            repo.SaveChanges();
        }

        public void InsertInPrintBadge(PrintBadgeSet pb)
        {
            var repo = this._repostitoryFactory.GetPrintBadgeRepository(this._dbContext);
            repo.Insert(pb);
            repo.SaveChanges();
        }

        public void DeleteRowPosition(int idBadge, int idEvent, string templateName)
        {
            this._repostitoryFactory.GetPositionRepository(this._dbContext).RemoveRows(idBadge, idEvent, templateName);
        }

        public void UpdateDefaultPrint(int idBadgeEvent)
        {
            this._repostitoryFactory.GetBadgeEventRepository(this._dbContext)
                .UpdateDefaultPrint(idBadgeEvent, this._idEvent);
        }

        public void InsertNewFilter(FilterSet newFilter)
        {
            this._repostitoryFactory.GetFilterRepository(this._dbContext).Insert(newFilter);
        }

        public void UpdateFilter(int filterId, FilterSet updatedFilter)
        {
            this._repostitoryFactory.GetFilterRepository(this._dbContext).UpdateFilter(filterId, updatedFilter);
        }

        public void DeleteFilter(int filterId)
        {
            FilterSet filterToDelete = this._repostitoryFactory.GetFilterRepository(this._dbContext).GetById(filterId);
            List<RuleSet> rulesToDelete = this._repostitoryFactory.GetRuleRepository(this._dbContext).GetAll()
                .Where(r => r.FilterID_Filter == filterToDelete.ID_Filter).ToList();
            foreach(RuleSet item in rulesToDelete)
            {
                DeleteRule(item.ID_Rule);
            }
            this._repostitoryFactory.GetFilterRepository(this._dbContext).Delete(filterToDelete);
        }

        public void InsertNewRule(RuleSet newRule)
        {
            this._repostitoryFactory.GetRuleRepository(this._dbContext).Insert(newRule);
        }

        public void UpdateRule(int ruleId, RuleSet updatedRule)
        {
            this._repostitoryFactory.GetRuleRepository(this._dbContext).UpdateRule(ruleId, updatedRule);
        }

        public void DeleteRule(int ruleId)
        {
            RuleSet ruleToDelete = this._repostitoryFactory.GetRuleRepository(this._dbContext).GetById(ruleId);
            this._repostitoryFactory.GetRuleRepository(this._dbContext).Delete(ruleToDelete);
        }

        public void InsertNewTarget(TargetSet newTarget)
        {
            this._repostitoryFactory.GetTargetRepository(this._dbContext).Insert(newTarget);
        }

        public void UpdateUser(int idUser, Dictionary<string, string> newValues)
        {
            this._repostitoryFactory.GetEventFieldUserRepository(this._dbContext).Update(idUser, this._idEvent, newValues);
        }

        public void UpdateEventField(string field, bool visibility)
        {
            EventFieldSet evf = this._repostitoryFactory
                                    .GetEventFieldRepository(this._dbContext)
                                    .SearchFor(e => e.EventID_Event == this._idEvent && e.FieldSet.Name.Equals(field))
                                    .FirstOrDefault();
            if (evf == null) return;
            evf.Visibility = visibility;
            this._repostitoryFactory.GetEventFieldRepository(this._dbContext).Update(evf);
        }


        /*********************************************************************************************************************************************************************/
        /*********** OTHER *************/
        /*********************************************************************************************************************************************************************/

        public void SaveAllChanges()
        {
            this._repostitoryFactory.GetFieldRepository(this._dbContext).SaveChanges();
            this._repostitoryFactory.GetUserRepository(this._dbContext).SaveChanges();
            this._repostitoryFactory.GetEventRepository(this._dbContext).SaveChanges();
            this._repostitoryFactory.GetEventFieldRepository(this._dbContext).SaveChanges();
            this._repostitoryFactory.GetEventFieldUserRepository(this._dbContext).SaveChanges();
            this._repostitoryFactory.GetPrintBadgeRepository(this._dbContext).SaveChanges();
            this._repostitoryFactory.GetFilterRepository(this._dbContext).SaveChanges();
            this._repostitoryFactory.GetRuleRepository(this._dbContext).SaveChanges();
            this._repostitoryFactory.GetTargetRepository(this._dbContext).SaveChanges();
        }

        public void Clear()
        {
            this._myUsers.Clear();
        }
    }
}