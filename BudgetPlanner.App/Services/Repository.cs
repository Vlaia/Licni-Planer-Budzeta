using Microsoft.EntityFrameworkCore;
using BudgetPlanner.App.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BudgetPlanner.App.Services
{
    /// <summary>
    /// Generička implementacija Repository pattern-a.
    /// Implementira IRepository interfejs.
    /// </summary>
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly BudgetDbContext _context;
        protected readonly DbSet<T> _dbSet;
        
        public Repository()
        {
            _context = BudgetDbContext.Instance;
            _dbSet = _context.Set<T>();
        }
        
        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        
        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
        
        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveChangesAsync();
            return entity;
        }
        
        public virtual async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await SaveChangesAsync();
        }
        
        public virtual async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await SaveChangesAsync();
        }
        
        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            if (predicate == null)
                return await _dbSet.CountAsync();
            return await _dbSet.CountAsync(predicate);
        }
        
        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }
        
        public virtual async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
