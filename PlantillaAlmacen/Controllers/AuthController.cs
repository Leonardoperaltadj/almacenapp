using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantillaAlmacen.Models;
using PlantillaAlmacen.Models.Auth;
using PlantillaAlmacen.Security;
using System.Security.Claims;

namespace PlantillaAlmacen.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly AlmacenDbContext _context;
        private readonly IPasswordHasherService _hasher;

        public AuthController(AlmacenDbContext context, IPasswordHasherService hasher)
        {
            _context = context;
            _hasher = hasher;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM vm, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid) return View(vm);

            var user = await _context.Usuarios
                .Include(u => u.Personal)
                .Include(u => u.UsuarioRoles).ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.Email == vm.Email && u.Estatus);

            if (user == null || !_hasher.Verify(vm.Password, user.Contrasena))
            {
                ModelState.AddModelError("", "Correo o contraseña incorrectos.");
                return View(vm);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Name, user.Personal?.Nombre ?? $"Usuario {user.IdUsuario}")
            };

            foreach (var r in user.UsuarioRoles.Select(x => x.Rol?.Nombre).Where(x => !string.IsNullOrWhiteSpace(x)))
                claims.Add(new Claim(ClaimTypes.Role, r!));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var props = new AuthenticationProperties
            {
                IsPersistent = vm.Recordarme,
                ExpiresUtc = vm.Recordarme
                    ? DateTimeOffset.UtcNow.AddDays(7)
                    : DateTimeOffset.UtcNow.AddHours(8),
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }
    }
}
