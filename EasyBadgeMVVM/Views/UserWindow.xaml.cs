using CsvHelper;

using EasyBadgeMVVM.Models;
using EasyBadgeMVVM.Views;
using EasyBadgeMVVM.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Printing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;

using SolidColorBrush = System.Windows.Media.SolidColorBrush;
using Label = System.Windows.Controls.Label;
using TextBox = System.Windows.Controls.TextBox;
using Button = System.Windows.Controls.Button;

namespace EasyBadgeMVVM.Views
{
    /// <summary>
    /// Interaction logic for NewUser.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private IUserVM _userVM;
        private IBadgeVM _badgeVM;
        private List<EventFieldUser> _currentUser;
        private bool _isNew;
        private int _idEvent;

        private const int FONTSIZELABEL = 16;
        private const double GRIDLENGTHHEIGHT = 60;
        private const int COLUMNPROPS = 2;

        private const string LABELFIELDNAME = "label";
        private const string SHOWNAME = "show";
        private const string NEWNAME = "name";
        private const string BUTTON_CONFIRM = "Confirm";
        private const string BUTTON_PRINTBADGE = "Print Badge";

        private SolidColorBrush[] brushes = new SolidColorBrush[2] { System.Windows.Media.Brushes.White, System.Windows.Media.Brushes.WhiteSmoke};

        public UserWindow(bool isNew, List<EventFieldUser> list, int idEvent)
        {
            this._isNew = isNew;
            this._idEvent = idEvent;
            this._userVM = new UserVM(idEvent);
            this._badgeVM = new BadgeVM(idEvent);
            this._currentUser = list;
            this.DataContext = this._userVM;
            InitializeComponent();
            if (this._isNew)
            {
                NewUser();
            }
            else
            {
                ShowUser();
            }
        }

        private void ShowUser()
        {
            for (int i = 1; i <= this._currentUser.Count; i++)
            {
                CreateFields(i);

                Grid grid2 = new Grid();
                grid2.Background = this.brushes[i % this.brushes.Length];
                grid2.SetValue(Grid.RowProperty, i);
                grid2.SetValue(Grid.ColumnProperty, COLUMNPROPS);

                Label label2 = new Label();
                label2.Name = SHOWNAME + i;
                label2.Content = this._currentUser[(i - 1)].Value;
                label2.VerticalAlignment = VerticalAlignment.Center;
                label2.FontSize = FONTSIZELABEL;
                grid2.Children.Add(label2);

                this.UserWindowGrid.Children.Add(grid2);

                RegisterName(SHOWNAME + i, label2);
            }

            CreateButton(BUTTON_PRINTBADGE, this._currentUser.Count + 1);
        }

        private void NewUser()
        {
            for (int i = 1; i <= this._currentUser.Count; i++)
            {
                CreateFields(i);

                Grid grid2 = new Grid();
                grid2.Background = this.brushes[i % this.brushes.Length];
                grid2.SetValue(Grid.RowProperty, i);
                grid2.SetValue(Grid.ColumnProperty, COLUMNPROPS);

                TextBox textBox = new TextBox();
                textBox.Name = NEWNAME + i;
                textBox.Width = 500;
                textBox.FontSize = FONTSIZELABEL;
                textBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                textBox.VerticalAlignment = VerticalAlignment.Center;
                grid2.Children.Add(textBox);

                this.UserWindowGrid.Children.Add(grid2);

                RegisterName(NEWNAME + i, textBox);
            }

            CreateButton(BUTTON_CONFIRM, this._currentUser.Count + 1);
        }

        private void CreateFields(int i)
        {
            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Height = new GridLength(GRIDLENGTHHEIGHT);
            this.UserWindowGrid.RowDefinitions.Add(rowDefinition);

            Grid grid = new Grid();
            grid.Background = this.brushes[i % this.brushes.Length];
            grid.SetValue(Grid.RowProperty, i);
            grid.SetValue(Grid.ColumnProperty, COLUMNPROPS - 1);

            Label label = new Label();
            label.Name = LABELFIELDNAME + i;
            label.Content = this._currentUser[(i - 1)].EventField.Field.Name + " : ";
            label.FontSize = FONTSIZELABEL;
            label.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(label);

            this.UserWindowGrid.Children.Add(grid);

            RegisterName(LABELFIELDNAME + i, label);
        }

