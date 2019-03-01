using EasyBadgeMVVM.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.DataAccess
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(EasyModelContext dbContext) : base(dbContext)
        {

        }

        public User GetLastUser()
        {
            return this._dbContext.Set<User>().OrderByDescending(u => u.Barcode).FirstOrDefault();
        }

        public User GetUserByBarcode(string barcode)
        {
            return this._dbContext.Set<User>().Where(u => u.Barcode.Equals(barcode)).SingleOrDefault();
        }

        public void SetAllUserInactive()
        {
            List<User> myList = this._dbContext.Set<User>().ToList();
            int i = 0;
            foreach (User u in myList)
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
