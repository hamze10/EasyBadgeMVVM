using EasyBadgeMVVM.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.DataAccess
{
    public class FieldRepository : BaseRepository<FieldSet>, IFieldRepository
    {
        public FieldRepository(EasyBadgeModelContext dbContext) : base(dbContext)
        {
            
        }

        public FieldSet GetFieldByName(string name)
        {
            return this._dbContext.Set<FieldSet>().Where(f => f.Name.ToLower().Equals(name.ToLower())).SingleOrDefault();
        }

        public FieldSet CheckSimilarField(string name)
        {
            FieldSet f1 = this._dbContext.Set<FieldSet>().Where(f => f.Name.Equals(name)).SingleOrDefault();
            return f1 ?? this._dbContext.Set<FieldSet>().Where(f => f.Name.ToLower().Contains(name.ToLower())).SingleOrDefault();
        }
    }
}