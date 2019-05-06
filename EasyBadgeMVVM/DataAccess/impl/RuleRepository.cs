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
    public class RuleRepository : BaseRepository<Rule>, IRuleRepository
    {
        public RuleRepository(EasyModelContext dbContext) : base(dbContext)
        {

        }

        public void UpdateRule(int ruleId, Rule updatedRule)
        {
            Rule toUpdate = this._dbContext.Set<Rule>().FirstOrDefault(r => r.ID_Rule == ruleId);
            if (toUpdate == null) return;
            toUpdate.HexaCode = updatedRule.HexaCode;
            toUpdate.TargetID_Target = updatedRule.TargetID_Target;
            toUpdate.BadgeEventID_BadgeEvent = updatedRule.BadgeEventID_BadgeEvent;
            this._dbContext.Entry(toUpdate).State = EntityState.Modified;
            this._dbContext.SaveChanges();
        }
    }
}
