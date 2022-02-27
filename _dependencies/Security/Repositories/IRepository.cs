using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OverTheBoard.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        DbContext Context { get; }
        IQueryable<TEntity> Query();
        void Add(TEntity entity);
        void RemoveRange(ICollection<TEntity> list);
        void Remove(TEntity entity);
        bool Save();
    }
}