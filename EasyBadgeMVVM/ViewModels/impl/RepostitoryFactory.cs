using EasyBadgeMVVM.DataAccess;
using EasyBadgeMVVM.Models;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.ViewModels
{
    public class RepostitoryFactory : IRepostitoryFactory
    {
        public UserRepository GetUserRepository(EasyBadgeModelContext model)
        {
            return new UserRepository(model);
        }
        public FieldRepository GetFieldRepository(EasyBadgeModelContext model)
        {
            return new FieldRepository(model);
        }
        public EventRepository GetEventRepository(EasyBadgeModelContext model)
        {
            return new EventRepository(model);
        }

        public PrintBadgeRepository GetPrintBadgeRepository(EasyBadgeModelContext model)
        {
            return new PrintBadgeRepository(model);
        }

        public EventFieldRepository GetEventFieldRepository(EasyBadgeModelContext model)
        {
            return new EventFieldRepository(model);
        }

        public EventFieldUserRepository GetEventFieldUserRepository(EasyBadgeModelContext model)
        {
            return new EventFieldUserRepository(model);
        }

        public BadgeRepository GetBadgeRepository(EasyBadgeModelContext model)
        {
            return new BadgeRepository(model);
        }

        public BadgeEventRepository GetBadgeEventRepository(EasyBadgeModelContext model)
        {
            return new BadgeEventRepository(model);
        }

        public PositionRepository GetPositionRepository(EasyBadgeModelContext model)
        {
            return new PositionRepository(model);
        }
    }
}
