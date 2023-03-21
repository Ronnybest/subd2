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
    public class PaymentsController : Controller
    {

        private readonly Lab610Context _context;

        public PaymentsController(Lab610Context context)
        {
            _context = context;
        }
        public static int selectedMonth;
        public static int selectedYear;
        // GET: Payments
        public async Task<IActionResult> Index()
        {

            ViewData["Month"] = new SelectList(_context.Month, "Id", "Months");
            ViewData["Year"] = new List<int> { 2023, 2024, 2025, 2026, 2027, 2028 };
            var lab610Context = _context.Payments.Include(b => b.EmployeeNavigation).Include(b => b.MonthNavigation);
            return View(await lab610Context.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Index(int month, int year)
        {
            selectedMonth = month;
            selectedYear = year;
            ViewData["Month"] = new SelectList(_context.Month, "Id", "Months");
            ViewData["Year"] = new List<int> { 2023, 2024, 2025, 2026, 2027, 2028 };
            var result = await _context.Payments.Where(p => p.Year == year && p.Month == month).Include(b => b.EmployeeNavigation).Include(b => b.MonthNavigation).ToListAsync();
            return View(result);
        }
        public IActionResult GivePay()
        {
            //string query = "SELECT COUNT(*) FROM Stuff";
            //FormattableString formattableString = FormattableStringFactory.Create(query);
            //var result = _context.Stuffs.FromSql(formattableString).FirstOrDefault();
            var Stuff = _context.Stuffs.ToList();
            List<GivePay> givePays = new List<GivePay>();
            var budget = _context.Budgets.FirstOrDefault(b => b.Id == 1);
            for (int i = 0; i < Stuff.Count(); i++)
            {
                var reserv = new GivePay();
                reserv.EmployeeID = Stuff[i].Id;
                reserv.EmployeeName = Stuff[i].FullName;
                reserv.BuyRawMatCount = Stuff[i].BuyRawMatCount;
                reserv.BuyRawMatCount = Stuff[i].BuyRawMatCount;
                reserv.CreateProductCount = Stuff[i].CreateProductCount;
                reserv.SellProductCount = Stuff[i].SellProductCount;
                reserv.Month = selectedMonth;
                var month = _context.Month.FirstOrDefault(x => x.Id == selectedMonth);
                if (month != null)
                {
                    reserv.MonthNavigationGP = new Month { Id = selectedMonth, Months = month.Months };
                }
                reserv.Year = selectedYear;
                reserv.IsPaid = false;
                reserv.Id = i+1;
                reserv.Salary = Stuff[i].Salary;
                reserv.TotalWorkCount = reserv.SellProductCount + reserv.CreateProductCount + reserv.BuyRawMatCount;
                reserv.AdditionalPercent = budget?.PercentForPayment * reserv.TotalWorkCount;
                reserv.Result = Convert.ToDouble(Math.Round((decimal)(reserv.Salary + (reserv.Salary * (reserv.AdditionalPercent/100)))));
                givePays.Add(reserv);
            }
            //lab610Context.Year = selectedYear;
            //lab610Context.Month = selectedMonth;
            double CountFull = CountFullResult(givePays);
            ViewBag.CountFull = CountFull;
            return View("PayTime", givePays);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult GivePayConfirm(List<double> salayResult, string results)
        {
            List<double> parsedResults = Newtonsoft.Json.JsonConvert.DeserializeObject<List<double>>(results);
            var lab610Context = _context.Payments.Include(b => b.EmployeeNavigation).Include(b => b.MonthNavigation);
            var Stuff = _context.Stuffs.ToList();
            var budget = _context.Budgets.FirstOrDefault(b => b.Id == 1);
            for (int i = 0; i < Stuff.Count(); i++)
            {
                var InsertedModel = new Payments
                {
                    EmployeeID = Stuff[i].Id,
                    Salary = Stuff[i].Salary,
                    BuyRawMatCount = Stuff[i].BuyRawMatCount,
                    CreateProductCount = Stuff[i].CreateProductCount,
                    SellProductCount = Stuff[i].SellProductCount,
                    AdditionalPercent = budget!.PercentForPayment * (Stuff[i].BuyRawMatCount + Stuff[i].CreateProductCount + Stuff[i].SellProductCount),
                    Year = selectedYear,
                    Month = selectedMonth,
                    IsPaid = true,
                    Result = parsedResults![i],
                };
                _context.Payments.Add(InsertedModel);
                
            }
            _context.SaveChanges();
            return View(nameof(Index));
        }
        public double CountFullResult (List<GivePay> model)
        {
            double? result = 0;
            for(int i = 0; i < model.Count(); i++)
            {
                result += model[i].Result;
            }
            return result.Value;
        }


        // GET: Payments/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.Payments == null)
        //    {
        //        return NotFound();
        //    }

        //    var payments = await _context.Payments
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (payments == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(payments);
        //}

        //// GET: Payments/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Payments/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,EmployeeID,Salary,BuyRawMatCount,SellProductCount,CreateProductCount,AdditionalPercent,Year,Month,IsPaid,Result")] Payments payments)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(payments);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(payments);
        //}

        //// GET: Payments/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Payments == null)
        //    {
        //        return NotFound();
        //    }

        //    var payments = await _context.Payments.FindAsync(id);
        //    if (payments == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(payments);
        //}

        //// POST: Payments/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeID,Salary,BuyRawMatCount,SellProductCount,CreateProductCount,AdditionalPercent,Year,Month,IsPaid,Result")] Payments payments)
        //{
        //    if (id != payments.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(payments);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!PaymentsExists(payments.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(payments);
        //}

        //// GET: Payments/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Payments == null)
        //    {
        //        return NotFound();
        //    }

        //    var payments = await _context.Payments
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (payments == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(payments);
        //}

        //// POST: Payments/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Payments == null)
        //    {
        //        return Problem("Entity set 'Lab610Context.Payments'  is null.");
        //    }
        //    var payments = await _context.Payments.FindAsync(id);
        //    if (payments != null)
        //    {
        //        _context.Payments.Remove(payments);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool PaymentsExists(int id)
        {
          return (_context.Payments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
