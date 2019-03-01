using EasyBadgeMVVM.Models;

namespace EasyBadgeMVVM.DataAccess
{
    public interface IUserRepository
    {
        User GetLastUser();
        User GetUserByBarcode(string barcode);
        void SetAllUserInactive();
    }
}