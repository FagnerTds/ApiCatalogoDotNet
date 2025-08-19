using ApiCatalogo.Context;
using ApiCatalogo.Model;
using ApiCatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{    
    public CategoriaRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<PagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriaFiltroNome categoriasParameters)
    {
        var categorias = GetAll();

        if (!String.IsNullOrEmpty(categoriasParameters.Nome))
        {
            categorias = categorias.Where(c => c.Nome.Contains(categoriasParameters.Nome));
        }
        var categoriasPaginadas = await PagedList<Categoria>.ToPagedListAsync(categorias, categoriasParameters.pageNumber,
                                                                    categoriasParameters.pageSize);
        return categoriasPaginadas;
    }

    public async Task<PagedList<Categoria>> GetParametersAsync(CategoriasParameters categoriasParameters)
    {
        var categorias = GetAll().OrderBy(c => c.CategoriaId);
        var categoriasOrdenadas = await PagedList<Categoria>.ToPagedListAsync(categorias, 
            categoriasParameters.pageNumber, categoriasParameters.pageSize);
        return categoriasOrdenadas;
    }
}
