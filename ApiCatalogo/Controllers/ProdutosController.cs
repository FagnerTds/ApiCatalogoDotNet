using ApiCatalogo.Context;
using ApiCatalogo.Model;
using ApiCatalogo.Repositories;
using ApiCatalogo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public ProdutosController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> GetProdutos()
        {
            var produtos = _uow.ProdutoRepository.GetAll();

            if (!produtos.Any())
                return NotFound("Produtos não encontrados");

            return Ok(produtos);
        }

        [HttpGet("porCategoria/{id}")]
        public ActionResult<IEnumerable<Produto>> GetProdutosByCategoria(int id)
        {
            var produtos = _uow.ProdutoRepository.GetProdutos(id);

            if (produtos is null || !produtos.Any())
                return NotFound($"produtos da categiria com id {id}, não encontrado");
            return Ok(produtos);
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<Produto> GetById(int id)
        {
            var produto = _uow.ProdutoRepository.Get(p => p.ProdutoId == id);

            if (produto is null)
                return NotFound("Produto não encontrado");
            return Ok(produto);
        }

        [HttpPost]
        public ActionResult<Produto> Post(Produto produto)
        {
            if (produto is null)
                return BadRequest();
            var produtoCriado = _uow.ProdutoRepository.Create(produto);
            _uow.Commit();
            return new CreatedAtRouteResult("ObterProduto",
                new { id = produtoCriado.ProdutoId }, produtoCriado);
        }

        [HttpPut("{id}")]
        public ActionResult PutProduto(Produto produto, int id)
        {
            if (id != produto.ProdutoId)
                return BadRequest();
            var atualizado = _uow.ProdutoRepository.Update(produto);
            _uow.Commit();
            return Ok(atualizado);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteProduto(int id)
        {
            var produto = _uow.ProdutoRepository.Get(p => p.ProdutoId == id);
            if (produto is null)
               return NotFound($"Categoria com id={id} não encontrada...");
            
            _uow.ProdutoRepository.Delete(produto);
            _uow.Commit();
            return NoContent();
        }

    }
}
