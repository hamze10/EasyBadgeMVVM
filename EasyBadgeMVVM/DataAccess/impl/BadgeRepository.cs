using EasyBadgeMVVM.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.DataAccess
{
    public class BadgeRepository : BaseRepository<BadgeSet>, IBadgeRepository
    {
        public BadgeRepository(EasyBadgeModelContext dbContext) : base(dbContext)
        {

        }
    }
}
