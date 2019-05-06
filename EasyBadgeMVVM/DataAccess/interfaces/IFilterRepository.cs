using EasyBadgeMVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.DataAccess.interfaces
{
    public interface IFilterRepository
    {
        void UpdateFilter(int filterId, FilterSet updatedFilter);
    }
}
