﻿using EasyBadgeMVVM.Models;

namespace EasyBadgeMVVM.DataAccess
{
    public interface IEventRepository
    {
        Event GetLastEvent();
    }
}