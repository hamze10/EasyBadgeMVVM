using EasyBadgeMVVM.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EasyBadgeMVVM.ViewModels
{
    public interface IMainWindow
    {
        ObservableCollection<EventFieldUserSet> MainFields { get; }
        string MachineName { get; }
        string Search { get; set; }
        EventFieldUserSet SelectedUserEvent { get; set; }
        void LoadFromImport(string content);
        ObservableCollection<ExportDataDTO> GetExportData();
        void SetDeleteButton(bool value);
        List<EventFieldUserSet> GetEventFieldUserByValues(List<string> values);
        List<EventFieldUserSet> GetAllFieldsOfEvent(int idEvent);
        ObservableCollection<EventFieldSet> GetEventFieldByEvent(int idEvent);
        ObservableCollection<EventFieldUserSet> RefreshMainsFields();
        List<PrintBadgeSet> GetAllPrintBadge();
    }

    public class ExportDataDTO
    {
        public string Barcode { get; set; }

        public DateTime CreationDate { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Company { get; set; }

        public DateTime PrintBadge { get; set; }

        public string Profile { get; set; }

        public string ColorProfile { get; set; }

        public string EventName { get; set; }

        public DateTime DateOfEvent { get; set; }


    }
}