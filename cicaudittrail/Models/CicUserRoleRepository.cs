using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace cicaudittrail.Models
{
    public class CicUserRoleRepository : ICicUserRoleRepository
    {
        cicaudittrailContext context = new cicaudittrailContext();

        public IQueryable<CicUserRole> All
        {
            get { return context.CicUserRole; }
        }

        public IQueryable<CicUserRole> AllIncluding(params Expression<Func<CicUserRole, object>>[] includeProperties)
        {
            IQueryable<CicUserRole> query = context.CicUserRole;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public CicUserRole Find(long id)
        {
            return context.CicUserRole.Find(id);
        }


        public IQueryable<CicUserRole> FindRolesByUser(string username)
        {
            var Roles = context.CicUserRole.ToList().Where(
                r => r.Username.ToLower() == username.ToLower()
                ).AsQueryable();
            return Roles;
        }


        public string[] FindRoleNamesByUser(string username)
        {
            var roleNames = from ru in FindRolesByUser(username)
                            from r in context.CicRole
                            where r.CicRoleId == ru.CicRoleId
                            select r.Name;
            if (roleNames != null)
                return roleNames.ToArray();
            else
                return new string[] { }; ;
        }


        public bool IsUserInRole(string username, string roleName)
        {
            var roleNames = FindRoleNamesByUser(username);
            return roleNames.Any(r => r.Equals(roleName, StringComparison.CurrentCultureIgnoreCase));
        }


        public void InsertOrUpdate(CicUserRole cicuserrole)
        {
            if (cicuserrole.CicUserRoleId == default(long))
            {
                // New entity
                context.CicUserRole.Add(cicuserrole);
            }
            else
            {
                // Existing entity
                context.Entry(cicuserrole).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var cicuserrole = context.CicUserRole.Find(id);
            context.CicUserRole.Remove(cicuserrole);
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

    public interface ICicUserRoleRepository : IDisposable
    {
        IQueryable<CicUserRole> All { get; }
        IQueryable<CicUserRole> AllIncluding(params Expression<Func<CicUserRole, object>>[] includeProperties);
        IQueryable<CicUserRole> FindRolesByUser(string username);
        string[] FindRoleNamesByUser(string username);
        bool IsUserInRole(string username, string roleName);
        CicUserRole Find(long id);
        void InsertOrUpdate(CicUserRole cicuserrole);
        void Delete(long id);
        void Save();
    }
}