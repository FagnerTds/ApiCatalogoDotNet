using ApiCatalogo.Context;
using System.Linq.Expressions;

namespace ApiCatalogo.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>();
        }       

        public T? Get(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }


        public T Create(T entity)
        {
            throw new NotImplementedException();
        }

        public T Update(T entity)
        {
            throw new NotImplementedException();
        }

        public T Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
