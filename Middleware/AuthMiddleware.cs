using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FernandoLibrary.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Verificar si el usuario está autenticado
            var token = context.Session.GetString("AuthToken");

            if (string.IsNullOrEmpty(token) && !context.Request.Path.StartsWithSegments("/Autenticacion"))
            {
                // Redirigir a la página de login si no está autenticado
                context.Response.Redirect("/Autenticacion/Login");
                return;
            }

            // Continuar con la siguiente middleware en la cadena
            await _next(context);
        }
    }
}