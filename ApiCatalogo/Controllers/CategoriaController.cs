using ApiCatalogo.Context;
using ApiCatalogo.Filters;
using ApiCatalogo.Model;
using ApiCatalogo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace ApiCatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaRepository _repository;
        private readonly ILogger _logger;

        public CategoriaController(ILogger<CategoriaController> logger, ICategoriaRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> GetAll()
        {
            var categorias = _repository.GetCategorias();
            return Ok(categorias);
        }

        //[HttpGet("produtos")]
        //public async Task<ActionResult<IEnumerable<Categoria>>> ProdutosCategoria()
        //{
        //    var categorias = await _repository.GetCategoria(id);
        //    return Ok(categorias);
        //}

        [HttpGet("{id}", Name = "ObterCategoria")]
        public ActionResult<Categoria> GetById(int id)
        {

            var categoria = _repository.GetCategoria(id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id= {id} não encontrada...");
                return NotFound($"Categoria com id= {id} não encontrada...");
            }

            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult<Categoria> Post(Categoria categoria)
        {
            if (categoria is null)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos");
            }
            var categoriaCriada = _repository.Create(categoria);

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = categoria.CategoriaId }, categoriaCriada);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Categoria categoria, int id)
        {
            if (id != categoria.CategoriaId)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos");
            }

            var categoriaAtualizada = _repository.Update(categoria);

            return Ok(categoriaAtualizada);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteProduto(int id)
        {
            var categoria = _repository.GetCategoria(id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id={id} não encontrada...");
                return NotFound($"Categoria com id={id} não encontrada...");
            }

            _repository.Delete(id);

            return NoContent();
        }

    }

}
