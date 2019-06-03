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

        List<EventFieldUserSet> GetEventFieldUserByValues(List<string> values);
        List<EventFieldUserSet> GetAllFieldsOfEvent(int idEvent);
        ObservableCollection<EventFieldSet> GetEventFieldByEvent(int idEvent);
        List<PrintBadgeSet> GetAllPrintBadge();

        void LoadFromImport(string content);
        void SetDeleteButton(bool value);
        ObservableCollection<EventFieldUserSet> RefreshMainsFields();
    }
}