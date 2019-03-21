using EasyBadgeMVVM.Models;
using EasyBadgeMVVM.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for FieldMatching.xaml
    /// </summary>
    public partial class FieldMatching : Window
    {
        private IFieldVM _fieldVM;

        private ObservableCollection<Field> _fieldsImported;
        private ObservableCollection<Field> _fieldsInDb;
        public Dictionary<string, string> FieldsAccepted { get; set; }
        public HashSet<string> FieldsUnique { get; set; }

        public FieldMatching(ObservableCollection<Field> fieldsDB, ObservableCollection<Field> fieldsImport)
        {
            this._fieldVM = new FieldVM();
            this._fieldsInDb = fieldsDB;
            this._fieldsImported = fieldsImport;
            this.FieldsAccepted = new Dictionary<string, string>();
            this.FieldsUnique = new HashSet<string>();
            InitializeComponent();
        }

        public void CreateMessages()
        {
            int i = 1;
            foreach(Field myField in this._fieldsImported)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(60);
                this.FieldMatchingGrid.RowDefinitions.Add(rowDefinition);

                Grid grid = new Grid();
                grid.SetValue(Grid.RowProperty, i);
                grid.SetValue(Grid.ColumnProperty, 0);

                Label label = new Label();
                label.Content = myField.Name.Replace("_", "__");
                label.Name = "labelField" + i;
                label.FontSize = 23;
                label.VerticalAlignment = VerticalAlignment.Center;
                label.HorizontalAlignment = HorizontalAlignment.Center;
                label.VerticalContentAlignment = VerticalAlignment.Center;
                label.HorizontalContentAlignment = HorizontalAlignment.Center;
                RegisterName("labelField" + i, label);

                CheckBox checkbox = new CheckBox();
                checkbox.Name = "checkboxField" + i;
                checkbox.Margin = new Thickness(30, 5, 0, 0);
                RegisterName("checkboxField" + i, checkbox);

                grid.Children.Add(checkbox);
                grid.Children.Add(label);

                Grid grid2 = new Grid();
                grid2.SetValue(Grid.RowProperty, i);
                grid2.SetValue(Grid.ColumnProperty, 1);

                Label label2 = new Label();
                label2.Content = "Choose a correspondance with the DB";
                label2.FontSize = 23;
                label2.VerticalAlignment = VerticalAlignment.Center;
                label2.HorizontalAlignment = HorizontalAlignment.Center;
                label2.VerticalContentAlignment = VerticalAlignment.Center;
                label2.HorizontalContentAlignment = HorizontalAlignment.Center;
                grid2.Children.Add(label2);

                Grid grid3 = new Grid();
                grid3.SetValue(Grid.RowProperty, i);
                grid3.SetValue(Grid.ColumnProperty, 2);

                //TODO RETRIEVE SELECTED VALUE
                ComboBox comboBox = new ComboBox();
                comboBox.DisplayMemberPath = "Name";
                comboBox.ItemsSource = this._fieldsInDb;
                comboBox.FontSize = 20;
                comboBox.Name = "comboboxField" + i;
                RegisterName("comboboxField" + i, comboBox);
                grid3.Children.Add(comboBox);


                this.FieldMatchingGrid.Children.Add(grid);
                this.FieldMatchingGrid.Children.Add(grid2);
                this.FieldMatchingGrid.Children.Add(grid3);

                i++;
            }

            RowDefinition rowDefinition2 = new RowDefinition();
            rowDefinition2.Height = new GridLength(60);
            this.FieldMatchingGrid.RowDefinitions.Add(rowDefinition2);

            Grid grid4 = new Grid();
            grid4.SetValue(Grid.RowProperty, i);
            grid4.SetValue(Grid.ColumnProperty, 1);

            Button button = new Button();
            button.Content = "Confirm";
            button.Width = 150;
            button.Height = 50;
            button.FontSize = 22;
            button.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#0a3d62"));
            button.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#0a3d62"));
            button.Click += new RoutedEventHandler(FieldMatchingClick);
            grid4.Children.Add(button);

            this.FieldMatchingGrid.Children.Add(grid4);
        }

        //SEND RESPONSE OF KEEPING OR NOT THE FIELD
        private void FieldMatchingClick(object sender, RoutedEventArgs e)
        {
            for(int i = 1; i <= this._fieldsImported.Count; i++)
            {
                Field fieldChoice = (Field)((ComboBox)this.FindName("comboboxField" + i)).SelectedItem;
                string choice = string.Empty;
                if (fieldChoice != null)
                {
                    choice = fieldChoice.Name;
                }

                string labelChoice = ((Label)this.FindName("labelField" + i)).Content.ToString();

                bool isCheck = ((CheckBox)this.FindName("checkboxField" + i)).IsChecked.Value;

                if (isCheck)
                {
                    this.FieldsUnique.Add(choice.Equals(string.Empty) ? labelChoice : choice);
                }

                this.FieldsAccepted.Add(labelChoice, choice.Equals(string.Empty) ? labelChoice : choice);
            }
            this.DialogResult = true;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            //BUTTON CLOSE CLICKED?
            this.DialogResult = this.DialogResult ?? false;
        }
    }
}
