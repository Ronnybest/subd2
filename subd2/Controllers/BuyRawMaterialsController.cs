using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using subd2.Data;
using subd2.Models;

namespace subd2.Controllers
{
    public class BuyRawMaterialsController : Controller
    {
        private readonly Lab610Context _context;

        public BuyRawMaterialsController(Lab610Context context)
        {
            _context = context;
        }

        // GET: BuyRawMaterials
        public async Task<IActionResult> Index()
        {
            var lab610Context = _context.BuyRawMaterials.Include(b => b.EmployeeNavigation).Include(b => b.RawMaterialsNavigation);
            return View(await lab610Context.ToListAsync());
        }

        // GET: BuyRawMaterials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BuyRawMaterials == null)
            {
                return NotFound();
            }

            var buyRawMaterial = await _context.BuyRawMaterials
                .Include(b => b.EmployeeNavigation)
                .Include(b => b.RawMaterialsNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (buyRawMaterial == null)
            {
                return NotFound();
            }

            return View(buyRawMaterial);
        }

        // GET: BuyRawMaterials/Create
        public IActionResult Create()
        {
            ViewBag.Date = DateTime.Now;
            string querry = "SELECT * from dbo.Budget where ID = 1";
            FormattableString formattableString = FormattableStringFactory.Create(querry);
            var result = _context.Budgets.FromSql(formattableString).FirstOrDefault();
            var ok = result.Budget1.GetValueOrDefault();
            ViewBag.Budget = ok.ToString();
            ViewData["Employee"] = new SelectList(_context.Stuffs, "Id", "FullName");
            ViewData["RawMaterials"] = new SelectList(_context.RawMaterials, "Id", "Name");
            return View();
        }

        // POST: BuyRawMaterials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RawMaterials,Count,Summ,Date,Employee")] BuyRawMaterial buyRawMaterial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(buyRawMaterial);
                await _context.SaveChangesAsync();
                string querry = String.Format($"UPDATE Budget SET Budget.Budget -= {buyRawMaterial.Summ}");
                FormattableString formattableString = FormattableStringFactory.Create(querry);
                await _context.Database.ExecuteSqlInterpolatedAsync(formattableString);
                querry = $"UPDATE Raw_materials SET Count += {buyRawMaterial.Count} where ID = {buyRawMaterial.RawMaterials}";
                FormattableString formattableString2 = FormattableStringFactory.Create(querry);
                await _context.Database.ExecuteSqlInterpolatedAsync(formattableString2);
                querry = $"UPDATE Raw_materials SET Sum += {buyRawMaterial.Summ} where ID = {buyRawMaterial.RawMaterials}";
                FormattableString formattableString3 = FormattableStringFactory.Create(querry);
                await _context.Database.ExecuteSqlInterpolatedAsync(formattableString3);
                querry = $"UPDATE Stuff SET BuyRawMatCount += 1 where ID = {buyRawMaterial.Employee}";
                FormattableString formattableString4 = FormattableStringFactory.Create(querry);
                await _context.Database.ExecuteSqlInterpolatedAsync(formattableString4);
                var buyRawMaterialWithNames = _context.BuyRawMaterials.Include(b => b.RawMaterialsNavigation)
                    .Include(b => b.EmployeeNavigation)
                    .FirstOrDefault(b => b.Id == buyRawMaterial.Id);
                var historyOfProduct = new HistoryOfProduct
                {
                    Product = buyRawMaterialWithNames?.RawMaterialsNavigation?.Name,
                    Count = buyRawMaterialWithNames?.Count,
                    Summ = buyRawMaterialWithNames?.Summ,
                    Date = buyRawMaterialWithNames?.Date,
                    Employee = buyRawMaterialWithNames?.EmployeeNavigation?.FullName, 
                    Action = "Покупка сырья"
                };
                _context.HistoryOfProducts.Add(historyOfProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Employee"] = new SelectList(_context.Stuffs, "Id", "Id", buyRawMaterial.Employee);
            ViewData["RawMaterials"] = new SelectList(_context.RawMaterials, "Id", "Id", buyRawMaterial.RawMaterials);
            return View(buyRawMaterial);
        }

        // GET: BuyRawMaterials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BuyRawMaterials == null)
            {
                return NotFound();
            }

            var buyRawMaterial = await _context.BuyRawMaterials.FindAsync(id);
            if (buyRawMaterial == null)
            {
                return NotFound();
            }
            ViewData["Employee"] = new SelectList(_context.Stuffs, "Id", "Id", buyRawMaterial.Employee);
            ViewData["RawMaterials"] = new SelectList(_context.RawMaterials, "Id", "Id", buyRawMaterial.RawMaterials);
            return View(buyRawMaterial);
        }

        // POST: BuyRawMaterials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RawMaterials,Count,Summ,Date,Employee")] BuyRawMaterial buyRawMaterial)
        {
            if (id != buyRawMaterial.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(buyRawMaterial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuyRawMaterialExists(buyRawMaterial.Id))
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
            ViewData["Employee"] = new SelectList(_context.Stuffs, "Id", "Id", buyRawMaterial.Employee);
            ViewData["RawMaterials"] = new SelectList(_context.RawMaterials, "Id", "Id", buyRawMaterial.RawMaterials);
            return View(buyRawMaterial);
        }

        // GET: BuyRawMaterials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BuyRawMaterials == null)
            {
                return NotFound();
            }

            var buyRawMaterial = await _context.BuyRawMaterials
                .Include(b => b.EmployeeNavigation)
                .Include(b => b.RawMaterialsNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (buyRawMaterial == null)
            {
                return NotFound();
            }

            return View(buyRawMaterial);
        }

        // POST: BuyRawMaterials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BuyRawMaterials == null)
            {
                return Problem("Entity set 'Lab610Context.BuyRawMaterials'  is null.");
            }
            var buyRawMaterial = await _context.BuyRawMaterials.FindAsync(id);
            if (buyRawMaterial != null)
            {
                _context.BuyRawMaterials.Remove(buyRawMaterial);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BuyRawMaterialExists(int id)
        {
          return _context.BuyRawMaterials.Any(e => e.Id == id);
        }
    }
}
