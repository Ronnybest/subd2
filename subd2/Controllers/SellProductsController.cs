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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace subd2.Controllers
{
    public class SellProductsController : Controller
    {
        private readonly Lab610Context _context;

        public SellProductsController(Lab610Context context)
        {
            _context = context;
        }

        // GET: SellProducts
        public async Task<IActionResult> Index()
        {
            var lab610Context = _context.SellProducts.Include(s => s.EmployeeNavigation).Include(s => s.ProductNavigation);
            return View(await lab610Context.ToListAsync());
        }

        // GET: SellProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SellProducts == null)
            {
                return NotFound();
            }

            var sellProduct = await _context.SellProducts
                .Include(s => s.EmployeeNavigation)
                .Include(s => s.ProductNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sellProduct == null)
            {
                return NotFound();
            }

            return View(sellProduct);
        }

        // GET: SellProducts/Create
        public IActionResult Create()
        {
            string getPercent = $"select [Percent] from Budget where ID = 1";
            FormattableString formattablegetPercent = FormattableStringFactory.Create(getPercent);
            var result = _context.Budgets.FromSqlInterpolated(formattablegetPercent).Select(rm => rm.Percent).FirstOrDefault();
            ViewBag.Percent = result;
            ViewBag.Date = DateTime.Now;
            ViewData["Employee"] = new SelectList(_context.Stuffs, "Id", "FullName");
            ViewData["Product"] = new SelectList(_context.FinishedProducts, "Id", "Name");
            return View();
        }

        // POST: SellProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Product,Count,Summ,Date,Employee")] SellProduct sellProduct, decimal? percent)
        {
            string checkRawMat = $"select Count from Finished_products where ID = {sellProduct.Product}";
            FormattableString formattablecheckRawMat = FormattableStringFactory.Create(checkRawMat);
            var result = await _context.FinishedProducts.FromSqlInterpolated(formattablecheckRawMat).Select(rm => rm.Count).FirstOrDefaultAsync();
            if(result < sellProduct.Count)
            {
                TempData["Message"] = "Недостаточно продукта на складе";
                return RedirectToAction(nameof(Create));
            }
            if (ModelState.IsValid)
            {
                string CostFinishedProductOne = $"select Cost from Finished_products where ID = {sellProduct.Product}";
                FormattableString formattableStringCostFinishedProductOne = FormattableStringFactory.Create(CostFinishedProductOne);
                var CostFinishedProductOneResult = await _context.FinishedProducts.FromSqlInterpolated(formattableStringCostFinishedProductOne).Select(rm => rm.Cost).FirstOrDefaultAsync();
                string querry_sellProduct = $"update Finished_products set Count -= {sellProduct.Count} where ID = {sellProduct.Product}";
                FormattableString formattablequerry_sellProduct = FormattableStringFactory.Create(querry_sellProduct);
                await _context.Database.ExecuteSqlAsync(formattablequerry_sellProduct);
                string querry_updBudget = $"update Budget set Budget += {CostFinishedProductOneResult * sellProduct.Count * (1+percent/100)} where ID = 1";
                FormattableString formattablequerry_updBudget = FormattableStringFactory.Create(querry_updBudget);
                await _context.Database.ExecuteSqlAsync(formattablequerry_updBudget);
                sellProduct.Summ = CostFinishedProductOneResult * sellProduct.Count * (1 + percent / 100);
                string MinusSumm = $"update Finished_products set Sum -= {CostFinishedProductOneResult * sellProduct.Count} where ID = {sellProduct.Product}";
                FormattableString formattablequerry_updSum = FormattableStringFactory.Create(MinusSumm);
                await _context.Database.ExecuteSqlAsync(formattablequerry_updSum);
                string querry = $"UPDATE Stuff SET SellProductCount += 1 where ID = {sellProduct.Employee}";
                FormattableString formattableString4 = FormattableStringFactory.Create(querry);
                await _context.Database.ExecuteSqlInterpolatedAsync(formattableString4);
                _context.Add(sellProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Employee"] = new SelectList(_context.Stuffs, "Id", "Id", sellProduct.Employee);
            ViewData["Product"] = new SelectList(_context.FinishedProducts, "Id", "Id", sellProduct.Product);
            return View(sellProduct);
        }

        // GET: SellProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SellProducts == null)
            {
                return NotFound();
            }

            var sellProduct = await _context.SellProducts.FindAsync(id);
            if (sellProduct == null)
            {
                return NotFound();
            }
            ViewData["Employee"] = new SelectList(_context.Stuffs, "Id", "Id", sellProduct.Employee);
            ViewData["Product"] = new SelectList(_context.FinishedProducts, "Id", "Id", sellProduct.Product);
            return View(sellProduct);
        }

        // POST: SellProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Product,Count,Summ,Date,Employee")] SellProduct sellProduct)
        {
            if (id != sellProduct.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sellProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SellProductExists(sellProduct.Id))
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
            ViewData["Employee"] = new SelectList(_context.Stuffs, "Id", "Id", sellProduct.Employee);
            ViewData["Product"] = new SelectList(_context.FinishedProducts, "Id", "Id", sellProduct.Product);
            return View(sellProduct);
        }

        // GET: SellProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SellProducts == null)
            {
                return NotFound();
            }

            var sellProduct = await _context.SellProducts
                .Include(s => s.EmployeeNavigation)
                .Include(s => s.ProductNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sellProduct == null)
            {
                return NotFound();
            }

            return View(sellProduct);
        }

        // POST: SellProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SellProducts == null)
            {
                return Problem("Entity set 'Lab610Context.SellProducts'  is null.");
            }
            var sellProduct = await _context.SellProducts.FindAsync(id);
            if (sellProduct != null)
            {
                _context.SellProducts.Remove(sellProduct);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SellProductExists(int id)
        {
          return _context.SellProducts.Any(e => e.Id == id);
        }
    }
}
