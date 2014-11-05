using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace cicaudittrail.Models
{ 
    public class CicMessageMailRepository : ICicMessageMailRepository
    {
        cicaudittrailContext context = new cicaudittrailContext();

        public IQueryable<CicMessageMail> All
        {
            get { return context.CicMessageMail; }
        }

        public IQueryable<CicMessageMail> AllIncluding(params Expression<Func<CicMessageMail, object>>[] includeProperties)
        {
            IQueryable<CicMessageMail> query = context.CicMessageMail;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public CicMessageMail Find(long id)
        {
            return context.CicMessageMail.Find(id);
        }

        public void InsertOrUpdate(CicMessageMail cicmessagemail)
        {
            if (cicmessagemail.CicMessageMailId == default(long)) {
                // New entity
                context.CicMessageMail.Add(cicmessagemail);
            } else {
                // Existing entity
                context.Entry(cicmessagemail).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var cicmessagemail = context.CicMessageMail.Find(id);
            context.CicMessageMail.Remove(cicmessagemail);
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

    public interface ICicMessageMailRepository : IDisposable
    {
        IQueryable<CicMessageMail> All { get; }
        IQueryable<CicMessageMail> AllIncluding(params Expression<Func<CicMessageMail, object>>[] includeProperties);
        CicMessageMail Find(long id);
        void InsertOrUpdate(CicMessageMail cicmessagemail);
        void Delete(long id);
        void Save();
    }
}