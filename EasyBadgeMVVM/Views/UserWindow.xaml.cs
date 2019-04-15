using CsvHelper;

using EasyBadgeMVVM.Models;
using EasyBadgeMVVM.Views;
using EasyBadgeMVVM.ViewModels.impl;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        private UserVM _userVM;
        private List<EventFieldUser> _CurrentUser;
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
            this._CurrentUser = list;
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
            for (int i = 1; i <= this._CurrentUser.Count; i++)
            {
                CreateFields(i);

                Grid grid2 = new Grid();
                grid2.Background = this.brushes[i % this.brushes.Length];
                grid2.SetValue(Grid.RowProperty, i);
                grid2.SetValue(Grid.ColumnProperty, COLUMNPROPS);

                Label label2 = new Label();
                label2.Name = SHOWNAME + i;
                label2.Content = this._CurrentUser[(i - 1)].Value;
                label2.VerticalAlignment = VerticalAlignment.Center;
                label2.FontSize = FONTSIZELABEL;
                grid2.Children.Add(label2);

                this.UserWindowGrid.Children.Add(grid2);

                RegisterName(SHOWNAME + i, label2);
            }

            CreateButton(BUTTON_PRINTBADGE, this._CurrentUser.Count + 1);
        }

        private void NewUser()
        {
            for (int i = 1; i <= this._CurrentUser.Count; i++)
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

            CreateButton(BUTTON_CONFIRM, this._CurrentUser.Count + 1);
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
            label.Content = this._CurrentUser[(i - 1)].EventField.Field.Name + " : ";
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

            for(int i = 1; i <= this._CurrentUser.Count; i++)
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
            /*PrintBadge printBadge = new PrintBadge(this._idEvent);
            printBadge.Show();
            this.Close();*/
        }
    }
}
