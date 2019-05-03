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
        ObservableCollection<EventFieldUserSet> GetAllUsers();
        void InsertNewField(string field);
        void InsertNewUser(int index, string field, string data, bool visiblity);
        void SetIdEvent(int idEvent);
        void SaveAllChanges();
        HashSet<string> FieldsToShow { get; set; }
        List<EventFieldUserSet> GetAllFieldsOfEvent(int idEvent);
        bool CheckIfAlreadyExists(List<string> allFields, HashSet<string> fieldToShow, string datas);
        void Clear();
        ObservableCollection<EventSet> GetEvents();
        EventSet GetEventById(int idEvent);
        EventSet SearchFor(Expression<Func<EventSet, bool>> predicate);
        bool InsertInEventTable(EventSet ev);
        List<EventFieldUserSet> GetEventFieldUserByValues(List<string> values);
        ObservableCollection<FieldSet> GetAllFields();
        ObservableCollection<EventFieldSet> GetEventFieldByEvent(int idEvent);
        bool GetVisibilityField(string field);
        ObservableCollection<BadgeSet> GetAllBadges();
        BadgeEventSet InsertInBadgeEvent(int idBadge, int idEvent, string templateName);
        void InsertInPosition(BadgeEventSet be, FieldSet f, double posX, double posY, string fontFamily, int fontSize);
        BadgeEventSet GetBadgeEvent(int idBadge, int idEvent);
        void DeleteRowPosition(int idBadge, int idEvent, string templateName);
        List<PositionSet> GetPositions(int idBadge, int idEvent, string templateName);
        ObservableCollection<BadgeEventSet> GetAllBadgeEvent();
        BadgeEventSet GetBadgeEventById(int idBadgeEvent);
        void UpdateDefaultPrint(int idBadgeEvent);
        BadgeEventSet GetDefaultBadgeEvent();
        void InsertInPrintBadge(PrintBadgeSet pb);
        List<PrintBadgeSet> GetAllPrintBadge();
    }
}