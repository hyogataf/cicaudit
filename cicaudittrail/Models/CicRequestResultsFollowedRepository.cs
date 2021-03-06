using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Diagnostics;
using System.Globalization;

namespace cicaudittrail.Models
{
    public class CicRequestResultsFollowedRepository : ICicRequestResultsFollowedRepository
    {
        cicaudittrailContext context = new cicaudittrailContext();

        public IQueryable<CicRequestResultsFollowed> All
        {
            get { return context.CicRequestResultsFollowed; }
        }


        public IQueryable<CicRequestResultsFollowed> AllByRequest(long cicrequestid)
        {
            return context.CicRequestResultsFollowed.ToList().Where(
               r => r.CicRequestId == cicrequestid
               ).OrderByDescending(c => c.DateCreated).AsQueryable();
        }


        public IQueryable<CicRequestResultsFollowed> AllIncluding(params Expression<Func<CicRequestResultsFollowed, object>>[] includeProperties)
        {
            IQueryable<CicRequestResultsFollowed> query = context.CicRequestResultsFollowed.OrderByDescending(c => c.DateCreated);
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public IQueryable<CicRequestResultsFollowed> AllAnsweredIncluding(string[] status, params Expression<Func<CicRequestResultsFollowed, object>>[] includeProperties)
        {
            IQueryable<CicRequestResultsFollowed> query = context.CicRequestResultsFollowed.ToList().Where(
               r => status.Contains(r.Statut)
               ).OrderByDescending(c => c.DateCreated).AsQueryable();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public CicRequestResultsFollowed Find(long id)
        {
            return context.CicRequestResultsFollowed.Find(id);
        }


        public CicRequestResultsFollowed FindByIdAndStatus(long id, string[] status)
        {
            var Cicrequestesultsfollowed = context.CicRequestResultsFollowed.ToList().Where(
                r => r.CicRequestResultsFollowedId == id && status.Contains(r.Statut)
                );
            if (Cicrequestesultsfollowed.Count<CicRequestResultsFollowed>() > 0)
            {
                return Cicrequestesultsfollowed.First<CicRequestResultsFollowed>();
            }
            else
            {
                return null;
            }
        }


        public void InsertOrUpdate(CicRequestResultsFollowed Cicrequestresultsfollowed)
        {
            if (Cicrequestresultsfollowed.CicRequestResultsFollowedId == default(long))
            {
                // New entity
                context.CicRequestResultsFollowed.Add(Cicrequestresultsfollowed);
            }
            else
            {
                // Existing entity
                context.Entry(Cicrequestresultsfollowed).State = EntityState.Modified;
            }
        }


        public void Delete(long id)
        {
            var Cicrequestresultsfollowed = context.CicRequestResultsFollowed.Find(id);
            context.CicRequestResultsFollowed.Remove(Cicrequestresultsfollowed);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public IQueryable<CicRequestResultsFollowed> ExecuteSearch(string cicrequestid, string dateCreated, string userCreated, string comments, string rowContent)
        {
            IFormatProvider culture = new System.Globalization.CultureInfo("fr-FR", true);
            DateTime date = DateTime.Now;
            if (!string.IsNullOrEmpty(dateCreated))
            {
                date = Convert.ToDateTime(dateCreated);
            }
            //Debug.WriteLine("date = " + date);
            long requestid = 0;
            if (!string.IsNullOrEmpty(cicrequestid))
            {
                requestid = Convert.ToInt64(cicrequestid);
            }
            //Debug.WriteLine("requestid = " + requestid); 

            var baseQuery = from m in context.CicRequestResultsFollowed
                            where ((string.IsNullOrEmpty(userCreated) ? true : m.UserCreated.ToLower().Contains(userCreated.ToLower())) &&
                            ((string.IsNullOrEmpty(rowContent) ? true : m.RowContent.ToLower().Contains(rowContent.ToLower())) ||
                             (string.IsNullOrEmpty(comments) ? true : m.Comments.ToLower().Contains(rowContent.ToLower()))) &&
                            ((string.IsNullOrEmpty(cicrequestid) || requestid == 0) ? true : m.CicRequestId == requestid) &&
                             (string.IsNullOrEmpty(dateCreated) ? true : m.DateCreated >= date)
                            )
                            select m;
            return baseQuery.OrderByDescending(c => c.DateCreated).AsQueryable();
        }

    }

    public interface ICicRequestResultsFollowedRepository : IDisposable
    {
        IQueryable<CicRequestResultsFollowed> All { get; }
        IQueryable<CicRequestResultsFollowed> AllByRequest(long cicrequestid);
        IQueryable<CicRequestResultsFollowed> AllIncluding(params Expression<Func<CicRequestResultsFollowed, object>>[] includeProperties);

        //Recupere tous les CicRequestResultsFollowed selon une liste de statuts
        IQueryable<CicRequestResultsFollowed> AllAnsweredIncluding(string[] status, params Expression<Func<CicRequestResultsFollowed, object>>[] includeProperties);
        CicRequestResultsFollowed Find(long id);
        CicRequestResultsFollowed FindByIdAndStatus(long id, string[] status);
        IQueryable<CicRequestResultsFollowed> ExecuteSearch(string cicrequestid, string dateCreated, string userCreated, string comments, string rowContent);
        void InsertOrUpdate(CicRequestResultsFollowed Cicrequestresultsfollowed);
        void Delete(long id);
        void Save();
    }
}