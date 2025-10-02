using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlantillaAlmacen.Models;

namespace PlantillaAlmacen.Controllers
{
    public class PersonalesController : Controller
    {
        private readonly AlmacenDbContext _context;

        public PersonalesController(AlmacenDbContext context)
        {
            _context = context;
        }

        // GET: Personales
        public async Task<IActionResult> Index(string ordenacion, string buscador, string Buscador, int? numeroPagina, int? tamanoPagina)
        {
            ViewData["Ordenacion"] = ordenacion;
            ViewData["OrdenacionNombre"] = string.IsNullOrEmpty(ordenacion) ? "nombre_desc" : "";

            if (buscador != null) numeroPagina = 1;
            else buscador = Buscador;

            buscador = (buscador ?? "").Trim();
            ViewData["Buscador"] = buscador;

            int tamPag = tamanoPagina ?? 5;
            ViewData["TamanoPagina"] = tamPag;

            var q = _context.Personal
                .AsNoTracking()
                 .Include(p => p.Puesto)
                .Include(p => p.Departamento)
                .Include(p => p.EstatusPersonal)
                .Include(p => p.Frente)
                .AsQueryable();

            if (!string.IsNullOrEmpty(buscador))
            {
                q = q.Where(p =>
                    EF.Functions.Like(p.Nombre, $"%{buscador}%") ||
                    EF.Functions.Like(p.Puesto.Nombre, $"%{buscador}%") ||
                    EF.Functions.Like(p.Departamento.Nombre, $"%{buscador}%") ||
                    EF.Functions.Like(p.Frente.Nombre, $"%{buscador}%") ||
                    EF.Functions.Like(p.EstatusPersonal.Nombre, $"%{buscador}%")
        );
            }

            q = ordenacion switch
            {
                "nombre_desc" => q.OrderByDescending(p => p.Nombre),
                _ => q.OrderBy(p => p.Nombre)
            };

            ViewBag.Puestos = new SelectList(
               _context.Puestos.AsNoTracking().Where(x => x.Activo).OrderBy(x => x.Nombre).ToList(),
               "IdPuesto", "Nombre");
            ViewBag.Departamentos = new SelectList(
                _context.Departamentos.AsNoTracking().Where(x => x.Activo).OrderBy(x => x.Nombre).ToList(),
                "IdDepartamento", "Nombre"
            );
            ViewBag.Estatuses = new SelectList(
                _context.EstatusPersonales.AsNoTracking().Where(x => x.Activo).OrderBy(x => x.Nombre).ToList(),
                "IdEstatusPersonal", "Nombre"
            );
            ViewBag.Frentes = new SelectList(
                _context.Frentes.AsNoTracking().Where(x => x.Activo).OrderBy(x => x.Nombre).ToList(),
                "IdFrente", "Nombre"
            );

            var pagina = await Paginacion<Personal>.CreateAsync(q, numeroPagina ?? 1, tamPag);
            return View(pagina);
        }

        // GET: Personales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personal = await _context.Personal
                .FirstOrDefaultAsync(m => m.IdPersonal == id);
            if (personal == null)
            {
                return NotFound();
            }

