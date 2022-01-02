using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverTheBoard.Data.Repositories
{
    public interface ISecurityRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Query();
        void Add(TEntity entity);
        void RemoveRange(ICollection<TEntity> list);
        void Remove(TEntity entity);
    }
}
