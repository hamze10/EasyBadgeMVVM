using EasyBadgeMVVM.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.DataAccess
{
    public class PositionRepository : BaseRepository<PositionSet>, IPositionRepository
    {
        public PositionRepository(EasyBadgeModelContext dbContext) : base(dbContext)
        {

        }

        public void RemoveRows(int idBadge, int idEvent, string templateName)
        {
            var ctx = this._dbContext;
            var setPos = ctx.Set<PositionSet>();
            List<PositionSet> result = setPos
                .Where(pos => pos.BadgeEventSet.BadgeID_Badge == idBadge && pos.BadgeEventSet.EventID_Event == idEvent && pos.BadgeEventSet.Name.Equals(templateName))
                .ToList();

            foreach (PositionSet p in result)
            {
                setPos.Remove(p);
                ctx.SaveChanges();
            }
        }
    }
}
