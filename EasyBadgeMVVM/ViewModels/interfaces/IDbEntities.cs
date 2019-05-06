using EasyBadgeMVVM.Models;


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace EasyBadgeMVVM.ViewModels
{
    public interface IDbEntities
    {
        ObservableCollection<EventFieldUser> GetAllUsers();
        void InsertNewField(string field);
        void InsertNewUser(int index, string field, string data, bool visiblity);
        void SetIdEvent(int idEvent);
        void SaveAllChanges();
        HashSet<string> FieldsToShow { get; set; }
        List<EventFieldUser> GetAllFieldsOfEvent(int idEvent);
        bool CheckIfAlreadyExists(List<string> allFields, HashSet<string> fieldToShow, string datas);
        void Clear();
        ObservableCollection<Event> GetEvents();
        Event GetEventById(int idEvent);
        Event SearchFor(Expression<Func<Event, bool>> predicate);
        bool InsertInEventTable(Event ev);
        List<EventFieldUser> GetEventFieldUserByValues(List<string> values);
        ObservableCollection<Field> GetAllFields();
        ObservableCollection<EventField> GetEventFieldByEvent(int idEvent);
        bool GetVisibilityField(string field);
        ObservableCollection<Badge> GetAllBadges();
        BadgeEvent InsertInBadgeEvent(int idBadge, int idEvent, string templateName);
        void InsertInPosition(BadgeEvent be, Field f, double posX, double posY, string fontFamily, int fontSize);
        BadgeEvent GetBadgeEvent(int idBadge, int idEvent);

        ObservableCollection<Filter> GetAllFilters();
        ObservableCollection<Filter> GetAllFilters(int eventId);
        void InsertNewFilter(Filter newFilter);
        void UpdateFilter(int filterId, Filter updatedFilter);
        void DeleteFilter(int filterId);

        ObservableCollection<Rule> GetAllRules(int filterId);
        void InsertNewRule(Rule newRule);
        void UpdateRule(int ruleId, Rule updatedRule);
        void DeleteRule(int ruleId);

        ObservableCollection<Target> GetAllTargets();
        void InsertNewTarget(Target newTarget);

        void DeleteRowPosition(int idBadge, int idEvent, string templateName);
        List<Position> GetPositions(int idBadge, int idEvent, string templateName);
        ObservableCollection<BadgeEvent> GetAllBadgeEvent();
        ObservableCollection<BadgeEvent> GetAllBadgeEvent(int eventId);
        BadgeEvent GetBadgeEventById(int idBadgeEvent);
        void UpdateDefaultPrint(int idBadgeEvent);
        BadgeEvent GetDefaultBadgeEvent();

    }
}