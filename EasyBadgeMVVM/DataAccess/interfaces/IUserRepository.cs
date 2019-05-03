using EasyBadgeMVVM.Models;

namespace EasyBadgeMVVM.DataAccess
{
    public interface IUserRepository
    {
        UserSet GetLastUser();
        UserSet GetUserByBarcode(string barcode);
        void SetAllUserInactive();
    }
}