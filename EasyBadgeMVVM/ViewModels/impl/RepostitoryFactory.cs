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
        public UserRepository GetUserRepository(EasyModelContext model)
        {
            return new UserRepository(model);
        }
        public FieldRepository GetFieldRepository(EasyModelContext model)
        {
            return new FieldRepository(model);
        }
        public EventRepository GetEventRepository(EasyModelContext model)
        {
            return new EventRepository(model);
        }

        public PrintBadgeRepository GetPrintBadgeRepository(EasyModelContext model)
        {
            return new PrintBadgeRepository(model);
        }

        public EventFieldRepository GetEventFieldRepository(EasyModelContext model)
        {
            return new EventFieldRepository(model);
        }

        public EventFieldUserRepository GetEventFieldUserRepository(EasyModelContext model)
        {
            return new EventFieldUserRepository(model);
        }

        public BadgeRepository GetBadgeRepository(EasyModelContext model)
        {
            return new BadgeRepository(model);
        }

        public BadgeEventRepository GetBadgeEventRepository(EasyModelContext model)
        {
            return new BadgeEventRepository(model);
        }
    }
}
