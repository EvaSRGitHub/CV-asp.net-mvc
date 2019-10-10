using CVApp.Common.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVApp.Data
{
    public class Repository<TEntity> : IRepository<TEntity>
        //, IDisposable
        where TEntity: class
    {
        private readonly CVAppDbContext db;
        private DbSet<TEntity> dbset;
        public Repository(CVAppDbContext db)
        {
            this.db = db;
            this.dbset = this.db.Set<TEntity>();
        }

        public IQueryable<TEntity> All()
        {
            return this.dbset;
        }

        public void Delete(TEntity entity)
        {
            this.dbset.Remove(entity);
        }

        //public void Dispose()
        //{
        //    this.db.Dispose();
        //}

        public Task<int> SaveChangesAsync()
        {
            return this.db.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            var entry = this.db.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                this.dbset.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public Task AddAsync(TEntity entity)
        {
            return this.dbset.AddAsync(entity);
        }

        public Task<TEntity> GetByIdAsync(params object[] id)
        {
            return this.dbset.FindAsync(id);
        }
    }
}
