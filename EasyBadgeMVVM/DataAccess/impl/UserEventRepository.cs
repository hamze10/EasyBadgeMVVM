﻿using EasyBadgeMVVM.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.DataAccess
{
    public class UserEventRepository : BaseRepository<UserEvent>, IUserEventRepository
    {
        public UserEventRepository(EasyModelContext dbContext) : base(dbContext)
        {

        }

    }
}
