using EasyBadgeMVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.DataAccess.interfaces
{
    public interface IRuleRepository
    {
        void UpdateRule(int ruleId, Rule updatedRule);
    }
}
