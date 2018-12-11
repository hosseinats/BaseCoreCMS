using CMS.Contracts;
using CMS.DataLayer.Context;
using CMS.Entities;
using CMS.Entities.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Services
{
    public  class BaseRepository<T> :  IBaseRepository<T> where T : class , IBaseEntity 
    {
        protected CMSContext cmsContext;
        public BaseRepository(CMSContext cmsContext)
        {
            this.cmsContext = cmsContext;
        }

        public void Add(T Entity)
        {
            cmsContext.Set<T>().Add(Entity);
            cmsContext.SaveChanges();
        }

        public void Delete(int id)
        {

            cmsContext.Set<T>().Remove(GetById(id));
            cmsContext.SaveChanges();
        }

        public virtual IQueryable<T> GetAll()
        {
            return cmsContext.Set<T>().AsNoTracking();
        }

        public T GetById(int id)
        {
           return cmsContext.Set<T>().AsNoTracking().FirstOrDefault(p => p.Id == id);
        }
        public T GetByIdTrackable(int id)
        {
            return cmsContext.Set<T>().FirstOrDefault(p => p.Id == id);
        }
        public void Update(T Entity)
        {
            cmsContext.Set<T>().Update(Entity);
            cmsContext.SaveChanges();
        }

        public int getUserId(string userName)
        {
          return  cmsContext.Users.FirstOrDefault(p => p.UserName == userName || p.Email == userName).Id;
        }
    }
}
