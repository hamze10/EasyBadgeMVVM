using EasyBadgeMVVM.Models;

namespace EasyBadgeMVVM.DataAccess
{
    public interface IEventFieldRepository
    {
        /// <summary>
        /// Change the visibility of an eventfield
        /// </summary>
        /// <param name="evf">The chosen evenfield</param>
        void Update(EventFieldSet evf);
    }
}
