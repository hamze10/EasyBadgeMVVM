using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EasyBadgeMVVM.DataAccess
{
    public interface IRepository
    {
    }

    public interface IRepository<T> : IRepository
    {
        void Insert(T entity);
        void Delete(T entity);
        IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate);
        bool Save(T entity, Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAll();
        T GetById(int id);
        void SaveChanges();
        //Check if the table is empty or not
        bool isDataExists();
    }
}
