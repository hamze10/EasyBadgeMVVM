using EasyBadgeMVVM.Models;

namespace EasyBadgeMVVM.DataAccess
{
    public interface IEventRepository
    {
        /// <summary>
        /// Return the last event (bigger id)
        /// </summary>
        /// <returns></returns>
        EventSet GetLastEvent();
    }
}