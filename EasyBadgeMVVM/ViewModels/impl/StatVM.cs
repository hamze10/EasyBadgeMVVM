using LiveCharts;
using LiveCharts.Wpf;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.ViewModels
{
    public class StatVM : IStatVM
    {
        private int _idEvent;

        private int _nbrUser;
        private int _nbrUserOnline;
        private int _nbrUserOnsite;
        private int _nbrUniqueAttendance;

        private IDbEntities _dbEntities;

        //FOR CHARTS
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public SeriesCollection SeriesCollection2 { get; set; }
        public string[] Labels2 { get; set; }
        public Func<double, string> Formatter { get; set; }

        public StatVM(int idEvent)
        {
            this._dbEntities = new DbEntities();
            this._dbEntities.SetIdEvent(idEvent);
            this._idEvent = idEvent;
            this._nbrUser = -1;
            this._nbrUserOnline = -1;
            this._nbrUserOnsite = -1;
            this._nbrUniqueAttendance = -1;
        }

        public int NbrUser
        {
            get
            {
                if (this._nbrUser == -1)
                {
                    this._nbrUser = this._dbEntities.GetAllUsers().GroupBy(u => u.UserID_User).Select(v => v.Key).Count();
                }

                return this._nbrUser;
            }
        }

        public int NbrUserOnline
        {
            get
            {
                if (this._nbrUserOnline == -1)
                {
                    this._nbrUserOnline = this._dbEntities.GetAllUsers().Where(x => x.UserSet.Onsite == false).GroupBy(u => u.UserID_User).Select(v => v.Key).Count();
                }

                return this._nbrUserOnline;
            }
        }

        public int NbrUserOnsite
        {
            get
            {
                if (this._nbrUserOnsite == -1)
                {
                    this._nbrUserOnsite = this._dbEntities.GetAllUsers().Where(x => x.UserSet.Onsite == true).GroupBy(u => u.UserID_User).Select(v => v.Key).Count();
                }

                return this._nbrUserOnsite;
            }
        }

        public int NbrUniqueAttendance
        {
            get
            {
                if (this._nbrUniqueAttendance == -1)
                {
                    this._nbrUniqueAttendance = this._dbEntities.GetAllPrintBadge().Count();
                }

                return _nbrUniqueAttendance;
            }
        }

        public void AttendancePerDay() //BASIC LINE CHART
        {
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,4 }
                },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> { 6, 7, 3, 4 ,6 },
                },
                new LineSeries
                {
                    Title = "Series 3",
                    Values = new ChartValues<double> { 4,2,7,2,7 },
                }
            };

            Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => value.ToString("C");

            //modifying the series collection will animate and update the chart
            SeriesCollection.Add(new LineSeries
            {
                Title = "Series 4",
                Values = new ChartValues<double> { 5, 3, 2, 4 },
            });

            //modifying any series values will also animate and update the chart
            SeriesCollection[3].Values.Add(5d);
        }

        public void AttendancePerProfile() //BASIC STACKED
        {
            SeriesCollection2 = new SeriesCollection
            {
                new StackedColumnSeries
                {
                    Values = new ChartValues<double> {4, 5, 6, 8},
                    StackMode = StackMode.Values, // this is not necessary, values is the default stack mode
                    DataLabels = true
                },
                new StackedColumnSeries
                {
                    Values = new ChartValues<double> {2, 5, 6, 7},
                    StackMode = StackMode.Values,
                    DataLabels = true
                }
            };

            //adding series updates and animates the chart
            SeriesCollection2.Add(new StackedColumnSeries
            {
                Values = new ChartValues<double> { 6, 2, 7 },
                StackMode = StackMode.Values
            });

            //adding values also updates and animates
            SeriesCollection2[2].Values.Add(4d);

            Labels2 = new[] { "Chrome", "Mozilla", "Opera", "IE" };
            Formatter = value => value + " Mill";
        }
    }
}
