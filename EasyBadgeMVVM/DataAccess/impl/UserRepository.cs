using EasyBadgeMVVM.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.DataAccess
{
    public class UserRepository : BaseRepository<UserSet>, IUserRepository
    {
        public UserRepository(EasyBadgeModelContext dbContext) : base(dbContext)
        {

        }

        public UserSet GetLastUser()
        {
            return this._dbContext.Set<UserSet>().OrderByDescending(u => u.Barcode).FirstOrDefault();
        }

        public UserSet GetUserByBarcode(string barcode)
        {
            return this._dbContext.Set<UserSet>().Where(u => u.Barcode.Equals(barcode)).SingleOrDefault();
        }

        public void SetAllUserInactive()
        {
            List<UserSet> myList = this._dbContext.Set<UserSet>().ToList();
            int i = 0;
            foreach (UserSet u in myList)
            {
                if (!u.Active) continue;
                u.Active = false;
                this._dbContext.Entry(u).State = System.Data.Entity.EntityState.Modified;
                
                if (i >= 500 && i % (myList.Count/10) == 0)
                {
                    this._dbContext.SaveChanges();
                }
                i++;
            }
            this._dbContext.SaveChanges();
        }
    }
}
