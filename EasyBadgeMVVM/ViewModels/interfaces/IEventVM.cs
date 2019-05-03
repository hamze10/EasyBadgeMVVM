using EasyBadgeMVVM.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.ViewModels
{
    public interface IEventVM
    {
        EventSet InsertEvent(EventSet ev);
        int SelectedEvent { get; set; }
        EventSet GetEventById(int idEvent);
    }
}
