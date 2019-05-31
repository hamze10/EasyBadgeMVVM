using EasyBadgeMVVM.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.DataAccess
{
    public class BadgeEventRepository : BaseRepository<BadgeEventSet>, IBadgeEventRepository
    {
        public BadgeEventRepository(EasyBadgeModelContext dbContext) : base(dbContext)
        {

        }

        public void UpdateDefaultPrint(int idBadgeEvent, int idEvent)
        {
            var ctx = this._dbContext;
            var allBadgeEvent = ctx.Set<BadgeEventSet>().Where(be => be.EventID_Event == idEvent);

            foreach(var badgeE in allBadgeEvent)
            {
                badgeE.DefaultPrint = badgeE.ID_BadgeEvent == idBadgeEvent;
                ctx.Entry(badgeE).State = System.Data.Entity.EntityState.Modified;
            }
            ctx.SaveChanges();
        }

        public void UpdateWithBackground(BadgeEventSet be)
        {
            var ctx = this._dbContext;
            BadgeEventSet be1 = ctx.Set<BadgeEventSet>().Where(b => b.BadgeID_Badge == be.BadgeID_Badge).FirstOrDefault();
            be1.BackgroundImage = be.BackgroundImage;
            ctx.Entry(be1).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }
    }
}