using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.DataAccess
{
    public interface IBadgeEventRepository
    {
        /// <summary>
        /// Change the default template used to print badges
        /// </summary>
        /// <param name="idBadgeEvent"> ID of the new default template</param>
        /// <param name="idEvent"> ID of the event</param>
        void UpdateDefaultPrint(int idBadgeEvent, int idEvent);

        /// <summary>
        /// Change the background image of a template
        /// </summary>
        /// <param name="be">the chosen template</param>
        void UpdateWithBackground(EasyBadgeMVVM.Models.BadgeEventSet be);
    }
}
