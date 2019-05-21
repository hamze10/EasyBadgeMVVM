using EasyBadgeMVVM.Models;

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EasyBadgeMVVM.ViewModels
{
    public interface IBadgeVM
    {
        ObservableCollection<BadgeDTO> ListBadgeType { get; set; }
        ObservableCollection<BadgeDTO> ListBadgeTypeWithoutEmpty { get; set; }
        BadgeDTO SelectedBadge { get; set; }
        string SelectedTemplate { get; set; }
        int SelectedBadgeEvent { get; set; }
        List<string> GetAllFields();
        FieldSet GetFieldByName(string name);
        BadgeEventSet SaveOnBadgeEvent(string templateName);
        void SaveOnPosition(BadgeEventSet be, FieldSet f, double posX, double posY, string fontFamily, int fontSize);
        BadgeEventSet GetBadgeEvent();
        void RemoveRowsPosition(string templateName);
        List<PositionSet> GetPositions(int idBadge, int idEvent, string templateName);
        void RefreshListBadgeType();
        BadgeEventSet GetById(int idBadgeEvent);
        void UpdateDefaultPrint();
        BadgeEventSet GetDefaultBadge();
        void SaveOnPrintBadge(int idUser);
        void SaveOnBadge(string name, string typeBadge, int dimX, int dimY);
    }

    public class BadgeDTO
    {
        public int ID_BadgeEvent { get; set; }
        public int ID { get; set; }
        public string Name { get; set; } //Badge -> Name
        public double Height { get; set; } //Badge -> Dimension_X
        public double Width { get; set; } //Badge -> Dimension_Y
        public string Type { get; set; } //Badge -> TypeBadge
        public string Template { get; set; } //BadgeEvent -> Name
    }
}