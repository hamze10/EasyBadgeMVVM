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
    public class FilterRepository : BaseRepository<Filter>, IFilterRepository
    {
        public FilterRepository(EasyModelContext dbContext) : base(dbContext)
        {

        }

        public void UpdateFilter(int filterId, Filter updatedFilter)
        {
            Filter toUpdate = this._dbContext.Set<Filter>().FirstOrDefault(f => f.ID_Filter == filterId);
            toUpdate.EventFieldFieldID_Field = updatedFilter.EventFieldFieldID_Field;
            toUpdate.LogicalOperator = updatedFilter.LogicalOperator;
            toUpdate.Value = updatedFilter.Value;
            this._dbContext.Entry(toUpdate).State = EntityState.Modified;
            this._dbContext.SaveChanges();
        }
    }
}
