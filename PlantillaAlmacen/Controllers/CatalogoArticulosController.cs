using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PlantillaAlmacen.Models;

namespace PlantillaAlmacen.Controllers
{
    public class CatalogoArticulosController : Controller
    {
        private readonly AlmacenDbContext _context;
        public CatalogoArticulosController(AlmacenDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string ordenacion, string buscador, string Buscador, int? numeroPagina, int? tamanoPagina)
        {
            ViewData["Ordenacion"] = ordenacion;
            ViewData["OrdenacionNombres"] = string.IsNullOrEmpty(ordenacion) ? "nombres_descendientes" : "";

            if (buscador != null)
                numeroPagina = 1;
            else
                buscador = Buscador;

            buscador = (buscador ?? "").Trim();
            ViewData["Buscador"] = buscador;

            int tamPag = tamanoPagina ?? 5;
            ViewData["TamanoPagina"] = tamPag;

            var catalogos = _context.CatalogoArticulos
                .Include(c => c.Categoria)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(buscador))
            {
                catalogos = catalogos.Where(c =>
                    EF.Functions.Like(c.Nombre, $"%{buscador}%") ||
                    EF.Functions.Like(c.Categoria.Nombre, $"%{buscador}%")
                );
            }

            switch (ordenacion)
            {
                case "nombres_descendientes":
                    catalogos = catalogos.OrderByDescending(c => c.Nombre);
                    break;
                default:
                    catalogos = catalogos.OrderBy(c => c.Nombre);
                    break;
            }
            ViewBag.UnidadesMedida = new SelectList(
            _context.UnidadesMedidas.Where(u => u.Estatus).ToList(),
            "IdUnidadMedida",
            "Nombre");

            ViewBag.Categorias = new SelectList(
                _context.Categorias.Where(c => c.Estatus).ToList(),
                "IdCategoria",
                "Nombre"
            );

            return View(await Paginacion<CatalogoArticulo>.CreateAsync(catalogos, numeroPagina ?? 1, tamPag));
        }


        // GET: CatalogoArticulos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catalogoArticulo = await _context.CatalogoArticulos
                .Include(i => i.Categoria)
                .FirstOrDefaultAsync(m => m.IdCatalogoArticulo == id);
            if (catalogoArticulo == null)
            {
                return NotFound();
            }

            return View(catalogoArticulo);
        }

        // GET: CatalogoArticulos/Create
        public IActionResult Create()
        {
            ViewBag.Categorias = new SelectList(_context.Categorias.Where(c => c.Estatus).ToList(), "IdCategoria", "Nombre");
            ViewBag.UnidadesMedida = new SelectList(_context.UnidadesMedidas.Where(u => u.Estatus).ToList(), "IdUnidadMedida", "Nombre");

            return View();
        }

        // POST: CatalogoArticulos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCategoria,Nombre,Descripcion, IdUnidadMedida")] CatalogoArticulo catalogoArticulo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(catalogoArticulo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("", "No se ha podido crear el nuevo producto. Si persiste el error, póngase en contacto con el administrador.");
            }

            ViewBag.Categorias = new SelectList(_context.Categorias.Where(c => c.Estatus).ToList(), "IdCategoria", "Nombre", catalogoArticulo.IdCategoria);
            ViewBag.UnidadesMedida = new SelectList(_context.UnidadesMedidas.Where(u => u.Estatus).ToList(), "IdUnidadMedida", "Nombre", catalogoArticulo.IdUnidadMedida);

            return View(catalogoArticulo);
        }

        // GET: CatalogoArticulos/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var producto = await _context.CatalogoArticulos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.IdCatalogoArticulo == id);

            if (producto == null) return NotFound();

            var categorias = _context.Categorias
                .Where(c => c.Estatus)
                .Select(c => new { c.IdCategoria, c.Nombre })
                .ToList();
            var unidades = _context.UnidadesMedidas
                .Where(u => u.Estatus)
                .Select(u => new { u.IdUnidadMedida, u.Nombre })
                .ToList();

            return Json(new
            {
                idCatalogoArticulo = producto.IdCatalogoArticulo,
                idCategoria = producto.IdCategoria,
                idUnidadMedida = producto.IdUnidadMedida,
                nombre = producto.Nombre,
                descripcion = producto.Descripcion,
                categorias
            });
        }

        // POST: CatalogoArticulos/EditPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id, [FromForm] int IdCategoria, [FromForm] string Nombre, [FromForm] string Descripcion, [FromForm] int? IdUnidadMedida)
        {
            var producto = await _context.CatalogoArticulos.FindAsync(id);
            if (producto == null) return NotFound();

            producto.IdCategoria = IdCategoria;
            producto.Nombre = Nombre;
            producto.Descripcion = Descripcion;
            producto.IdUnidadMedida = IdUnidadMedida;


            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateException)
            {
                return BadRequest("No se pudo actualizar el producto");
            }
        }

        // GET: CatalogoArticulos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catalogoArticulo = await _context.CatalogoArticulos
                .FirstOrDefaultAsync(m => m.IdCatalogoArticulo == id);
            if (catalogoArticulo == null)
            {
                return NotFound();
            }

            return View(catalogoArticulo);
        }

        // POST: CatalogoArticulos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var catalogoArticulo = await _context.CatalogoArticulos.FindAsync(id);
            if (catalogoArticulo != null)
            {
                _context.CatalogoArticulos.Remove(catalogoArticulo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CatalogoArticuloExists(int id)
        {
            return _context.CatalogoArticulos.Any(e => e.IdCatalogoArticulo == id);
        }

        // GET: CatalogoArticulos/DetailsJson/5
        [HttpGet]
        public async Task<IActionResult> DetailsJson(int? id)
        {
            if (id == null) return BadRequest();

            var p = await _context.CatalogoArticulos
                .AsNoTracking()
                .Include(x => x.Categoria)
                .FirstOrDefaultAsync(x => x.IdCatalogoArticulo == id);

            if (p == null) return NotFound();

            return Json(new
            {
                idCatalogoArticulo = p.IdCatalogoArticulo,
                categoria = p.Categoria?.Nombre,
                nombre = p.Nombre,
                descripcion = p.Descripcion,
                unidadMedida = p.UnidadMedida?.Nombre,
                estatus = p.Estatus
            });
        }

        // POST: CatalogoArticulos/CambiarEstatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstatus(int id)
        {
            var producto = await _context.CatalogoArticulos.FindAsync(id);
            if (producto == null) return NotFound();

            try
            {
                producto.Estatus = !producto.Estatus;
                await _context.SaveChangesAsync();
                return Ok(new { estatus = producto.Estatus });
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult GetByCategoria(int idCategoria)
        {
            var productos = _context.CatalogoArticulos
                .Where(c => c.IdCategoria == idCategoria && c.Estatus)
                .Select(c => new
                {
                    idCatalogoArticulo = c.IdCatalogoArticulo,
                    nombre = c.Nombre
                })
                .ToList();

            return Json(productos);
        }

    }
}
