using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.ViewModels
{
    public class ExportVM : IExportVM
    {
        private IDbEntities _dbEntities;
        private int _idEvent;

        public ExportVM(int idEvent)
        {
            this._dbEntities = new DbEntities();
            this._dbEntities.SetIdEvent(idEvent);
            this._idEvent = idEvent;
        }

        public List<ExportDTO> GetAllUsersToExport()
        {
            List<ExportDTO> list = new List<ExportDTO>();
            var allUsers = this._dbEntities.GetAllUsers();
            foreach(var us in allUsers)
            {
                list.Add(new ExportDTO
                {
                    EventFieldUserExport = us,
                    PrintBadgeExport = this._dbEntities.GetAllPrintBadge().Where(p => p.UserID_User == us.UserID_User).FirstOrDefault()
                });
            }

            return list; 
        }

        public List<ExportDTO> GetAllRegisteredUserToExport()
        {
            List<ExportDTO> list = new List<ExportDTO>();
            var allUsers = this._dbEntities.GetAllUsers();
            foreach(var us in allUsers)
            {
                var printBadge = this._dbEntities.GetAllPrintBadge().Where(p => p.UserID_User == us.UserID_User).FirstOrDefault();
                if (printBadge == null) continue;

                list.Add(new ExportDTO
                {
                    EventFieldUserExport = us,
                    PrintBadgeExport = printBadge
                });
            }

            return list;
        }
    }
}
