using EasyBadgeMVVM.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.DataAccess
{
    public interface IPositionRepository
    {
        /// <summary>
        /// Delete row matching with parameters in db
        /// </summary>
        /// <param name="idBadge"></param>
        /// <param name="idEvent"></param>
        /// <param name="templateName"></param>
        void RemoveRows(int idBadge, int idEvent, string templateName);
    }
}
