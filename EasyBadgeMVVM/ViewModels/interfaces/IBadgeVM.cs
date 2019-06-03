using EasyBadgeMVVM.Models;

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EasyBadgeMVVM.ViewModels
{
    public interface IBadgeVM
    {
        /// <summary>
        /// Get all badge (with template if possible) used to display in configbadge.xaml
        /// </summary>
        ObservableCollection<BadgeDTO> ListBadgeType { get; set; }

        /// <summary>
        /// Get all template (used for default print in configbadge.xaml)
        /// </summary>
        ObservableCollection<BadgeDTO> ListBadgeTypeWithoutEmpty { get; set; }

        BadgeDTO SelectedBadge { get; set; }
        string SelectedTemplate { get; set; }
        int SelectedBadgeEvent { get; set; }

        List<string> GetAllFields();
        FieldSet GetFieldByName(string name);
        BadgeEventSet GetBadgeEvent();
        List<PositionSet> GetPositions(int idBadge, int idEvent, string templateName);
        BadgeEventSet GetById(int idBadgeEvent);
        BadgeEventSet GetDefaultBadge();

        BadgeEventSet SaveOnBadgeEvent(string templateName, System.Windows.Media.Imaging.BitmapSource imageSrc);
        void SaveOnPosition(BadgeEventSet be, FieldSet f, double posX, double posY, string fontFamily, int fontSize, double layoutTransform);
        void SaveOnPrintBadge(int idUser);
        void SaveOnBadge(string name, string typeBadge, int dimX, int dimY);

        /// <summary>
        /// Remove all fields in the chosen template
        /// </summary>
        /// <param name="templateName"></param>
        void RemoveRowsPosition(string templateName);
        
        void RefreshListBadgeType();
        
        void UpdateDefaultPrint();
        

    }

    /// <summary>
    /// DTO used for display different template in ConfigBadge.XAML
    /// </summary>
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