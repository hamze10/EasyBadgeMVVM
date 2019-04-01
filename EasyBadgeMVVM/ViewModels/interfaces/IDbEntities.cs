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
        //List<UserEvent> GetAllFieldsOfEvent(int idEvent);
        bool CheckIfAlreadyExists(List<string> allFields, HashSet<string> fieldToShow, string datas);
        void Clear();
        ObservableCollection<Event> GetEvents();
        Event GetEventById(int idEvent);
        Event SearchFor(Expression<Func<Event, bool>> predicate);
        bool InsertInEventTable(Event ev);
        //List<UserEvent> GetUserEventByDTO(UserEventDTO dto);
        ObservableCollection<Field> GetAllFields();
        ObservableCollection<EventField> GetEventFieldByEvent(int idEvent);
    }
}