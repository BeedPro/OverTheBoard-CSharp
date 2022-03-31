using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OverTheBoard.Data.Repositories
{
    public class SecurityRepository<TEntity> : ISecurityRepository<TEntity> where TEntity : class
    {
        private readonly SecurityDbContext _dbContext;
        private readonly DbSet<TEntity> dbSet;
        
        public SecurityRepository(SecurityDbContext dbContext)
        {
            _dbContext = dbContext;
            this.dbSet = dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> Query()
        {
            return dbSet.AsQueryable();
        }

        public void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public bool Save()
        {
            _dbContext.SaveChanges();
            return true;
        }
    }
}