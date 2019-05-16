
using EasyBadgeMVVM.Models;
using EasyBadgeMVVM.ViewModels;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows;
using System.Windows.Controls;

using SolidColorBrush = System.Windows.Media.SolidColorBrush;
using Label = System.Windows.Controls.Label;
using TextBox = System.Windows.Controls.TextBox;
using Button = System.Windows.Controls.Button;
using MaterialDesignThemes.Wpf;

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

        public UserWindow(bool isNew, List<EventFieldUserSet> list, int idEvent)
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
            // Color Window card if a filter/rule is defined
            string color = _userVM.DetermineColorForCard(_currentUser);
            if (color != null)
            {
                Card userCard = (Card) this.UserWindowGrid.FindName("UserWindowCard");
                userCard.Background = (SolidColorBrush)(new System.Windows.Media.BrushConverter().ConvertFrom(color));
            }
            CreateUserUI(SHOWNAME, true, BUTTON_PRINTBADGE);
        }

        private void NewUser()
        {
            CreateUserUI(NEWNAME, false, BUTTON_CONFIRM);
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
                textbox2.Width = 500;
                textbox2.Text = text ? this._currentUser[(i - 1)].Value : string.Empty; //DatePicker ??
                textbox2.VerticalAlignment = VerticalAlignment.Center;
                textbox2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                textbox2.FontSize = FONTSIZELABEL;
                grid2.Children.Add(textbox2);

                this.MyCard.Children.Add(grid2);

                RegisterName(name + i, textbox2);
            }

            CreateButton(buttonName, this._currentUser.Count + 1);
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

        private void CreateButton(string content, int i)
        {
            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Height = new GridLength(GRIDLENGTHHEIGHT);
            this.UserWindowGrid.RowDefinitions.Add(rowDefinition);

            Grid grid = new Grid();
            grid.SetValue(Grid.RowProperty, i);
            grid.SetValue(Grid.ColumnProperty, COLUMNPROPS);

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
