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

    public PagedList<Categoria> GetCategoriasFiltroNome(CategoriaFiltroNome categoriasParameters)
    {
        var categorias = GetAll();

        if (!String.IsNullOrEmpty(categoriasParameters.Nome))
        {
            categorias = categorias.Where(c => c.Nome.Contains(categoriasParameters.Nome));
        }
        var categoriasPaginadas = PagedList<Categoria>.ToPagedList(categorias, categoriasParameters.pageNumber,
                                                                    categoriasParameters.pageSize);
        return categoriasPaginadas;
    }

    public PagedList<Categoria> GetParameters(CategoriasParameters categoriasParameters)
    {
        var categorias = GetAll().OrderBy(c => c.CategoriaId);
        var categoriasOrdenadas = PagedList<Categoria>.ToPagedList(categorias, 
            categoriasParameters.pageNumber, categoriasParameters.pageSize);
        return categoriasOrdenadas;
    }
}
