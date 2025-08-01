﻿using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiCatalogo.Model;

public class Categoria
{
    public Categoria()
    {
        Produtos = new Collection<Produto>();
    }

    [Key]
    public int CategoriaId { get; set; }

    [Required(ErrorMessage = "O Nome é obrigatório")]
    [StringLength(200), ]
    public string? Nome { get; set; }

    [StringLength(300)]
    public string? ImagemUrl { get; set; }

    [JsonIgnore]
    public ICollection<Produto> Produtos { get; set; }
}
