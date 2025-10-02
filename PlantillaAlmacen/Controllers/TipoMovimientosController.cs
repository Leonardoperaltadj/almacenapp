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
    public class TipoMovimientosController : Controller
    {
        private readonly AlmacenDbContext _context;

        public TipoMovimientosController(AlmacenDbContext context)
        {
            _context = context;
        }

        // GET: TipoMovimientos
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoMovimientos.ToListAsync());
        }

        // GET: TipoMovimientos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoMovimiento = await _context.TipoMovimientos
                .FirstOrDefaultAsync(m => m.IdTipoMovimiento == id);
            if (tipoMovimiento == null)
            {
                return NotFound();
            }

            return View(tipoMovimiento);
        }

        // GET: TipoMovimientos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoMovimientos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipoMovimiento,Nombre,Estatus")] TipoMovimiento tipoMovimiento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoMovimiento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoMovimiento);
        }

        // GET: TipoMovimientos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoMovimiento = await _context.TipoMovimientos.FindAsync(id);
            if (tipoMovimiento == null)
            {
                return NotFound();
            }
            return View(tipoMovimiento);
        }

        // POST: TipoMovimientos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTipoMovimiento,Nombre,Estatus")] TipoMovimiento tipoMovimiento)
        {
            if (id != tipoMovimiento.IdTipoMovimiento)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoMovimiento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoMovimientoExists(tipoMovimiento.IdTipoMovimiento))
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
            return View(tipoMovimiento);
        }

        // GET: TipoMovimientos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoMovimiento = await _context.TipoMovimientos
                .FirstOrDefaultAsync(m => m.IdTipoMovimiento == id);
            if (tipoMovimiento == null)
            {
                return NotFound();
            }

            return View(tipoMovimiento);
        }

        // POST: TipoMovimientos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoMovimiento = await _context.TipoMovimientos.FindAsync(id);
            if (tipoMovimiento != null)
            {
                _context.TipoMovimientos.Remove(tipoMovimiento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoMovimientoExists(int id)
        {
            return _context.TipoMovimientos.Any(e => e.IdTipoMovimiento == id);
        }
    }
}
