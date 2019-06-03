using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.DataAccess
{
    public interface IEventFieldUserRepository
    {
        /// <summary>
        /// Change the eventfielduser
        /// </summary>
        /// <param name="idUser"></param>
        /// <param name="idEvent"></param>
        /// <param name="newValues"></param>
        void Update(int idUser, int idEvent, Dictionary<string, string> newValues);
    }
}
