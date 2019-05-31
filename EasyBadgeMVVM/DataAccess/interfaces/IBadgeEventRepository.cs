using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.DataAccess
{
    public interface IBadgeEventRepository
    {
        void UpdateDefaultPrint(int idBadgeEvent, int idEvent);
        void UpdateWithBackground(EasyBadgeMVVM.Models.BadgeEventSet be);
    }
}