        private void CreateButton(string content, int i)
        {
            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Height = new GridLength(GRIDLENGTHHEIGHT);
            this.UserWindowGrid.RowDefinitions.Add(rowDefinition);

            Grid grid = new Grid();
            grid.SetValue(Grid.RowProperty, i);
            grid.SetValue(Grid.ColumnProperty, COLUMNPROPS - 1);

            Button button = new Button();
            button.Content = content;
            button.Width = 150;
            button.Height = 45;
            button.FontSize = 22;
            button.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            button.Background = this.TitleBar.Background;
            button.BorderBrush = this.TitleBar.Background;

            RoutedEventHandler routedEventHandler = null;
            switch (content)
            {
                case BUTTON_CONFIRM:
                    routedEventHandler = Add_New;
                    break;
                case BUTTON_PRINTBADGE:
                    routedEventHandler = Print_Badge;
                    break;
                default:
                    break;
            }
            button.Click += new RoutedEventHandler(routedEventHandler);

            grid.Children.Add(button);

            this.UserWindowGrid.Children.Add(grid);
        }

        private void Add_New(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> toSend = new Dictionary<string, string>();

            for(int i = 1; i <= this._currentUser.Count; i++)
            {
                string key = ((Label)this.FindName(LABELFIELDNAME + i)).Content.ToString();
                string value = ((TextBox)this.FindName(NEWNAME + i)).Text.ToString();

                toSend.Add(key, value);
            }

            this._userVM.InsertNewUser(toSend);
            this.DialogResult = true;
            this.Close();
        }

        private void Print_Badge(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.PrintPreviewDialog pdi = new System.Windows.Forms.PrintPreviewDialog();
            pdi.ClientSize = new System.Drawing.Size(800, 600);
            pdi.DesktopLocation = new System.Drawing.Point(29, 29);
            pdi.Name = "PrintPreviewDialog1";

            PrintDocument printDocument = new PrintDocument();
            printDocument.DefaultPageSettings.PaperSize = new PaperSize("PVC", 650, 408);
            printDocument.PrintPage += new PrintPageEventHandler(document_PrintPage);
            pdi.Document = printDocument;

            if (pdi.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Console.WriteLine("good");
            }
        }

        private void document_PrintPage(object sender, PrintPageEventArgs e)
        {
            //Recupérer positions
            //Recupérer valeur de l'user selectionné

            //String text = "Henry"
            //Font printFont = new Font("FontFamily", "FontSize (a calculer)", FontStyle.Regular)
            //e.Graphics.DrawString(text, printfont, Brushes.Black, Position_X, Position_Y)

            List<Position> positions = this._badgeVM.GetPositions(4, this._idEvent);

            foreach (Position p in positions)
            {
                string text = this._currentUser.Find(efu => efu.EventField.Field.Name.Equals(p.Field.Name)).Value;
                Font printFont = new Font(p.FontFamily, (float) p.FontSize, System.Drawing.FontStyle.Regular);
                e.Graphics.DrawString(text, printFont, Brushes.Black, (float) p.Position_X, (float) p.Position_Y);
            }

            //--------------------------------------------------------------------------------------------//
            /*string text = "Company";
            string text2 = "LastName";
            string text3 = "Profile";
            Font printFont = new Font("Blackoak Std", 35, System.Drawing.FontStyle.Regular);
            Font printFont2 = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
            Font printFont3 = new Font("Segoe Script", 46, System.Drawing.FontStyle.Regular);
            e.Graphics.DrawString(text, printFont, Brushes.Black, 24, (float) 43.133885);
            e.Graphics.DrawString(text2, printFont2, Brushes.Black, (float) 389.133885, (float) 677.10634);
            e.Graphics.DrawString(text3, printFont3, Brushes.Black, (float) 110.133885, (float) 630.10634);*/
        }
    }
}
