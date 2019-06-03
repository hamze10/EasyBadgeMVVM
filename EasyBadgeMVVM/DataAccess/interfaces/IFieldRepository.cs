using EasyBadgeMVVM.Models;

namespace EasyBadgeMVVM.DataAccess
{
    public interface IFieldRepository
    {
        /// <summary>
        /// Return the field matching with the name in parameter
        /// </summary>
        /// <param name="name"></param>
        /// <returns> Field with the name in parameter or null</returns>
        FieldSet GetFieldByName(string name);

        /// <summary>
        /// Check if there is a similar field with the name in parameter
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Similar field with the name in paramater or null</returns>
        FieldSet CheckSimilarField(string name);
    }
}