using EasyBadgeMVVM.DataAccess;
using EasyBadgeMVVM.DataAccess.impl;
using EasyBadgeMVVM.Models;

namespace EasyBadgeMVVM.ViewModels
{
    public interface IRepostitoryFactory
    {

        EventRepository GetEventRepository(EasyBadgeModelContext model);
        FieldRepository GetFieldRepository(EasyBadgeModelContext model);
        UserRepository GetUserRepository(EasyBadgeModelContext model);
        PrintBadgeRepository GetPrintBadgeRepository(EasyBadgeModelContext model);
        EventFieldRepository GetEventFieldRepository(EasyBadgeModelContext model);
        EventFieldUserRepository GetEventFieldUserRepository(EasyBadgeModelContext model);
        BadgeRepository GetBadgeRepository(EasyBadgeModelContext model);
        BadgeEventRepository GetBadgeEventRepository(EasyBadgeModelContext model);
        PositionRepository GetPositionRepository(EasyBadgeModelContext model);
        FilterRepository GetFilterRepository(EasyBadgeModelContext model);
        RuleRepository GetRuleRepository(EasyBadgeModelContext model);
        TargetRepository GetTargetRepository(EasyBadgeModelContext model);
    }
}