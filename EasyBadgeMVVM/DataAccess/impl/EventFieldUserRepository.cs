using EasyBadgeMVVM.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.DataAccess
{
    public class EventFieldUserRepository : BaseRepository<EventFieldUserSet>, IEventFieldUserRepository
    {
        public EventFieldUserRepository(EasyBadgeModelContext dbContext) : base(dbContext)
        {

        }

    }
}
