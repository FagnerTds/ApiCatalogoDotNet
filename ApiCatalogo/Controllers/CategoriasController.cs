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
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly ILogger _logger;

        public CategoriasController(ILogger<CategoriasController> logger, IUnitOfWork uof)
        {
            _logger = logger;
            _uof = uof;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> GetAll()
        {
            var categorias = _uof.CategoriaRepository.GetAll();
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

            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

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
            var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = categoriaCriada.CategoriaId }, categoriaCriada);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Categoria categoria, int id)
        {
            if (id != categoria.CategoriaId)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos");
            }

            var categoriaAtualizada = _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();

            return Ok(categoriaAtualizada);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteProduto(int id)
        {
            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id={id} não encontrada...");
                return NotFound($"Categoria com id={id} não encontrada...");
            }

            _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();


            return NoContent();
        }

    }

}
