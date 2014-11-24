using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace cicaudittrail.Models
{
    public class CicFollowedPropertiesValuesRepository : ICicFollowedPropertiesValuesRepository
    {
        cicaudittrailContext context = new cicaudittrailContext();

        public IQueryable<CicFollowedPropertiesValues> All
        {
            get { return context.CicFollowedPropertiesValues; }
        }

        public IQueryable<CicFollowedPropertiesValues> AllIncluding(params Expression<Func<CicFollowedPropertiesValues, object>>[] includeProperties)
        {
            IQueryable<CicFollowedPropertiesValues> query = context.CicFollowedPropertiesValues;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public CicFollowedPropertiesValues Find(long id)
        {
            return context.CicFollowedPropertiesValues.Find(id);
        }


        public CicFollowedPropertiesValues FindByRequestFollowedAndProperty(long RequestFollowedId, string property)
        {
            var CicFollowedPropertiesValues = context.CicFollowedPropertiesValues.ToList().Where(
                    r => r.CicRequestResultsFollowedId == RequestFollowedId && r.Property == property
                    );
            if (CicFollowedPropertiesValues.Count<CicFollowedPropertiesValues>() > 0)
            {
                return CicFollowedPropertiesValues.First<CicFollowedPropertiesValues>();
            }
            else
            {
                return null;
            }
        }


        public IQueryable<CicFollowedPropertiesValues> FindAllByRequestFollowed(long RequestFollowedId)
        {
            return context.CicFollowedPropertiesValues.ToList().Where(
                    r => r.CicRequestResultsFollowedId == RequestFollowedId
                    ).AsQueryable();
        }


        public void InsertOrUpdate(CicFollowedPropertiesValues cicfollowedpropertiesvalues)
        {
            if (cicfollowedpropertiesvalues.CicFollowedPropertiesValuesId == default(long))
            {
                // New entity
                context.CicFollowedPropertiesValues.Add(cicfollowedpropertiesvalues);
            }
            else
            {
                // Existing entity
                context.Entry(cicfollowedpropertiesvalues).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var cicfollowedpropertiesvalues = context.CicFollowedPropertiesValues.Find(id);
            context.CicFollowedPropertiesValues.Remove(cicfollowedpropertiesvalues);
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

    public interface ICicFollowedPropertiesValuesRepository : IDisposable
    {
        IQueryable<CicFollowedPropertiesValues> All { get; }
        IQueryable<CicFollowedPropertiesValues> AllIncluding(params Expression<Func<CicFollowedPropertiesValues, object>>[] includeProperties);
        CicFollowedPropertiesValues Find(long id);
        CicFollowedPropertiesValues FindByRequestFollowedAndProperty(long id, string property);
        IQueryable<CicFollowedPropertiesValues> FindAllByRequestFollowed(long RequestFollowedId);
        void InsertOrUpdate(CicFollowedPropertiesValues cicfollowedpropertiesvalues);
        void Delete(long id);
        void Save();
    }
}