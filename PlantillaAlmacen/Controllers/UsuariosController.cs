using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlantillaAlmacen;
using PlantillaAlmacen.Models;
using PlantillaAlmacen.Security;
using System;
using System.Linq;
using System.Threading.Tasks;

public class UsuariosController : Controller
{
    private readonly AlmacenDbContext _context;
    private readonly IPasswordHasherService _hasher;

    public UsuariosController(AlmacenDbContext context, IPasswordHasherService hasher)
    {
        _context = context;
        _hasher = hasher;
    }

    public async Task<IActionResult> Index(string ordenacion, string buscador, string Buscador, int? numeroPagina, int? tamanoPagina)
    {
        ViewData["Ordenacion"] = ordenacion;
        ViewData["OrdenacionNombres"] = string.IsNullOrEmpty(ordenacion) ? "nombres_desc" : "";
        ViewData["OrdenacionEmail"] = ordenacion == "email" ? "email_desc" : "email";

        if (buscador != null) numeroPagina = 1;
        else buscador = Buscador;

        buscador = (buscador ?? "").Trim();
        ViewData["Buscador"] = buscador;

        int tamPag = tamanoPagina ?? 5;
        ViewData["TamanoPagina"] = tamPag;

        var q = _context.Usuarios
            .AsNoTracking()
            .Include(u => u.Personal)
            .AsQueryable();

        if (!string.IsNullOrEmpty(buscador))
        {
            q = q.Where(u =>
                EF.Functions.Like(u.Email, $"%{buscador}%") ||
                EF.Functions.Like(u.Personal.Nombre, $"%{buscador}%"));
        }

        switch (ordenacion)
        {
            case "email":
                q = q.OrderBy(u => u.Email);
                break;
            case "email_desc":
                q = q.OrderByDescending(u => u.Email);
                break;
            default:
                q = q.OrderBy(u => u.Personal.Nombre);
                break;
        }

        ViewBag.Personales = new SelectList(
            _context.Personal.AsNoTracking().OrderBy(p => p.Nombre).ToList(),
            "IdPersonal", "Nombre"
        );

        var pagina = await Paginacion<Usuario>.CreateAsync(q, numeroPagina ?? 1, tamPag);
        return View(pagina);
    }

    // GET: Usuarios/DetailsJson/5 (para modal detalles)
    [HttpGet]
    public async Task<IActionResult> DetailsJson(int? id)
    {
        if (id == null) return BadRequest();

        var u = await _context.Usuarios
            .AsNoTracking()
            .Include(x => x.Personal)
            .FirstOrDefaultAsync(x => x.IdUsuario == id);

        if (u == null) return NotFound();

        return Json(new
        {
            idUsuario = u.IdUsuario,
            email = u.Email,
            personal = u.Personal?.Nombre,
            estatus = u.Estatus,
            fechaAlta = u.FechaAlta.ToString("yyyy-MM-dd HH:mm")
        });
    }

    // GET: Usuarios/Edit/5 (para modal editar)
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return BadRequest();

        var u = await _context.Usuarios
            .AsNoTracking()
            .Include(x => x.Personal)
            .FirstOrDefaultAsync(x => x.IdUsuario == id);

        if (u == null) return NotFound();

        var personales = _context.Personal
            .AsNoTracking()
            .OrderBy(p => p.Nombre)
            .Select(p => new { p.IdPersonal, p.Nombre })
            .ToList();

        return Json(new
        {
            idUsuario = u.IdUsuario,
            email = u.Email,
            idPersonal = u.IdPersonal,
            estatus = u.Estatus,
            personales
        });
    }
    public IActionResult Create()
    {
        return View();
    }

    // POST: Usuarios/Create (AJAX)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] string Contrasena, [FromForm] string Email, [FromForm] int IdPersonal)
    {
        if (string.IsNullOrWhiteSpace(Contrasena) || IdPersonal == 0)
            return BadRequest("Datos inválidos.");

        var nuevo = new Usuario
        {
            Contrasena = _hasher.Hash(Contrasena),
            Email = Email?.Trim(),
            IdPersonal = IdPersonal,
            Estatus = true,
            FechaAlta = DateTime.Now
        };

        try
        {
            _context.Add(nuevo);
            await _context.SaveChangesAsync();
            return Ok();
        }
        catch (DbUpdateException)
        {
            return BadRequest("No se pudo crear el usuario.");
        }
    }

    // POST: Usuarios/EditPost/5 (AJAX)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPost(int id, [FromForm] string Email, [FromForm] int IdPersonal, [FromForm] string Contrasena)
    {
        var u = await _context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == id);
        if (u == null) return NotFound();

        if (IdPersonal == 0)
            return BadRequest("Datos inválidos.");

        u.Email = Email?.Trim();
        u.IdPersonal = IdPersonal;

        try
        {
            await _context.SaveChangesAsync();
            return Ok();
        }
        catch (DbUpdateException)
        {
            return BadRequest("No se pudo actualizar el usuario.");
        }
    }

    // POST: Usuarios/CambiarEstatus/5 (toggle)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarEstatus(int id)
    {
        var u = await _context.Usuarios.FindAsync(id);
        if (u == null) return NotFound();

        try
        {
            u.Estatus = !u.Estatus;
            await _context.SaveChangesAsync();
            return Ok(new { estatus = u.Estatus });
        }
        catch (DbUpdateException)
        {
            return BadRequest("No se pudo actualizar el estatus.");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarPassword(int id, [FromForm] string NuevaContrasena)
    {
        var u = await _context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == id);
        if (u == null) return NotFound();

        if (string.IsNullOrWhiteSpace(NuevaContrasena) || NuevaContrasena.Length < 8)
            return BadRequest("La contraseña debe tener al menos 8 caracteres.");

        try
        {
            u.Contrasena = _hasher.Hash(NuevaContrasena);
            await _context.SaveChangesAsync();
            return Ok();
        }
        catch (DbUpdateException)
        {
            return BadRequest("No se pudo actualizar la contraseña.");
        }
    }
}
