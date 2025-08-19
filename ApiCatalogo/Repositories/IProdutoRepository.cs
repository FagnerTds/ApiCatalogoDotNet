using ApiCatalogo.Model;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repositories;

public interface IProdutoRepository: IRepository<Produto>
{
    PagedList<Produto> GetProdutosPagination(ProdutosParameters produtosParameters);
    PagedList<Produto> GetProdutosFiltroPreco(ProdutoFiltroPreco produtoFiltroPreco);
    IEnumerable<Produto> GetProdutos(int id);

}
