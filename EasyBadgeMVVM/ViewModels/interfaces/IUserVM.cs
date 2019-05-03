using EasyBadgeMVVM.Models;

using System.Collections.Generic;

namespace EasyBadgeMVVM.ViewModels
{
    public interface IUserVM
    {
        int IdEvent { get; }
        void InsertNewUser(Dictionary<string, string> values);
        EventSet GetEventById(int idEvent);
    }
}