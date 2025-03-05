using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FernandoLibrary.Persistance;
using FernandoLibrary.Models;
    using FernandoLibrary.Persistance.Entities;
    
namespace FernandoLibrary.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult Privacy() => View();
    }
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

        public IActionResult Create() => View();

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

    public class PrestamosController : Controller
    {
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


