using ApiCatalogo.Context;
using ApiCatalogo.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{    
    public CategoriaRepository(AppDbContext context) : base(context)
    {
    }
}
