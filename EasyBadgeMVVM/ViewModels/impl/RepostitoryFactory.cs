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
        public FieldUserRepository GetFieldUserRepository(EasyModelContext model)
        {
            return new FieldUserRepository(model);
        }
        public UserEventRepository GetUserEventRepository(EasyModelContext model)
        {
            return new UserEventRepository(model);
        }

        public PrintBadgeRepository GetPrintBadgeRepository(EasyModelContext model)
        {
            return new PrintBadgeRepository(model);
        }
    }
}
