using EasyBadgeMVVM.DataAccess;
using EasyBadgeMVVM.DataAccess.impl;
using EasyBadgeMVVM.Models;

namespace EasyBadgeMVVM.ViewModels
{
    public interface IRepostitoryFactory
    {
        EventRepository GetEventRepository(EasyModelContext model);
        FieldRepository GetFieldRepository(EasyModelContext model);
        UserRepository GetUserRepository(EasyModelContext model);
        PrintBadgeRepository GetPrintBadgeRepository(EasyModelContext model);
        EventFieldRepository GetEventFieldRepository(EasyModelContext model);
        EventFieldUserRepository GetEventFieldUserRepository(EasyModelContext model);
        BadgeRepository GetBadgeRepository(EasyModelContext model);
        BadgeEventRepository GetBadgeEventRepository(EasyModelContext model);
        PositionRepository GetPositionRepository(EasyModelContext model);

        FilterRepository GetFilterRepository(EasyModelContext model);

        RuleRepository GetRuleRepository(EasyModelContext model);

        TargetRepository GetTargetRepository(EasyModelContext model);
    }
}