using EasyBadgeMVVM.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.DataAccess
{
    public class PositionRepository : BaseRepository<Position>, IPositionRepository
    {
        public PositionRepository(EasyModelContext dbContext) : base(dbContext)
        {

        }

        public void RemoveRows(int idBadge, int idEvent)
        {
            var ctx = this._dbContext;
            var setPos = ctx.Set<Position>();
            List<Position> result = setPos.Where(pos => pos.BadgeEvent.BadgeID_Badge == idBadge && pos.BadgeEvent.EventID_Event == idEvent).ToList();

            foreach (Position p in result)
            {
                setPos.Remove(p);
                ctx.SaveChanges();
            }
        }
    }
}
