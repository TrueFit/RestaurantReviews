using System;
using System.Collections.Generic;
using System.Linq;
using RestaurantReviews.Core;

namespace RestaurantReviews.Data
{
    public abstract class GenericRepositoryEF<T> where T: BaseModel, new()
    {
        internal RRContext Context
        {
            get
            {
                return new RRContext();
            }
        }

        public virtual int? AddEntity(T entity)
        {
            int? entityID = null;
            using (RRContext ctx = Context)
            {
                ctx.Set<T>().Add(entity);
                ctx.SaveChanges();
                entityID = entity.Id;
            }
            return entityID;
        }

        public virtual IEnumerable<T> GetAll()
        {
            IEnumerable<T> objects = null;
            using (RRContext ctx = Context)
            {
                objects = ctx.Set<T>().ToList();
            }
            return objects;
        }

        public virtual IEnumerable<T> Filter(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> objects = null;
            using (RRContext ctx = Context)
            {
                objects = ctx.Set<T>().Where(predicate).ToList();
            }
            return objects;
        }

        public virtual T GetById(int entityId)
        {
            T entity = default(T);
            using (RRContext ctx = Context)
            {
                entity = ctx.Set<T>().Find(entityId);
            }
            return entity;
        }

        public virtual void UpdateEntity(T entity)
        {
            using (RRContext ctx = Context)
            {
                ctx.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public virtual void DeleteEntity(T entity)
        {
            using (RRContext ctx = Context)
            {
                ctx.Set<T>().Remove(entity);
                ctx.SaveChanges();
            }
        }
    }
}
