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
    public class CicRequestRepository : ICicRequestRepository
    {
        cicaudittrailContext context = new cicaudittrailContext();

        public cicaudittrailContext GetContext
        {
            get
            {
                return this.context;
            }
        }


        public IQueryable<CicRequest> All
        {
            get
            {

                //get { return context.CicRequest.SqlQuery("select * from ROLE where CODE='OP5_10M'").AsQueryable(); }

                return context.CicRequest.ToList().Where(
                r => r.IsDeleted == 0
                ).AsQueryable();

                /*   var sql = "select code from Cicrequest";
                   var results = context.Database.SqlQuery<String>(sql);
                   foreach (var req in results)
                   {
                       Trace.WriteLine("req raw sql= " + req); 

                   }

                 return context.CicRequest;*/
            }
        }

        public IQueryable<CicRequest> AllIncluding(params Expression<Func<CicRequest, object>>[] includeProperties)
        {
            IQueryable<CicRequest> query = context.CicRequest;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public CicRequest Find(long id)
        {
            return context.CicRequest.Find(id);
        }

        public IEnumerable<Object> ExecuteRequest(string request)
        {
            return context.Database.SqlQuery<Object>(request);
        }

        public void InsertOrUpdate(CicRequest Cicrequest)
        {
            if (Cicrequest.CicRequestId == default(long))
            {
                // New entity
                context.CicRequest.Add(Cicrequest);
            }
            else
            {
                // Existing entity
                context.Entry(Cicrequest).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var Cicrequest = context.CicRequest.Find(id);
            Cicrequest.IsDeleted = 1;
            InsertOrUpdate(Cicrequest);

            // context.CicRequest.Remove(Cicrequest);
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

    public interface ICicRequestRepository : IDisposable
    {
        cicaudittrailContext GetContext { get; }
        IQueryable<CicRequest> All { get; }
        IQueryable<CicRequest> AllIncluding(params Expression<Func<CicRequest, object>>[] includeProperties);
        CicRequest Find(long id);
        IEnumerable<Object> ExecuteRequest(string request);
        void InsertOrUpdate(CicRequest Cicrequest);
        void Delete(long id);
        void Save();
    }
}