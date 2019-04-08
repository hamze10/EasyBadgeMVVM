using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.ViewModels.impl
{
    public class UserVM : INotifyPropertyChanged, IUserVM
    {
        /*********************************************************************************************************************************************************************/
        /****** CONSTRUCTOR ******/
        /*********************************************************************************************************************************************************************/

        private int _idEvent;
        private IDbEntities _dbEntities;

        public UserVM(int idEvent)
        {
            this._dbEntities = new DbEntities();
            this._idEvent = idEvent;
            this._dbEntities.SetIdEvent(this._idEvent);
        }

        public int IdEvent
        {
            get
            {
                return this._idEvent;
            }
        }

        /*********************************************************************************************************************************************************************/
        /****** PROPERTY CHANGED ******/
        /*********************************************************************************************************************************************************************/

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /*********************************************************************************************************************************************************************/
        /****** MAIN ******/
        /*********************************************************************************************************************************************************************/

        int i = 0;
        public void InsertNewUser(Dictionary<string, string> values)
        {
            foreach(KeyValuePair<string, string> entry in values)
            {
                string field = entry.Key.Trim().Split(':')[0].Trim();
                this._dbEntities.InsertNewUser(i, field, entry.Value, this._dbEntities.GetVisibilityField(field));
                i++;
            }
            this._dbEntities.SaveAllChanges();
        }
    }
}
