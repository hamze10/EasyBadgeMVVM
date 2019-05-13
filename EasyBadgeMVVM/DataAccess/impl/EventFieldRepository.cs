using EasyBadgeMVVM.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.DataAccess
{
    public class EventFieldRepository : BaseRepository<EventFieldSet>, IEventFieldRepository
    {
        public EventFieldRepository(EasyBadgeModelContext dbContext) : base(dbContext)
        {

        }

        public void Update(EventFieldSet evf)
        {
            var ctx = this._dbContext;
            var eventField = ctx.Set<EventFieldSet>().Where(e => e.FieldID_Field == evf.FieldID_Field && e.EventID_Event == evf.EventID_Event).FirstOrDefault();
            if (eventField == null) return;
            eventField.Visibility = evf.Visibility;
            ctx.Entry(eventField).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }
    }
}
