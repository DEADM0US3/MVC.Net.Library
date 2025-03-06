using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FernandoLibrary.Persistance;
using FernandoLibrary.Persistance.Entities;

namespace FernandoLibrary.Controllers;

public class AutorController : Controller
    {
        private readonly LibraryDbContext _context;

        public AutorController(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var autores = _context.Autores.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                autores = autores.Where(a =>
                    a.Nombre.Contains(searchString));
            }

            ViewData["SearchString"] = searchString;

            return View(await autores.ToListAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre")] Autor autor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(autor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(autor);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var autor = await _context.Autores.FindAsync(id);
            if (autor == null) return NotFound();

            return View(autor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] Autor autor)
        {
            if (id != autor.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(autor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutorExists(autor.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(autor);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var autor = await _context.Autores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (autor == null) return NotFound();

            return View(autor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var autor = await _context.Autores.FindAsync(id);
            _context.Autores.Remove(autor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AutorExists(int id) => _context.Autores.Any(e => e.Id == id);
    }