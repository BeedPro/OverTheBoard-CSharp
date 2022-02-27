using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OverTheBoard.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<TEntity> dbSet;

        public Repository(ApplicationDbContext dbContext)
        {
            dbContext.Set<TEntity>().AsNoTracking();
            _dbContext = dbContext;
            this.dbSet = dbContext.Set<TEntity>();
        }

        public DbContext Context => _dbContext;

        public IQueryable<TEntity> Query()
        {
            return dbSet.AsQueryable();
        }

        public void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }
        public void RemoveRange(ICollection<TEntity> list)
        {
            dbSet.RemoveRange(list);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return GetQuery().ToList();
        }

        public void Remove(TEntity entity)
        {
            if (entity == null) return;

            dbSet.Remove(entity);
        }

        public IQueryable<TEntity> GetQuery()
        {
            return dbSet;
        }

        public bool Save()
        {
            _dbContext.SaveChanges();
            return true;
        }
    }
}