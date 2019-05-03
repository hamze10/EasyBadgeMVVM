using EasyBadgeMVVM.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.DataAccess
{
    public class EventRepository : BaseRepository<EventSet>, IEventRepository
    {

        public EventRepository(EasyBadgeModelContext dbContext) : base(dbContext)
        {

        }

        public EventSet GetLastEvent()
        {
            return this._dbContext.Set<EventSet>().OrderByDescending(e => e.ID_Event).FirstOrDefault();
        }
    }
}
