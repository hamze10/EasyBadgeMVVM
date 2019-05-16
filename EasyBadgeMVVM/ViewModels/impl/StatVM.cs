using LiveCharts;
using LiveCharts.Wpf;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EasyBadgeMVVM.ViewModels
{
    public class StatVM : IStatVM
    {
        private int _idEvent;

        private int _nbrUser;
        private int _nbrUserOnline;
        private int _nbrUserOnsite;
        private int _nbrUniqueAttendance;
        private string[] _allProfiles;
        private IDictionary<string, double> _nbrUserPerProfile;
        private IDictionary<string, double> _printedBadgePerProfile; 

        private IDbEntities _dbEntities;

        private IList<string> differentProfiles = Util.differentProfiles;

        //FOR CHARTS
        public SeriesCollection SeriesCollection { get; set; }
        public int[] Labels { get; set; }
        public Func<double, string> XFormatter { get; set; }

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

            this._allProfiles = this._dbEntities.GetAllUsers()
                                    .Where(e => differentProfiles.Contains(e.EventFieldSet.FieldSet.Name.ToLower()))
                                    .Select(f => f.Value).Distinct().ToArray();

            this._nbrUserPerProfile = new Dictionary<string, double>();
            this._printedBadgePerProfile = new Dictionary<string, double>();
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
            SeriesCollection = new SeriesCollection();

            var allPrintedBadge = this._dbEntities.GetAllPrintBadge();
            var allDays = allPrintedBadge.Select(c => c.PrintDate.Date).Distinct();
            foreach (var day in allDays)
            {
                double[] eachHour = new double[24];
                for(int i = 0; i < 24; i++)
                {
                    eachHour[i] = allPrintedBadge.Where(c => c.PrintDate.Date.Equals(day) && c.PrintDate.Hour == i).Count();
                }

                SeriesCollection.Add(new LineSeries
                {
                    Title = day.ToShortDateString(),
                    Values = new ChartValues<double>(eachHour)
                });
            }

            Labels = Enumerable.Range(0, 24).ToArray();
            XFormatter = value => value + "h";
        }

        public void AttendancePerProfile() //BASIC STACKED
        {
            var allEventFieldUser = this._dbEntities.GetAllUsers();
            foreach (var efu in allEventFieldUser)
            {
                string profile = efu.EventFieldSet.FieldSet.Name;
                if (!differentProfiles.Contains(profile.ToLower())) continue;
                profile = efu.Value;
                if (this._nbrUserPerProfile.ContainsKey(profile))
                {
                    this._nbrUserPerProfile[profile] = this._nbrUserPerProfile[profile] + 1;
                }
                else
                {
                    this._printedBadgePerProfile.Add(profile, 0);
                    this._nbrUserPerProfile.Add(profile, 1);
                }
            }

            var allPrintedBadge = this._dbEntities.GetAllPrintBadge();
            HashSet<int> alreadyIdUser = new HashSet<int>();
            foreach (var printed in allPrintedBadge)
            {
                if (alreadyIdUser.Contains(printed.UserID_User)) continue;
                alreadyIdUser.Add(printed.UserID_User);
                var profileUser = this._dbEntities.GetAllUsers().Where(e => e.UserID_User == printed.UserID_User).ToList();
                string profileName = profileUser.Where(e => differentProfiles.Contains(e.EventFieldSet.FieldSet.Name.ToLower())).FirstOrDefault().Value;
                this._printedBadgePerProfile[profileName] = this._printedBadgePerProfile[profileName] + 1;
            }

            SeriesCollection2 = new SeriesCollection();

            SeriesCollection2.Add(new StackedColumnSeries
            {
                Values = new ChartValues<double>(this._printedBadgePerProfile.Values.ToList()),
                StackMode = StackMode.Values,
                DataLabels = true,
                FontSize = 15,
                Title = "Printed and/or scanned"
            });

            SeriesCollection2.Add(new StackedColumnSeries
            {
                Values = new ChartValues<double>(this._nbrUserPerProfile.Values.ToList()),
                StackMode = StackMode.Values,
                DataLabels = true,
                FontSize = 15,
                Title = "Registered"
            });

            Labels2 = this._allProfiles;
            Formatter = value => value + "";
        }
    }
}
