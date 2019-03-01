using EasyBadgeMVVM.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM.DataAccess
{
    public class FieldUserRepository : BaseRepository<FieldUser>, IFieldUserRepository
    {
        public FieldUserRepository(EasyModelContext dbContext) : base(dbContext)
        {

        }

        public FieldUser GetFieldUserByUserAndField(string barcode, string fieldName)
        {
            return this._dbContext.Set<FieldUser>().Where(fu => fu.User.Barcode.Equals(barcode) && fu.Field.Name.ToLower().Equals(fieldName.ToLower())).SingleOrDefault();
        }

        public void UpdateWithAdditionnalInformation(FieldUser fu)
        {
            FieldUser f = this._dbContext.Set<FieldUser>().Where(x => x.ID_FieldUser == fu.ID_FieldUser).SingleOrDefault();
            if (f != null)
            {
                f.AdditionnalInformation = fu.AdditionnalInformation;
                this._dbContext.Entry(f).State = System.Data.Entity.EntityState.Modified;
                this._dbContext.SaveChanges();
            }
        }

    }
}
