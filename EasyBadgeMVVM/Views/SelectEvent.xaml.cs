﻿using EasyBadgeMVVM.DataAccess;
using EasyBadgeMVVM.Models;
using EasyBadgeMVVM.ViewModels;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EasyBadgeMVVM.Views
{
    /// <summary>
    /// Interaction logic for SelectEvent.xaml
    /// </summary>
    public partial class SelectEvent : Window
    {
        private IEventVM _eventVm;

        public SelectEvent()
        {
            this._eventVm = new EventVM();
            this.DataContext = this._eventVm;
            InitializeComponent();
        }

        private void EventConfirm(object sender, RoutedEventArgs e)
        {
            Event selected = this._eventVm.GetEventById(this._eventVm.SelectedEvent);
            if (selected == null) return;
            ShowMainWindow(selected.ID_Event);
        }

        private void AddNewEvent(object sender, RoutedEventArgs e)
        {
            string name = this.AddEventName.Text;
            DateTime dateS = this.AddEventDate.SelectedDate.GetValueOrDefault();
            DateTime hour = this.AddEventTime.Time;

            if (dateS == null || hour == null || name == string.Empty)
            {
                MessageBox.Show("Please enter a valid date and/or name");
                return;
            }
            
            DateTime date = new DateTime(dateS.Year, dateS.Month, dateS.Day, hour.Hour, hour.Minute, hour.Second);
            Event ev = new Event();
            ev.DateOfEvent = date;
            ev.Name = name;
            Event inserted = this._eventVm.InsertEvent(ev);
            ShowMainWindow(inserted.ID_Event);
        }

        private void ShowMainWindow(int idEvent)
        {
            MainWindow mainWindow = new MainWindow(idEvent);
            mainWindow.Show();
            this.Close();
        }

    }
}