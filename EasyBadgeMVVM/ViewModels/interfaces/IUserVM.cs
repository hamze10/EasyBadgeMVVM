using System.Collections.Generic;

namespace EasyBadgeMVVM.ViewModels.impl
{
    public interface IUserVM
    {
        int IdEvent { get; }
        void InsertNewUser(Dictionary<string, string> values);
    }
}