            return View(personal);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Personal nuevo, IFormFile? ArchivoLicencia)
        {
            if (!nuevo.TieneLicenciaManejo)
            {
                nuevo.VigenciaLicencia = null;
                nuevo.ArchivoLicencia = null;
                ArchivoLicencia = null;
            }
            else
            {
                if (!nuevo.VigenciaLicencia.HasValue)
                    ModelState.AddModelError(nameof(Personal.VigenciaLicencia), "La vigencia es obligatoria.");

                if (ArchivoLicencia == null || ArchivoLicencia.Length == 0)
                    ModelState.AddModelError("ArchivoLicencia", "Adjunta el archivo de la licencia.");
            }

            if (!ModelState.IsValid)
            {
                var errores = string.Join(" | ",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest("Datos inválidos. " + errores);
            }

            if (ArchivoLicencia != null && ArchivoLicencia.Length > 0)
            {
                string bucket = "almacen-d9e8e.firebasestorage.app";
                var credentialPath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "firebase-adminsdk.json");
                var credential = GoogleCredential.FromFile(credentialPath);
                var storage = StorageClient.Create(credential);

                var extension = Path.GetExtension(ArchivoLicencia.FileName);
                var fileName = $"Licencia_{Guid.NewGuid()}{extension}";

                using (var ms = new MemoryStream())
                {
                    await ArchivoLicencia.CopyToAsync(ms);
                    ms.Position = 0;

                    await storage.UploadObjectAsync(
                        bucket,
                        $"licencias/{fileName}",
                        ArchivoLicencia.ContentType,
                        ms
                    );
                }

                nuevo.ArchivoLicencia = $"licencias/{fileName}";
            }

            _context.Add(nuevo);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            var p = await _context.Personal
                .AsNoTracking()
                .Include(x => x.Puesto)
                .Include(x => x.Departamento)
                .Include(x => x.Frente)
                .Include(x => x.EstatusPersonal)
                .FirstOrDefaultAsync(x => x.IdPersonal == id);

            if (p == null) return NotFound();

            var puestos = await _context.Puestos
                .AsNoTracking().Where(x => x.Activo).OrderBy(x => x.Nombre)
                .Select(x => new { x.IdPuesto, x.Nombre })
                .ToListAsync();

            var departamentos = await _context.Departamentos
                .AsNoTracking().Where(x => x.Activo).OrderBy(x => x.Nombre)
                .Select(x => new { x.IdDepartamento, x.Nombre })
                .ToListAsync();

            var frentes = await _context.Frentes
                .AsNoTracking().Where(x => x.Activo).OrderBy(x => x.Nombre)
                .Select(x => new { x.IdFrente, x.Nombre })
                .ToListAsync();

            var estatuses = await _context.EstatusPersonales
                .AsNoTracking().Where(x => x.Activo).OrderBy(x => x.Nombre)
                .Select(x => new { x.IdEstatusPersonal, x.Nombre })
                .ToListAsync();

            string archivoLicenciaUrl = null;
            if (!string.IsNullOrEmpty(p.ArchivoLicencia))
            {
                string bucket = "almacen-d9e8e.firebasestorage.app";
                var credentialPath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "firebase-adminsdk.json");
                var urlSigner = UrlSigner.FromCredentialFile(credentialPath);

                archivoLicenciaUrl = urlSigner.Sign(
                    bucket,
                    p.ArchivoLicencia,
                    TimeSpan.FromMinutes(15),
                    HttpMethod.Get
                );
            }

