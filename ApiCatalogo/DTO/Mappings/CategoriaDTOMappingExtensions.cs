using ApiCatalogo.Model;
using System.Runtime.CompilerServices;

namespace ApiCatalogo.DTO.Mappings;

public static class CategoriaDTOMappingExtensions
{
    public static CategoriaDTO? ToCategoriaDto(this Categoria categoria)
    {
        if (categoria is null)
            return null;
        return new CategoriaDTO()
        {
            CategoriaId = categoria.CategoriaId,
            Nome = categoria.Nome,
            ImagemUrl = categoria.ImagemUrl,
        };
    }

    public static Categoria? ToCategoria(this CategoriaDTO categoriaDto)
    {
        if(categoriaDto is null)
            return null;
        return new Categoria()
        {
            CategoriaId = categoriaDto.CategoriaId,
            Nome = categoriaDto.Nome,
            ImagemUrl = categoriaDto.ImagemUrl
        };
    }

    public static IEnumerable<CategoriaDTO> toListCategoriaDto(this IEnumerable<Categoria> categoria)
    {
        if (!categoria.Any() || categoria is null)
            return new List<CategoriaDTO>();

        return categoria.Select(categoria => new CategoriaDTO
        {
            CategoriaId = categoria.CategoriaId,
            Nome = categoria.Nome,
            ImagemUrl = categoria.ImagemUrl,
        }).ToList();
    }
}
