using ApiCatalogo.Model;
using ApiCatalogo.Pagination;
using System.Runtime.InteropServices;

namespace ApiCatalogo.Repositories
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<PagedList<Categoria>> GetParametersAsync(CategoriasParameters categoriasParameters);
        Task<PagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriaFiltroNome categoriasParameters);
    }
}
