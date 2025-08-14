using System.Linq.Expressions;

namespace ApiCatalogo.Repositories;

public interface IRepository<T>
{
    IQueryable<T> GetAll();
    T? Get(Expression<Func<T, bool >> predicate);
    T Create(T entity);
    T Update(T entity);
    T Delete(T entity);
}
