using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace cicaudittrail.Models
{ 
    public class CicMessageMailDocumentsRepository : ICicMessageMailDocumentsRepository
    {
        cicaudittrailContext context = new cicaudittrailContext();

        public IQueryable<CicMessageMailDocuments> All
        {
            get { return context.CicMessageMailDocuments; }
        }

        public IQueryable<CicMessageMailDocuments> AllIncluding(params Expression<Func<CicMessageMailDocuments, object>>[] includeProperties)
        {
            IQueryable<CicMessageMailDocuments> query = context.CicMessageMailDocuments;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public CicMessageMailDocuments Find(long id)
        {
            return context.CicMessageMailDocuments.Find(id);
        }

        public void InsertOrUpdate(CicMessageMailDocuments cicmessagemaildocuments)
        {
            if (cicmessagemaildocuments.CicMessageMailDocumentsId == default(long)) {
                // New entity
                context.CicMessageMailDocuments.Add(cicmessagemaildocuments);
            } else {
                // Existing entity
                context.Entry(cicmessagemaildocuments).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var cicmessagemaildocuments = context.CicMessageMailDocuments.Find(id);
            context.CicMessageMailDocuments.Remove(cicmessagemaildocuments);
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

    public interface ICicMessageMailDocumentsRepository : IDisposable
    {
        IQueryable<CicMessageMailDocuments> All { get; }
        IQueryable<CicMessageMailDocuments> AllIncluding(params Expression<Func<CicMessageMailDocuments, object>>[] includeProperties);
        CicMessageMailDocuments Find(long id);
        void InsertOrUpdate(CicMessageMailDocuments cicmessagemaildocuments);
        void Delete(long id);
        void Save();
    }
}