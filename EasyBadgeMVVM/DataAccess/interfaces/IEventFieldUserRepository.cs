using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.DataAccess
{
    public interface IEventFieldUserRepository
    {
        void Update(int idUser, int idEvent, Dictionary<string, string> newValues);
    }
}
