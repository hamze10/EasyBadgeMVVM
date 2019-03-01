using EasyBadgeMVVM.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.DataAccess
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {

        public EventRepository(EasyModelContext dbContext) : base(dbContext)
        {

        }

        public Event GetLastEvent()
        {
            return this._dbContext.Set<Event>().OrderByDescending(e => e.ID_Event).FirstOrDefault();
        }
    }
}
