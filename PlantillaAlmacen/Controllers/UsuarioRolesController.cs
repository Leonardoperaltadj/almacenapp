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
    public class UsuarioRolesController : Controller
    {
        private readonly AlmacenDbContext _context;

        public UsuarioRolesController(AlmacenDbContext context)
        {
            _context = context;
        }

        // GET: UsuarioRoles
        public async Task<IActionResult> Index()
        {
            return View(await _context.UsuarioRoles.ToListAsync());
        }

        // GET: UsuarioRoles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioRol = await _context.UsuarioRoles
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuarioRol == null)
            {
                return NotFound();
            }

            return View(usuarioRol);
        }

        // GET: UsuarioRoles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UsuarioRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUsuario,IdRol")] UsuarioRol usuarioRol)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuarioRol);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuarioRol);
        }

        // GET: UsuarioRoles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioRol = await _context.UsuarioRoles.FindAsync(id);
            if (usuarioRol == null)
            {
                return NotFound();
            }
            return View(usuarioRol);
        }

        // POST: UsuarioRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUsuario,IdRol")] UsuarioRol usuarioRol)
        {
            if (id != usuarioRol.IdUsuario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuarioRol);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioRolExists(usuarioRol.IdUsuario))
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
            return View(usuarioRol);
        }

        // GET: UsuarioRoles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioRol = await _context.UsuarioRoles
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuarioRol == null)
            {
                return NotFound();
            }

            return View(usuarioRol);
        }

        // POST: UsuarioRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuarioRol = await _context.UsuarioRoles.FindAsync(id);
            if (usuarioRol != null)
            {
                _context.UsuarioRoles.Remove(usuarioRol);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioRolExists(int id)
        {
            return _context.UsuarioRoles.Any(e => e.IdUsuario == id);
        }
    }
}
