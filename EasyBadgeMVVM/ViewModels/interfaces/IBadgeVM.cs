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
        Field GetFieldByName(string name);
        BadgeEvent SaveOnBadgeEvent(string templateName);
        void SaveOnPosition(BadgeEvent be, Field f, double posX, double posY, string fontFamily, int fontSize);
        BadgeEvent GetBadgeEvent();
        void RemoveRowsPosition(string templateName);
        List<Position> GetPositions(int idBadge, int idEvent, string templateName);
        void RefreshListBadgeType();
        BadgeEvent GetById(int idBadgeEvent);
        void UpdateDefaultPrint();
        BadgeEvent GetDefaultBadge();
        void SaveOnPrintBadge(PrintBadge pb);
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