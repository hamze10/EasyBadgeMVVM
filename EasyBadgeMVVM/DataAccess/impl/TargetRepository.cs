using EasyBadgeMVVM.DataAccess.interfaces;
using EasyBadgeMVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.DataAccess.impl
{
    public class TargetRepository : BaseRepository<Target>, ITargetRepository
    {
        public TargetRepository(EasyModelContext dbContext) : base(dbContext)
        {

        }
    }
}
