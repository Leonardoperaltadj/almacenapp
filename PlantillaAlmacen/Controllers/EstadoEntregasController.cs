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
    public class EstadoEntregasController : Controller
    {
        private readonly AlmacenDbContext _context;

        public EstadoEntregasController(AlmacenDbContext context)
        {
            _context = context;
        }

        // GET: EstadoEntregas
        public async Task<IActionResult> Index()
        {
            return View(await _context.EstadoEntregas.ToListAsync());
        }

        // GET: EstadoEntregas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoEntrega = await _context.EstadoEntregas
                .FirstOrDefaultAsync(m => m.IdEstadoEntrega == id);
            if (estadoEntrega == null)
            {
                return NotFound();
            }

            return View(estadoEntrega);
        }

        // GET: EstadoEntregas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EstadoEntregas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEstadoEntrega,Nombre,Estatus")] EstadoEntrega estadoEntrega)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estadoEntrega);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estadoEntrega);
        }

        // GET: EstadoEntregas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoEntrega = await _context.EstadoEntregas.FindAsync(id);
            if (estadoEntrega == null)
            {
                return NotFound();
            }
            return View(estadoEntrega);
        }

        // POST: EstadoEntregas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEstadoEntrega,Nombre,Estatus")] EstadoEntrega estadoEntrega)
        {
            if (id != estadoEntrega.IdEstadoEntrega)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estadoEntrega);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstadoEntregaExists(estadoEntrega.IdEstadoEntrega))
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
            return View(estadoEntrega);
        }

        // GET: EstadoEntregas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoEntrega = await _context.EstadoEntregas
                .FirstOrDefaultAsync(m => m.IdEstadoEntrega == id);
            if (estadoEntrega == null)
            {
                return NotFound();
            }

            return View(estadoEntrega);
        }

        // POST: EstadoEntregas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estadoEntrega = await _context.EstadoEntregas.FindAsync(id);
            if (estadoEntrega != null)
            {
                _context.EstadoEntregas.Remove(estadoEntrega);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstadoEntregaExists(int id)
        {
            return _context.EstadoEntregas.Any(e => e.IdEstadoEntrega == id);
        }
    }
}
