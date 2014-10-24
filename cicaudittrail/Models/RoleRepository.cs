using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Diagnostics;

namespace cicaudittrail.Models
{ 
    public class RoleRepository : IRoleRepository
    {
        cicaudittrailContext context = new cicaudittrailContext();

        public IQueryable<Role> All
        {
            get {
                //Debug.WriteLine("context.Role= " + context.Role);
                //var result = from i in context.Role where i.Name=="Admin"
                //             select i ;
                //var sql = result.ToString();
                //Debug.WriteLine("sql 1= " + sql); 
                return context.Role; 
            }
        }

        public IQueryable<Role> AllIncluding(params Expression<Func<Role, object>>[] includeProperties)
        {
            IQueryable<Role> query = context.Role;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Role Find(long id)
        {
            return context.Role.Find(id);
        }

        public void InsertOrUpdate(Role role)
        {
            if (role.RoleId == default(long)) {
                // New entity
                context.Role.Add(role);
            } else {
                // Existing entity
                context.Entry(role).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var role = context.Role.Find(id);
            context.Role.Remove(role);
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

    public interface IRoleRepository : IDisposable
    {
        IQueryable<Role> All { get; }
        IQueryable<Role> AllIncluding(params Expression<Func<Role, object>>[] includeProperties);
        Role Find(long id);
        void InsertOrUpdate(Role role);
        void Delete(long id);
        void Save();
    }
}