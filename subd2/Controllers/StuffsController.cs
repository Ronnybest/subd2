using System;
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
    public class StuffsController : Controller
    {
        private readonly Lab610Context _context;

        public StuffsController(Lab610Context context)
        {
            _context = context;
        }

        // GET: Stuffs
        public async Task<IActionResult> Index()
        {
            var lab610Context = _context.Stuffs.Include(s => s.JobTitleNavigation);
            return View(await lab610Context.ToListAsync());
        }

        // GET: Stuffs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Stuffs == null)
            {
                return NotFound();
            }

            var stuff = await _context.Stuffs
                .Include(s => s.JobTitleNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stuff == null)
            {
                return NotFound();
            }

            return View(stuff);
        }

        // GET: Stuffs/Create
        public IActionResult Create()
        {
            ViewData["JobTitle"] = new SelectList(_context.JobsTitles, "Id", "JobTitle");
            return View();
        }

        // POST: Stuffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,JobTitle,Salary,Address,TelephoneNumber")] Stuff stuff)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stuff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["JobTitle"] = new SelectList(_context.JobsTitles, "Id", "JobTitle", stuff.JobTitle);
            return View(stuff);
        }

        // GET: Stuffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Stuffs == null)
            {
                return NotFound();
            }

            var stuff = await _context.Stuffs.FindAsync(id);
            if (stuff == null)
            {
                return NotFound();
            }
            ViewData["JobTitle"] = new SelectList(_context.JobsTitles, "Id", "JobTitle", stuff.JobTitle);
            return View(stuff);
        }

        // POST: Stuffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,JobTitle,Salary,Address,TelephoneNumber")] Stuff stuff)
        {
            if (id != stuff.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stuff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StuffExists(stuff.Id))
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
            ViewData["JobTitle"] = new SelectList(_context.JobsTitles, "Id", "JobTitle", stuff.JobTitle);
            return View(stuff);
        }

        // GET: Stuffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Stuffs == null)
            {
                return NotFound();
            }

            var stuff = await _context.Stuffs
                .Include(s => s.JobTitleNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stuff == null)
            {
                return NotFound();
            }

            return View(stuff);
        }

        // POST: Stuffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Stuffs == null)
            {
                return Problem("Entity set 'Lab610Context.Stuffs'  is null.");
            }
            var stuff = await _context.Stuffs.FindAsync(id);
            if (stuff != null)
            {
                _context.Stuffs.Remove(stuff);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StuffExists(int id)
        {
          return _context.Stuffs.Any(e => e.Id == id);
        }
    }
}
