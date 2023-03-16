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
    public class RecipesController : Controller
    {
        private readonly Lab610Context _context;

        public RecipesController(Lab610Context context)
        {
            _context = context;
        }

        private static int finishProductId;

        // GET: Recipes
        public async Task<IActionResult> Index(int? id)
        {
            int currFinishProdId;
            if(id == 0 || id == null)
            {
                currFinishProdId = finishProductId;
            }
            else
            {
                currFinishProdId = id.GetValueOrDefault();
            }
            string querry = "SELECT Ingredients.ID,Ingredients.NameProduction as [ProductId],Raw_materials.Unit as [UnitId], Unit.Name as [Unit]" +
                " ,Finished_products.Name, Raw_materials.Name as [RawMaterial], Ingredients.Count, Ingredients.RawMaterials as [RawMatId] from " +
                "Ingredients JOIN Raw_Materials on Ingredients.RawMaterials = Raw_materials.ID JOIN Finished_products on Ingredients.NameProduction = " +
                "Finished_products.ID JOIN Unit on Raw_materials.Unit = Unit.ID where NameProduction = " + currFinishProdId;
            var viewData = _context.Recipe.FromSqlRaw(querry).ToList();
            finishProductId = id.GetValueOrDefault(0);
            if (viewData.Count == 0)
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
            return View(viewData);
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id, int? idRawMat)
        {
            if (id == null || _context.Recipe == null)
            {
                return NotFound();
            }
            string querry = "select Ingredients.ID, Raw_materials.Unit as [UnitId], Unit.Name as [Unit], Ingredients.NameProduction as " +
                "[ProductId], Finished_products.Name, Raw_materials.Name as [RawMaterial], " +
                "Ingredients.Count, Ingredients.RawMaterials as [RawMatId] from Ingredients " +
                "JOIN Finished_products on Ingredients.NameProduction = Finished_products.ID JOIN Raw_materials on " +
                "Ingredients.RawMaterials = Raw_materials.ID JOIN Unit on Raw_materials.Unit = Unit.ID where Raw_materials.ID = " + idRawMat + 
                " and RawMaterials = " + idRawMat + " and NameProduction = " + id;
            finishProductId = id.GetValueOrDefault(0);
            var viewData = await _context.Recipe.FromSqlRaw(querry).FirstOrDefaultAsync();
            //var recipe = await _context.Ingredients
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (recipe == null)
            //{
            //    return NotFound();
            //}

            return View(viewData);
        }

        // GET: Recipes/Create
        public IActionResult Create(int? id)
        {
            ViewData["RawMatNames"] = new SelectList(_context.RawMaterials, "Id", "Name");
            ViewBag.IdFinishedProduct = id;
            finishProductId = id.GetValueOrDefault();
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NameProduction,RawMaterials,Count")] Ingredient recipe)
        {

            if (ModelState.IsValid)
            {
                _context.Add(recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new {id = finishProductId});
            }
            return View(recipe);
        }

        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(int? id, int? idRawMat)
        {
            if (id == null || _context.Recipe == null || idRawMat == null)
            {
                return NotFound();
            }

            var recipe = await _context.Ingredients.FirstOrDefaultAsync(x => x.NameProduction == id && x.RawMaterials == idRawMat);
            if (recipe == null)
            {
                return NotFound();
            }
            ViewData["RawMatNames"] = new SelectList(_context.RawMaterials, "Id", "Name");
            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int RawMaterial ,[Bind("Id,NameProduction,RawMaterials,Count")] Ingredient recipe)
        {

            if (id != recipe.Id || RawMaterial != recipe.RawMaterials)
            {
                return NotFound();
            }
            var existingRecipe = await _context.Ingredients
                .FirstOrDefaultAsync(r => r.NameProduction == id && r.RawMaterials == RawMaterial);

            if (existingRecipe == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    existingRecipe.NameProduction = recipe.NameProduction;
                    existingRecipe.RawMaterials = recipe.RawMaterials;
                    existingRecipe.Count = recipe.Count;
                    _context.Update(existingRecipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = id });
            }
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(int? id, int? RawMaterials)
        {
            if (id == null || _context.Ingredients == null || RawMaterials == null)
            {
                return NotFound();
            }

            var recipe = await _context.Ingredients
                .FirstOrDefaultAsync(m => m.NameProduction == id && m.RawMaterials == RawMaterials);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int RawMaterials)
        {
            if (_context.Ingredients == null)
            {
                return Problem("Entity set 'Lab610Context.Ingredients'  is null.");
            }
            var recipe = await _context.Ingredients.FirstOrDefaultAsync(x => x.NameProduction == id && x.RawMaterials == RawMaterials);
            if (recipe != null)
            {
                _context.Ingredients.Remove(recipe);
            }
            
            await _context.SaveChangesAsync();
            finishProductId = id;
            return RedirectToAction(nameof(Index), new { id = id });
        }

        private bool RecipeExists(int id)
        {
          return _context.Ingredients.Any(e => e.Id == id);
        }
    }
}
