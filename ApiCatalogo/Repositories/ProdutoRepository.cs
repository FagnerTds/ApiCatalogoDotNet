using ApiCatalogo.Context;
using ApiCatalogo.Model;
using ApiCatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{       

    public ProdutoRepository(AppDbContext context): base (context)
    {
    }

    public IEnumerable<Produto> GetProdutos(int id)
    {
        return GetAll().Where(c => c.CategoriaId == id);
    }

    public PagedList<Produto> GetProdutosPagination(ProdutosParameters produtosParameters)
    {
        var produtos = GetAll().OrderBy(p => p.ProdutoId).AsQueryable();
        var produtosOrdenados = PagedList<Produto>.ToPagedList(produtos, produtosParameters.pageNumber, produtosParameters.pageSize);
        return produtosOrdenados;

    }

    //public IEnumerable<Produto> GetProdutosPagination(ProdutosParameters produtosParameters)
    //{
    //    return GetAll()
    //        .OrderBy(p => p.Nome)
    //        .Skip((produtosParameters.pageNumber - 1) * produtosParameters.pageSize)
    //        .Take(produtosParameters.pageSize).ToList();
    //}


}
