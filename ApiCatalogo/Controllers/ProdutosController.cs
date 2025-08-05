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
        private readonly IProdutoRepository _repository;

        public ProdutosController(IProdutoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IQueryable<Produto>> GetProdutos()
        {
            var produtos = _repository.GetProdutos().ToList();

            if (!produtos.Any())
                return NotFound("Produtos não encontrados");

            return Ok(produtos);
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<Produto> GetById(int id)
        {
            var produto = _repository.GetProduto(id);

            if (produto is null)
                return NotFound("Produto não encontrado");
            return Ok(produto);
        }

        [HttpPost]
        public ActionResult<Produto> Post(Produto produto)
        {
            if (produto is null)
                return BadRequest();
            var produtoCriado = _repository.CreateProduto(produto);
            return new CreatedAtRouteResult("ObterProduto",
                new { id = produtoCriado.ProdutoId }, produtoCriado);
        }

        [HttpPut("{id}")]
        public ActionResult PutProduto(Produto produto, int id)
        {
            if (id != produto.ProdutoId)
                return BadRequest();
            var atualizado =_repository.UpdateProduto(produto);
            if(atualizado)
                return Ok(produto);
            return StatusCode(500, $"falha ao atualizar o produto de id = {id}"); 
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteProduto(int id)
        {
            var deletado = _repository.DeleteProduto(id);
            if (deletado)
                return NoContent();
            return StatusCode(500, $"falha ao Excluuir o produto de id = {id}");

        }

    }
}
