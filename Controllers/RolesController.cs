using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FernandoLibrary.Persistance.Entities;
using FernandoLibrary.Persistance;

namespace FernandoLibrary.Controllers
{
    public class RolesController : Controller
    {
        private readonly LibraryDbContext _context;

        public RolesController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: Roles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Roles.ToListAsync());
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // GET: Roles/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Permissions = await _context.Permissions.ToListAsync();
            return View();
        }

        // POST: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Roles role, List<int> selectedPermissions)
        {
            if (ModelState.IsValid)
            {
                role.Permissions = await _context.Permissions
                    .Where(p => selectedPermissions.Contains(p.Id))
                    .ToListAsync();

                _context.Add(role);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Permissions = await _context.Permissions.ToListAsync();
            return View(role);
        }

        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (role == null)
            {
                return NotFound();
            }

            ViewBag.Permissions = await _context.Permissions.ToListAsync();
            return View(role);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Roles role, List<int> selectedPermissions)
        {
            if (id != role.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                role.Permissions = await _context.Permissions
                    .Where(p => selectedPermissions.Contains(p.Id))
                    .ToListAsync();

                _context.Update(role);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Permissions = await _context.Permissions.ToListAsync();
            return View(role);
        }

        // GET: Roles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}