using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;
using PlantillaAlmacen.Models;
using PlantillaAlmacen.Services.Pdf;

namespace PlantillaAlmacen.Controllers
{
    public class AsignacionesController : Controller
    {
        private readonly AlmacenDbContext _context;

        public AsignacionesController(AlmacenDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Asignar(string ordenacion, string buscador, string Buscador, int? numeroPagina, int? tamanoPagina)
        {
            ViewData["Ordenacion"] = ordenacion;
            ViewData["OrdenacionFecha"] = string.IsNullOrEmpty(ordenacion) ? "fecha_desc" : "";

            if (buscador != null) numeroPagina = 1; else buscador = Buscador;
            buscador = (buscador ?? "").Trim();
            ViewData["Buscador"] = buscador;

            int tamPag = tamanoPagina ?? 5;
            ViewData["TamanoPagina"] = tamPag;

            var idActivo = await _context.EstatusArticulos
                .Where(e => e.Nombre.ToUpper() == "ACTIVO")
                .Select(e => e.IdEstatusArticulo)
                .FirstOrDefaultAsync();

            var abiertos = await _context.Asignaciones
                .AsNoTracking()
                .Where(a => a.FechaDevolucion == null)
                .Select(a => a.IdArticulo)
                .ToListAsync();

            var articulosDisponibles = await _context.Articulos
                .AsNoTracking()
                .Include(a => a.CatalogoArticulo).ThenInclude(c => c.Categoria)
                .Where(a => a.IdEstatusArticulo == idActivo && !abiertos.Contains(a.IdArticulo))
                .OrderBy(a => a.CatalogoArticulo.Nombre)
                .Select(a => new
                {
                    a.IdArticulo,
                    Texto = a.CatalogoArticulo.Nombre
                })
                .ToListAsync();

            ViewBag.Articulos = new SelectList(articulosDisponibles, "IdArticulo", "Texto");

            ViewBag.Categorias = new SelectList(
                await _context.Categorias.AsNoTracking()
                .Where(c => c.Estatus)
                .OrderBy(c => c.Nombre)
                .ToListAsync(),
               "IdCategoria", "Nombre");

            ViewBag.Personales = new SelectList(
                await _context.Personal.AsNoTracking()
                    .OrderBy(p => p.Nombre)
                    .Select(p => new { p.IdPersonal, p.Nombre })
                    .ToListAsync(), "IdPersonal", "Nombre");

            ViewBag.Departamentos = new SelectList(
                await _context.Departamentos.AsNoTracking()
                    .Where(d => d.Activo).OrderBy(d => d.Nombre).ToListAsync(),
                "IdDepartamento", "Nombre");

            ViewBag.Frentes = new SelectList(
                await _context.Frentes.AsNoTracking()
                    .Where(f => f.Activo).OrderBy(f => f.Nombre).ToListAsync(),
                "IdFrente", "Nombre");

            ViewBag.EstadosEntrega = new SelectList(
                await _context.EstadoEntregas
                    .AsNoTracking()
                    .Where(e => new int[] { 1, 2, 3 }.Contains(e.IdEstadoEntrega))
                    .OrderBy(e => e.Nombre)
                    .ToListAsync(),
                "IdEstadoEntrega", "Nombre");

            var q = _context.Asignaciones.AsNoTracking()
                .Include(a => a.Articulo).ThenInclude(x => x.CatalogoArticulo).ThenInclude(c => c.Categoria)
                .Include(a => a.Personal)
                .Include(a => a.Departamento)
                .Include(a => a.Frente)
                .Include(a => a.EstadoEntrega)
                .Include(a => a.EstadoRecepcion)
                .AsQueryable();

            if (!string.IsNullOrEmpty(buscador))
            {
                q = q.Where(a =>
                    EF.Functions.Like(a.Articulo.CatalogoArticulo.Nombre, $"%{buscador}%") ||
                    EF.Functions.Like(a.Personal.Nombre, $"%{buscador}%") ||
                    EF.Functions.Like(a.Departamento.Nombre, $"%{buscador}%") ||
                    EF.Functions.Like(a.Frente.Nombre, $"%{buscador}%")
                );
            }

            q = ordenacion == "fecha_desc" ? q.OrderByDescending(a => a.FechaAsignacion)
                                           : q.OrderBy(a => a.FechaAsignacion);

            var pagina = await Paginacion<Asignacion>.CreateAsync(q, numeroPagina ?? 1, tamPag);
            return View(pagina);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear([FromForm] int IdArticulo, [FromForm] int IdEstadoEntrega, [FromForm] int? IdPersonal,
                                                [FromForm] int? IdDepartamento, [FromForm] int? IdFrente, [FromForm] string? Observacion)
        {
            if (IdArticulo <= 0 || IdEstadoEntrega <= 0)
                return BadRequest("Datos inválidos.");

            int destinos = (IdPersonal.HasValue ? 1 : 0) + (IdDepartamento.HasValue ? 1 : 0) + (IdFrente.HasValue ? 1 : 0);
            if (destinos != 1)
                return BadRequest("Selecciona un único destino: Personal, Departamento o Frente.");

            var idActivo = await _context.EstatusArticulos
                .Where(e => e.Nombre.ToUpper() == "ACTIVO")
                .Select(e => e.IdEstatusArticulo)
                .FirstOrDefaultAsync();

            var idAsignado = await _context.EstatusArticulos
                .Where(e => e.Nombre.ToUpper() == "ASIGNADO")
                .Select(e => e.IdEstatusArticulo)
                .FirstOrDefaultAsync();

            if (idActivo == 0 || idAsignado == 0)
                return BadRequest("Faltan estatus base (ACTIVO/ASIGNADO).");

            var art = await _context.Articulos.FirstOrDefaultAsync(a => a.IdArticulo == IdArticulo);
            if (art == null) return NotFound("Artículo no encontrado.");
            if (art.IdEstatusArticulo != idActivo)
                return BadRequest("El artículo no está disponible (no está ACTIVO).");

            bool yaAbierta = await _context.Asignaciones
                .AnyAsync(x => x.IdArticulo == IdArticulo && x.FechaDevolucion == null);
            if (yaAbierta) return BadRequest("Este artículo ya tiene una asignación abierta.");

            int? idUsuario = null;
            using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                var asignacion = new Asignacion
                {
                    IdArticulo = IdArticulo,
                    IdEstadoEntrega = IdEstadoEntrega,
                    FechaAsignacion = DateTime.Now,
                    IdPersonal = IdPersonal,
                    IdDepartamento = IdDepartamento,
                    IdFrente = IdFrente,
                    Activo = true
                };

                int idEstatusAnterior = art.IdEstatusArticulo;
                art.IdEstatusArticulo = idAsignado;

                _context.Asignaciones.Add(asignacion);
                _context.Articulos.Update(art);

                _context.ArticuloEstadoHistoriales.Add(new ArticuloEstadoHistorial
                {
                    IdArticulo = art.IdArticulo,
                    IdEstatusArticuloAnterior = idEstatusAnterior,
                    IdEstatusArticuloNuevo = idAsignado,
                    Fecha = DateTime.Now,
                    IdUsuario = idUsuario,
                    Motivo = string.IsNullOrWhiteSpace(Observacion) ? "Asignación" : "Asignación: " + Observacion
                });

                _context.AsignacionEventos.Add(new AsignacionEvento
                {
                    Asignacion = asignacion,
                    Tipo = "ENTREGADA",
                    Fecha = DateTime.Now,
                    IdUsuario = idUsuario,
                    Comentario = Observacion,
                    IdEstadoEntrega = IdEstadoEntrega
                });

                await _context.SaveChangesAsync();
                await tx.CommitAsync();

                return Json(new { idAsignacion = asignacion.IdAsignacion });
            }
            catch
            {
                await tx.RollbackAsync();
                return BadRequest("No se pudo completar la asignación.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DescargarActaVehiculo(int idAsignacion)
        {
            var asig = await _context.Asignaciones
                .Include(a => a.Articulo).ThenInclude(ar => ar.CatalogoArticulo)
                .Include(a => a.Personal)
                .FirstOrDefaultAsync(a => a.IdAsignacion == idAsignacion);

            if (asig == null) return NotFound();
            var vehiculo = asig.Articulo as ArticuloVehiculo;

            var pdfService = new ActaResponsabilidadPdf();
            var pdfBytes = pdfService.Generar(
                asig.Personal?.Nombre ?? "—",
                vehiculo?.Marca ?? "—",
                vehiculo?.Modelo ?? "-",
                vehiculo?.Placas ?? "—",
                vehiculo?.NumeroSerie ?? "—",
                vehiculo?.Anio ?? 0,
                vehiculo?.Color ?? "—",
                vehiculo?.Combustible ?? "—",
                vehiculo?.NumeroInventario ?? "-",
                vehiculo?.Observacion ?? "-",
                vehiculo?.Kilometraje?.ToString() ?? "—",
                vehiculo?.Transmision ?? "—"
             );

            return File(pdfBytes, "application/pdf", "ActaResponsabilidad.pdf");
        }

        [HttpGet]
        public async Task<IActionResult> DescargarActa(int idAsignacion)
        {
            var asig = await _context.Asignaciones
                .Include(a => a.Articulo)
                    .ThenInclude(ar => ar.CatalogoArticulo)
                        .ThenInclude(c => c.Categoria)
                .Include(a => a.Personal)
                .FirstOrDefaultAsync(a => a.IdAsignacion == idAsignacion);

            if (asig == null) return NotFound("Asignación no encontrada.");

            var categoria = asig.Articulo?.CatalogoArticulo?.Categoria?.Nombre?.ToUpper() ?? "";
            var pdfService = new ActaResponsabilidadPdf();

            byte[] pdfBytes;
            string nombreArchivo;

            if (categoria.Contains("VEHICULO") || categoria.Contains("VEHÍCULOS"))
            {
                var vehiculo = asig.Articulo as ArticuloVehiculo;

                pdfBytes = pdfService.Generar(
                    asig.Personal?.Nombre ?? "—",
                    vehiculo?.Marca ?? "—",
                    vehiculo?.Modelo ?? "-",
                    vehiculo?.Placas ?? "—",
                    vehiculo?.NumeroSerie ?? "—",
                    vehiculo?.Anio ?? 0,
                    vehiculo?.Color ?? "—",
                    vehiculo?.Combustible ?? "—",
                    vehiculo?.NumeroInventario ?? "-",
                    vehiculo?.Observacion ?? "-",
                    vehiculo?.Kilometraje?.ToString() ?? "—",
                    vehiculo?.Transmision ?? "—"
                );

                nombreArchivo = $"ActaVehiculo_{asig.IdAsignacion}.pdf";
            }
            else if (categoria.Contains("TECNOLOGIA") || categoria.Contains("TECNOLOGÍA"))
            {
                var tecnologia = asig.Articulo as ArticuloTecnologia;

                pdfBytes = pdfService.GenerarTecnologia(
                    asig.Personal?.Nombre ?? "—",
                    asig.Articulo?.NumeroInventario ?? "-",
                    tecnologia.Caracteristicas ?? "-",
                    tecnologia.Marca ?? "-",
                    tecnologia.Modelo ?? "-",
                    tecnologia.NumeroSerie ?? "-",
                    "Intel Core i7",
                    "16 GB RAM",
                    "SSD 1TB"
                );

                nombreArchivo = $"ActaTecnologia_{asig.IdAsignacion}.pdf";
            }
            else if (categoria.Contains("COMUNICACIONES"))
            {
                var com = asig.Articulo as ArticuloComunicaciones;

                pdfBytes = pdfService.GenerarComunicaciones(
                    asig.Personal?.Nombre ?? "—",
                    asig.Articulo?.NumeroInventario ?? "-",
                    com?.Observacion ?? "-",
                    com?.Marca ?? "-",
                    com?.Modelo ?? "-",
                    com?.NumeroSerie ?? "-"
                );

                nombreArchivo = $"ActaComunicaciones_{asig.IdAsignacion}.pdf";
            }

            else
            {
                return BadRequest("No existe formato de acta para esta categoría.");
            }

            string bucket = "almacen-d9e8e.firebasestorage.app";

            var credentialPath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "firebase-adminsdk.json");
            var credential = GoogleCredential.FromFile(credentialPath);
            var storage = StorageClient.Create(credential);

            using (var stream = new MemoryStream(pdfBytes))
            {
                await storage.UploadObjectAsync(bucket, $"asignaciones/{nombreArchivo}", "application/pdf", stream);
            }

            asig.ArchivoAsignacion = $"asignaciones/{nombreArchivo}";
            _context.Update(asig);
            await _context.SaveChangesAsync();

            return File(pdfBytes, "application/pdf", nombreArchivo);
        }
        public async Task<IActionResult> Recibir(string ordenacion, string buscador, string Buscador, int? numeroPagina, int? tamanoPagina)
        {
            ViewData["Ordenacion"] = ordenacion;
            ViewData["OrdenacionFecha"] = string.IsNullOrEmpty(ordenacion) ? "fecha_desc" : "";

            if (buscador != null) numeroPagina = 1; else buscador = Buscador;
            buscador = (buscador ?? "").Trim();
            ViewData["Buscador"] = buscador;

            int tamPag = tamanoPagina ?? 5;
            ViewData["TamanoPagina"] = tamPag;

            ViewBag.EstadosRecepcion = new SelectList(
                await _context.EstadoEntregas
                    .AsNoTracking()
                     .Where(e => new int[] { 4, 5, 6, 7, 9, 10 }.Contains(e.IdEstadoEntrega))
                    .OrderBy(e => e.Nombre)
                    .ToListAsync(),
                "IdEstadoEntrega",
                "Nombre"
            );

            var q = _context.Asignaciones
                .AsNoTracking()
                .Include(a => a.Articulo)
                    .ThenInclude(x => x.CatalogoArticulo).ThenInclude(c => c.Categoria)
                .Include(a => a.Personal)
                .Include(a => a.Departamento)
                .Include(a => a.Frente)
                .Include(a => a.EstadoEntrega)
                .Where(a => a.FechaDevolucion == null)
                .AsQueryable();

            if (!string.IsNullOrEmpty(buscador))
            {
                q = q.Where(a =>
                    EF.Functions.Like(a.Articulo.CatalogoArticulo.Nombre, $"%{buscador}%") ||
                    EF.Functions.Like(a.Personal.Nombre, $"%{buscador}%") ||
                    EF.Functions.Like(a.Departamento.Nombre, $"%{buscador}%") ||
                    EF.Functions.Like(a.Frente.Nombre, $"%{buscador}%")
                );
            }

            q = ordenacion == "fecha_desc"
                ? q.OrderByDescending(a => a.FechaAsignacion)
                : q.OrderBy(a => a.FechaAsignacion);

            var pagina = await Paginacion<Asignacion>.CreateAsync(q, numeroPagina ?? 1, tamPag);
            return View(pagina);
        }

        [HttpGet]
        public async Task<IActionResult> DetallesJson(int id)
        {
            var a = await _context.Asignaciones
                .AsNoTracking()
                .Include(x => x.Articulo)
                    .ThenInclude(y => y.CatalogoArticulo).ThenInclude(z => z.Categoria)
                .Include(x => x.Personal)
                .Include(x => x.Departamento)
                .Include(x => x.Frente)
                .Include(x => x.EstadoEntrega)
                .FirstOrDefaultAsync(x => x.IdAsignacion == id && x.FechaDevolucion == null);

            if (a == null) return NotFound();

            var destinoTipo = a.IdPersonal.HasValue ? "PERSONAL"
                            : a.IdDepartamento.HasValue ? "DEPARTAMENTO"
                            : "FRENTE";
            var destinoNombre = a.Personal?.Nombre ?? a.Departamento?.Nombre ?? a.Frente?.Nombre;

            return Json(new
            {
                idAsignacion = a.IdAsignacion,
                articulo = a.Articulo?.CatalogoArticulo?.Nombre,
                categoria = a.Articulo?.CatalogoArticulo?.Categoria?.Nombre,
                fechaAsignacion = a.FechaAsignacion.ToString("yyyy-MM-dd"),
                estadoEntrega = a.EstadoEntrega?.Nombre,
                destinoTipo,
                destinoNombre
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Recibir(
     [FromForm] int IdAsignacion,
     [FromForm] int IdEstadoRecepcion,
     [FromForm] string? Observaciones,
     [FromForm] List<IFormFile>? Evidencias)
        {
            var asign = await _context.Asignaciones
                .Include(a => a.Articulo)
                    .ThenInclude(a => a.CatalogoArticulo)
                .Include(a => a.Personal)
                .FirstOrDefaultAsync(a => a.IdAsignacion == IdAsignacion);

            if (asign == null) return NotFound("Asignación no encontrada.");
            if (asign.FechaDevolucion != null || !asign.Activo)
                return BadRequest("La asignación ya está cerrada.");

            int nuevoEstatusArticulo;
            if (IdEstadoRecepcion == 6 || IdEstadoRecepcion == 7)
                nuevoEstatusArticulo = 4;
            else if (IdEstadoRecepcion == 9 || IdEstadoRecepcion == 10)
                nuevoEstatusArticulo = 8;
            else
            {
                var idActivo = await _context.EstatusArticulos
                    .Where(e => e.Nombre.ToUpper() == "ACTIVO")
                    .Select(e => e.IdEstatusArticulo)
                    .FirstOrDefaultAsync();
                nuevoEstatusArticulo = idActivo;
            }

            int idEstatusAnterior = asign.Articulo.IdEstatusArticulo;
            int? idUsuario = null;

            string bucket = "almacen-d9e8e.firebasestorage.app";
            var credentialPath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "firebase-adminsdk.json");
            var credential = GoogleCredential.FromFile(credentialPath);
            var storage = StorageClient.Create(credential);

            byte[]? pdfBytes = null;
            string? nombreArchivoDanios = null;

            using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                asign.IdEstadoRecepcion = IdEstadoRecepcion;
                asign.ObservacionesDevolucion = Observaciones;
                asign.FechaDevolucion = DateTime.Now;
                asign.Activo = false;

                asign.Articulo.IdEstatusArticulo = nuevoEstatusArticulo;
                _context.Articulos.Update(asign.Articulo);

                var listaEvidenciasFirmadas = new List<byte[]>();

                if (Evidencias != null && Evidencias.Any() && IdEstadoRecepcion != 4)
                {
                    foreach (var file in Evidencias)
                    {
                        var extension = Path.GetExtension(file.FileName);
                        var fileName = $"evidencias/{Guid.NewGuid()}{extension}";

                        using var ms = new MemoryStream();
                        await file.CopyToAsync(ms);
                        ms.Position = 0;

                        await storage.UploadObjectAsync(bucket, fileName, file.ContentType, ms);

                        var ev = new EvidenciaAsignacion
                        {
                            IdAsignacion = asign.IdAsignacion,
                            Archivo = fileName
                        };
                        _context.EvidenciaAsignaciones.Add(ev);

                        var urlSigner = UrlSigner.FromCredentialFile(credentialPath);
                        var url = await urlSigner.SignAsync(
                            bucket,
                            fileName,
                            TimeSpan.FromMinutes(15),
                            HttpMethod.Get
                        );

                        using var http = new HttpClient();
                        var bytes = await http.GetByteArrayAsync(url);
                        listaEvidenciasFirmadas.Add(bytes);
                    }
                }

                if (IdEstadoRecepcion != 4)
                {
                    var pdfService = new ActaResponsabilidadPdf();
                    var art = asign.Articulo;

                    pdfBytes = pdfService.GenerarActaDanios(
                        asign.Personal?.Nombre ?? "—",
                        art.NumeroInventario ?? "-",
                        art.CatalogoArticulo?.Nombre ?? "-",
                        art.Precio,
                        listaEvidenciasFirmadas
                    );

                    nombreArchivoDanios = $"ActaDanios_{asign.IdAsignacion}.pdf";

                    using var stream = new MemoryStream(pdfBytes);
                    await storage.UploadObjectAsync(bucket, $"danios/{nombreArchivoDanios}", "application/pdf", stream);

                    asign.ArchivoDanios = $"danios/{nombreArchivoDanios}";
                }


                _context.ArticuloEstadoHistoriales.Add(new ArticuloEstadoHistorial
                {
                    IdArticulo = asign.Articulo.IdArticulo,
                    IdEstatusArticuloAnterior = idEstatusAnterior,
                    IdEstatusArticuloNuevo = nuevoEstatusArticulo,
                    Fecha = DateTime.Now,
                    IdUsuario = idUsuario,
                    Motivo = string.IsNullOrWhiteSpace(Observaciones) ? "Devolución" : "Devolución: " + Observaciones
                });

                _context.AsignacionEventos.Add(new AsignacionEvento
                {
                    IdAsignacion = asign.IdAsignacion,
                    Tipo = "DEVUELTA",
                    Fecha = DateTime.Now,
                    IdUsuario = idUsuario,
                    Comentario = Observaciones,
                    IdEstadoEntrega = IdEstadoRecepcion
                });

                await _context.SaveChangesAsync();
                await tx.CommitAsync();

                if (pdfBytes != null && nombreArchivoDanios != null)
                {
                    return File(pdfBytes, "application/pdf", nombreArchivoDanios);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return BadRequest("No se pudo registrar la devolución. " + ex.Message);
            }
        }


        public async Task<IActionResult> Index()
        {
            return View(await _context.Asignaciones.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asignacion = await _context.Asignaciones
                .FirstOrDefaultAsync(m => m.IdAsignacion == id);
            if (asignacion == null)
            {
                return NotFound();
            }

            return View(asignacion);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAsignacion,IdArticulo,IdPersonal,IdEstadoEntrega,IdEstatusAsignacion,FechaAsinacion,EstadoDevolucion")] Asignacion asignacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(asignacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(asignacion);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asignacion = await _context.Asignaciones.FindAsync(id);
            if (asignacion == null)
            {
                return NotFound();
            }
            return View(asignacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAsignacion,IdArticulo,IdPersonal,IdEstadoEntrega,IdEstatusAsignacion,FechaAsinacion,EstadoDevolucion")] Asignacion asignacion)
        {
            if (id != asignacion.IdAsignacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(asignacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AsignacionExists(asignacion.IdAsignacion))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(asignacion);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asignacion = await _context.Asignaciones
                .FirstOrDefaultAsync(m => m.IdAsignacion == id);
            if (asignacion == null)
            {
                return NotFound();
            }

            return View(asignacion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var asignacion = await _context.Asignaciones.FindAsync(id);
            if (asignacion != null)
            {
                _context.Asignaciones.Remove(asignacion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AsignacionExists(int id)
        {
            return _context.Asignaciones.Any(e => e.IdAsignacion == id);
        }

        [HttpGet]
        public async Task<IActionResult> GetCatalogosPorCategoria(int idCategoria)
        {
            var catalogos = await _context.CatalogoArticulos
                .Where(c => c.IdCategoria == idCategoria && c.Estatus)
                .Select(c => new
                {
                    c.IdCatalogoArticulo,
                    c.Nombre
                })
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            return Json(catalogos);
        }

        [HttpGet]
        public async Task<IActionResult> GetArticulosPorCatalogo(int idCatalogo)
        {
            var idActivo = await _context.EstatusArticulos
                .Where(e => e.Nombre.ToUpper() == "ACTIVO")
                .Select(e => e.IdEstatusArticulo)
                .FirstOrDefaultAsync();

            var abiertos = await _context.Asignaciones
                .Where(a => a.FechaDevolucion == null)
                .Select(a => a.IdArticulo)
                .ToListAsync();

            var query = await _context.Articulos
                .AsNoTracking()
                .Include(a => a.CatalogoArticulo)
                .Where(a => a.IdCatalogoArticulo == idCatalogo
                         && a.IdEstatusArticulo == idActivo
                         && !abiertos.Contains(a.IdArticulo))
                .ToListAsync();

            var articulos = query
                .Select(a => new
                {
                    a.IdArticulo,
                    Texto =
                        a.NumeroInventario + " - " + a.CatalogoArticulo.Nombre
                        + (a is ArticuloTecnologia tec
                            ? $" ({tec.Marca} {tec.Modelo} - {tec.NumeroSerie})"
                        : a is ArticuloVehiculo veh
                            ? $" ({veh.Marca} {veh.Modelo} - {veh.Placas})"
                        : a is ArticuloHerramienta her
                            ? $" ({her.Marca} {her.Modelo} - {her.NumeroSerie})"
                        : a is ArticuloComunicaciones com
                            ? $" ({com.Marca} {com.Modelo} - {com.NumeroSerie})"
                        : "")
                })
                .OrderBy(a => a.Texto)
                .ToList();

            return Json(articulos);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportarAsignaciones(IFormFile ArchivoExcel)
        {
            if (ArchivoExcel == null || ArchivoExcel.Length == 0)
                return BadRequest("Debe seleccionar un archivo Excel.");

            using var stream = new MemoryStream();
            await ArchivoExcel.CopyToAsync(stream);
            using var workbook = new XLWorkbook(stream);
            var ws = workbook.Worksheet(1);

            var firstRow = 8;
            var firstCol = 2;
            var lastRow = ws.LastRowUsed().RowNumber();
            var lastCol = ws.LastColumnUsed().ColumnNumber();

            var rows = ws.Range(firstRow + 1, firstCol, lastRow, lastCol).RowsUsed();

            var errores = new List<(int fila, string nombre)>();

            var idActivo = await _context.EstatusArticulos
                .Where(e => e.Nombre.ToUpper() == "ACTIVO")
                .Select(e => e.IdEstatusArticulo)
                .FirstOrDefaultAsync();

            var idAsignado = await _context.EstatusArticulos
                .Where(e => e.Nombre.ToUpper() == "ASIGNADO")
                .Select(e => e.IdEstatusArticulo)
                .FirstOrDefaultAsync();

            var idEstadoEntrega = await _context.EstadoEntregas
                .Where(e => e.Nombre.ToUpper() == "ENTREGADO EN BUEN ESTADO")
                .Select(e => e.IdEstadoEntrega)
                .FirstOrDefaultAsync();

            using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var row in rows)
                {
                    string numInventario = row.Cell(1).GetString()?.Trim();
                    string nombrePersonal = row.Cell(3).GetString()?.Trim();
                    string fechaAsignacionStr = row.Cell(4).GetString()?.Trim();

                    if (string.IsNullOrEmpty(numInventario) || string.IsNullOrEmpty(nombrePersonal))
                        continue;

                    var art = await _context.Articulos
                        .FirstOrDefaultAsync(a => a.NumeroInventario == numInventario);

                    if (art == null || art.IdEstatusArticulo != idActivo)
                        continue;

                    var personal = await _context.Personal
                        .FirstOrDefaultAsync(p => p.Nombre.ToUpper() == nombrePersonal.ToUpper());

                    if (personal == null)
                    {
                        errores.Add((row.RowNumber(), nombrePersonal));
                        continue;
                    }

                    DateTime fechaAsignacion = DateTime.TryParse(fechaAsignacionStr, out var f)
                        ? f : DateTime.Now;

                    var asignacion = new Asignacion
                    {
                        IdArticulo = art.IdArticulo,
                        IdEstadoEntrega = idEstadoEntrega,
                        FechaAsignacion = fechaAsignacion,
                        IdPersonal = personal.IdPersonal,
                        Activo = true
                    };

                    int idEstatusAnterior = art.IdEstatusArticulo;
                    art.IdEstatusArticulo = idAsignado;

                    _context.Asignaciones.Add(asignacion);
                    _context.Articulos.Update(art);

                    _context.ArticuloEstadoHistoriales.Add(new ArticuloEstadoHistorial
                    {
                        IdArticulo = art.IdArticulo,
                        IdEstatusArticuloAnterior = idEstatusAnterior,
                        IdEstatusArticuloNuevo = idAsignado,
                        Fecha = fechaAsignacion,
                        Motivo = "Asignación automática desde Excel"
                    });

                    _context.AsignacionEventos.Add(new AsignacionEvento
                    {
                        Asignacion = asignacion,
                        Tipo = "ENTREGADA",
                        Fecha = fechaAsignacion,
                        Comentario = "Asignación automática desde Excel",
                        IdEstadoEntrega = idEstadoEntrega
                    });
                }

                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                return BadRequest("No se pudo completar la importación.");
            }

            if (errores.Any())
                return Json(new
                {
                    ok = false,
                    mensaje = "Algunas asignaciones no se realizaron",
                    errores = errores.Select(e => new { fila = e.fila, nombre = $"No existe el nombre '{e.nombre}' en el sistema" })
                });
            return Ok(new { ok = true, mensaje = "Asignaciones registradas correctamente" });
        }

        [HttpGet]
        public async Task<IActionResult> VerAsignacion(int idAsignacion)
        {
            var asig = await _context.Asignaciones.FindAsync(idAsignacion);
            if (asig == null || string.IsNullOrEmpty(asig.ArchivoAsignacion))
                return NotFound();

            string bucket = "almacen-d9e8e.firebasestorage.app";
            var credentialPath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "firebase-adminsdk.json");
            var urlSigner = UrlSigner.FromCredentialFile(credentialPath);

            var signedUrl = urlSigner.Sign(bucket, asig.ArchivoAsignacion, TimeSpan.FromMinutes(15), HttpMethod.Get);

            return Json(new { url = signedUrl });
        }

    }
}
