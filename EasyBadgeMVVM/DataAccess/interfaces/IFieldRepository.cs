using EasyBadgeMVVM.Models;

namespace EasyBadgeMVVM.DataAccess
{
    public interface IFieldRepository
    {
        Field GetFieldByName(string name);
        Field CheckSimilarField(string name);
    }
}