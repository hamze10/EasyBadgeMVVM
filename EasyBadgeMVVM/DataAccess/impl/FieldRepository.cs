using EasyBadgeMVVM.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.DataAccess
{
    public class FieldRepository : BaseRepository<Field>, IFieldRepository
    {
        public FieldRepository(EasyModelContext dbContext) : base(dbContext)
        {
            
        }

        public Field GetFieldByName(string name)
        {
            return this._dbContext.Set<Field>().Where(f => f.Name.ToLower().Equals(name.ToLower())).SingleOrDefault();
        }

        public Field CheckSimilarField(string name)
        {
            Field f1 = this._dbContext.Set<Field>().Where(f => f.Name.Equals(name)).SingleOrDefault();
            return f1 ?? this._dbContext.Set<Field>().Where(f => f.Name.ToLower().Contains(name.ToLower())).SingleOrDefault();
        }
    }
}