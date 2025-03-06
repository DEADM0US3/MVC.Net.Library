using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FernandoLibrary.Persistance;
using FernandoLibrary.Persistance.Entities;

namespace FernandoLibrary.Controllers;


    public class CategoriasController : Controller
    {
        private readonly LibraryDbContext _context;

        public CategoriasController(LibraryDbContext context) => _context = context;

        public async Task<IActionResult> Index(string searchString)
        {
            var categorias = _context.Categorias.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                categorias = categorias.Where(c =>
                    c.Nombre.Contains(searchString));
            }

            ViewData["SearchString"] = searchString;

            return View(await categorias.ToListAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return NotFound();

            return View(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Categoria categoria)
        {
            if (id != categoria.Id) return NotFound();

            _context.Update(categoria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null) return NotFound();

            return View(categoria);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }