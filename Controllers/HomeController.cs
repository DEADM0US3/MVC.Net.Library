using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FernandoLibrary.Persistance;
using FernandoLibrary.Persistance.Entities;
    
namespace FernandoLibrary.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly LibraryDbContext _context;

        public HomeController(LibraryDbContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            // Obtener libros prestados (con préstamos activos)
            var librosPrestados = _context.Prestamos
                .Include(p => p.Libro)
                .Include(p => p.Usuario)
                .Where(p => p.FechaDevolucion == null)
                .ToList();

            // Obtener libros disponibles (Cantidad - Préstamos activos > 0)
            var librosDisponibles = _context.Libros
                .Select(l => new
                {
                    l.Id,
                    l.Titulo,
                    l.Cantidad,
                    Prestados = _context.Prestamos.Count(p => p.LibroId == l.Id && p.FechaDevolucion == null)
                })
                .Where(l => l.Cantidad - l.Prestados > 0)
                .ToList();

            // Pasar datos a la vista
            ViewBag.LibrosPrestados = librosPrestados;
            ViewBag.LibrosDisponibles = librosDisponibles;

            return View();
        }
        
        
    }



    
}

