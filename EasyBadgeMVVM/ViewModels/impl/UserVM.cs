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


    }
}
