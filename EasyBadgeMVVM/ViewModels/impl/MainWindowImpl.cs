﻿using EasyBadgeMVVM.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.ViewModels
{
    public class MainWindowImpl : INotifyPropertyChanged, IUserEventVM
    {

        /*********************************************************************************************************************************************************************/
        /****** CONSTRUCTOR ******/
        /*********************************************************************************************************************************************************************/

        private int _idEvent;
        private IDbEntities _dbEntities;
        public string EventTitle { get; set; }

        public MainWindowImpl(int idEvent)
        {
            this._idEvent = idEvent;
            this._dbEntities = new DbEntities();
            this._dbEntities.SetIdEvent(idEvent);

            Event thisEvent = this._dbEntities.GetEventById(idEvent);
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

        private UserEventDTO _selectedUserEvent;
        private ObservableCollection<UserEventDTO> _mainFields;
        private ObservableCollection<UserEventDTO> _allUsers;
        public int NbrUser { get; set; }

        //Check if delete button is pressed in search bar
        private bool _isDelete = false;

        public UserEventDTO SelectedUserEvent
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

        public ObservableCollection<UserEventDTO> MainFields
        {
            get
            {
                if (this._mainFields == null)
                {
                    //this._mainFields = this._dbEntities.GetAllUsers();
                    this._mainFields = new ObservableCollection<UserEventDTO>();
                    this._allUsers = this._mainFields;
                    NbrUser = this._mainFields.Count;
                }

                return this._mainFields;
            }
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

        private void DoSearch()
        {
            var toSearch = this._search.ToLower();

            if (toSearch.Length == 0 || toSearch.Trim() == "")
            {
                //this._mainFields = this._dbEntities.GetAllUsers();
                this._mainFields = new ObservableCollection<UserEventDTO>();
                this.NbrUser = this._mainFields.Count;
                return;
            }

            string[] splitted = toSearch.Split(' ');
            toSearch = splitted.Length <= 1 ? splitted[0] : splitted[splitted.Length - 1];

            Func<UserEventDTO, bool> predicate = u => 
                            u.Barcode != null && u.Barcode.ToLower().Contains(toSearch) ||
                            u.Company != null && u.Company.ToLower().Contains(toSearch) ||
                            u.FirstName != null && u.FirstName.ToLower().Contains(toSearch) ||
                            u.LastName != null && u.LastName.ToLower().Contains(toSearch);

            if (this._isDelete && toSearch.Trim() != "")
            {
                this._isDelete = false;
                this._mainFields = new ObservableCollection<UserEventDTO>(this._allUsers.AsParallel().Where(predicate));
            }
            else
            {
                this._mainFields = new ObservableCollection<UserEventDTO>(this._mainFields.AsParallel().Where(predicate));
            }

            this.NbrUser = this._mainFields.Count;
 
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

            int indexOfLastName = -1;
            int indexOfFirstName = -1;
            int indexOfCompany = -1;

            foreach(string s in splitted)
            {
                if (s.Equals(string.Empty)) continue;

                //Fields
                if (i == 0)
                {
                    //1 Recupérer tous les champs dans la DB
                    //2 Ouvrir fenetre FieldMatching(list field_import, list field_db) 

                    int k = 0;
                    foreach(string field in s.Split(','))
                    {
                        bool fieldTrimUsed = true;

                        string nameTrim = Util.TrimWord(field.ToLower());
                        string translated = Util.TranslateField(nameTrim) ?? nameTrim;

                        if (translated.Equals("firstname"))
                        {
                            indexOfFirstName = k;
                        }

                        if (translated.Equals("lastname"))
                        {
                            indexOfLastName = k;
                        }

                        if (translated.Equals("company"))
                        {
                            indexOfCompany = k;
                        }

                        fieldTrimUsed = this._dbEntities.InsertNewField(translated, field);
                        allFields.Add(fieldTrimUsed == true ? translated : field);
                        k++;
                    }
                }

                //Data
                else
                {
                    bool exists = false;
                    if (indexOfLastName != -1 && indexOfFirstName != -1 && indexOfCompany != -1)
                    {
                        string[] datas = s.Split(',');
                        exists = this._dbEntities.CheckIfAlreadyExists(datas[indexOfLastName], datas[indexOfFirstName], datas[indexOfCompany]);
                    }

                    if (exists == true) continue;

                    int j = 0;
                    foreach (string data in s.Split(','))
                    {
                        this._dbEntities.InsertNewUser(j, allFields.ElementAt(j), data);
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
            this.NbrUser = this._mainFields.Count;
            OnPropertyChanged("MainFields");
            OnPropertyChanged("NbrUser");

            //Clear
            this._dbEntities.Clear();
        }

        public ObservableCollection<ExportDataDTO> GetExportData()
        {
            //TODO Faire query
            return null;
        }

        /*public List<UserEvent> GetUserEventByDTO(UserEventDTO dto)
        {
            return this._dbEntities.GetUserEventByDTO(dto);
        }

        public List<UserEvent> GetAllFieldsOfEvent(int idEvent)
        {
            return this._dbEntities.GetAllFieldsOfEvent(idEvent);
        }*/
    }
}