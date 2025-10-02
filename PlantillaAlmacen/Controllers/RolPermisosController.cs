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
    public class RolPermisosController : Controller
    {
        private readonly AlmacenDbContext _context;

        public RolPermisosController(AlmacenDbContext context)
        {
            _context = context;
        }

        // GET: RolPermisos
        public async Task<IActionResult> Index()
        {
            return View(await _context.RolPermisos.ToListAsync());
        }

        // GET: RolPermisos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rolPermiso = await _context.RolPermisos
                .FirstOrDefaultAsync(m => m.IdRol == id);
            if (rolPermiso == null)
            {
                return NotFound();
            }

            return View(rolPermiso);
        }

        // GET: RolPermisos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RolPermisos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRol,IdPermiso")] RolPermiso rolPermiso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rolPermiso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rolPermiso);
        }

        // GET: RolPermisos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rolPermiso = await _context.RolPermisos.FindAsync(id);
            if (rolPermiso == null)
            {
                return NotFound();
            }
            return View(rolPermiso);
        }

        // POST: RolPermisos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRol,IdPermiso")] RolPermiso rolPermiso)
        {
            if (id != rolPermiso.IdRol)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rolPermiso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RolPermisoExists(rolPermiso.IdRol))
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
            return View(rolPermiso);
        }

        // GET: RolPermisos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rolPermiso = await _context.RolPermisos
                .FirstOrDefaultAsync(m => m.IdRol == id);
            if (rolPermiso == null)
            {
                return NotFound();
            }

            return View(rolPermiso);
        }

        // POST: RolPermisos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rolPermiso = await _context.RolPermisos.FindAsync(id);
            if (rolPermiso != null)
            {
                _context.RolPermisos.Remove(rolPermiso);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RolPermisoExists(int id)
        {
            return _context.RolPermisos.Any(e => e.IdRol == id);
        }
    }
}
