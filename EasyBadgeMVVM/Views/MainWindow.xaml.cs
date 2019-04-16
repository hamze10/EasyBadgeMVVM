using CsvHelper;

using EasyBadgeMVVM.Models;
using EasyBadgeMVVM.Views;
using EasyBadgeMVVM.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;
using Binding = System.Windows.Data.Binding;

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
        private int _nbFields = 0;
        private DataTable _dt = new DataTable();

        public MainWindow(int idEvent)
        {
            this._idEvent = idEvent;
            this._mainWindowImpl = new MainWindowImpl(idEvent);
            this.DataContext = this._mainWindowImpl;
            InitializeComponent();
            this.ToggleButtonMenu.IsChecked = true;

            this.bgw.DoWork += myBgw_doWorker;
            this.bgw.RunWorkerCompleted += myBgw_RunWorkerCompleted;
            CreateColumnsDataGrid();

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
            int indexSelected = this.DataGridUsers.SelectedItems.Cast<DataRowView>().Select(view => this._dt.Rows.IndexOf(view.Row)).FirstOrDefault();
            DataRow dr = this._dt.Rows[indexSelected];
            List<string> toSend = new List<string>();
            foreach(DataColumn dc in this._dt.Columns)
            {
                toSend.Add(dr[dc].ToString());
            }

            UserWindow userWindow = new UserWindow(false, this._mainWindowImpl.GetEventFieldUserByValues(toSend), this._idEvent);
            userWindow.ShowDialog();
        }

        private void NewUserInfo(object sender, RoutedEventArgs e)
        {
            UserWindow userWindow = new UserWindow(true, this._mainWindowImpl.GetAllFieldsOfEvent(this._idEvent), this._idEvent);
            bool onClose = userWindow.ShowDialog().Value;
            if (onClose)
            {
                this.CreateRowsDataGrid(this._mainWindowImpl.RefreshMainsFields());
            }
        }

        private void EnterSearch(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (this.DataGridUsers.Items.Count == 1)
                {
                    DataRow selected = ((DataRowView) this.DataGridUsers.Items.GetItemAt(0)).Row;
                    List<string> toSend = new List<string>();
                    foreach (DataColumn dc in this._dt.Columns)
                    {
                        toSend.Add(selected[dc].ToString());
                    }
                    UserWindow userWindow = new UserWindow(false, this._mainWindowImpl.GetEventFieldUserByValues(toSend), this._idEvent);
                    userWindow.ShowDialog();
                }
            }

            else if (e.Key == Key.Back || e.Key == Key.Delete)
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
            /*this.GridLoading.Visibility = Visibility.Visible;

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

            ObservableCollection<EventFieldUser> data = this._mainWindowImpl.MainFields;
            DateTime now = DateTime.Now;
            string nameFile = filePath + '\\' + "users" + now.ToString("ddMMyyyy") + "-" + now.ToString("HHmmsstt") + ".csv";

            using (var writer = new StreamWriter(nameFile, false, Encoding.UTF8))
            using (var csvWriter = new CsvWriter(writer))
            {
                csvWriter.Configuration.Delimiter = DELIMITER;
                csvWriter.Configuration.HasHeaderRecord = true;
                csvWriter.Configuration.AutoMap<EventFieldUser>();

                csvWriter.WriteHeader<EventFieldUser>();
                csvWriter.NextRecord();
                csvWriter.WriteRecords(data);

                writer.Flush();

            }

            string[] arg = new string[] { WORKER_EXPORT, nameFile};
            this.RunMyWorker(arg);*/
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
            if (e.Result.ToString().Contains(MESSAGE_IMPORT))
            {
                CreateColumnsDataGrid();
            }
            this.ShowNotification((string) e.Result);
        }

        private void CreateColumnsDataGrid()
        {
            HashSet<string> myFields = this._mainWindowImpl.FieldToShow;
            if (myFields == null)
            {
                this._mainWindowImpl.FieldToShow = new HashSet<string>();
                myFields = new HashSet<string>();
                var tt = this._mainWindowImpl.GetEventFieldByEvent(this._idEvent).Where(e => e.Visibility == true);
                foreach (var eu in tt)
                {
                    this._mainWindowImpl.FieldToShow.Add(eu.Field.Name);
                    myFields.Add(eu.Field.Name);
                }
            }

            this._nbFields = myFields.Count;

            DataTable dt = new DataTable();
            foreach(string field in myFields)
            {
                dt.Columns.Add(new DataColumn(field, typeof(string)));
            }

            var obj = new object[myFields.Count];
            int i = 0;
            int nbUser = 0;
            var lastObj = new object[myFields.Count];

            var list = this._mainWindowImpl.MainFields.Where(e => e.EventField.Visibility == true);
            foreach (var efu in list)
            {
                if (i == myFields.Count)
                {
                    dt.Rows.Add(obj);
                    obj = new object[myFields.Count];
                    i = 0;
                    nbUser++;
                }
                obj[i] = efu.Value;
                i++;
                lastObj = obj;
            }

            dt.Rows.Add(lastObj);
            this._mainWindowImpl.NbrUser = ++nbUser;
            this.DataGridUsers.ColumnWidth = new DataGridLength(10, DataGridLengthUnitType.Star);
            this.DataGridUsers.ItemsSource = dt.DefaultView;
            this._dt = dt;
        }

        private void CreateRowsDataGrid(ObservableCollection<EventFieldUser> list)
        {
            this._dt.Rows.Clear();
            var obj = new object[this._nbFields];
            int i = 0;
            int nbUser = 0;
            var lastObj = new object[this._nbFields];

            foreach (var efu in list)
            {
                if (!this._mainWindowImpl.FieldToShow.Contains(efu.EventField.Field.Name)) continue;
                if (i == this._nbFields)
                {
                    this._dt.Rows.Add(obj);
                    obj = new object[this._nbFields];
                    i = 0;
                    nbUser++;
                }
                obj[i] = efu.Value;
                i++;
                lastObj = obj;
            }

            this._dt.Rows.Add(lastObj);
            this._mainWindowImpl.NbrUser = ++nbUser;
            this.DataGridUsers.ItemsSource = this._dt.DefaultView;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string toSearch = this.TextSearch.Text;
            this._mainWindowImpl.Search = toSearch;
            ObservableCollection<EventFieldUser> test = this._mainWindowImpl.DoSearch();
            CreateRowsDataGrid(test);
        }

        private void SettingsButton(object sender, RoutedEventArgs e)
        {
            ConfigBadge configBadge = new ConfigBadge(this._idEvent);
            configBadge.Show();

            /*MainSettings mainSettings = new MainSettings();
            mainSettings.Show();*/
        }
    }
}
