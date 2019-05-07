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

        public void Update(int idUser, int idEvent, Dictionary<string, string> newValues)
        {
            var ctx = this._dbContext;
            var allEventField = ctx.Set<EventFieldUserSet>().Where(e => e.UserID_User == idUser && e.EventFieldEventID_Event == idEvent);

            foreach(var efu in allEventField)
            {
                efu.Value = newValues[efu.EventFieldSet.FieldSet.Name];
                ctx.Entry(efu).State = System.Data.Entity.EntityState.Modified;
            }

            ctx.SaveChanges();
        }
    }
}
