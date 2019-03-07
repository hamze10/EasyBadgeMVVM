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
        private List<UserEvent> _CurrentUser;
        private bool _isNew;

        private const int FONTSIZELABEL = 16;
        private const double GRIDLENGTHHEIGHT = 60;

        private SolidColorBrush[] brushes = new SolidColorBrush[2] { System.Windows.Media.Brushes.White, System.Windows.Media.Brushes.WhiteSmoke};

        public UserWindow(bool isNew, List<UserEvent> list)
        {
            this._isNew = isNew;
            this._userVM = new UserVM();
            this._CurrentUser = list;
            this.DataContext = this._userVM;
            InitializeComponent();
            if (this._isNew)
            {
                NewUser(this._CurrentUser);
            }
            else
            {
                ShowUser(this._CurrentUser);
            }
        }

        private void ShowUser(List<UserEvent> list)
        {
            for (int i = 1; i <= list.Count; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(GRIDLENGTHHEIGHT);
                this.UserWindowGrid.RowDefinitions.Add(rowDefinition);

                Grid grid = new Grid();
                grid.Background = this.brushes[i % this.brushes.Length];
                grid.SetValue(Grid.RowProperty, i);

                Grid grid2 = new Grid();
                grid2.Background = this.brushes[i % this.brushes.Length];
                grid2.SetValue(Grid.RowProperty, i);
                grid2.SetValue(Grid.ColumnProperty, 1);;

                Label label = new Label();
                label.Content = list[(i - 1)].FieldUser.Field.Name + " : ";
                label.FontSize = FONTSIZELABEL;
                label.VerticalAlignment = VerticalAlignment.Center;
                grid.Children.Add(label);

                Label label2 = new Label();
                label2.Content = list[(i - 1)].FieldUser.Value;
                label2.FontSize = FONTSIZELABEL;
                label2.VerticalAlignment = VerticalAlignment.Center;
                grid2.Children.Add(label2);



                this.UserWindowGrid.Children.Add(grid);
                this.UserWindowGrid.Children.Add(grid2);
                
            }
        }

        private void NewUser(List<UserEvent> list)
        {
            for (int i = 1; i <= list.Count; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(GRIDLENGTHHEIGHT);
                this.UserWindowGrid.RowDefinitions.Add(rowDefinition);

                Grid grid = new Grid();
                grid.Background = this.brushes[i % this.brushes.Length];
                grid.SetValue(Grid.RowProperty, i);

                Grid grid2 = new Grid();
                grid2.Background = this.brushes[i % this.brushes.Length];
                grid2.SetValue(Grid.RowProperty, i);
                grid2.SetValue(Grid.ColumnProperty, 1);

                Label label = new Label();
                label.Content = list[(i - 1)].FieldUser.Field.Name + " : ";
                label.FontSize = FONTSIZELABEL;
                label.VerticalAlignment = VerticalAlignment.Center;
                grid.Children.Add(label);

                TextBox textBox = new TextBox();
                textBox.Width = 500;
                textBox.FontSize = FONTSIZELABEL;
                textBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                textBox.VerticalAlignment = VerticalAlignment.Center;
                grid2.Children.Add(textBox);

                this.UserWindowGrid.Children.Add(grid);
                this.UserWindowGrid.Children.Add(grid2);
            }
        }
    }
}