            return Json(new
            {
                idPersonal = p.IdPersonal,
                nombre = p.Nombre,
                idPuesto = p.IdPuesto,
                idDepartamento = p.IdDepartamento,
                idFrente = p.IdFrente,
                idEstatusPersonal = p.IdEstatusPersonal,
                tieneLicenciaManejo = p.TieneLicenciaManejo,
                vigenciaLicencia = p.VigenciaLicencia,
                archivoLicencia = archivoLicenciaUrl,
                puestos,
                departamentos,
                frentes,
                estatuses
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(
           int id,
           [FromForm] string Nombre,
           [FromForm] int IdPuesto,
           [FromForm] int IdDepartamento,
           [FromForm] int IdFrente,
           [FromForm] int IdEstatusPersonal,
           [FromForm] bool TieneLicenciaManejo,
           [FromForm] DateTime? VigenciaLicencia,
           IFormFile? ArchivoLicencia)
        {
            var p = await _context.Personal.FindAsync(id);
            if (p == null) return NotFound();

            p.Nombre = Nombre?.Trim();
            p.IdPuesto = IdPuesto;
            p.IdDepartamento = IdDepartamento;
            p.IdFrente = IdFrente;
            p.IdEstatusPersonal = IdEstatusPersonal;
            p.TieneLicenciaManejo = TieneLicenciaManejo;

            if (!TieneLicenciaManejo)
            {
                p.VigenciaLicencia = null;
                p.ArchivoLicencia = null;
            }
            else
            {
                if (!VigenciaLicencia.HasValue)
                    ModelState.AddModelError(nameof(Personal.VigenciaLicencia), "La vigencia es obligatoria.");

                if (string.IsNullOrEmpty(p.ArchivoLicencia) && (ArchivoLicencia == null || ArchivoLicencia.Length == 0))
                    ModelState.AddModelError("ArchivoLicencia", "Adjunta el archivo de la licencia.");

                if (ArchivoLicencia != null && ArchivoLicencia.Length > 0)
                {
                    string bucket = "almacen-d9e8e.firebasestorage.app";
                    var credentialPath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "firebase-adminsdk.json");
                    var credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromFile(credentialPath);
                    var storage = Google.Cloud.Storage.V1.StorageClient.Create(credential);

                    var fileName = $"Licencia_{p.IdPersonal}_{Guid.NewGuid()}{Path.GetExtension(ArchivoLicencia.FileName)}";

                    using var ms = new MemoryStream();
                    await ArchivoLicencia.CopyToAsync(ms);
                    ms.Position = 0;

                    await storage.UploadObjectAsync(bucket, $"licencias/{fileName}", ArchivoLicencia.ContentType, ms);

                    p.ArchivoLicencia = $"licencias/{fileName}";
                }

                p.VigenciaLicencia = VigenciaLicencia;
            }

            if (!ModelState.IsValid)
            {
                var errores = string.Join(" | ",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest("Datos inválidos. " + errores);
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateException)
            {
                return BadRequest("No se pudo actualizar el registro.");
            }
        }


        // GET: Personales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personal = await _context.Personal
                .FirstOrDefaultAsync(m => m.IdPersonal == id);
            if (personal == null)
            {
                return NotFound();
            }

            return View(personal);
        }

        // POST: Personales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var personal = await _context.Personal.FindAsync(id);
            if (personal != null)
            {
                _context.Personal.Remove(personal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonalExists(int id)
        {
            return _context.Personal.Any(e => e.IdPersonal == id);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsJson(int? id)
        {
            if (id == null) return BadRequest();

            var p = await _context.Personal
                .AsNoTracking()
                .Include(x => x.Puesto)
                .Include(x => x.Departamento)
                .Include(x => x.Frente)
                .Include(x => x.EstatusPersonal)
                .FirstOrDefaultAsync(x => x.IdPersonal == id);

            if (p == null) return NotFound();

            return Json(new
            {
                idPersonal = p.IdPersonal,
                nombre = p.Nombre,
                puesto = p.Puesto?.Nombre,
                departamento = p.Departamento?.Nombre,
                frente = p.Frente?.Nombre,
                estatus = p.EstatusPersonal?.Nombre,
                tieneLicenciaManejo = p.TieneLicenciaManejo,
                vigenciaLicencia = p.VigenciaLicencia,
                tieneArchivo = !string.IsNullOrEmpty(p.ArchivoLicencia)
            });
        }


        [HttpGet]
        public async Task<IActionResult> GetLicencia(int id)
        {
            var per = await _context.Personal
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.IdPersonal == id);

            if (per == null) return NotFound();

            return Json(new
            {
                tieneLicencia = per.TieneLicenciaManejo,
                vigencia = per.VigenciaLicencia?.ToString("yyyy-MM-dd")
            });
        }

        [HttpGet]
        public async Task<IActionResult> VerLicencia(int id)
        {
            var per = await _context.Personal
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.IdPersonal == id);

            if (per == null || string.IsNullOrEmpty(per.ArchivoLicencia))
                return NotFound("No se encontró la licencia.");

            string bucket = "almacen-d9e8e.firebasestorage.app";

            var credentialPath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "firebase-adminsdk.json");
            var urlSigner = UrlSigner.FromCredentialFile(credentialPath);

            var url = await urlSigner.SignAsync(
                bucket,
                per.ArchivoLicencia,
                TimeSpan.FromMinutes(15),
                HttpMethod.Get);

            return Json(new { url });
        }

    }
}
