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
    public class CreateProductsController : Controller
    {
        private readonly Lab610Context _context;

        public CreateProductsController(Lab610Context context)
        {
            _context = context;
        }

        // GET: CreateProducts
        public async Task<IActionResult> Index()
        {
            var lab610Context = _context.CreateProducts.Include(c => c.EmployeeNavigation).Include(c => c.ProductNavigation);
            return View(await lab610Context.ToListAsync());
        }

        // GET: CreateProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CreateProducts == null)
            {
                return NotFound();
            }

            var createProduct = await _context.CreateProducts
                .Include(c => c.EmployeeNavigation)
                .Include(c => c.ProductNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (createProduct == null)
            {
                return NotFound();
            }

            return View(createProduct);
        }

        // GET: CreateProducts/Create
        public IActionResult Create()
        {
            ViewBag.Date = DateTime.Now;
            ViewData["Employee"] = new SelectList(_context.Stuffs, "Id", "FullName");
            ViewData["Product"] = new SelectList(_context.FinishedProducts, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Product,Count,Date,Employee")] CreateProduct createProduct)
        {
            decimal? summForFinishedProduct = 0;
            if (ModelState.IsValid)
            {
                var needRecipe = _context.Ingredients.Where(i => i.NameProduction == createProduct.Product).ToList();
                for(int i = 0; i < needRecipe.Count; i++)
                {
                    
                    string checkRawMat = $"select Count from Raw_materials where ID = {needRecipe[i].RawMaterials}";
                    FormattableString formattablecheckRawMat = FormattableStringFactory.Create(checkRawMat);
                    var result = await _context.RawMaterials.FromSqlInterpolated(formattablecheckRawMat).Select(rm => rm.Count).FirstOrDefaultAsync();
                    if ((needRecipe[i].Count * createProduct.Count) > Convert.ToDecimal(result))
                    {
                        TempData["Message"] = "Недостаточно сырья";
                        return RedirectToAction(nameof(Create));
                    }
                }
                for (int i = 0; i < needRecipe.Count; i++)
                {
                    string checkRawMat = $"select Count from Raw_materials where ID = {needRecipe[i].RawMaterials}";
                    FormattableString formattablecheckRawMat = FormattableStringFactory.Create(checkRawMat);
                    var countRawMat = await _context.RawMaterials.FromSqlInterpolated(formattablecheckRawMat).Select(rm => rm.Count).FirstOrDefaultAsync();
                    string getSumm = $"select Sum from Raw_materials where ID = {needRecipe[i].RawMaterials}";
                    FormattableString formattablegetSumm = FormattableStringFactory.Create(getSumm);
                    var summRawMat = await _context.RawMaterials.FromSqlInterpolated(formattablegetSumm).Select(rm => rm.Sum).FirstOrDefaultAsync();
                    var minusSum = summRawMat / countRawMat * createProduct.Count * needRecipe[i].Count;
                    string querry_minusRawMat = $"update Raw_materials set Count -= {needRecipe[i].Count * createProduct.Count} where ID = {needRecipe[i].RawMaterials}";
                    FormattableString formattablequerry_minusRawMat = FormattableStringFactory.Create(querry_minusRawMat);
                    await _context.Database.ExecuteSqlAsync(formattablequerry_minusRawMat);
                    string querry_minusSum = $"update Raw_materials set Sum -= {minusSum} where ID = {needRecipe[i].RawMaterials}";
                    FormattableString formattablequerry_minusSum = FormattableStringFactory.Create(querry_minusSum);
                    await _context.Database.ExecuteSqlAsync(formattablequerry_minusSum);
                    summForFinishedProduct += minusSum;
                }
                string querry_plusFinishProduct = $"update Finished_products set Count += {createProduct.Count} where ID = {createProduct.Product}";
                FormattableString formattablequerry_plusFinishProduct = FormattableStringFactory.Create(querry_plusFinishProduct);
                await _context.Database.ExecuteSqlAsync(formattablequerry_plusFinishProduct);
                string querryPlusSummFinProd = $"update Finished_products set Sum += {summForFinishedProduct} where ID = {createProduct.Product}";
                FormattableString formattablequerryPlusSummFinProd = FormattableStringFactory.Create(querryPlusSummFinProd);
                await _context.Database.ExecuteSqlAsync(formattablequerryPlusSummFinProd);
                string querry = $"UPDATE Stuff SET CreateProductCount += 1 where ID = {createProduct.Employee}";
                FormattableString formattableString4 = FormattableStringFactory.Create(querry);
                await _context.Database.ExecuteSqlInterpolatedAsync(formattableString4);
                _context.Add(createProduct);
                await _context.SaveChangesAsync();
                var createProductWithNames = _context.CreateProducts.Include(b => b.ProductNavigation)
                    .Include(b => b.EmployeeNavigation)
                    .FirstOrDefault(b => b.Id == createProduct.Id);
                
                var historyOfProduct = new HistoryOfProduct
                {
                    Product = createProductWithNames?.ProductNavigation.Name,
                    Count = createProductWithNames?.Count,
                    Summ = 0,
                    Date = createProductWithNames?.Date,
                    Employee = createProductWithNames?.EmployeeNavigation?.FullName,
                    Action = "Производство сырья"
                };
                _context.HistoryOfProducts.Add(historyOfProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Employee"] = new SelectList(_context.Stuffs, "Id", "Id", createProduct.Employee);
            ViewData["Product"] = new SelectList(_context.FinishedProducts, "Id", "Id", createProduct.Product);
            return View(createProduct);
        }

        // GET: CreateProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CreateProducts == null)
            {
                return NotFound();
            }

            var createProduct = await _context.CreateProducts.FindAsync(id);
            if (createProduct == null)
            {
                return NotFound();
            }
            ViewData["Employee"] = new SelectList(_context.Stuffs, "Id", "Id", createProduct.Employee);
            ViewData["Product"] = new SelectList(_context.FinishedProducts, "Id", "Id", createProduct.Product);
            return View(createProduct);
        }

        // POST: CreateProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Product,Count,Date,Employee")] CreateProduct createProduct)
        {
            if (id != createProduct.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(createProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CreateProductExists(createProduct.Id))
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
            ViewData["Employee"] = new SelectList(_context.Stuffs, "Id", "Id", createProduct.Employee);
            ViewData["Product"] = new SelectList(_context.FinishedProducts, "Id", "Id", createProduct.Product);
            return View(createProduct);
        }

        // GET: CreateProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CreateProducts == null)
            {
                return NotFound();
            }

            var createProduct = await _context.CreateProducts
                .Include(c => c.EmployeeNavigation)
                .Include(c => c.ProductNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (createProduct == null)
            {
                return NotFound();
            }

            return View(createProduct);
        }

        // POST: CreateProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CreateProducts == null)
            {
                return Problem("Entity set 'Lab610Context.CreateProducts'  is null.");
            }
            var createProduct = await _context.CreateProducts.FindAsync(id);
            if (createProduct != null)
            {
                _context.CreateProducts.Remove(createProduct);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CreateProductExists(int id)
        {
          return _context.CreateProducts.Any(e => e.Id == id);
        }
    }
}
