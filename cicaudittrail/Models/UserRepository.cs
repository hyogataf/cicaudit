using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace cicaudittrail.Models
{ 
    public class UserRepository : IUserRepository
    {
        cicaudittrailContext context = new cicaudittrailContext();

        public IQueryable<User> All
        {
            get { return context.User; }
        }

        public IQueryable<User> AllIncluding(params Expression<Func<User, object>>[] includeProperties)
        {
            IQueryable<User> query = context.User;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public User Find(long id)
        {
            return context.User.Find(id);
        }

        public void InsertOrUpdate(User user)
        {
            if (user.UserId == default(long)) {
                // New entity
                context.User.Add(user);
            } else {
                // Existing entity
                context.Entry(user).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var user = context.User.Find(id);
            context.User.Remove(user);
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

    public interface IUserRepository : IDisposable
    {
        IQueryable<User> All { get; }
        IQueryable<User> AllIncluding(params Expression<Func<User, object>>[] includeProperties);
        User Find(long id);
        void InsertOrUpdate(User user);
        void Delete(long id);
        void Save();
    }
}