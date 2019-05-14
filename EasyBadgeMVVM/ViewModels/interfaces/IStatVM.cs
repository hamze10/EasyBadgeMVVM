using LiveCharts;
using LiveCharts.Wpf;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.ViewModels
{
    interface IStatVM
    {
        int NbrUser { get; }
        int NbrUniqueAttendance { get; }
        int NbrUserOnline { get; }
        int NbrUserOnsite { get; }

        SeriesCollection SeriesCollection { get; set; }
        string[] Labels { get; set; }
        Func<double, string> YFormatter { get; set; }
        SeriesCollection SeriesCollection2 { get; set; }
        string[] Labels2 { get; set; }
        Func<double, string> Formatter { get; set; }

        void AttendancePerDay();
        void AttendancePerProfile();
    }
}
