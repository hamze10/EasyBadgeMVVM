using CsvHelper;

using EasyBadgeMVVM.Views;
using EasyBadgeMVVM.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;

namespace EasyBadgeMVVM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private MainWindowImpl _mainWindowImpl;
        private BackgroundWorker bgw = new BackgroundWorker();

        private bool toggleState = false; //false no click, true click

        private const string DELIMITER = ",";

        private const string MESSAGE_IMPORT = "Selected file : ";
        private const string MESSAGE_EXPORT = "File created : ";
        private const string MESSAGE_SYNC = "Sync done !";

        private const string WORKER_IMPORT = "import";
        private const string WORKER_EXPORT = "export";
        private const string WORKER_SYNC = "sync";

        private readonly string[] EXTENSIONS = new string[] { ".csv" };

        private int _idEvent;

        public MainWindow(int idEvent)
        {
            this._idEvent = idEvent;
            this._mainWindowImpl = new MainWindowImpl(idEvent);
            this.DataContext = this._mainWindowImpl;
            InitializeComponent();
            this.ToggleButtonMenu.IsChecked = true;

            this.bgw.DoWork += myBgw_doWorker;
            this.bgw.RunWorkerCompleted += myBgw_RunWorkerCompleted;

        }

        private void ToggleClick(object sender, RoutedEventArgs e)
        {
            this.GridOptionsList.Visibility = toggleState ? Visibility.Visible : Visibility.Hidden;
            int colProp = toggleState ? 1 : 0;
            this.GridDataGrid.SetValue(Grid.ColumnProperty, colProp);
            this.GridDataGrid.SetValue(Grid.ColumnSpanProperty, 2);
            toggleState = !toggleState;
        }

        private void ShowUserInfo(object sender, RoutedEventArgs e)
        {
            UserEventDTO dto = this._mainWindowImpl.SelectedUserEvent;
            if (dto == null || dto.Barcode.Equals(string.Empty)) return;
            /*UserWindow userWindow = new UserWindow(false, this._userEventVM.GetUserEventByDTO(dto), this._idEvent);
            userWindow.ShowDialog();*/
        }

        private void NewUserInfo(object sender, RoutedEventArgs e)
        {
            /*UserWindow userWindow = new UserWindow(true, this._userEventVM.GetAllFieldsOfEvent(this._idEvent), this._idEvent);
            userWindow.ShowDialog();*/
        }

        private void EnterSearch(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (this.DataGridUsers.Items.Count == 1)
                {
                    UserEventDTO dto = (UserEventDTO) this.DataGridUsers.Items.GetItemAt(0);
                    /*UserWindow userWindow = new UserWindow(false, this._userEventVM.GetUserEventByDTO(dto), this._idEvent);
                    userWindow.ShowDialog();*/
                }
            }

            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                this._mainWindowImpl.SetDeleteButton(true);
            }

        }

        private void ImportUsers(object sender, RoutedEventArgs e)
        {
            var filePath = string.Empty;
            var fileContent = string.Empty;
            IList<string> acceptedExtension = new List<string>(EXTENSIONS);
            this.GridLoading.Visibility = Visibility.Visible;

            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.Filter = "csv files (*.csv)|*.csv|All Files (*.*)|*.*";
                if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    filePath = fd.FileName;
                    if (!acceptedExtension.Contains(Path.GetExtension(filePath)))
                    {
                        this.GridLoading.Visibility = Visibility.Hidden;
                        this.ShowNotification("File not accepted");
                        return;
                    }

                    var filestream = fd.OpenFile();
                    using (StreamReader reader = new StreamReader(filestream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }

            if (filePath.Equals(string.Empty))
            {
                this.GridLoading.Visibility = Visibility.Hidden;
                return;
            }

            string[] arg = new string[] { WORKER_IMPORT, fileContent, filePath };
            this.RunMyWorker(arg);
        }

        private void ExportUsers(object sender, RoutedEventArgs e)
        {
            this.GridLoading.Visibility = Visibility.Visible;

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
                this.GridLoading.Visibility = Visibility.Hidden;
                return;
            }

            ObservableCollection<UserEventDTO> data = this._mainWindowImpl.MainFields;
            DateTime now = DateTime.Now;
            string nameFile = filePath + '\\' + "users" + now.ToString("ddMMyyyy") + "-" + now.ToString("HHmmsstt") + ".csv";

            using (var writer = new StreamWriter(nameFile, false, Encoding.UTF8))
            using (var csvWriter = new CsvWriter(writer))
            {
                csvWriter.Configuration.Delimiter = DELIMITER;
                csvWriter.Configuration.HasHeaderRecord = true;
                csvWriter.Configuration.AutoMap<UserEventDTO>();

                csvWriter.WriteHeader<UserEventDTO>();
                csvWriter.NextRecord();
                csvWriter.WriteRecords(data);

                writer.Flush();

            }

            string[] arg = new string[] { WORKER_EXPORT, nameFile};
            this.RunMyWorker(arg);
        }

        private void SyncUsers(object sender, RoutedEventArgs e)
        {
            this.GridLoading.Visibility = Visibility.Visible;

            string[] arg = new string[] { WORKER_SYNC };
            this.RunMyWorker(arg);
        }

        private void ShowNotification(string message)
        {
            this.GridLoading.Visibility = Visibility.Hidden;
            var messageQueue = this.SnackbarOne.MessageQueue;
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
                case WORKER_IMPORT:
                    this._mainWindowImpl.LoadFromImport(arguments[1]);
                    e.Result = MESSAGE_IMPORT + arguments[2];
                    break;
                case WORKER_EXPORT:
                    e.Result = MESSAGE_EXPORT + arguments[1];
                    break;
                case WORKER_SYNC:
                    e.Result = MESSAGE_SYNC;
                    break;
                default:
                    break;
            }
        }

        private void myBgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ShowNotification((string) e.Result);
        }
    }
}
