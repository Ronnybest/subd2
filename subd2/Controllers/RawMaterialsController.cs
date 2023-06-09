﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using subd2.Data;
using subd2.Models;

namespace subd2.Controllers
{
    public class RawMaterialsController : Controller
    {
        private readonly Lab610Context _context;

        public RawMaterialsController(Lab610Context context)
        {
            _context = context;
        }

        // GET: RawMaterials
        public async Task<IActionResult> Index()
        {
            var lab610Context = _context.RawMaterials.Include(r => r.UnitNavigation);
            return View(await lab610Context.ToListAsync());
        }

        // GET: RawMaterials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RawMaterials == null)
            {
                return NotFound();
            }

            var rawMaterial = await _context.RawMaterials
                .Include(r => r.UnitNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rawMaterial == null)
            {
                return NotFound();
            }

            return View(rawMaterial);
        }

        // GET: RawMaterials/Create
        public IActionResult Create()
        {
            ViewData["Unit"] = new SelectList(_context.Units, "Id", "Name");
            return View();
        }

        // POST: RawMaterials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Count,Sum,Unit,Cost")] RawMaterial rawMaterial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rawMaterial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Unit"] = new SelectList(_context.Units, "Id", "Name", rawMaterial.Unit);
            return View(rawMaterial);
        }

        // GET: RawMaterials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RawMaterials == null)
            {
                return NotFound();
            }

            var rawMaterial = await _context.RawMaterials.FindAsync(id);
            if (rawMaterial == null)
            {
                return NotFound();
            }
            ViewData["Unit"] = new SelectList(_context.Units, "Id", "Name", rawMaterial.Unit);
            return View(rawMaterial);
        }

        // POST: RawMaterials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Count,Sum,Unit,Cost")] RawMaterial rawMaterial)
        {
            if (id != rawMaterial.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rawMaterial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RawMaterialExists(rawMaterial.Id))
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
            ViewData["Unit"] = new SelectList(_context.Units, "Id", "Name", rawMaterial.Unit);
            return View(rawMaterial);
        }

        // GET: RawMaterials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RawMaterials == null)
            {
                return NotFound();
            }

            var rawMaterial = await _context.RawMaterials
                .Include(r => r.UnitNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rawMaterial == null)
            {
                return NotFound();
            }

            return View(rawMaterial);
        }

        // POST: RawMaterials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RawMaterials == null)
            {
                return Problem("Entity set 'Lab610Context.RawMaterials'  is null.");
            }
            var rawMaterial = await _context.RawMaterials.FindAsync(id);
            if (rawMaterial != null)
            {
                _context.RawMaterials.Remove(rawMaterial);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RawMaterialExists(int id)
        {
          return _context.RawMaterials.Any(e => e.Id == id);
        }
    }
}
