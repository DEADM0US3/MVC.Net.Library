using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FernandoLibrary.Persistance;
using FernandoLibrary.Models;
using System.Threading.Tasks;
using FernandoLibrary.Persistance.Entities;
using FernandoLibrary.Presentation.Common;
using FernandoLibrary.Presentation.Services.Authentication;

namespace FernandoLibrary.Controllers;


    public class AutenticacionController : Controller
    {
        private readonly LibraryDbContext _context;
        private readonly OneUserRepository _oneUserRepository;
        private readonly PasswordService _passwordService = new PasswordService();
        
        public AutenticacionController(LibraryDbContext context, OneUserRepository oneUserRepository)
        {
            _context = context;
            _oneUserRepository = oneUserRepository;
        }

        public IActionResult Login() => View();
        
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var hashPassword = _passwordService.Hash(model.Password);
            
            var user = await _oneUserRepository.FindBySpecification(u => u.Email == model.Email && u.Password == hashPassword);
            if (user.Email != null && user.RoleId == 1)
            {
                
                HttpContext.Session.SetString("AuthToken", await _oneUserRepository.GenerateJwtToken());

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register( Usuario model)
        {
                
                var find = await _oneUserRepository.FindBySpecification(u => u.Email == model.Email && u.UserName == model.UserName);

                if (find.Email == null && find.UserName == null)
                {
                    await _oneUserRepository.Create(model.Nombre, model.LastName, model.UserName, model.Email, _passwordService.Hash(model.Password), 2); 
                    
                    // Redirigir a la página de inicio
                    return RedirectToAction("Register", "Autenticacion");

                    
                }

                return View();

        }
        
        public IActionResult Logout()
        {
            // Eliminar el token de la sesión
            HttpContext.Session.Remove("AuthToken");

            // Redirigir a la página de login
            return RedirectToAction("Login");
        }
    }

    public record  LoginViewModel(
        string Email,
        string Password
        );
        