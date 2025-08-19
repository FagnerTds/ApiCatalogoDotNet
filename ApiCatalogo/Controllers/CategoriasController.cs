using ApiCatalogo.Context;
using ApiCatalogo.DTO;
using ApiCatalogo.DTO.Mappings;
using ApiCatalogo.Filters;
using ApiCatalogo.Model;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        public ActionResult<IEnumerable<CategoriaDTO>> GetAll()
        {
            var categorias = _uof.CategoriaRepository.GetAll();

            var categoriasDto = categorias.ToListCategoriaDto();
            return Ok(categoriasDto);
        }

        [HttpGet("pagination")]
        public ActionResult<IEnumerable<CategoriaDTO>> GetAllParameters([FromQuery] CategoriasParameters categoriasParameters)
        {
            var categorias = _uof.CategoriaRepository.GetParameters(categoriasParameters);
            return ObterFIltroCategoria(categorias);
        }

        private ActionResult<IEnumerable<CategoriaDTO>> ObterFIltroCategoria(PagedList<Categoria> categorias)
        {
            var metadata = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevious
            };
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
            var categoriasDto = categorias.ToListCategoriaDto();
            return Ok(categoriasDto);
        }

        [HttpGet("filter/nome/pagination")]
        public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriaFiltroNome(
                                                    [FromQuery] CategoriaFiltroNome categoriaFiltro)
        {
            var categorias = _uof.CategoriaRepository.GetCategoriasFiltroNome(categoriaFiltro);
            return ObterFIltroCategoria(categorias);
            
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> GetById(int id)
        {

            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id= {id} não encontrada...");
                return NotFound($"Categoria com id= {id} não encontrada...");
            }
            var categoriaDto = categoria.ToCategoriaDto();

            return Ok(categoriaDto);
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos");
            }
            var categoria = categoriaDto.ToCategoria();
            var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();

            var novaCategoriaDto = categoriaCriada.ToCategoriaDto();
            return new CreatedAtRouteResult("ObterCategoria",
                new { id = categoriaCriada.CategoriaId }, categoriaCriada);
        }

        [HttpPut("{id}")]
        public ActionResult<CategoriaDTO> Put(CategoriaDTO categoriaDto, int id)
        {
            if (id != categoriaDto.CategoriaId)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos");
            }

            var categoria = categoriaDto.ToCategoria();

            var categoriaAtualizada = _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();

            var novaCategoriaDto = categoriaAtualizada.ToCategoriaDto();

            return Ok(novaCategoriaDto);
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
