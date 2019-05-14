using EasyBadgeMVVM.ViewModels;

using System;
using System.Collections.Generic;
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
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class Statistics : Window
    {

        private IStatVM _statVM;

        public Statistics(int idEvent)
        {
            InitializeComponent();
            this._statVM = new StatVM(idEvent);
            this._statVM.AttendancePerDay();
            this._statVM.AttendancePerProfile();
            DataContext = this._statVM;
        }
    }
}
