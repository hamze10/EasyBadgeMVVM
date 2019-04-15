using EasyBadgeMVVM.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.DataAccess
{
    public class BadgeEventRepository : BaseRepository<BadgeEvent>, IBadgeEventRepository
    {
        public BadgeEventRepository(EasyModelContext dbContext) : base(dbContext)
        {

        }
    }
}