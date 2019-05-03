using EasyBadgeMVVM.Models;

namespace EasyBadgeMVVM.DataAccess
{
    public interface IFieldRepository
    {
        FieldSet GetFieldByName(string name);
        FieldSet CheckSimilarField(string name);
    }
}