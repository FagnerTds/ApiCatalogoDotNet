using ApiCatalogo.Context;
using ApiCatalogo.Model;
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
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public ProdutosController(AppDbContext appDbContext, IConfiguration configuration)
        {
            _context = appDbContext;
            _configuration = configuration;
        }

        [HttpGet("configuration")]
        public ActionResult<string> Get()
        {
            var valor1 = _configuration["chave"];
            var valor2 = _configuration["chave2"];
            var secao = _configuration["secao:chave2"];
            return $"Valor1 = {valor1} \n Valor2 = {valor2} \n Seção: chave2 => {secao}";
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            var produtos = await _context.Produtos.AsNoTracking().ToListAsync();

            if (!produtos.Any())
                return NotFound("Produtos não encontrados");

            return Ok(produtos);
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public async Task<ActionResult<Produto>> GetById(int id)
        {
            var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.ProdutoId == id);

            if (produto is null)
                return NotFound("Produto não encontrado");
            return Ok(produto);
        }

        [HttpPost]
        public async Task<ActionResult<Produto>> Post(Produto produto)
        {
            if (produto is null)
                return BadRequest();
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutProduto(Produto produto, int id)
        {
            if (id != produto.ProdutoId)
                return BadRequest();

            _context.Entry(produto).State = EntityState.Modified;
            await _context.SaveChangesAsync();


            return Ok(produto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto is null)
                return NotFound("Produto não encontrado");

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
