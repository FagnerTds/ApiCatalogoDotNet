using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopularProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert Into Produtos (Nome, Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
                "Values('Coca-cola', 'Refrigerante de cola 350 ml', '5.45', 'coca.jpg', 50, now(), 1)");
            mb.Sql("Insert Into Produtos (Nome, Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
                "Values('Lanche de Pernil', 'Lanche de pernil com 250g', '17.30', 'lanche.jpg', 5, now(), 2)");
            mb.Sql("Insert Into Produtos (Nome, Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" +
                "Values('Pudim ', 'Pudim de leite condensado 100g', '11.99', 'pudim.jpg', 22, now(), 3)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Produtos");
        }
    }
}
