using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GarageMVC.Data;
using GarageMVC.Models;
using GarageMVC.Models.ViewModels;
using GarageMVC.Filter;

namespace GarageMVC.Controllers
{
    public class ParkedVehiclesController : Controller
    {
        private readonly GarageMVCContext db;

        public ParkedVehiclesController(GarageMVCContext context)
        {
            db = context;
        }

        // GET: ParkedVehicles
        public async Task<IActionResult> Index(string RegNo = null)
        {
            var pvv = UpdateViewContentIQueryable();

            if(RegNo != null)
            {
                pvv = pvv.Where(p => p.RegNumber.Contains(RegNo));
            }
            //                "Index"
            return View(nameof(Index),await pvv.ToListAsync());
        }

        // GET: ParkedVehicles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ParkedVehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VehicleType,RegNumber,Color,Brand,Model,NumberOfWheels,ArrivalTime")] ParkedVehicle parkedVehicle)
        {
            bool RegNumberExist = db.ParkedVehicle.Any(v => v.RegNumber.ToLower().Equals(parkedVehicle.RegNumber.ToLower()));
            if (RegNumberExist)
            {
                ModelState.AddModelError(string.Empty, "Registration Number already exist.");
            }
            else if (ModelState.IsValid)
            {
                parkedVehicle.ArrivalTime = DateTime.Now;
                db.Add(parkedVehicle);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(parkedVehicle);
        }

        // GET: ParkedVehicles/Edit/5
        [ModelNotNull]
        [RequiredID("id")]
        public async Task<IActionResult> Edit(int? id)
        {
            ParkedVehicle p = await db.ParkedVehicle.FindAsync(id);
            ParkedVehicleViewModel pVVM = ParkedVehicleViewModelInstance(p);
            return View(pVVM);
        }

        // POST: ParkedVehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [ModelNotNull]
        [RequiredID("id")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VehicleType,RegNumber,Color,Brand,Model,NumberOfWheels,ArrivalTime")] ParkedVehicleViewModel pVVM)
        {

            bool RegNumberExist = db.ParkedVehicle.Any(v => v.Id != pVVM.Id && v.RegNumber.ToLower().Equals(pVVM.RegNumber.ToLower()));

            var pv = await db.ParkedVehicle.FindAsync(id);
            ParkedVehicleInstance(pVVM, pv);

            if (RegNumberExist)
            {
                ModelState.AddModelError(string.Empty, "Registration Number already exist.");
            }
            else if (ModelState.IsValid)
            {
                try
                {
                    db.Update(pv);
                    db.Entry(pv).Property("ArrivalTime").IsModified = false;
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParkedVehicleExists(pv.Id))
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
            return View(pVVM);
        }

        // GET: ParkedVehicles/Delete/5
        [RequiredID("id")]
        [ModelNotNull]
        public async Task<IActionResult> Delete(int? id)
        {
            //if (id == null) 
            //{ 
            //    return NotFound();
            //}
            var parkedVehicle = await db.ParkedVehicle
                .FirstOrDefaultAsync(m => m.Id == id);

            //if (parkedVehicle == null) 
            //{
            //    return NotFound(); 
            //}

            return View(parkedVehicle);
        }

        // POST: ParkedVehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parkedVehicle = await db.ParkedVehicle.FindAsync(id);
            db.ParkedVehicle.Remove(parkedVehicle);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: ParkedVehicles/Details/5
        [RequiredID("id")]
        [ModelNotNull]
        public async Task<IActionResult> Details(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            var parkedVehicle = await db.ParkedVehicle
                .FirstOrDefaultAsync(m => m.Id == id);
            //if (parkedVehicle == null)
            //{
            //    return NotFound();
            //}

            return View(parkedVehicle);
        }

        private bool ParkedVehicleExists(int id)
        {
            return db.ParkedVehicle.Any(e => e.Id == id);
        }

        //WHY!!!!!
        //private IQueryable<ParkedVehicleViewModel> UpdateViewContentIQueryable()
        //{
        //    IQueryable<ParkedVehicleViewModel> iQpVVM = db.ParkedVehicle.Select(p => ParkedVehicleViewModelInstance(p));
        //    return iQpVVM;
        //}

        private IQueryable<ParkedVehicleViewModel> UpdateViewContentIQueryable()
        {
            IQueryable<ParkedVehicleViewModel> iQpVVM = db.ParkedVehicle.Select(p => new ParkedVehicleViewModel()
            {
                Id = p.Id,
                VehicleType = p.VehicleType,
                RegNumber = p.RegNumber,
                Color = p.Color,
                Brand = p.Brand,
                Model = p.Model,
                NumberOfWheels = p.NumberOfWheels,
                ArrivalTime = p.ArrivalTime,
                ParkedTime = DateTime.Now - p.ArrivalTime
            });
            return iQpVVM;
        }

        private ParkedVehicleViewModel ParkedVehicleViewModelInstance(ParkedVehicle p)
        {
            var pVVM = new ParkedVehicleViewModel
            {
                Id = p.Id,
                VehicleType = p.VehicleType,
                RegNumber = p.RegNumber,
                Color = p.Color,
                Brand = p.Brand,
                Model = p.Model,
                NumberOfWheels = p.NumberOfWheels,
                ArrivalTime = p.ArrivalTime,
                ParkedTime = DateTime.Now - p.ArrivalTime
            };
            return pVVM;
        }

        private void ParkedVehicleInstance(ParkedVehicleViewModel pVVM, ParkedVehicle pv)
        {
            pv.VehicleType = pVVM.VehicleType;
            pv.RegNumber = pVVM.RegNumber;
            pv.Color = pVVM.Color;
            pv.Brand = pVVM.Brand;
            pv.Model = pVVM.Model;
            pv.NumberOfWheels = pVVM.NumberOfWheels;
        }
      

    }
}
