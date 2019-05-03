using EasyBadgeMVVM.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.DataAccess
{
    public class PrintBadgeRepository : BaseRepository<PrintBadgeSet>, IPrintBadgeRepository
    {
        public PrintBadgeRepository(EasyBadgeModelContext dbContext) : base(dbContext)
        {

        }
    }
}
