using ApiCatalogo.Model;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repositories;

public interface IProdutoRepository: IRepository<Produto>
{
    Task<PagedList<Produto>> GetProdutosPaginationAsync(ProdutosParameters produtosParameters);
    Task<PagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutoFiltroPreco produtoFiltroPreco);
    Task<IEnumerable<Produto>> GetProdutosAsync(int id);

}
