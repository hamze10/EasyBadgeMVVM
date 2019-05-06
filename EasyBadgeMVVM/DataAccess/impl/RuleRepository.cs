using EasyBadgeMVVM.DataAccess.interfaces;
using EasyBadgeMVVM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.DataAccess.impl
{
    public class RuleRepository : BaseRepository<RuleSet>, IRuleRepository
    {
        public RuleRepository(EasyBadgeModelContext dbContext) : base(dbContext)
        {

        }

        public void UpdateRule(int ruleId, RuleSet updatedRule)
        {
            RuleSet toUpdate = this._dbContext.Set<RuleSet>().FirstOrDefault(r => r.ID_Rule == ruleId);
            if (toUpdate == null) return;
            toUpdate.HexaCode = updatedRule.HexaCode;
            toUpdate.TargetID_Target = updatedRule.TargetID_Target;
            toUpdate.BadgeEventID_BadgeEvent = updatedRule.BadgeEventID_BadgeEvent;
            this._dbContext.Entry(toUpdate).State = EntityState.Modified;
            this._dbContext.SaveChanges();
        }
    }
}
