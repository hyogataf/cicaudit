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
    public class CicRequestExecutionRepository : ICicRequestExecutionRepository
    {
        cicaudittrailContext context = new cicaudittrailContext();

        public IQueryable<CicRequestExecution> All
        {
            get { return context.CicRequestExecution.OrderByDescending(c => c.DateCreated); }
        }

        public IQueryable<CicRequestExecution> AllIncluding(params Expression<Func<CicRequestExecution, object>>[] includeProperties)
        {
            IQueryable<CicRequestExecution> query = context.CicRequestExecution.OrderByDescending(c => c.DateCreated);
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public CicRequestExecution Find(long id)
        {
            return context.CicRequestExecution.Find(id);
        }


        public IQueryable<CicRequestExecution> FindByRequest(long RequestId)
        {
            return context.CicRequestExecution.ToList().Where(
                    r => r.CicRequestId == RequestId
                    ).AsQueryable();
        }

        public void InsertOrUpdate(CicRequestExecution Cicrequestexecution)
        { 
            if (Cicrequestexecution.CicRequestExecutionId == default(long))
            {
                // New entity
                context.CicRequestExecution.Add(Cicrequestexecution);
            }
            else
            {
                // Existing entity
                context.Entry(Cicrequestexecution).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var Cicrequestexecution = context.CicRequestExecution.Find(id);
            context.CicRequestExecution.Remove(Cicrequestexecution);
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

    public interface ICicRequestExecutionRepository : IDisposable
    {
        IQueryable<CicRequestExecution> All { get; }
        IQueryable<CicRequestExecution> AllIncluding(params Expression<Func<CicRequestExecution, object>>[] includeProperties);
        CicRequestExecution Find(long id);
        IQueryable<CicRequestExecution> FindByRequest(long id);
        void InsertOrUpdate(CicRequestExecution Cicrequestexecution);
        void Delete(long id);
        void Save();
    }
}