using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities;
using Persistance.Data.Contexts;

namespace Persistance.Repositories
{
    public class UnitOfWork(StoreDbContext _context) : IUnitOfWork
    {
        //private Dictionary<string, object> _repositories = new Dictionary<string, object>() ;
        private ConcurrentDictionary<string, object> _repositories = new ConcurrentDictionary<string, object>();

        //public IGenericRepository<TKey, TEntity> GetRepository<TKey, TEntity>() where TEntity : BaseEntity<TKey>
        //{
        //    var type = typeof(TEntity).Name;
        //    if (!_repositories.ContainsKey(type))
        //    {
        //        var repositoryInstance = new GenericRepository<TKey, TEntity>(_context);
        //        _repositories.Add(type, repositoryInstance);
        //    }

        //    return (IGenericRepository < TKey, TEntity >) _repositories[type];
        //}

        public IGenericRepository<TKey, TEntity> GetRepository<TKey, TEntity>() where TEntity : BaseEntity<TKey>
        {
            return (IGenericRepository<TKey, TEntity>)_repositories.GetOrAdd(typeof(TEntity).Name, new GenericRepository<TKey,TEntity>(_context));
            
               
        }

        public async Task<int> SaveChangesAsync()
        {
            return await  _context.SaveChangesAsync();
        }
    }
}
