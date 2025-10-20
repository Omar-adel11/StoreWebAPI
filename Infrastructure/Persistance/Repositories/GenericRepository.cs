using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Persistance.Data.Contexts;

namespace Persistance.Repositories
{
    public class GenericRepository<Tkey, TEntity>(StoreDbContext _context) : IGenericRepository<Tkey, TEntity> where TEntity : BaseEntity<Tkey>
    {
        public async Task<IEnumerable<TEntity>> GetAllAsync(bool ChangeTracker = false)
        {
            if(typeof(TEntity) == typeof(Product))
            {
                return ChangeTracker ? await _context.Products.Include(p => p.Brand).Include(P => P.Type).ToListAsync() as IEnumerable<TEntity>:
                                    await _context.Products.Include(p => p.Brand).Include(P => P.Type).AsNoTracking().ToListAsync() as IEnumerable<TEntity>;
            }
            return  ChangeTracker ? await _context.Set<TEntity>().ToListAsync() :
                                    await _context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(Tkey id)
        {
            if (typeof(TEntity) == typeof(Product))
            {
                return await _context.Products.Include(p => p.Brand).Include(P => P.Type).FirstOrDefaultAsync(p=>p.Id == id as int?) as TEntity;
            }
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
             await _context.AddAsync(entity);
        }

      
        public void Update(TEntity entity)
        {
            _context.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<Tkey, TEntity> spec, bool ChangeTracker = false)
        {
            return await ApplySpecifications(spec).ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(ISpecifications<Tkey, TEntity> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }

        private IQueryable<TEntity> ApplySpecifications(ISpecifications<Tkey, TEntity> spec)
        {
            return SpecificationsEvaluator.GetQuery(_context.Set<TEntity>(), spec);
        }
    }



}
