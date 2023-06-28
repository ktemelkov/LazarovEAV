using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LazarovEAV.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class EntityManager : IDisposable
    {
        private AppDbContext ctx;
        internal AppDbContext CTX { get { return this.ctx;  } }


        /// <summary>
        /// 
        /// </summary>
        public EntityManager()
        {
            try
            {
                this.ctx = new AppDbContext();
            }
            catch (Exception)
            {
                this.ctx = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            ctx.Dispose();
        }


        /// <summary>
        /// 
        /// </summary>
        public List<TEntity> loadItems<TEntity>()
            where TEntity : class
        {
            return (this.ctx != null) ? ctx.Set<TEntity>().ToList() : null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        public TEntity getItem<TEntity>(TEntity t)
            where TEntity : class
        {
            if (this.ctx != null && ctx.Set<TEntity>().Contains(t))
                return ctx.Entry(t).Entity;

            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        public TEntity getItemById<TEntity>(long id)
            where TEntity : class
        {            
            if (this.ctx != null)
            {
                var qres = this.ctx.Set<TEntity>().SqlQuery("SELECT * FROM " + typeof(TEntity).Name + " WHERE id=" + id.ToString());
                
                if (qres.Count() == 0)
                    return null;
                
                return qres.First();
            }

            return null;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public List<TEntity> queryItems<TEntity>(string sqlQuery)
            where TEntity : class
        {
            if (this.ctx != null)
            {
                var qres = this.ctx.Set<TEntity>().SqlQuery(sqlQuery);

                return qres.ToList();
            }

            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> getItemsByFilter<TEntity>(Func<TEntity, bool> predicate, int limit = -1)
            where TEntity : class
        {
            if (this.ctx == null)
                throw new Exception("Database context not initialized!");

            try
            {
                var res = this.ctx.Set<TEntity>().Where(predicate);
                    
                if (limit >= 0)
                    res = res.Take(limit);

                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        public void addItem<TEntity>(TEntity t)
            where TEntity : class
        {
            if (this.ctx != null)
                ctx.Set<TEntity>().Add(t);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        public void updateItem<TEntity>(TEntity t)
            where TEntity : class
        {
            if (this.ctx != null)
                ctx.Entry(t).State = EntityState.Modified;
        }


        /// <summary>
        /// 
        /// </summary>
        public void saveChanges()
        {
            if (this.ctx != null)
                ctx.SaveChanges();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        public void deleteItem<TEntity>(TEntity t)
            where TEntity : class
        {
            if (this.ctx != null)
            {
                this.ctx.Set<TEntity>().Remove(t);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        public void deleteItemById<TEntity>(long id)
            where TEntity : class
        {
            if (this.ctx != null)
            {
                this.ctx.Set<TEntity>().SqlQuery("DELETE FROM " + typeof(TEntity).Name + " WHERE Id=" + id.ToString());
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public void deleteItemsByFilter<TEntity>(Func<TEntity, bool> predicate)
            where TEntity : class
        {
            if (this.ctx == null)
                throw new Exception("Database context not initialized!");

            try
            {
                this.ctx.Set<TEntity>().RemoveRange(this.ctx.Set<TEntity>().Where(predicate));
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
