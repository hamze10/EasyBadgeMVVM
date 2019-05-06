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
    public class FilterRepository : BaseRepository<FilterSet>, IFilterRepository
    {
        public FilterRepository(EasyBadgeModelContext dbContext) : base(dbContext)
        {

        }

        public void UpdateFilter(int filterId, FilterSet updatedFilter)
        {
            FilterSet toUpdate = this._dbContext.Set<FilterSet>().FirstOrDefault(f => f.ID_Filter == filterId);
            if (toUpdate == null) return;
            toUpdate.EventFieldFieldID_Field = updatedFilter.EventFieldFieldID_Field;
            toUpdate.LogicalOperator = updatedFilter.LogicalOperator;
            toUpdate.Value = updatedFilter.Value;
            this._dbContext.Entry(toUpdate).State = EntityState.Modified;
            this._dbContext.SaveChanges();
        }
    }
}
