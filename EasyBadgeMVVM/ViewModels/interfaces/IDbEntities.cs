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
        HashSet<string> FieldsToShow { get; set; }

        ObservableCollection<EventFieldUserSet> GetAllUsers();
        List<EventFieldUserSet> GetAllFieldsOfEvent(int idEvent);
        ObservableCollection<EventSet> GetEvents();
        EventSet GetEventById(int idEvent);
        List<EventFieldUserSet> GetEventFieldUserByValues(List<string> values);
        ObservableCollection<FieldSet> GetAllFields();
        ObservableCollection<EventFieldSet> GetEventFieldByEvent(int idEvent);
        bool GetVisibilityField(string field);
        ObservableCollection<UserSet> GetAllUsersSet();
        ObservableCollection<BadgeSet> GetAllBadges();
        BadgeEventSet GetBadgeEvent(int idBadge, int idEvent);
        List<PositionSet> GetPositions(int idBadge, int idEvent, string templateName);
        ObservableCollection<BadgeEventSet> GetAllBadgeEvent();
        BadgeEventSet GetBadgeEventById(int idBadgeEvent);
        List<PrintBadgeSet> GetAllPrintBadge();
        ObservableCollection<FilterSet> GetAllFilters();
        ObservableCollection<FilterSet> GetAllFilters(int eventId);
        ObservableCollection<RuleSet> GetAllRules(int filterId);
        ObservableCollection<TargetSet> GetAllTargets();
        ObservableCollection<BadgeEventSet> GetAllBadgeEvent(int eventId);
        BadgeEventSet GetDefaultBadgeEvent();

        void SetIdEvent(int idEvent);

        void InsertNewField(string field);
        void InsertNewUser(int index, string field, string data, bool visiblity, bool onsite = false);
        bool InsertInEventTable(EventSet ev);
        BadgeEventSet InsertInBadgeEvent(int idBadge, int idEvent, string templateName, System.Windows.Media.Imaging.BitmapSource imageSrc);
        void InsertInPosition(BadgeEventSet be, FieldSet f, double posX, double posY, string fontFamily, int fontSize, double layoutTransform);
        void InsertInPrintBadge(PrintBadgeSet pb);
        void InsertInBadge(BadgeSet badge);
        void InsertNewFilter(FilterSet newFilter);
        void InsertNewRule(RuleSet newRule);
        void InsertNewTarget(TargetSet newTarget);

        void UpdateUser(int idUser, Dictionary<string, string> newValues);
        void UpdateEventField(string field, bool visibility);
        void UpdateDefaultPrint(int idBadgeEvent);
        void UpdateFilter(int filterId, FilterSet updatedFilter);
        void UpdateRule(int ruleId, RuleSet updatedRule);

        void DeleteRule(int ruleId);
        void DeleteFilter(int filterId);
        void DeleteRowPosition(int idBadge, int idEvent, string templateName);

        void SaveAllChanges();
        bool CheckIfAlreadyExists(List<string> allFields, HashSet<string> fieldToShow, string datas);
        void Clear();
        EventSet SearchFor(Expression<Func<EventSet, bool>> predicate);
    }
}