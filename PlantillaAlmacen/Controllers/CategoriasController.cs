using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlantillaAlmacen.Models;

namespace PlantillaAlmacen.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly AlmacenDbContext _context;
        public CategoriasController(AlmacenDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string ordenacion, string buscador, string Buscador, int? numeroPagina)
        {
            ViewData["Ordenacion"] = ordenacion;
            ViewData["OrdenacionNombres"] = String.IsNullOrEmpty(ordenacion) ? "nombres_descendientes" : "";

            if (buscador != null)
            {
                numeroPagina = 1;
            }
            else
            {
                buscador = Buscador;
            }
            ViewData["Buscador"] = buscador;
            var categorias = from c in _context.Categorias
                             select c;
            if (!String.IsNullOrEmpty(buscador))
            {
                categorias = categorias.Where(c => c.Nombre.Contains(buscador));
            }
            switch (ordenacion)
            {
                case "nombres_descendientes":
                    categorias = categorias.OrderByDescending(a => a.Nombre);
                    break;
            }

            int tamanoPagina = 5;

            return View(await Paginacion<Categoria>.CreateAsync(categorias.AsNoTracking(), numeroPagina ?? 1, tamanoPagina));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Categorias
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.IdCategoria == id);

            if (categoria == null) return NotFound();

            return Json(new
            {
                idCategoria = categoria.IdCategoria,
                nombre = categoria.Nombre,
                estatus = categoria.Estatus
            });
        }


        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre")] Categoria nuevaCategoria)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(nuevaCategoria);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "No se ha podido crear la nueva categoria." + "Si persiste el error, póngase en contacto con el administrador.");
            }

            return View(nuevaCategoria);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return NotFound();

            return Json(new { idCategoria = categoria.IdCategoria, nombre = categoria.Nombre });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id, [FromForm] string Nombre)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return NotFound();

            categoria.Nombre = Nombre;

            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateException)
            {
                return BadRequest("No se pudo actualizar la categoría");
            }
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Categorias
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.IdCategoria == id);

            if (categoria == null) return NotFound();

            return Json(new
            {
                idCategoria = categoria.IdCategoria,
                nombre = categoria.Nombre,
                estatus = categoria.Estatus
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
                return NotFound();

            try
            {
                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstatus(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
                return NotFound();

            try
            {
                categoria.Estatus = !categoria.Estatus;
                await _context.SaveChangesAsync();
                return Ok(new { estatus = categoria.Estatus });
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> DetallesJson(int? id)
        {
            if (id == null)
                return BadRequest();

            var categoria = await _context.Categorias
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.IdCategoria == id);

            if (categoria == null)
                return NotFound();

            return Json(new
            {
                idCategoria = categoria.IdCategoria,
                nombre = categoria.Nombre,
                estatus = categoria.Estatus
            });
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.IdCategoria == id);
        }
    }
}
