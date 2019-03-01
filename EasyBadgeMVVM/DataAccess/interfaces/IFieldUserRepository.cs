using EasyBadgeMVVM.Models;

namespace EasyBadgeMVVM.DataAccess
{
    public interface IFieldUserRepository
    {
        FieldUser GetFieldUserByUserAndField(string barcode, string fieldName);
    }
}