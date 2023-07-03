using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Travel.Data;
using Travel.Models;

namespace Travel.Controllers
{
    public class TravelSummaryController : Controller
    {
        private readonly ApplicationDbContext _Context;
        private readonly IWebHostEnvironment WebHostEnvironment;

        public TravelSummaryController(ApplicationDbContext context, IWebHostEnvironment HostEnvironment)
        {
            _Context = context;
            WebHostEnvironment = HostEnvironment;
        }

        // GET: TravelSummary
        public IActionResult Index()
        {


            IEnumerable<TravelSummary> Obj = _Context.TravelSummaries.Where(p => p.Deleted == false);

            return View(Obj);

        }

        // GET: TravelSummary/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _Context.TravelSummaries == null)
            {
                return NotFound();
            }
            var _id = _Context.TravelSummaries.FirstOrDefault(m => m.Id == id);

            if (_id == null)
            {
                return NotFound();
            }
            //Explicit Load

            _id.TravelItinenaryDetail = _Context.Entry(_id).Collection(b => b.TravelItinenaryDetail).Query().ToList();
            return View(_id);
        }

        // GET: TravelSummary/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TravelSummary/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TravelSummary travelSummary, IFormFile Picture)

        {
            /* if (ModelState.IsValid)
             {*/
            var filename = Guid.NewGuid().ToString();
            if (Picture.Length > 0)
            {
                var extension = Path.GetExtension(Picture.FileName);
                var paths = Path.GetFullPath(WebHostEnvironment.WebRootPath);
                var file = Path.Combine(paths, "images", filename + extension);

                using (var stream = System.IO.File.Create(file))
                {
                    Picture.CopyTo(stream);
                }

                travelSummary.Image = "/images/" + filename + extension;
            }

            travelSummary.CreatedName = "Samir Maharjan";
            travelSummary.UpdatedDate = DateTime.Today;
            travelSummary.UpdatedName = "Samir Maharjan";
            travelSummary.Status = true;



            _Context.Add(travelSummary);
            _Context.SaveChanges();
            return RedirectToAction("Index");
            /* }
             return View(travelSummary);*/
        }

        // GET: TravelSummary/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _Context.TravelSummaries == null)
            {
                return NotFound();
            }
            var travelSummary = await _Context.TravelSummaries.FindAsync(id);
            if (travelSummary == null)
            {
                return NotFound();
            }
            //IEnumerable<TravelSummary> Obj = _Context.TravelSummaries.Where(p => p.Deleted == false);
            travelSummary.TravelItinenaryDetail = _Context.Entry(travelSummary).Collection(b => b.TravelItinenaryDetail).Query().Where(p => p.Deleted == false).ToList();
            return View(travelSummary);
        }

        // POST: TravelSummary/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TravelSummary travelSummary, IFormFile Picture)
        {
            var Img = Picture;
            //get value from db
            var oldData = _Context.TravelSummaries.Find(travelSummary.Id);
            //explicit loading
            oldData.TravelItinenaryDetail = _Context.Entry(oldData).Collection(b => b.TravelItinenaryDetail).Query().ToList();
            
            var newoldData = _Context.TravelSummaries.Find(travelSummary.Id);
            if (oldData == null)
            {
                throw new Exception("Related ID nt found in db.");
            }

            var newData = travelSummary.TravelItinenaryDetail.Where(u => u.Id == 0).ToList();
            foreach (var item in newData)
            {
                item.UpdatedName = DateTime.UtcNow;
                item.UpdatedDate = DateTime.UtcNow;
                item.Deleted = false;
                oldData.TravelItinenaryDetail.Add(item);
            }

            //Purno or db maa vayeko data cha ki chaina check gareko 
            var newoldData1 = travelSummary.TravelItinenaryDetail.Where(u => u.Id != 0).ToList();
            foreach (var item in newoldData1)
            {
                var rec1 = oldData.TravelItinenaryDetail.FirstOrDefault(x => x.Id == item.Id);
                if (rec1 == null)//to check whether the received id is valid or not OR available in db or not
                {
                    throw new Exception("Invalid Record match with db...");
                }
                rec1.UpdatedName = DateTime.UtcNow;
                rec1.UpdatedDate = DateTime.UtcNow;
                rec1.Deleted = false;
                rec1.Hotel = item.Hotel;
                rec1.Date = item.Date;
                rec1.Time = item.Time;
                //rec1.Hotel = item.Hotel;
                //newoldData.TravelItinenaryDetail.Add(item);
            }
            //db ma bhako but bahira bata aako ma na bhako lai chutaunu paryo
            var OldDeletedData = oldData.TravelItinenaryDetail.Where(u => u.Id > 0 && !travelSummary.TravelItinenaryDetail.Any(x => x.Id == u.Id)).ToList();

            //chuti sakeko lai db bata remove garnu
            _Context.TravelItinenaryDetail.RemoveRange(OldDeletedData);



            var filename = Guid.NewGuid().ToString();
            if (Img != null)
            {
                var extension = Path.GetExtension(Img.FileName);
                var paths = Path.GetFullPath(WebHostEnvironment.WebRootPath);
                var file = Path.Combine(paths, "images", filename + extension);

                using (var stream = System.IO.File.Create(file))
                {
                    Img.CopyTo(stream);
                }

                travelSummary.Image = "/images/" + filename + extension;
            }
            else
            {
                travelSummary.Image = oldData.Image;
            }

            travelSummary.UpdatedDate = DateTime.Today;
            travelSummary.UpdatedName = "Up_Samir_Maharjan";



            //remap values from user to db object
            //manual way
            //oldData.EmpName = travelSummary.EmpName;
            //oldData.EmpName = travelSummary.EmpName;
            //oldData.EmpName = travelSummary.EmpName;
            //oldData.EmpName = travelSummary.EmpName;

            //2. alt way using auto mapper

            //3. alt way copy using set values
            _Context.Entry(oldData).CurrentValues.SetValues(travelSummary);




            _Context.Update(oldData);

            _Context.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: TravelSummary/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _Context.TravelSummaries == null)
            {
                return NotFound();
            }

            var travelSummary = await _Context.TravelSummaries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (travelSummary == null)
            {
                return NotFound();
            }

            return View(travelSummary);
        }

        // POST: TravelSummary/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_Context.TravelSummaries == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TravelSummaries'  is null.");
            }
            var travelSummary = await _Context.TravelSummaries.FindAsync(id);
            if (travelSummary != null)
            {
                travelSummary.Deleted = true;
                _Context.TravelSummaries.Update(travelSummary);
            }

            await _Context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TravelSummaryExists(int id)
        {
            return (_Context.TravelSummaries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public ActionResult SampleCreate()
        {
            return View();
        }

    }
}
