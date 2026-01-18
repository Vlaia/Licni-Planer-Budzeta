using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BudgetPlanner.App.Services
{
    /// <summary>
    /// Generički interfejs za Repository pattern.
    /// Apstrakcija data access sloja - Strukturni dizajn šablon.
    /// </summary>
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task SaveChangesAsync();
    }
}
