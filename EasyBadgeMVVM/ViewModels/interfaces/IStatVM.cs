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
        int NbrUser { get; set; }
        int NbrUniqueAttendance { get; set; }
        int NbrUserOnline { get; set; }
        int NbrUserOnsite { get; set; }

        SeriesCollection SeriesCollection { get; set; }
        int[] Labels { get; set; }
        Func<double, string> XFormatter { get; set; }
        SeriesCollection SeriesCollection2 { get; set; }
        string[] Labels2 { get; set; }
        Func<double, string> Formatter { get; set; }

        void AttendancePerDay();
        void AttendancePerProfile();

        void Refresh(string[] fields);
    }
}
