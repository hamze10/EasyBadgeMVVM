using EasyBadgeMVVM.Models;
using EasyBadgeMVVM.Views;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EasyBadgeMVVM.ViewModels
{
    public class MainWindowImpl : INotifyPropertyChanged, IMainWindow
    {

        /*********************************************************************************************************************************************************************/
        /****** CONSTRUCTOR ******/
        /*********************************************************************************************************************************************************************/

        private int _idEvent;
        private IDbEntities _dbEntities;
        public string EventTitle { get; set; }
        private string[] Months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        private string[] Prefix = new string[] { "st", "nd", "rd", "th" };
        public string MachineName {
            get {
                DateTime now = DateTime.Now;
                int rest = now.Day;
                string display = now.DayOfWeek + " " + now.Day + Prefix[rest > 3 ? 3 : rest - 1] + " " + Months[now.Month - 1] + " " + now.Year;
                return Environment.MachineName + " - " + display;
            }
        }

        public MainWindowImpl(int idEvent)
        {
            this._idEvent = idEvent;
            this._dbEntities = new DbEntities();
            this._dbEntities.SetIdEvent(idEvent);
            EventSet thisEvent = this._dbEntities.GetEventById(idEvent);
            this.EventTitle = thisEvent.Name + " - " + thisEvent.DateOfEvent.ToString();
        }

        public int IdEvent
        {
            get
            {
                return this._idEvent;
            }
        }

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

        /*********************************************************************************************************************************************************************/
        /********** FIELDS ************/
        /*********************************************************************************************************************************************************************/

        private EventFieldUserSet _selectedUserEvent;
        private ObservableCollection<EventFieldUserSet> _mainFields;
        private ObservableCollection<EventFieldUserSet> _allUsers;
        private int _nbrUser;

        public int NbrUser
        {
            get
            {
                return this._nbrUser;
            }
            set
            {
                this._nbrUser = value;
                OnPropertyChanged("NbrUser");
            }
        }

        public HashSet<string> FieldToShow { get; set; }

        //Check if delete button is pressed in search bar
        private bool _isDelete = false;

        public EventFieldUserSet SelectedUserEvent
        {
            get
            {
                return this._selectedUserEvent;
            }
            set
            {
                this._selectedUserEvent = value;
            }
        }

        public ObservableCollection<EventFieldUserSet> MainFields
        {
            get
            {
                if (this._mainFields == null)
                {
                    this._mainFields = this._dbEntities.GetAllUsers();
                    this._allUsers = this._mainFields;
                    NbrUser = this._mainFields.Count;
                }

                return this._mainFields;
            }
        }

        public ObservableCollection<EventFieldUserSet> RefreshMainsFields()
        {
            this._mainFields = this._dbEntities.GetAllUsers();
            NbrUser = this._mainFields.Count;
            return this._mainFields;
        }

        /*********************************************************************************************************************************************************************/
        /********** SEARCH ************/
        /*********************************************************************************************************************************************************************/

        private string _search;

        public string Search
        {
            get { return this._search; }
            set
            {
                this._search = value;
                DoSearch();
                OnPropertyChanged("MainFields");
                OnPropertyChanged("Search");
                OnPropertyChanged("NbrUser");
            }
        }

        private ObservableCollection<EventFieldUserSet> previousLastSearch = null;
        private ObservableCollection<EventFieldUserSet> lastSearch = null;

        public ObservableCollection<EventFieldUserSet> DoSearch()
        {
            if (lastSearch == null)
            {
                lastSearch = this._mainFields;
            }

            if (previousLastSearch == null)
            {
                previousLastSearch = this._mainFields;
            }

            var toSearch = this._search.ToLower();
            if (toSearch.Length == 0)
            {
                lastSearch = this._mainFields;
                return this._mainFields;
            }

            string[] splitted = toSearch.Split(' ');
            toSearch = splitted.Length <= 1 ? splitted[0] : splitted[splitted.Length - 1];
            if (toSearch.Trim() == string.Empty)
            {
                previousLastSearch = lastSearch;
                return null;
            }

            Func<EventFieldUserSet, bool> predicate = ef => ef.EventFieldSet.EventID_Event == this._idEvent 
                                                        && ef.Value != null
                                                        && ef.Value.ToLower().Contains(toSearch);

            ObservableCollection<EventFieldUserSet> toSend = new ObservableCollection<EventFieldUserSet>();

            foreach(var tt in this._mainFields.Where(predicate))
            {
                var p = this._mainFields.Where(ee => ee.UserID_User == tt.UserID_User);
                foreach (var tte in p)
                {
                    var whereSearch = !this._isDelete ? lastSearch : previousLastSearch;
                    if (this.FieldToShow.Contains(tte.EventFieldSet.FieldSet.Name) && !toSend.Contains(tte) && whereSearch.Contains(tte))
                    {
                        toSend.Add(tte);
                    }
                }
            }

            lastSearch = toSend;
            return toSend;
        }

        public void SetDeleteButton(bool value)
        {
            this._isDelete = value;
        }

        /*********************************************************************************************************************************************************************/
        /*********** MAIN *************/
        /*********************************************************************************************************************************************************************/

        public void LoadFromImport(string content)
        {
            string[] splitted = content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            int nbrForInsertion = splitted.Length >= 500 ? splitted.Length >= 5000 ? splitted.Length/25 : splitted.Length/10 : 200;
            int i = 0;
            List<string> allFields = new List<string>();
            Dictionary<int, bool> fieldsVisiblity = new Dictionary<int, bool>();
            bool cancel = false;

            foreach(string s in splitted)
            {
                if (s.Equals(string.Empty)) continue;

                //Fields
                if (i == 0)
                {
                    //1 Recupérer tous les champs dans la DB
                    //2 Ouvrir fenetre FieldMatching(list field_import, list field_db) 
                    ObservableCollection<FieldSet> fieldsDB = this._dbEntities.GetAllFields();
                    ObservableCollection<FieldSet> fieldsImport = new ObservableCollection<FieldSet>();

                    foreach(string field2 in s.Split(','))
                    {
                        FieldSet f = new FieldSet();
                        f.Name = field2;
                        fieldsImport.Add(f);
                    }

                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        FieldMatching fm = new FieldMatching(fieldsDB, fieldsImport);
                        fm.CreateMessages();
                        Nullable<Boolean> waiting = fm.ShowDialog();
                        if (waiting == true)
                        {
                            //DO CHANGES
                            this.FieldToShow = fm.FieldsToShow;
                            this._dbEntities.FieldsToShow = fm.FieldsToShow;
                            Dictionary<string, string> myDico = fm.FieldsAccepted;
                            int j = 0;
                            foreach (KeyValuePair<string, string> kk in myDico)
                            {
                                allFields.Add(kk.Value);
                                fieldsVisiblity.Add(j++, this._dbEntities.FieldsToShow.Contains(kk.Value));

                                if (kk.Key.Equals(kk.Value))
                                {
                                    //INSERT IN TABLE FIELD
                                    this._dbEntities.InsertNewField(kk.Value);
                                }
                            }
                        }
                        else
                        {
                            cancel = true;
                        }
                    });

                    if (cancel) return;
                }

                //Data
                else
                {
                    int l = 0;
                    foreach(string fi in allFields)
                    {
                        this._dbEntities.UpdateEventField(fi, fieldsVisiblity.ElementAt(l).Value);
                        l++;
                    }

                    if (this._dbEntities.CheckIfAlreadyExists(allFields, this.FieldToShow, s)) continue;

                    int j = 0;
                    foreach (string data in s.Split(','))
                    {
                        this._dbEntities.InsertNewUser(j, allFields.ElementAt(j), data, fieldsVisiblity.ElementAt(j).Value);
                        j++;
                    }
                }

                //After every n insertions, save changes in DB (performance)
                if (i != 0 && i% nbrForInsertion == 0)
                {
                    this._dbEntities.SaveAllChanges();
                }

                i++;
            }

            //Save Changes in DB
            this._dbEntities.SaveAllChanges();
            this._mainFields = this._dbEntities.GetAllUsers();
            this._allUsers = this._mainFields;
            OnPropertyChanged("MainFields");

            //Clear
            this._dbEntities.Clear();
        }

        public ObservableCollection<ExportDataDTO> GetExportData()
        {
            //TODO Faire query
            return null;
        }

        public List<EventFieldUserSet> GetEventFieldUserByValues(List<string> values)
        {
            return this._dbEntities.GetEventFieldUserByValues(values);
        }

        public List<EventFieldUserSet> GetAllFieldsOfEvent(int idEvent)
        {
            return this._dbEntities.GetAllFieldsOfEvent(idEvent);
        }

        public ObservableCollection<EventFieldSet> GetEventFieldByEvent(int idEvent)
        {
            return this._dbEntities.GetEventFieldByEvent(idEvent);
        }

        public List<PrintBadgeSet> GetAllPrintBadge()
        {
            return this._dbEntities.GetAllPrintBadge();
        }
    }
}