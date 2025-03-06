using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FernandoLibrary.Persistance;
using FernandoLibrary.Persistance.Entities;


namespace FernandoLibrary.Controllers;

public class PrestamosController : Controller {
    
    private readonly LibraryDbContext _context;
    
    public PrestamosController(LibraryDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string searchString)
        {
            var prestamos = _context.Prestamos
                .Include(p => p.Usuario)
                .Include(p => p.Libro)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                prestamos = prestamos.Where(p =>
                    p.Usuario.Nombre.Contains(searchString) ||
                    p.Libro.Titulo.Contains(searchString));
            }

            prestamos = prestamos.OrderByDescending(p => p.Id);

            ViewData["SearchString"] = searchString;

            return View(await prestamos.ToListAsync());
        }

        public async Task<IActionResult> Prestados(string searchString)
        {
            var prestamos = _context.Prestamos
                .Include(p => p.Usuario)
                .Include(p => p.Libro)
                .Where(p => p.FechaDevolucion == null)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                prestamos = prestamos.Where(p =>
                    p.Usuario.Nombre.Contains(searchString) ||
                    p.Libro.Titulo.Contains(searchString));
            }

            prestamos = prestamos.OrderByDescending(p => p.Id);

            ViewData["SearchString"] = searchString;

            return View("Index", await prestamos.ToListAsync());
        }

        public async Task<IActionResult> Devueltos(string searchString)
        {
            var prestamos = _context.Prestamos
                .Include(p => p.Usuario)
                .Include(p => p.Libro)
                .Where(p => p.FechaDevolucion != null)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                prestamos = prestamos.Where(p =>
                    p.Usuario.Nombre.Contains(searchString) ||
                    p.Libro.Titulo.Contains(searchString));
            }

            prestamos = prestamos.OrderByDescending(p => p.Id);

            ViewData["SearchString"] = searchString;

            return View("Index", await prestamos.ToListAsync());
        }

        public async Task<IActionResult> Prestar()
            {
            var librosDisponibles = await _context.Libros
                .Where(l => l.Cantidad - _context.Prestamos
                    .Count(p => p.LibroId == l.Id && p.FechaDevolucion == null) > 0)
                .ToListAsync();

            ViewBag.Libros = librosDisponibles;
            ViewBag.Usuarios = await _context.Usuarios.ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Prestar(Prestamo prestamo)
        {
            var libro = await _context.Libros
                .Where(l => l.Id == prestamo.LibroId)
                .Select(l => new
                {
                    l.Cantidad,
                    Prestados = _context.Prestamos.Count(p => p.LibroId == l.Id && p.FechaDevolucion == null)
                })
                .FirstOrDefaultAsync();

            if (libro == null || libro.Cantidad - libro.Prestados <= 0)
            {
                ModelState.AddModelError("", "El libro no está disponible para préstamo.");
                return View(prestamo);
            }

            prestamo.FechaPrestamo = DateTime.UtcNow;
            _context.Prestamos.Add(prestamo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Devolver(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos
                .Include(p => p.Libro)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prestamo == null)
            {
                return NotFound();
            }

            return View(prestamo);
        }

        [HttpPost, ActionName("Devolver")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DevolverConfirmed(int id)
        {
            var prestamo = await _context.Prestamos
                .Include(p => p.Libro)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prestamo == null)
            {
                return NotFound();
            }

            prestamo.FechaDevolucion = DateTime.UtcNow;
            _context.Prestamos.Update(prestamo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
    