using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace cicaudittrail.Models
{ 
    public class CicMessageTemplateRepository : ICicMessageTemplateRepository
    {
        cicaudittrailContext context = new cicaudittrailContext();

        public IQueryable<CicMessageTemplate> All
        {
            get { return context.CicMessageTemplate; }
        }

        public IQueryable<CicMessageTemplate> AllIncluding(params Expression<Func<CicMessageTemplate, object>>[] includeProperties)
        {
            IQueryable<CicMessageTemplate> query = context.CicMessageTemplate;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public CicMessageTemplate Find(long id)
        {
            return context.CicMessageTemplate.Find(id);
        }

        public void InsertOrUpdate(CicMessageTemplate cicmessagetemplate)
        {
            if (cicmessagetemplate.CicMessageTemplateId == default(long)) {
                // New entity
                context.CicMessageTemplate.Add(cicmessagetemplate);
            } else {
                // Existing entity
                context.Entry(cicmessagetemplate).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var cicmessagetemplate = context.CicMessageTemplate.Find(id);
            context.CicMessageTemplate.Remove(cicmessagetemplate);
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

    public interface ICicMessageTemplateRepository : IDisposable
    {
        IQueryable<CicMessageTemplate> All { get; }
        IQueryable<CicMessageTemplate> AllIncluding(params Expression<Func<CicMessageTemplate, object>>[] includeProperties);
        CicMessageTemplate Find(long id);
        void InsertOrUpdate(CicMessageTemplate cicmessagetemplate);
        void Delete(long id);
        void Save();
    }
}