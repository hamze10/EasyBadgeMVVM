using EasyBadgeMVVM.Models;

namespace EasyBadgeMVVM.DataAccess
{
    public interface IUserRepository
    {
        /// <summary>
        /// Get the last user (bigger barcode)
        /// </summary>
        /// <returns> The last user or null</returns>
        UserSet GetLastUser();

        /// <summary>
        /// Get the user by the barcode
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns> The user matching or null</returns>
        UserSet GetUserByBarcode(string barcode);

        /// <summary>
        /// Set inactive to all users
        /// </summary>
        void SetAllUserInactive();
    }
}