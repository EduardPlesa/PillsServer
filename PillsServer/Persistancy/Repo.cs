using System;
using System.Linq.Expressions;

namespace PillsServer.Persistancy
{
    public interface IRepo<T>
    {
        IQueryable<T> Query(Expression<Func<T, bool>>? whereFilter = null);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
    public class Repo<T>(PillsDbContext appContext) : IRepo<T> where T : class
    {
        private readonly PillsDbContext context = appContext;
        public void Add(T entity)
            => context.Add(entity);

        public IQueryable<T> Query(Expression<Func<T, bool>>? whereFilter = null)
            => whereFilter == null ? context.Set<T>() : context.Set<T>().Where(whereFilter);

        public void Remove(T entity)
            => context.Remove(entity);

        public void RemoveRange(IEnumerable<T> entities)
            => context.RemoveRange(entities);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => await context.SaveChangesAsync(cancellationToken);
    }

}
