using EasyBadgeMVVM.Models;
using EasyBadgeMVVM.ViewModels;

using System;
using System.Collections.Generic;
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

        private string _fieldImported;
        private string _fieldInDb;

        public static string TO_REPLACE = "//---//";
        private string MESSAGE = "The field 1" + TO_REPLACE + " already seems to be in the database under the name 2" + TO_REPLACE + ".\nIs this correct?";

        public FieldMatching()
        {
            this._fieldVM = new FieldVM();
            InitializeComponent();
        }

        public string FieldImported
        {
            set
            {
                this._fieldImported = value;
            }
        }

        public string FieldInDb
        {
            set
            {
                this._fieldInDb = value;
            }
        }

        public void CreateMessages()
        {
            Grid grid = new Grid();
            grid.SetValue(Grid.RowProperty, 1);
            grid.SetValue(Grid.ColumnProperty, 1);

            TextBox textBox = new TextBox();
            string mess = MESSAGE.Replace("1" + TO_REPLACE, "'"+this._fieldImported+"'");
            textBox.Text = mess.Replace("2" + TO_REPLACE, "'" + this._fieldInDb + "'");
            textBox.IsReadOnly = true;
            textBox.VerticalAlignment = VerticalAlignment.Center;
            textBox.HorizontalAlignment = HorizontalAlignment.Center;
            textBox.VerticalContentAlignment = VerticalAlignment.Center;
            textBox.HorizontalContentAlignment = HorizontalAlignment.Center;
            textBox.AcceptsReturn = true;
            textBox.TextWrapping = TextWrapping.Wrap;
            textBox.Width = 475;
            textBox.Height = 86;
            textBox.Margin = new Thickness(0, 74, 0, 171);

            grid.Children.Add(textBox);

            this.FieldMatchingGrid.Children.Add(grid);
        }

        /* SEND RESPONSE OF KEEPING OR NOT THE FIELD */
        private void FieldMatchingClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = (sender as Button).Tag.Equals("yes") ? true : false;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            //BUTTON CLOSE CLICKED?
            this.DialogResult = this.DialogResult ?? true;
        }
    }
}
