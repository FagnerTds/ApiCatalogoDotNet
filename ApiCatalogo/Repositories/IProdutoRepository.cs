using ApiCatalogo.Model;

namespace ApiCatalogo.Repositories
{
    public interface IProdutoRepository
    {
        IQueryable<Produto> GetProdutos();
        Produto GetProduto(int id);
        Produto CreateProduto(Produto produto);
        bool UpdateProduto(Produto produto);
        bool DeleteProduto(int id);
    }
}
