using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace cicaudittrail.Models
{
    public class CicDiversRequestResultsRepository : ICicDiversRequestResultsRepository
    {
        cicaudittrailContext context = new cicaudittrailContext();

        public cicaudittrailContext GetContext
        {
            get
            {
                return this.context;
            }
        }

        public IQueryable<CicDiversRequestResults> All
        {
            get { return context.CicDiversRequestResults; }
        }

        public IQueryable<CicDiversRequestResults> AllIncluding(params Expression<Func<CicDiversRequestResults, object>>[] includeProperties)
        {
            IQueryable<CicDiversRequestResults> query = context.CicDiversRequestResults;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public CicDiversRequestResults Find(long id)
        {
            return context.CicDiversRequestResults.Find(id);
        }

        public void InsertOrUpdate(CicDiversRequestResults cicdiversrequestresults)
        {
            if (cicdiversrequestresults.CicDiversRequestResultsId == default(long))
            {
                // New entity
                context.CicDiversRequestResults.Add(cicdiversrequestresults);
            }
            else
            {
                // Existing entity
                context.Entry(cicdiversrequestresults).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var cicdiversrequestresults = context.CicDiversRequestResults.Find(id);
            context.CicDiversRequestResults.Remove(cicdiversrequestresults);
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

    public interface ICicDiversRequestResultsRepository : IDisposable
    {
        cicaudittrailContext GetContext { get; }
        IQueryable<CicDiversRequestResults> All { get; }
        IQueryable<CicDiversRequestResults> AllIncluding(params Expression<Func<CicDiversRequestResults, object>>[] includeProperties);
        CicDiversRequestResults Find(long id);
        void InsertOrUpdate(CicDiversRequestResults cicdiversrequestresults);
        void Delete(long id);
        void Save();
    }
}