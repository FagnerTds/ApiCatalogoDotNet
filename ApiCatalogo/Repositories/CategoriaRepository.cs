﻿using ApiCatalogo.Context;
using ApiCatalogo.Model;

namespace ApiCatalogo.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly AppDbContext _context;

    public CategoriaRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Categoria> GetCategorias()
    {
        return _context.Categorias.ToList();
    }

    public Categoria GetCategoria(int id)
    {
        return _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);
    }

    public Categoria Create(Categoria categoria)
    {
        if (categoria is null)
            throw new ArgumentException(nameof(categoria));
        
        _context.Categorias.Add(categoria);
        _context.SaveChanges();
        return categoria;
    }

    public Categoria Update(Categoria categoria)
    {
        if (categoria is null)
            throw new ArgumentException(nameof(categoria));
        _context.Entry(categoria).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        _context.SaveChanges();
        return categoria;
    }

    public Categoria Delete(int id)
    {
        var categoria =_context.Categorias.Find(id);
        if (categoria is null)
            throw new ArgumentException(nameof(categoria));
        _context.Categorias.Remove(categoria);
        _context.SaveChanges();
        return categoria;
    }
}
