using EasyBadgeMVVM.Models;

using System.Collections.Generic;

namespace EasyBadgeMVVM.ViewModels
{
    public interface IUserVM
    {
        int IdEvent { get; }
        void InsertNewUser(Dictionary<string, string> values);
        EventSet GetEventById(int idEvent);
        void UpdateUser(int idUser, Dictionary<string, string> newValues);
        /// <summary>
        /// Determine if the given user match the defined filters/rules.
        /// If that's the case, the UI should display the define color.
        /// 
        /// Comparison can be currently done on : double, string.
        /// You can then compare with the name, the company name, the age...
        /// The following ones could be added in a future version : bool, datetime
        /// </summary>
        /// <returns>Returns the hexadecimal code, null if there is no filter that match.</returns>
        string DetermineColorForCard(List<EventFieldUserSet> currentUser);
    }
}