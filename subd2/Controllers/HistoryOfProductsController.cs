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
    public class HistoryOfProductsController : Controller
    {
        private readonly Lab610Context _context;

        public HistoryOfProductsController(Lab610Context context)
        {
            _context = context;
        }

        // GET: HistoryOfProducts
        public async Task<IActionResult> Index()
        {
              return View(await _context.HistoryOfProducts.ToListAsync());
        }

        // GET: HistoryOfProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HistoryOfProducts == null)
            {
                return NotFound();
            }

            var historyOfProduct = await _context.HistoryOfProducts
                .FirstOrDefaultAsync(m => m.ID_Counter == id);
            if (historyOfProduct == null)
            {
                return NotFound();
            }

            return View(historyOfProduct);
        }

        // GET: HistoryOfProducts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HistoryOfProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Counter,Id,Product,Count,Summ,Date,Employee,Action,Idproduct")] HistoryOfProduct historyOfProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(historyOfProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(historyOfProduct);
        }

        // GET: HistoryOfProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HistoryOfProducts == null)
            {
                return NotFound();
            }

            var historyOfProduct = await _context.HistoryOfProducts.FindAsync(id);
            if (historyOfProduct == null)
            {
                return NotFound();
            }
            return View(historyOfProduct);
        }

        // POST: HistoryOfProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Counter,Id,Product,Count,Summ,Date,Employee,Action,Idproduct")] HistoryOfProduct historyOfProduct)
        {
            if (id != historyOfProduct.ID_Counter)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(historyOfProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HistoryOfProductExists(historyOfProduct.ID_Counter))
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
            return View(historyOfProduct);
        }

        // GET: HistoryOfProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HistoryOfProducts == null)
            {
                return NotFound();
            }

            var historyOfProduct = await _context.HistoryOfProducts
                .FirstOrDefaultAsync(m => m.ID_Counter == id);
            if (historyOfProduct == null)
            {
                return NotFound();
            }

            return View(historyOfProduct);
        }

        // POST: HistoryOfProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HistoryOfProducts == null)
            {
                return Problem("Entity set 'Lab610Context.HistoryOfProducts'  is null.");
            }
            var historyOfProduct = await _context.HistoryOfProducts.FindAsync(id);
            if (historyOfProduct != null)
            {
                _context.HistoryOfProducts.Remove(historyOfProduct);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HistoryOfProductExists(int id)
        {
          return _context.HistoryOfProducts.Any(e => e.ID_Counter == id);
        }
    }
}
