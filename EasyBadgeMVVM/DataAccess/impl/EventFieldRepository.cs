using EasyBadgeMVVM.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.DataAccess
{
    public class EventFieldRepository : BaseRepository<EventField>, IEventFieldRepository
    {
        public EventFieldRepository(EasyModelContext dbContext) : base(dbContext)
        {

        }
    }
}
