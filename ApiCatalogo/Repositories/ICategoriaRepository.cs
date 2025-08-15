using ApiCatalogo.Model;
using ApiCatalogo.Pagination;
using System.Runtime.InteropServices;

namespace ApiCatalogo.Repositories
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        PagedList<Categoria> GetParameters(CategoriasParameters categoriasParameters);
    }
}
