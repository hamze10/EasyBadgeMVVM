using EasyBadgeMVVM.DataAccess;
using EasyBadgeMVVM.Models;

namespace EasyBadgeMVVM.ViewModels
{
    public interface IRepostitoryFactory
    {
        EventRepository GetEventRepository(EasyModelContext model);
        FieldRepository GetFieldRepository(EasyModelContext model);
        FieldUserRepository GetFieldUserRepository(EasyModelContext model);
        UserEventRepository GetUserEventRepository(EasyModelContext model);
        UserRepository GetUserRepository(EasyModelContext model);
        PrintBadgeRepository GetPrintBadgeRepository(EasyModelContext model);
    }
}