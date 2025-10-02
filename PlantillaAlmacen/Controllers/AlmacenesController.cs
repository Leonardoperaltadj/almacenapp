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
    [Route("[controller]")]
    public class AlmacenesController : Controller
    {
        private readonly AlmacenDbContext _context;

        public AlmacenesController(AlmacenDbContext context)
        {
            _context = context;
        }

        // GET: Almacenes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Almacenes.ToListAsync());
        }

        // GET: Almacenes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var almacen = await _context.Almacenes
                .FirstOrDefaultAsync(m => m.IdAlmacen == id);
            if (almacen == null)
            {
                return NotFound();
            }

            return View(almacen);
        }

        // GET: Almacenes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Almacenes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAlmacen,Nombre,Ubicacion,Estatus")] Almacen almacen)
        {
            if (ModelState.IsValid)
            {
                _context.Add(almacen);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(almacen);
        }

        // GET: Almacenes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var almacen = await _context.Almacenes.FindAsync(id);
            if (almacen == null)
            {
                return NotFound();
            }
            return View(almacen);
        }

        // POST: Almacenes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAlmacen,Nombre,Ubicacion,Estatus")] Almacen almacen)
        {
            if (id != almacen.IdAlmacen)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(almacen);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlmacenExists(almacen.IdAlmacen))
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
            return View(almacen);
        }

        // GET: Almacenes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var almacen = await _context.Almacenes
                .FirstOrDefaultAsync(m => m.IdAlmacen == id);
            if (almacen == null)
            {
                return NotFound();
            }

            return View(almacen);
        }

        // POST: Almacenes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var almacen = await _context.Almacenes.FindAsync(id);
            if (almacen != null)
            {
                _context.Almacenes.Remove(almacen);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlmacenExists(int id)
        {
            return _context.Almacenes.Any(e => e.IdAlmacen == id);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var almacenes = _context.Almacenes
                .Select(a => new { idAlmacen = a.IdAlmacen, nombre = a.Nombre })
                .ToList();

            return Json(almacenes);
        }
    }
}
