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

    public IEnumerable<Produto> GetProdutos(int id)
    {
        return GetAll().Where(c => c.CategoriaId == id);
    }

    public PagedList<Produto> GetProdutosFiltroPreco(ProdutoFiltroPreco produtoFiltroPreco)
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
        var produtosFiltrados = PagedList<Produto>.ToPagedList(produtos, produtoFiltroPreco.pageNumber,
                                                                produtoFiltroPreco.pageSize);
        return produtosFiltrados;
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
