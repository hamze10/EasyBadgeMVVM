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
    public class BadgeVM : INotifyPropertyChanged, IBadgeVM
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

        private int _idEvent;
        private IDbEntities _dbEntities;
        private ObservableCollection<BadgeDTO> _listBadgeType;
        private ObservableCollection<BadgeDTO> _listBadgeTypeWithoutEmpty;
        public BadgeDTO SelectedBadge { get; set; }
        public int SelectedBadgeEvent { get; set; }
        private string _selectedTemplate;
        private const double MM_PX = 3.779528;

        public BadgeVM(int idEvent)
        {
            this._dbEntities = new DbEntities();
            this._idEvent = idEvent;
            this._dbEntities.SetIdEvent(this._idEvent);
        }

        public string SelectedTemplate
        {
            get
            {
                return this._selectedTemplate;
            }

            set
            {
                this._selectedTemplate = value;
                OnPropertyChanged("SelectedTemplate");
            }
        }

        public ObservableCollection<BadgeDTO> ListBadgeType
        {
            get
            {
                if (this._listBadgeType == null)
                {
                    this._listBadgeType = LoadBadgesType();
                }

                return this._listBadgeType;
            }

            set
            {
                this._listBadgeType = value;
                OnPropertyChanged("ListBadgeType");
            }
        }

        public ObservableCollection<BadgeDTO> ListBadgeTypeWithoutEmpty
        {
            get
            {
                if (this._listBadgeTypeWithoutEmpty == null)
                {
                    this._listBadgeTypeWithoutEmpty = new ObservableCollection<BadgeDTO>(this._listBadgeType.Where(l => l.ID_BadgeEvent != -1));
                }

                return this._listBadgeTypeWithoutEmpty;
            }

            set
            {
                this._listBadgeTypeWithoutEmpty = value;
                OnPropertyChanged("ListBadgeTypeWithoutEmpty");
            }
        }

        private ObservableCollection<BadgeDTO> LoadBadgesType()
        {
            ObservableCollection<BadgeSet> listBadge = this._dbEntities.GetAllBadges();
            ObservableCollection<BadgeEventSet> listBadgeEvent = this._dbEntities.GetAllBadgeEvent();
            List<BadgeDTO> badgeDTOs = new List<BadgeDTO>();

            foreach (BadgeSet b in listBadge)
            {
                var myBadgeEvent = listBadgeEvent.Where(bEv => bEv.BadgeID_Badge == b.ID_Badge);

                BadgeDTO bdto = new BadgeDTO();
                bdto.ID_BadgeEvent = -1;
                bdto.ID = b.ID_Badge;
                bdto.Name = b.Name;
                double multi = GiveMultiplicator(b.Dimension_X * MM_PX, b.Dimension_Y * MM_PX);
                bdto.Width = (b.Dimension_X * MM_PX) * multi;
                bdto.Height = (b.Dimension_Y * MM_PX) * multi;
                bdto.Type = b.TypeBadge;
                bdto.Template = string.Empty;
                badgeDTOs.Add(bdto);

                foreach (BadgeEventSet be in myBadgeEvent)
                {
                    BadgeDTO bdto2 = new BadgeDTO();
                    bdto2.ID_BadgeEvent = be.ID_BadgeEvent;
                    bdto2.ID = b.ID_Badge;
                    bdto2.Name = b.Name;
                    double multi2 = GiveMultiplicator(b.Dimension_X * MM_PX, b.Dimension_Y * MM_PX);
                    bdto2.Width = (b.Dimension_X * MM_PX) * multi2;
                    bdto2.Height = (b.Dimension_Y * MM_PX) * multi2;
                    bdto2.Type = b.TypeBadge;
                    bdto2.Template = be.Name;
                    badgeDTOs.Add(bdto2);
                }
            }

            return new ObservableCollection<BadgeDTO>(badgeDTOs);
        }

        public void RefreshListBadgeType()
        {
            this.ListBadgeType = LoadBadgesType();
            this.ListBadgeTypeWithoutEmpty = new ObservableCollection<BadgeDTO>(this.ListBadgeType.Where(l => l.ID_BadgeEvent != -1));
        }

        private double GiveMultiplicator(double dimX, double dimY)
        {
            return 1;
            //return dimX + dimY < 650 ? 2 : dimX + dimY > 1000 ? 1 : 1.25;
        }

        public List<string> GetAllFields()
        {
            return this._dbEntities.GetAllFieldsOfEvent(this._idEvent).Select(f => f.EventFieldSet.FieldSet.Name).ToList();
        }

        public FieldSet GetFieldByName(string name)
        {
            return this._dbEntities.GetAllFieldsOfEvent(this._idEvent).FirstOrDefault(f => f.EventFieldSet.FieldSet.Name.Equals(name)).EventFieldSet.FieldSet;
        }

        public BadgeEventSet GetBadgeEvent()
        {
            return this._dbEntities.GetBadgeEvent(this.SelectedBadge.ID, this._idEvent);
        }

        public BadgeEventSet GetById(int idBadgeEvent)
        {
            return this._dbEntities.GetBadgeEventById(idBadgeEvent);
        }

        public List<PositionSet> GetPositions(int idBadge, int idEvent, string templateName)
        {
            return this._dbEntities.GetPositions(idBadge, idEvent, templateName);
        }

        public BadgeEventSet GetDefaultBadge()
        {
            return this._dbEntities.GetDefaultBadgeEvent();
        }

        public BadgeEventSet SaveOnBadgeEvent(string templateName)
        {
            return this._dbEntities.InsertInBadgeEvent(this.SelectedBadge.ID, this._idEvent, templateName);
        }

        public void SaveOnPosition(BadgeEventSet be, FieldSet f, double posX, double posY, string fontFamily, int fontSize)
        {
            this._dbEntities.InsertInPosition(be, f, posX, posY, fontFamily, fontSize);
        }

        public void SaveOnPrintBadge(int idUser)
        {
            PrintBadgeSet pb = new PrintBadgeSet();
            pb.UserID_User = idUser;
            pb.EventID_Event = this._idEvent;
            pb.PrintDate = DateTime.Now;
            pb.PrintBy = Environment.MachineName;

            this._dbEntities.InsertInPrintBadge(pb);
        }

        public void UpdateDefaultPrint()
        {
            if (this.SelectedBadgeEvent == 0) return;
            this._dbEntities.UpdateDefaultPrint(this.SelectedBadgeEvent);
        }

        public void RemoveRowsPosition(string templateName)
        {
            this._dbEntities.DeleteRowPosition(this.SelectedBadge.ID, this._idEvent, templateName);
        }
    }
}
