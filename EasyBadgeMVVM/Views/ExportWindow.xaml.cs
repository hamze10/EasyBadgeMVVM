using CsvHelper;

using EasyBadgeMVVM.ViewModels;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EasyBadgeMVVM.Views
{
    /// <summary>
    /// Interaction logic for ExportWindow.xaml
    /// </summary>
    public partial class ExportWindow : Window, INotifyPropertyChanged
    {
        private BackgroundWorker bgw = new BackgroundWorker();
        private IExportVM _exportVM;

        private const string EXPORT_ALL = "exportAllUsers";
        private const string EXPORT_REGISTERED = "exportRegistered";
        private const string EXPORT_STATS = "exportStatistics";

        private const string MESSAGE_ALL = "Export all users done";
        private const string MESSAGE_REGISTERED = "Export all registered user done";
        private const string MESSAGE_STATS = "Export all stats done";

        private bool _buttonEnabled;

        public ExportWindow(int idEvent)
        {
            InitializeComponent();
            DataContext = this;

            this._exportVM = new ExportVM(idEvent);
            this._buttonEnabled = true;
            this.bgw.DoWork += myBgw_doWorker;
            this.bgw.RunWorkerCompleted += myBgw_RunWorkerCompleted;
        }

        public bool ButtonEnabled
        {
            get
            {
                return this._buttonEnabled;
            }

            set
            {
                this._buttonEnabled = value;
                OnPropertyChanged("ButtonEnabled");
            }
        }

        private void ExportAllUsers(object sender, RoutedEventArgs e)
        {
            this.ExportLoading.Visibility = Visibility.Visible;
            this.ButtonEnabled = false;

            var filePath = string.Empty;
            using (FolderBrowserDialog fd = new FolderBrowserDialog())
            {
                if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    filePath = fd.SelectedPath;
                }
            }

            if (filePath.Equals(string.Empty))
            {
                this.ExportLoading.Visibility = Visibility.Hidden;
                return;
            }

            
            string[] arg = new string[] { EXPORT_ALL, filePath };
            this.RunMyWorker(arg);
        }

        private void ExportRegisteredUsers(object sender, RoutedEventArgs e)
        {
            this.ExportLoading.Visibility = Visibility.Visible;
            this.ButtonEnabled = false;

            var filePath = string.Empty;
            using (FolderBrowserDialog fd = new FolderBrowserDialog())
            {
                if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    filePath = fd.SelectedPath;
                }
            }

            if (filePath.Equals(string.Empty))
            {
                this.ExportLoading.Visibility = Visibility.Hidden;
                return;
            }

            string[] arg = new string[] { EXPORT_REGISTERED, filePath };
            this.RunMyWorker(arg);
        }

        private void ExportStatistics(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("In progress ...");
            return;
            //string[] arg = new string[] { EXPORT_STATS };
            //this.RunMyWorker(arg);
        }

        private void ShowNotification(string message)
        {
            this.ExportLoading.Visibility = Visibility.Hidden;
            var messageQueue = this.ExportNotification.MessageQueue;
            Task.Factory.StartNew(() => messageQueue.Enqueue(message));
        }

        /*********************************************************************************************************************************************************************/
        /*********** BACKGROUNDWORKERS *************/
        /*********************************************************************************************************************************************************************/

        private void RunMyWorker(string[] args)
        {
            if (this.bgw.IsBusy) return;
            this.bgw.RunWorkerAsync(args);
        }

        private void myBgw_doWorker(object sender, DoWorkEventArgs e)
        {
            string[] arguments = e.Argument as string[];
    
            switch (arguments[0])
            {
                case EXPORT_ALL:
                    e.Result = new object[] { this._exportVM.GetAllUsersToExport(), arguments[1] };
                    break;
                case EXPORT_REGISTERED:
                    e.Result = new object[] { this._exportVM.GetAllRegisteredUserToExport(), arguments[1] };
                    break;
                case EXPORT_STATS:
                    e.Result = MESSAGE_STATS;
                    break;
            }  
        }

        private void myBgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ExportLoading.Visibility = Visibility.Hidden;
            bool isList = false;
            object[] myResult = e.Result as object[];
            if (myResult[0] is List<ExportDTO>)
            {
                isList = true;
                var data = (List<ExportDTO>)myResult[0];
                DateTime now = DateTime.Now;
                string nameFile = myResult[1] + "\\" +  "export" + now.ToString("ddMMyyyy") + "-" + now.ToString("HHmmsstt") + ".csv";
                int lastUser = -1;
                bool firsttime = true;
                ExportDTO lastDto = new ExportDTO();

                using (var writer = new StreamWriter(nameFile, false, Encoding.UTF8))
                using (var csvWriter = new CsvWriter(writer))
                {
                    csvWriter.Configuration.Delimiter = ",";

                    //HEADER (Event, barcode, onsite, all differents fields from csv, printdate, printby)
                    foreach (ExportDTO dto in data)
                    {
                        if (lastUser == -1)
                        {
                            lastUser = dto.EventFieldUserExport.UserID_User;
                            csvWriter.WriteField("Event");
                            csvWriter.WriteField("Barcode");
                            csvWriter.WriteField("Onsite");
                        }
                        if (lastUser != dto.EventFieldUserExport.UserID_User) break;
                        csvWriter.WriteField(dto.EventFieldUserExport.EventFieldSet.FieldSet.Name);
                    }
                    csvWriter.WriteField("PrintDate");
                    csvWriter.WriteField("PrintBy");
                    csvWriter.NextRecord();

                    //FIELDS
                    foreach (ExportDTO dto in data)
                    {
                        if (firsttime)
                        {
                            lastUser = dto.EventFieldUserExport.UserID_User;
                            csvWriter.WriteField(dto.EventFieldUserExport.EventFieldSet.EventSet.Name);
                            csvWriter.WriteField(dto.EventFieldUserExport.UserSet.Barcode);
                            csvWriter.WriteField(dto.EventFieldUserExport.UserSet.Onsite);
                            firsttime = false;
                        }

                        if (lastUser != dto.EventFieldUserExport.UserID_User)
                        {
                            csvWriter.WriteField(lastDto.PrintBadgeExport == null ? "//" : lastDto.PrintBadgeExport.PrintDate.ToString());
                            csvWriter.WriteField(lastDto.PrintBadgeExport == null ? "//" : lastDto.PrintBadgeExport.PrintBy);
                            csvWriter.NextRecord();
                            lastUser = dto.EventFieldUserExport.UserID_User;
                            csvWriter.WriteField(dto.EventFieldUserExport.EventFieldSet.EventSet.Name);
                            csvWriter.WriteField(dto.EventFieldUserExport.UserSet.Barcode);
                            csvWriter.WriteField(dto.EventFieldUserExport.UserSet.Onsite);
                        }

                        csvWriter.WriteField(dto.EventFieldUserExport.Value);
                        lastDto = dto;
                    }

                    csvWriter.WriteField(lastDto.PrintBadgeExport == null ? "//" : lastDto.PrintBadgeExport.PrintDate.ToString());
                    csvWriter.WriteField(lastDto.PrintBadgeExport == null ? "//" : lastDto.PrintBadgeExport.PrintBy);
                    writer.Flush();
                }
            }

            this.ButtonEnabled = true;
            this.ShowNotification(isList ? MESSAGE_ALL : (string) e.Result);
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
    }
}
