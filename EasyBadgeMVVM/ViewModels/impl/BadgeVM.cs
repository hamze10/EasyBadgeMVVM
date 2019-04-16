using EasyBadgeMVVM.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.ViewModels
{ 
    public class BadgeVM : IBadgeVM
    {
        private int _idEvent;
        private IDbEntities _dbEntities;
        private ObservableCollection<BadgeDTO> _listBadgeType;
        public BadgeDTO SelectedBadge { get; set; }
        private const double MM_PX = 3.779528;

        public BadgeVM(int idEvent)
        {
            this._dbEntities = new DbEntities();
            this._idEvent = idEvent;
            this._dbEntities.SetIdEvent(this._idEvent);
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
        }

        private ObservableCollection<BadgeDTO> LoadBadgesType()
        {
            ObservableCollection<Badge> listBadge = this._dbEntities.GetAllBadges();
            HashSet<BadgeDTO> badgeDTOs = new HashSet<BadgeDTO>();

            foreach (Badge b in listBadge)
            {
                BadgeDTO bdto = new BadgeDTO();
                bdto.ID = b.ID_Badge;
                bdto.Name = b.Name;

                double multi = GiveMultiplicator(b.Dimension_X * MM_PX, b.Dimension_Y * MM_PX);
                bdto.Width = (b.Dimension_X * MM_PX)*multi;
                bdto.Height = (b.Dimension_Y * MM_PX)*multi;
                bdto.Type = b.TypeBadge;

                badgeDTOs.Add(bdto);
            }

            return new ObservableCollection<BadgeDTO>(badgeDTOs);
        }

        private double GiveMultiplicator(double dimX, double dimY)
        {
            return dimX + dimY < 650 ? 2 : dimX + dimY > 1000 ? 1 : 1.25;
        }

        public List<string> GetAllFields()
        {
            return this._dbEntities.GetAllFieldsOfEvent(this._idEvent).Select(f => f.EventField.Field.Name).ToList();
        }

        public Field GetFieldByName(string name)
        {
            return this._dbEntities.GetAllFieldsOfEvent(this._idEvent).FirstOrDefault(f => f.EventField.Field.Name.Equals(name)).EventField.Field;
        }

        public BadgeEvent GetBadgeEvent()
        {
            return this._dbEntities.GetBadgeEvent(this.SelectedBadge.ID, this._idEvent);
        }

        public List<Position> GetPositions(int idBadge, int idEvent)
        {
            return this._dbEntities.GetPositions(idBadge, idEvent);
        }

        public BadgeEvent SaveOnBadgeEvent()
        {
            return this._dbEntities.InsertInBadgeEvent(this.SelectedBadge.ID, this._idEvent);
        }

        public void SaveOnPosition(BadgeEvent be, Field f, double posX, double posY, string fontFamily, int fontSize)
        {
            this._dbEntities.InsertInPosition(be, f, posX, posY, fontFamily, fontSize);
        }

        public void RemoveRowsPosition()
        {
            this._dbEntities.DeleteRowPosition(this.SelectedBadge.ID, this._idEvent);
        }
    }
}
