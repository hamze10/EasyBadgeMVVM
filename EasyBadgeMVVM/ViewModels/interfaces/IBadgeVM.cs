using EasyBadgeMVVM.Models;

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EasyBadgeMVVM.ViewModels
{
    public interface IBadgeVM
    {
        ObservableCollection<BadgeDTO> ListBadgeType { get; }
        BadgeDTO SelectedBadge { get; set; }
        List<string> GetAllFields();
        Field GetFieldByName(string name);
        void SaveOnBadgeEvent();
    }

    public class BadgeDTO
    {
        public int ID { get; set; }
        public string Name { get; set; } //Badge -> Name
        public double Height { get; set; } //Badge -> Dimension_X
        public double Width { get; set; } //Badge -> Dimension_Y
        public string Type { get; set; } //Badge -> TypeBadge
    }
}