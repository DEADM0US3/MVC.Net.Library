using FernandoLibrary.Persistance;
using FernandoLibrary.Persistance.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FernandoLibrary.Controllers;

public class UsuariosController : Controller
{
    private readonly LibraryDbContext _context;

    public UsuariosController(LibraryDbContext context) => _context = context;

    public async Task<IActionResult> Index(string searchString)
    {
        var usuarios = _context.Usuarios.AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            usuarios = usuarios.Where(u =>
                u.Nombre.Contains(searchString) ||
                u.Email.Contains(searchString));
        }
            
        ViewData["SearchString"] = searchString;

        return View(await usuarios.ToListAsync());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) return NotFound();
        
        return View(usuario);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Usuario usuario)
    {
        if (id != usuario.Id) return NotFound();

        _context.Update(usuario);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(m => m.Id == id);
        if (usuario == null) return NotFound();

        return View(usuario);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}

