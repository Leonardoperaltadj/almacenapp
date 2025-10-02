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
    public class EstatusArticulosController : Controller
    {
        private readonly AlmacenDbContext _context;

        public EstatusArticulosController(AlmacenDbContext context)
        {
            _context = context;
        }

        // GET: EstatusArticulos
        public async Task<IActionResult> Index()
        {
            return View(await _context.EstatusArticulos.ToListAsync());
        }

        // GET: EstatusArticulos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estatusArticulo = await _context.EstatusArticulos
                .FirstOrDefaultAsync(m => m.IdEstatusArticulo == id);
            if (estatusArticulo == null)
            {
                return NotFound();
            }

            return View(estatusArticulo);
        }

        // GET: EstatusArticulos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EstatusArticulos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEstatusArticulo,Nombre,Estatus")] EstatusArticulo estatusArticulo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estatusArticulo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estatusArticulo);
        }

        // GET: EstatusArticulos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estatusArticulo = await _context.EstatusArticulos.FindAsync(id);
            if (estatusArticulo == null)
            {
                return NotFound();
            }
            return View(estatusArticulo);
        }

        // POST: EstatusArticulos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEstatusArticulo,Nombre,Estatus")] EstatusArticulo estatusArticulo)
        {
            if (id != estatusArticulo.IdEstatusArticulo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estatusArticulo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstatusArticuloExists(estatusArticulo.IdEstatusArticulo))
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
            return View(estatusArticulo);
        }

        // GET: EstatusArticulos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estatusArticulo = await _context.EstatusArticulos
                .FirstOrDefaultAsync(m => m.IdEstatusArticulo == id);
            if (estatusArticulo == null)
            {
                return NotFound();
            }

            return View(estatusArticulo);
        }

        // POST: EstatusArticulos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estatusArticulo = await _context.EstatusArticulos.FindAsync(id);
            if (estatusArticulo != null)
            {
                _context.EstatusArticulos.Remove(estatusArticulo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstatusArticuloExists(int id)
        {
            return _context.EstatusArticulos.Any(e => e.IdEstatusArticulo == id);
        }
    }
}
