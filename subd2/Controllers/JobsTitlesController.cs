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
    public class JobsTitlesController : Controller
    {
        private readonly Lab610Context _context;

        public JobsTitlesController(Lab610Context context)
        {
            _context = context;
        }

        // GET: JobsTitles
        public async Task<IActionResult> Index()
        {
              return View(await _context.JobsTitles.ToListAsync());
        }

        // GET: JobsTitles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.JobsTitles == null)
            {
                return NotFound();
            }

            var jobsTitle = await _context.JobsTitles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobsTitle == null)
            {
                return NotFound();
            }

            return View(jobsTitle);
        }

        // GET: JobsTitles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JobsTitles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,JobTitle")] JobsTitle jobsTitle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jobsTitle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jobsTitle);
        }

        // GET: JobsTitles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.JobsTitles == null)
            {
                return NotFound();
            }

            var jobsTitle = await _context.JobsTitles.FindAsync(id);
            if (jobsTitle == null)
            {
                return NotFound();
            }
            return View(jobsTitle);
        }

        // POST: JobsTitles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,JobTitle")] JobsTitle jobsTitle)
        {
            if (id != jobsTitle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobsTitle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobsTitleExists(jobsTitle.Id))
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
            return View(jobsTitle);
        }

        // GET: JobsTitles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.JobsTitles == null)
            {
                return NotFound();
            }

            var jobsTitle = await _context.JobsTitles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobsTitle == null)
            {
                return NotFound();
            }

            return View(jobsTitle);
        }

        // POST: JobsTitles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.JobsTitles == null)
            {
                return Problem("Entity set 'Lab610Context.JobsTitles'  is null.");
            }
            var jobsTitle = await _context.JobsTitles.FindAsync(id);
            if (jobsTitle != null)
            {
                _context.JobsTitles.Remove(jobsTitle);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobsTitleExists(int id)
        {
          return _context.JobsTitles.Any(e => e.Id == id);
        }
    }
}
