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
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>List of users in exportDTO</returns>
        List<ExportDTO> GetAllUsersToExport();

        /// <summary>
        /// Get all users whose badges have been printed
        /// </summary>
        /// <returns></returns>
        List<ExportDTO> GetAllRegisteredUserToExport();
    }

    /// <summary>
    /// Used for export (EventfieldUser for all information about a user | printBadge for all information about printed badge of a user)
    /// </summary>
    public class ExportDTO
    {
        public EventFieldUserSet EventFieldUserExport { get; set; }
        public PrintBadgeSet PrintBadgeExport { get; set; }
    }

}
