using ApiCatalogo.Context;
using ApiCatalogo.DTO;
using ApiCatalogo.Model;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ApiCatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet("pagination")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetPagination([FromQuery]ProdutosParameters produtosParam)
        {
            var produtos = _uow.ProdutoRepository.GetProdutosPagination(produtosParam);

            return ObterProdutos(produtos);

        }

        [HttpGet("filter/preco/pagination")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosFilterPreco([FromQuery]ProdutoFiltroPreco produtoFiltroPreco)
        {
            var produtos = _uow.ProdutoRepository.GetProdutosFiltroPreco(produtoFiltroPreco);
            return ObterProdutos(produtos);
        }

        private ActionResult<IEnumerable<ProdutoDTO>> ObterProdutos(PagedList<Produto> produtos)
        {
            var metadata = new
            {
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return Ok(produtosDto);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutos()
        {
            var produtos = _uow.ProdutoRepository.GetAll();

            if (!produtos.Any())
                return NotFound("Produtos não encontrados");
            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDto);
        }

        [HttpGet("porCategoria/{id}")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosByCategoria(int id)
        {
            var produtos = _uow.ProdutoRepository.GetProdutos(id);

            if (produtos is null || !produtos.Any())
                return NotFound($"produtos da categiria com id {id}, não encontrado");
            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return Ok(produtosDto);
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> GetById(int id)
        {
            var produto = _uow.ProdutoRepository.Get(p => p.ProdutoId == id);

            if (produto is null)
                return NotFound("Produto não encontrado");
            var produtoDto = _mapper.Map<ProdutoDTO>(produto);
            return Ok(produtoDto);
        }

        [HttpPost]
        public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDto)
        {
            if (produtoDto is null)
                return BadRequest();
            var produto = _mapper.Map<Produto>(produtoDto);

            var produtoCriado = _uow.ProdutoRepository.Create(produto);
            _uow.Commit();

            var novoProdutoDto = _mapper.Map<ProdutoDTO>(produtoCriado);

            return new CreatedAtRouteResult("ObterProduto",
                new { id = novoProdutoDto.ProdutoId }, novoProdutoDto);
        }

        [HttpPatch("{id}/UpdatePartial")]
        public ActionResult<ProdutoDTOUpdateResponse> Patch(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDTO)
        {
            if (patchProdutoDTO is null || id <= 0)
                return BadRequest();

            var produto = _uow.ProdutoRepository.Get(c => c.ProdutoId == id);
            if (produto is null)
                return NotFound();

            var produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);

            patchProdutoDTO.ApplyTo(produtoUpdateRequest, ModelState);
            if (!ModelState.IsValid || !TryValidateModel(produtoUpdateRequest))
                return BadRequest(ModelState);
            _mapper.Map(produtoUpdateRequest, produto);

            _uow.ProdutoRepository.Update(produto);
            _uow.Commit();

            return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));

        }
            

        [HttpPut("{id}")]
        public ActionResult<ProdutoDTO> PutProduto(ProdutoDTO produtoDto, int id)
        {
            if (id != produtoDto.ProdutoId)
                return BadRequest();

            var produto = _mapper.Map<Produto>(produtoDto);

            var atualizado = _uow.ProdutoRepository.Update(produto);
            _uow.Commit();
            
            var novoProdutoDto =_mapper.Map<ProdutoDTO>(atualizado);
            return Ok(novoProdutoDto);
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
