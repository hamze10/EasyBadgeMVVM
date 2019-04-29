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

        public void UpdateDefaultPrint(int idBadgeEvent, int idEvent)
        {
            var ctx = this._dbContext;
            var allBadgeEvent = ctx.Set<BadgeEvent>().Where(be => be.EventID_Event == idEvent);

            foreach(var badgeE in allBadgeEvent)
            {
                badgeE.DefaultPrint = badgeE.ID_BadgeEvent == idBadgeEvent;
                ctx.Entry(badgeE).State = System.Data.Entity.EntityState.Modified;
            }
            ctx.SaveChanges();
        }
    }
}