using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ArteEva.Data;
using ArteEva.Models;

namespace ArteEva.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseModel
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetByIDWithTrackingAsync(int id)
        {
            var res= await _dbSet.AsTracking().FirstOrDefaultAsync(s=>s.Id==id && !s.IsDeleted);
            return res;
        }
        public async Task<T> GetByIdAsync(int id)
        {
            var res = await _dbSet.FirstOrDefaultAsync(s=>s.Id == id && !s.IsDeleted);
            return res;
        }

        public   IQueryable<T> GetAllAsync()
        {
            return _dbSet.Where(s => !s.IsDeleted);
        }

        public  IQueryable<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return  _dbSet.Where(predicate);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public async Task UpdateAsync(T entity)
        {
              _dbSet.Update(entity);
            
        }
        public async Task Delete(int id)
        {
            var res = await GetByIDWithTrackingAsync(id);
            if(res!= null)
            res.IsDeleted = true;
        }
        public async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _dbSet
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetPagedAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize)
        {
            return await _dbSet
                .Where(predicate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        protected IQueryable<T> Query()
        {
            return _context.Set<T>();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        #region extraMethods
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
                return await _dbSet.CountAsync();

            return await _dbSet.CountAsync(predicate);
        }
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }


        public void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }


        #endregion
    }
}
