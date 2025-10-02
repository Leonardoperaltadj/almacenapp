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
    public class FacturasController : Controller
    {
        private readonly AlmacenDbContext _context;

        public FacturasController(AlmacenDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetFacturas()
        {
            var facturas = await _context.Facturas
                .OrderByDescending(f => f.Fecha)
                .Select(f => new { f.IdFactura, f.NumeroFactura })
                .ToListAsync();

            return Json(facturas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFactura(string NumeroFactura, IFormFile ArchivoFactura)
        {
            if (string.IsNullOrWhiteSpace(NumeroFactura))
                return BadRequest("Debes ingresar el número de factura.");

            if (ArchivoFactura == null || ArchivoFactura.Length == 0)
                return BadRequest("Debes adjuntar el archivo de la factura.");

            string bucket = "almacen-d9e8e.firebasestorage.app";
            var credentialPath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "firebase-adminsdk.json");
            var credential = GoogleCredential.FromFile(credentialPath);
            var storage = StorageClient.Create(credential);

            var extension = Path.GetExtension(ArchivoFactura.FileName);
            var fileName = $"{NumeroFactura}_{Guid.NewGuid()}{extension}";

            using (var ms = new MemoryStream())
            {
                await ArchivoFactura.CopyToAsync(ms);
                ms.Position = 0;

                await storage.UploadObjectAsync(
                    bucket,
                    $"facturas/{fileName}",
                    ArchivoFactura.ContentType,
                    ms
                );
            }

            var factura = new Factura
            {
                NumeroFactura = NumeroFactura,
                ArchivoFactura = $"facturas/{fileName}",
                Fecha = DateTime.Now
            };

            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();

            return Ok(new { factura.IdFactura, factura.NumeroFactura, factura.ArchivoFactura });
        }

        // GET: Facturas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Facturas.ToListAsync());
        }

        // GET: Facturas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var factura = await _context.Facturas
                .FirstOrDefaultAsync(m => m.IdFactura == id);
            if (factura == null)
            {
                return NotFound();
            }

            return View(factura);
        }

        // GET: Facturas/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFactura,NumeroFactura,ArchivoFactura,Fecha")] Factura factura)
        {
            if (ModelState.IsValid)
            {
                _context.Add(factura);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(factura);
        }

        // GET: Facturas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var factura = await _context.Facturas.FindAsync(id);
            if (factura == null)
            {
                return NotFound();
            }
            return View(factura);
        }

        // POST: Facturas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdFactura,NumeroFactura,ArchivoFactura,Fecha")] Factura factura)
        {
            if (id != factura.IdFactura)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(factura);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacturaExists(factura.IdFactura))
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
            return View(factura);
        }

        // GET: Facturas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var factura = await _context.Facturas
                .FirstOrDefaultAsync(m => m.IdFactura == id);
            if (factura == null)
            {
                return NotFound();
            }

            return View(factura);
        }

        // POST: Facturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var factura = await _context.Facturas.FindAsync(id);
            if (factura != null)
            {
                _context.Facturas.Remove(factura);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FacturaExists(int id)
        {
            return _context.Facturas.Any(e => e.IdFactura == id);
        }
    }
}
