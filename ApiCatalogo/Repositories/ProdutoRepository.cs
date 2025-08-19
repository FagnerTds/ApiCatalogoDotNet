using ApiCatalogo.Context;
using ApiCatalogo.Model;
using ApiCatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{

    public ProdutoRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Produto>> GetProdutosAsync(int id)
    {
        return await GetAll().Where(c => c.CategoriaId == id).ToListAsync();
    }

    public async Task<PagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutoFiltroPreco produtoFiltroPreco)
    {
        var produtos = GetAll();
        if (produtoFiltroPreco.Preco.HasValue && !string.IsNullOrEmpty(produtoFiltroPreco.PrecoCriterio))
        {
            if (produtoFiltroPreco.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
                produtos = produtos.Where(p => p.Preco > produtoFiltroPreco.Preco.Value).OrderBy(p => p.Preco);
            if (produtoFiltroPreco.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
                produtos = produtos.Where(p => p.Preco < produtoFiltroPreco.Preco.Value).OrderBy(p => p.Preco);
            if (produtoFiltroPreco.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
                produtos = produtos.Where(p => p.Preco == produtoFiltroPreco.Preco.Value).OrderBy(p => p.Preco);
        }
        var produtosFiltrados = await PagedList<Produto>.ToPagedListAsync(produtos, produtoFiltroPreco.pageNumber,
                                                                produtoFiltroPreco.pageSize);
        return produtosFiltrados;
    }

    public async Task<PagedList<Produto>> GetProdutosPaginationAsync(ProdutosParameters produtosParameters)
    {
        var produtos = GetAll().OrderBy(p => p.ProdutoId);
        var produtosOrdenados = await PagedList<Produto>.ToPagedListAsync(produtos, produtosParameters.pageNumber, produtosParameters.pageSize);
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
