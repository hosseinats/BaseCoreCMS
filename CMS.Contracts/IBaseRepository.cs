using CMS.Entities;
using CMS.Entities.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CMS.Contracts
{
    public interface IBaseRepository<T>  where T : class , IBaseEntity
    {
        T GetById(int id);
        T GetByIdTrackable(int id);
        IQueryable<T> GetAll();
        void Add(T Entity);
        void Update(T Entity);
        void Delete(int id);
        int getUserId(string userName);
        //IList<T> FindBy(Expression<Func<T, bool>> predicate);
        //IList<T> FindAllById(int id);
    }
}
