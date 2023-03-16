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
    public class FinishedProductsController : Controller
    {
        private readonly Lab610Context _context;

        public FinishedProductsController(Lab610Context context)
        {
            _context = context;
        }

        // GET: FinishedProducts
        public async Task<IActionResult> Index()
        {
            var lab610Context = _context.FinishedProducts.Include(f => f.Unit);
            return View(await lab610Context.ToListAsync());
        }

        // GET: FinishedProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FinishedProducts == null)
            {
                return NotFound();
            }

            var finishedProduct = await _context.FinishedProducts
                .Include(f => f.Unit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (finishedProduct == null)
            {
                return NotFound();
            }

            return View(finishedProduct);
        }

        // GET: FinishedProducts/Create
        public IActionResult Create()
        {
            ViewData["UnitId"] = new SelectList(_context.Units, "Id", "Name");
            return View();
        }

        // POST: FinishedProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,UnitId,Count,Sum,Cost,Sebes")] FinishedProduct finishedProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(finishedProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UnitId"] = new SelectList(_context.Units, "Id", "Name", finishedProduct.UnitId);
            return View(finishedProduct);
        }

        // GET: FinishedProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FinishedProducts == null)
            {
                return NotFound();
            }

            var finishedProduct = await _context.FinishedProducts.FindAsync(id);
            if (finishedProduct == null)
            {
                return NotFound();
            }
            ViewData["UnitId"] = new SelectList(_context.Units, "Id", "Name", finishedProduct.UnitId);
            return View(finishedProduct);
        }

        // POST: FinishedProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,UnitId,Count,Sum,Cost,Sebes")] FinishedProduct finishedProduct)
        {
            if (id != finishedProduct.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(finishedProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FinishedProductExists(finishedProduct.Id))
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
            ViewData["UnitId"] = new SelectList(_context.Units, "Id", "Name", finishedProduct.UnitId);
            return View(finishedProduct);
        }

        // GET: FinishedProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FinishedProducts == null)
            {
                return NotFound();
            }

            var finishedProduct = await _context.FinishedProducts
                .Include(f => f.Unit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (finishedProduct == null)
            {
                return NotFound();
            }

            return View(finishedProduct);
        }

        // POST: FinishedProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FinishedProducts == null)
            {
                return Problem("Entity set 'Lab610Context.FinishedProducts'  is null.");
            }
            var finishedProduct = await _context.FinishedProducts.FindAsync(id);
            if (finishedProduct != null)
            {
                _context.FinishedProducts.Remove(finishedProduct);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Recipe(int? id, [Bind("Id,Name,RawMaterial,Count")] Recipe recipe)
        {
            if (id == null || _context.FinishedProducts == null)
            {
                return NotFound();
            }
            string querry = "SELECT Ingredients.ID,Raw_materials.Unit as [UnitId],Unit.Name as [Unit] ,Ingredients.NameProduction as [ProductId] ," +
                " Finished_products.Name, Raw_materials.Name " +
                "as [RawMaterial]," +
                "Ingredients.Count, Ingredients.RawMaterials as [RawMatId] from Ingredients JOIN Raw_Materials on Ingredients.RawMaterials = " +
                "Raw_materials.ID " +
                "JOIN Finished_products on Ingredients.NameProduction = Finished_products.ID JOIN Unit on Raw_materials.Unit = Unit.ID where NameProduction = " + id;
            var viewData = _context.Recipe.FromSqlRaw(querry).ToList();
            if(viewData.Count == 0)
            {
                Recipe recipeWhenEmpty = new Recipe();
                string nameQuerry = "Select * from Finished_products where ID = " + id;
                recipeWhenEmpty.ProductId = id;
                recipeWhenEmpty.Id = id.GetValueOrDefault();
                var nameResult = await _context.FinishedProducts.FromSqlRaw(nameQuerry).FirstOrDefaultAsync();
                if (nameResult != null)
                {
                    recipeWhenEmpty.Name = nameResult.Name;
                }
                ViewBag.Name = recipeWhenEmpty.Name;
                ViewBag.ProductId = recipeWhenEmpty.ProductId;
                var list = new List<Recipe> { /*recipeWhenEmpty*/ };
                return View("~/Views/Recipes/Index.cshtml", list);
            }
            return View("~/Views/Recipes/Index.cshtml",viewData);
        }

        private bool FinishedProductExists(int id)
        {
          return _context.FinishedProducts.Any(e => e.Id == id);
        }
    }
}
