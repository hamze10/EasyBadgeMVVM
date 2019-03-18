﻿using EasyBadgeMVVM.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.DataAccess
{
    public class PrintBadgeRepository : BaseRepository<PrintBadge>, IPrintBadgeRepository
    {
        public PrintBadgeRepository(EasyModelContext dbContext) : base(dbContext)
        {

        }
    }
}