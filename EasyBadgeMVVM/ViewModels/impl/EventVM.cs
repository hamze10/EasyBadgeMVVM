using EasyBadgeMVVM.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.ViewModels
{
    public class EventVM : IEventVM, INotifyPropertyChanged
    {

        /*********************************************************************************************************************************************************************/
        /****** PROPERTY CHANGED ******/
        /*********************************************************************************************************************************************************************/

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private IDbEntities _dbEntities;
        private ObservableCollection<Event> _listOfEvents;
        private int _selectedEvent;

        public EventVM()
        {
            this._dbEntities = new DbEntities();
        }

        public ObservableCollection<Event> ListOfEvents
        {
            get
            {
                if (this._listOfEvents == null)
                {
                    this._listOfEvents = LoadEvents();
                }

                return this._listOfEvents;
            }
        }

        public ObservableCollection<Event> LoadEvents()
        {
            return this._dbEntities.GetEvents();
        }

        public int SelectedEvent
        {
            get
            {
                return this._selectedEvent;
            }

            set
            {
                this._selectedEvent = value;
            }
        }

        public Event InsertEvent(Event ev)
        {
            this._dbEntities.InsertInEventTable(ev);
            return this._dbEntities.SearchFor(e1 => e1.Name.Equals(ev.Name) && e1.DateOfEvent.Equals(ev.DateOfEvent));
        }

        public Event GetEventById(int idEvent)
        {
            return this._dbEntities.GetEventById(idEvent);
        }
    }
}