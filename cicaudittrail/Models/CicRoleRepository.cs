using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace cicaudittrail.Models
{ 
    public class CicRoleRepository : ICicRoleRepository
    {
        cicaudittrailContext context = new cicaudittrailContext();

        public IQueryable<CicRole> All
        {
            get { return context.CicRole; }
        }

        public IQueryable<CicRole> AllIncluding(params Expression<Func<CicRole, object>>[] includeProperties)
        {
            IQueryable<CicRole> query = context.CicRole;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }


        public CicRole Find(long id)
        {
            return context.CicRole.Find(id);
        }

        public void InsertOrUpdate(CicRole cicrole)
        {
            if (cicrole.CicRoleId == default(long)) {
                // New entity
                context.CicRole.Add(cicrole);
            } else {
                // Existing entity
                context.Entry(cicrole).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var cicrole = context.CicRole.Find(id);
            context.CicRole.Remove(cicrole);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }
    }

    public interface ICicRoleRepository : IDisposable
    {
        IQueryable<CicRole> All { get; }
        IQueryable<CicRole> AllIncluding(params Expression<Func<CicRole, object>>[] includeProperties);
    
        CicRole Find(long id);
        void InsertOrUpdate(CicRole cicrole);
        void Delete(long id);
        void Save();
    }
}