using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PlantillaAlmacen.Models;
using Microsoft.EntityFrameworkCore;


namespace PlantillaAlmacen.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AlmacenDbContext _context;

    public HomeController(ILogger<HomeController> logger, AlmacenDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Personal
        ViewBag.TotalPersonal = await _context.Personal.CountAsync();

        // Artículos
        ViewBag.TotalArticulos = await _context.Articulos.CountAsync();

        // Por categoría
        ViewBag.TotalVehiculos = await _context.Articulos.OfType<ArticuloVehiculo>().CountAsync();
        ViewBag.TotalTecnologia = await _context.Articulos.OfType<ArticuloTecnologia>().CountAsync();
        ViewBag.TotalHerramientas = await _context.Articulos.OfType<ArticuloHerramienta>().CountAsync();

        // Detalle productos
        ViewBag.TotalMouses = await _context.Articulos
            .OfType<ArticuloTecnologia>()
            .CountAsync(a => a.CatalogoArticulo.Nombre.Contains("Mouse"));

        ViewBag.TotalImpresoras = await _context.Articulos
            .OfType<ArticuloTecnologia>()
            .CountAsync(a => a.CatalogoArticulo.Nombre.Contains("Impresora"));

        ViewBag.TotalDesktops = await _context.Articulos
            .OfType<ArticuloTecnologia>()
            .CountAsync(a => a.CatalogoArticulo.Nombre.Contains("Desktop"));

        ViewBag.TotalLaptops = await _context.Articulos
            .OfType<ArticuloTecnologia>()
            .CountAsync(a => a.CatalogoArticulo.Nombre.Contains("Laptop"));

        // Vehículos por tipo
        ViewBag.TotalCamionetas = await _context.Articulos
            .OfType<ArticuloVehiculo>()
            .CountAsync(a => a.CatalogoArticulo.Nombre.Contains("Camioneta"));

        // Asignados (ejemplo para laptops y camionetas)
        ViewBag.LaptopsAsignadas = await _context.Asignaciones
    .Include(a => a.Articulo)
        .ThenInclude(c => c.CatalogoArticulo)
    .Where(a => a.FechaDevolucion == null)
    .Where(a => a.Articulo is ArticuloTecnologia)
    .CountAsync();


        ViewBag.CamionetasAsignadas = await _context.Asignaciones
    .Where(a => a.FechaDevolucion == null)
    .Where(a => a.Articulo is ArticuloVehiculo)
    .Select(a => (ArticuloVehiculo)a.Articulo)
    .CountAsync(v => v.CatalogoArticulo.Nombre.Contains("Camioneta"));

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
