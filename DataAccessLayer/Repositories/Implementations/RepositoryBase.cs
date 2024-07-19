using DataAccessLayer.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccessLayer.Repositories.Implementations
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected EventContext _eventContext;
        public RepositoryBase(EventContext eventContext) => _eventContext = eventContext;
        public IQueryable<T> FindAll(bool trackChanges)
        {
            return !trackChanges ? _eventContext.Set<T>().AsNoTracking() : _eventContext.Set<T>();
        }
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            return !trackChanges ? _eventContext.Set<T>().Where(expression).AsNoTracking()
                : _eventContext.Set<T>().Where(expression);
        }
        public void Create(T entity) => _eventContext.Set<T>().Add(entity);
        public void Update(T entity) => _eventContext.Set<T>().Update(entity);
        public void Delete(T entity) => _eventContext.Set<T>().Remove(entity);
    }
}
