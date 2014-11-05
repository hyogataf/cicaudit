using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics;
using Oracle.ManagedDataAccess.Client;

namespace cicaudittrail.Models
{
    public class CicRequestResultsRepository : ICicRequestResultsRepository
    {
        cicaudittrailContext context = new cicaudittrailContext();

        public IQueryable<CicRequestResults> All
        {
            get { return context.CicRequestResults; }
        }

        public IQueryable<CicRequestResults> AllIncluding(params Expression<Func<CicRequestResults, object>>[] includeProperties)
        {
            IQueryable<CicRequestResults> query = context.CicRequestResults;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public CicRequestResults Find(long id)
        {
            return context.CicRequestResults.Find(id);
        }

        public DbSqlQuery<CicRequestResults> FindAllByRequest(long Cicrequestid)
        {
            /*
             *  return context.CicRequest.ToList().Where(
                r => r.IsDeleted == 0
                ).AsQueryable();
             */
            return context.CicRequestResults.SqlQuery("select * from CicRequestResults where CicRequestId=:P0 order by CicRequestResultsId", Cicrequestid);
        }

        public DbSqlQuery<CicRequestResults> FindAllByRequestAndDate(long Cicrequestid, DateTime date)
        {
            /*
             *  return context.CicRequest.ToList().Where(
                r => r.IsDeleted == 0
                ).AsQueryable();
             */
            return context.CicRequestResults.SqlQuery("select * from CicRequestResults where CicRequestId=:P0 and DateCreated>=:P1 order by DateCreated",
                new OracleParameter("P0", Cicrequestid),
                new OracleParameter("P1", date));
        }

        public void InsertOrUpdate(CicRequestResults Cicrequestresults)
        {
            if (Cicrequestresults.CicRequestResultsId == default(long))
            {
                // New entity
                context.CicRequestResults.Add(Cicrequestresults);
            }
            else
            {
                // Existing entity
                context.Entry(Cicrequestresults).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var Cicrequestresults = context.CicRequestResults.Find(id);
            context.CicRequestResults.Remove(Cicrequestresults);
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

    public interface ICicRequestResultsRepository : IDisposable
    {
        IQueryable<CicRequestResults> All { get; }
        IQueryable<CicRequestResults> AllIncluding(params Expression<Func<CicRequestResults, object>>[] includeProperties);
        CicRequestResults Find(long id);
        DbSqlQuery<CicRequestResults> FindAllByRequest(long Cicrequestid);
        DbSqlQuery<CicRequestResults> FindAllByRequestAndDate(long Cicrequestid, DateTime date);
        void InsertOrUpdate(CicRequestResults Cicrequestresults);
        void Delete(long id);
        void Save();
    }
}