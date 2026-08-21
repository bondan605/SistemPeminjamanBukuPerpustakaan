using System.Linq.Expressions;
using LibraryManagementSystem.Base;

namespace LibraryManagementSystem.Application.Interfaces.Repositories;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    IQueryable<T> GetAll();
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<int> CountAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}