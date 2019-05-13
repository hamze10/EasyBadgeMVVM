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
        private List<EventFieldUserSet> _currentUser;
        private bool _isNew;
        private int _idEvent;

        private const int FONTSIZELABEL = 16;
        private const double GRIDLENGTHHEIGHT = 60;
        private const int COLUMNPROPS = 1;
        private const double MM_PX = 3.779528;
        private const int MAXFONT = 17;

        private const string LABELFIELDNAME = "label";
        private const string SHOWNAME = "show";
        private const string NEWNAME = "name";
        private const string BUTTON_CONFIRM = "Confirm";
        private const string BUTTON_PRINTBADGE = "Print Badge";

        private SolidColorBrush[] brushes = new SolidColorBrush[2] { System.Windows.Media.Brushes.White, System.Windows.Media.Brushes.WhiteSmoke};

        public UserWindow(bool isNew, List<EventFieldUserSet> list, int idEvent, bool alreadyPrint = false)
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
                this.ProfileBorder.Visibility = Visibility.Hidden;
                this.MessageAddNewUser.Visibility = Visibility.Visible;
                NewUser();
            }
            else
            {
                this.MessageBadgePrinted.Visibility = alreadyPrint ? Visibility.Visible : Visibility.Hidden;
                this.MessageBadgeNotPrinted.Visibility = alreadyPrint ? Visibility.Hidden : Visibility.Visible;
                CheckIfColorExists();
                ShowUser();
            }
        }

        private void CheckIfColorExists()
        {
            List<string> PotentialColors = new List<string>();
            string profileName = string.Empty;
            foreach(var e in this._currentUser)
            {
                if (e.Value == string.Empty) continue;

                //CHECK COLOR
                string capitalizeValue = char.ToUpper(e.Value.Trim()[0]) + e.Value.Trim().Substring(1).ToLower();
                if (Color.FromName(capitalizeValue).IsKnownColor)
                {
                    PotentialColors.Add(e.Value);
                }else if (capitalizeValue.StartsWith("#"))
                {
                    PotentialColors.Add(e.Value);
                }

                //CHECK PROFILE
                string profile = e.EventFieldSet.FieldSet.Name.ToLower().Trim();
                List<string> differentProfiles = new List<string>(new string[] { "profile", "profiel", "profil"});
                if (differentProfiles.Contains(profile) && profileName == string.Empty)
                {
                    profileName = e.Value;
                }
            }

            if (PotentialColors.Count == 1)
            {
                System.Windows.Media.Color c = (System.Windows.Media.Color) System.Windows.Media.ColorConverter.ConvertFromString(PotentialColors[0]);
                this.ProfileBorder.Background = new SolidColorBrush(c);
                this.ProfileBorderName.Content = profileName;
            }
        }

        private void ShowUser()
        {
            CreateUserUI(SHOWNAME, true, BUTTON_PRINTBADGE);

            Button button = new Button();
            //button.Width = 150;
            button.Height = 50;
            button.FontSize = 20;
            button.Background = this.TitleBar.Background;
            button.VerticalAlignment = VerticalAlignment.Top;
            button.Margin = new Thickness(161, 10, 450, 0);
            button.Content = "🖨️ Print Badge";
            button.Click += new RoutedEventHandler(Print_Badge);

            this.ButtonsPlace.Children.Add(button);

            this.ButtonSave.Content = "Update";
            this.ButtonSave.Click += new RoutedEventHandler(Update_Profile);
        }

        private void NewUser()
        {
            CreateUserUI(NEWNAME, false, BUTTON_CONFIRM);

            this.ButtonSave.Click += new RoutedEventHandler(Add_New);
        }

        private void CreateUserUI(string name, bool text, string buttonName)
        {
            for (int i = 1; i <= this._currentUser.Count; i++)
            {
                CreateFields(i);

                Grid grid2 = new Grid();
                grid2.Background = this.brushes[i % this.brushes.Length];
                grid2.SetValue(Grid.RowProperty, i);
                grid2.SetValue(Grid.ColumnProperty, COLUMNPROPS);

                TextBox textbox2 = new TextBox();
                textbox2.Name = name + i;
                textbox2.Width = 350;
                textbox2.Text = text ? this._currentUser[(i - 1)].Value : string.Empty; //DatePicker ??
                textbox2.VerticalAlignment = VerticalAlignment.Center;
                textbox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                textbox2.FontSize = FONTSIZELABEL;
                grid2.Children.Add(textbox2);

                this.MyCard.Children.Add(grid2);

                RegisterName(name + i, textbox2);
            }
        }

        private void CreateFields(int i)
        {
            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Height = new GridLength(GRIDLENGTHHEIGHT);
            this.MyCard.RowDefinitions.Add(rowDefinition);

            Grid grid = new Grid();
            grid.Background = this.brushes[i % this.brushes.Length];
            grid.SetValue(Grid.RowProperty, i);
            grid.SetValue(Grid.ColumnProperty, COLUMNPROPS - 1);

            Label label = new Label();
            label.Name = LABELFIELDNAME + i;
            label.Content = this._currentUser[(i - 1)].EventFieldSet.FieldSet.Name + " : ";
            label.FontSize = FONTSIZELABEL;
            label.VerticalAlignment = VerticalAlignment.Center;
            label.FontWeight = FontWeights.Bold;
            grid.Children.Add(label);

            this.MyCard.Children.Add(grid);

            RegisterName(LABELFIELDNAME + i, label);
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

        private void Update_Profile(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> newUser = new Dictionary<string, string>();

            for (int i = 1; i <= this._currentUser.Count; i++)
            {
                string key = ((Label)this.FindName(LABELFIELDNAME + i)).Content.ToString();
                string value = ((TextBox)this.FindName(SHOWNAME + i)).Text.ToString();

                key = key.Trim().Split(':')[0].Trim();
                newUser.Add(key, value);
            }

            this._userVM.UpdateUser(this._currentUser[0].UserID_User, newUser);
            this.DialogResult = true;
            this.Close();

        }

        private void Print_Badge(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.PrintPreviewDialog pdi = new System.Windows.Forms.PrintPreviewDialog();
            pdi.ClientSize = new System.Drawing.Size(800, 600);
            pdi.DesktopLocation = new System.Drawing.Point(29, 29);
            pdi.Name = "PrintPreviewDialog1";

            BadgeEventSet defaultBadge = this._badgeVM.GetDefaultBadge();

            if (defaultBadge == null)
            {
                System.Windows.MessageBox.Show("Please configure a default print in badge settings");
                return;
            }

            PrintDocument printDocument = new PrintDocument();
            printDocument.DefaultPageSettings.PaperSize = new PaperSize("PVC", 
                                                                        Convert.ToInt32(defaultBadge.BadgeSet.Dimension_X * MM_PX), 
                                                                        Convert.ToInt32(defaultBadge.BadgeSet.Dimension_Y * MM_PX));

            printDocument.PrintPage += (sender2, e2) => document_PrintPage(sender2, e2, defaultBadge); // new PrintPageEventHandler(document_PrintPage);
            pdi.Document = printDocument;
            if (pdi.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //INSERT IN PRINTBADGE
                PrintBadgeSet pbadge = new PrintBadgeSet();
                pbadge.UserSet = this._currentUser[0].UserSet;
                pbadge.EventSet = this._userVM.GetEventById(this._idEvent);
                pbadge.PrintDate = DateTime.Now;
                pbadge.PrintBy = Environment.MachineName;
                this._badgeVM.SaveOnPrintBadge(pbadge);

                //TODO Comment : how to allow someone to add comment after print
            }
        }

        private void document_PrintPage(object sender, PrintPageEventArgs e, BadgeEventSet defaultBadge)
        {
            //retrieve positions
            //retrieve selected user value

            //String text = "Henry"
            //Font printFont = new Font("FontFamily", "FontSize (a calculer)", FontStyle.Regular)
            //e.Graphics.DrawString(text, printfont, Brushes.Black, Position_X, Position_Y)

            List<PositionSet> positions = this._badgeVM.GetPositions(defaultBadge.BadgeID_Badge, defaultBadge.EventID_Event, defaultBadge.Name);

            foreach (PositionSet p in positions)
            {
                string text = this._currentUser.Find(efu => efu.EventFieldSet.FieldSet.Name.Equals(p.FieldSet.Name)).Value;
                Font printFont = new Font(p.FontFamily, (float) p.FontSize, System.Drawing.FontStyle.Regular);
                float computedSize = 
                    GetNewFontSize(e.Graphics.MeasureString(text, printFont), 
                                   defaultBadge.BadgeSet.Dimension_Y * MM_PX, 
                                   printFont.Size, 
                                   e, 
                                   text, 
                                   printFont);
                printFont = new Font(p.FontFamily, computedSize, System.Drawing.FontStyle.Regular);
                e.Graphics.DrawString(text, printFont, Brushes.Black, (float) p.Position_X, (float) p.Position_Y);
            }
        }

        //CHECK ALGORITHM (it seems to work well oO)
        private float GetNewFontSize(SizeF measureString, double posY, double fontSize, PrintPageEventArgs ppevent, string text, Font printFont)
        {
            float newFontSize = (float)fontSize;
            float firstFontSize = newFontSize;

            while (measureString.Width >= posY)
            {
                newFontSize = newFontSize - 1;
                Font other = new Font(printFont.FontFamily, newFontSize, System.Drawing.FontStyle.Regular);
                measureString = ppevent.Graphics.MeasureString(text, other);
            }

            float result = firstFontSize - newFontSize;
            int minus = result == 0 ? 0 : result < 5 ? 1 : result >= 10 ? 3 : 2;

            newFontSize = newFontSize - minus;

            return newFontSize;
        }
    }
}
