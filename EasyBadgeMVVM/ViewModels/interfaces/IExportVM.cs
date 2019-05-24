using EasyBadgeMVVM.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.ViewModels
{
    public interface IExportVM
    {
        List<ExportDTO> GetAllUsersToExport();
        List<ExportDTO> GetAllRegisteredUserToExport();
    }

    public class ExportDTO
    {
        public EventFieldUserSet EventFieldUserExport { get; set; }
        public PrintBadgeSet PrintBadgeExport { get; set; }
    }

}
