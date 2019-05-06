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
        private const string MESSAGE_REFRESH = "Refresh done !";

        private const string WORKER_IMPORT = "import";
        private const string WORKER_EXPORT = "export";
        private const string WORKER_SYNC = "sync";
        private const string WORKER_REFRESH = "refresh";

        private readonly string[] EXTENSIONS = new string[] { ".csv" };

        private const string PRINT_BADGE = "Print Badge";
        private const string EMPTY_COLUMN = "//";

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
                this.ShowNotification("User added");
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

        private void RefreshList(object sender, RoutedEventArgs e)
        {
            this.GridLoading.Visibility = Visibility.Visible;
            string[] arg = new string[] { WORKER_REFRESH };
            this.RunMyWorker(arg);
        }

        private void ShowNotification(string message)
        {
            this.GridLoading.Visibility = Visibility.Hidden;
            var messageQueue = this.SnackbarOne.MessageQueue;
            Task.Factory.StartNew(() => messageQueue.Enqueue(message));
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string toSearch = this.TextSearch.Text;
            this._mainWindowImpl.Search = toSearch;
            ObservableCollection<EventFieldUserSet> test = this._mainWindowImpl.DoSearch();
            CreateRowsDataGrid(test);
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
                case WORKER_REFRESH:
                    this._mainWindowImpl.RefreshMainsFields();
                    e.Result = MESSAGE_REFRESH;
                    break;
                default:
                    break;
            }
        }

        private void myBgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result.ToString().Contains(MESSAGE_IMPORT) || e.Result.ToString().Contains(MESSAGE_REFRESH))
            {
                CreateColumnsDataGrid();
            }
            this.ShowNotification((string) e.Result);
        }


        /*********************************************************************************************************************************************************************/
        /*********** FILL USERS TABLE *************/
        /*********************************************************************************************************************************************************************/

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
                    this._mainWindowImpl.FieldToShow.Add(eu.FieldSet.Name);
                    myFields.Add(eu.FieldSet.Name);
                }
            }

            this._nbFields = myFields.Count;

            DataTable dt = new DataTable();
            foreach(string field in myFields)
            {
                dt.Columns.Add(new DataColumn(field, typeof(string)));
            }

            if (myFields.Count > 0) dt.Columns.Add(new DataColumn(PRINT_BADGE, typeof(string)));

            var size = myFields.Count + 1;
            var obj = new object[size];
            int i = 0;
            int nbUser = 0;
            var lastObj = new object[size];
            EventFieldUserSet lastUser = null;
            var allPrintBadge = this._mainWindowImpl.GetAllPrintBadge();
            var list = this._mainWindowImpl.MainFields.Where(e => e.EventFieldSet.Visibility == true);

            foreach (var efu in list)
            {
                if (i == myFields.Count)
                {
                    //Get printbadge
                    var datePrint = allPrintBadge.Where(p => p.UserID_User == lastUser.UserID_User).OrderByDescending(p => p.PrintDate).FirstOrDefault();
                    obj[i] = datePrint != null ? datePrint.PrintDate.ToString() : EMPTY_COLUMN;
                    dt.Rows.Add(obj);
                    obj = new object[size];
                    i = 0;
                    nbUser++;
                }
                obj[i] = efu.Value;
                i++;
                lastObj = obj;
                lastUser = efu;
            }

            if (lastObj[0] != null)
            {
                var datePrint2 = allPrintBadge.Where(p => p.UserID_User == lastUser.UserID_User).OrderByDescending(p => p.PrintDate).FirstOrDefault();
                obj[i] = datePrint2 != null ? datePrint2.PrintDate.ToString() : EMPTY_COLUMN;
                dt.Rows.Add(lastObj);
            }

            this._mainWindowImpl.NbrUser = lastObj[0] != null ? ++nbUser : 0;
            this.DataGridUsers.ColumnWidth = new DataGridLength(10, DataGridLengthUnitType.Star);
            this.DataGridUsers.ItemsSource = dt.DefaultView;
            this._dt = dt;
        }

        private void CreateRowsDataGrid(ObservableCollection<EventFieldUserSet> list)
        {
            this._dt.Rows.Clear();
            var size = this._nbFields + 1;
            var obj = new object[size];
            int i = 0;
            int nbUser = 0;
            var lastObj = new object[size];
            var allPrintBadge = this._mainWindowImpl.GetAllPrintBadge();
            EventFieldUserSet lastUser = null;

            foreach (var efu in list)
            {
                if (!this._mainWindowImpl.FieldToShow.Contains(efu.EventFieldSet.FieldSet.Name)) continue;
                if (i == this._nbFields)
                {
                    var datePrint = allPrintBadge.Where(p => p.UserID_User == lastUser.UserID_User).OrderByDescending(p => p.PrintDate).FirstOrDefault();
                    obj[i] = datePrint != null ? datePrint.PrintDate.ToString() : EMPTY_COLUMN;
                    this._dt.Rows.Add(obj);
                    obj = new object[size];
                    i = 0;
                    nbUser++;
                }
                obj[i] = efu.Value;
                i++;
                lastObj = obj;
                lastUser = efu;
            }

            if (lastObj[0] != null)
            {
                var datePrint2 = allPrintBadge.Where(p => p.UserID_User == lastUser.UserID_User).OrderByDescending(p => p.PrintDate).FirstOrDefault();
                obj[i] = datePrint2 != null ? datePrint2.PrintDate.ToString() : EMPTY_COLUMN;
                this._dt.Rows.Add(lastObj);
            }
            this._mainWindowImpl.NbrUser = lastObj[0] != null ? ++nbUser : 0;
            this.DataGridUsers.ItemsSource = this._dt.DefaultView;
        }

        private void SettingsButton(object sender, RoutedEventArgs e)
        {
            /*ConfigBadge configBadge = new ConfigBadge(this._idEvent);
            configBadge.Show();*/
            FiltersWindow filtersWindow = new FiltersWindow(this._idEvent);
            filtersWindow.Show();

            /*MainSettings mainSettings = new MainSettings();
            mainSettings.Show();*/
        }

    }
}
