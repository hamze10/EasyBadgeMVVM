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
        Event InsertEvent(Event ev);
        int SelectedEvent { get; set; }
        Event GetEventById(int idEvent);
    }
}
