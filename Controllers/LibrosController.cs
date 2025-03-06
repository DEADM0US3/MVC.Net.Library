using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FernandoLibrary.Persistance;
using FernandoLibrary.Persistance.Entities;

namespace FernandoLibrary.Controllers;


public class LibrosController : Controller
{
    private readonly LibraryDbContext _context;

    public LibrosController(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string searchString)
    {
        var libros = _context.Libros
            .Include(l => l.Autor)
            .Include(l => l.Categoria)
            .Select(l => new LibroViewModel
            {
                Id = l.Id,
                Titulo = l.Titulo,
                Autor = l.Autor,
                Categoria = l.Categoria,
                Cantidad = l.Cantidad,
                Prestados = _context.Prestamos.Count(p => p.LibroId == l.Id && p.FechaDevolucion == null)
            })
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            libros = libros.Where(l =>
                l.Titulo.Contains(searchString) ||
                l.Autor.Nombre.Contains(searchString) ||
                l.Categoria.Nombre.Contains(searchString));
        }

        ViewData["SearchString"] = searchString;
        return View(await libros.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var libro = await _context.Libros
            .Include(l => l.Autor)
            .Include(l => l.Categoria)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (libro == null)
        {
            return NotFound();
        }

        return View(libro);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Autores = await _context.Autores.ToListAsync();
        ViewBag.Categorias = await _context.Categorias.ToListAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Libro libro)
    {
        _context.Add(libro);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var libro = await _context.Libros.FindAsync(id);
        if (libro == null)
        {
            return NotFound();
        }

        ViewBag.Autores = await _context.Autores.ToListAsync();
        ViewBag.Categorias = await _context.Categorias.ToListAsync();
        return View(libro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Libro libro)
    {
        if (id != libro.Id)
        {
            return NotFound();
        }

        _context.Update(libro);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var libro = await _context.Libros
            .Include(l => l.Autor)
            .Include(l => l.Categoria)
            .FirstOrDefaultAsync(m => m.Id == id);

        
        if (libro == null)
        {
            return NotFound();
        }

        return View(libro);  
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var libro = await _context.Libros.FindAsync(id);
        _context.Libros.Remove(libro);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}

public class LibroViewModel
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public Autor Autor { get; set; }
    public Categoria Categoria { get; set; }
    public int Cantidad { get; set; }
    public int Prestados { get; set; }
}
