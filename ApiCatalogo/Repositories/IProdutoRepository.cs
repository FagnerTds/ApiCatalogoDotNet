using ApiCatalogo.Model;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repositories;

public interface IProdutoRepository: IRepository<Produto>
{
    PagedList<Produto> GetProdutosPagination(ProdutosParameters produtosParameters);
    IEnumerable<Produto> GetProdutos(int id);
}
