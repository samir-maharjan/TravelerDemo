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
    public class TravelItinenaryDetailController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public TravelItinenaryDetailController(ApplicationDbContext context, IWebHostEnvironment HostEnvironment)
        {
            _context = context;
            webHostEnvironment = HostEnvironment;
        }

        // GET: TravelItinenaryDetail
        public IActionResult Index()
        {
            
            IEnumerable<TravelItinenaryDetail> obj = _context.TravelItinenaryDetail
                .Include(x=>x.Empolyee)
                .Where(p => p.Deleted == false);
            return View(obj);
        }

        // GET: TravelItinenaryDetail/Details/5
        public  IActionResult Details(int? id)
        {
            /* if (id == null || _context.TravelItinenaryDetail == null)
             {
                 return NotFound();
             }*/
            //var student = db.Student.FirstOrDefault(m => m.Id == id);
            var obj = _context.TravelItinenaryDetail
                .Include(m => m.Empolyee)
                .FirstOrDefault(m => m.Id == id);
            /*var travelItinenaryDetail = await _context.TravelItinenaryDetail.FirstOrDefaultAsync(m => m.Id == id);*/
         /*   if (obj == null)
            {
                return NotFound();
            }*/

            return View(obj);
        }

        // GET: TravelItinenaryDetail/Create
        public IActionResult Create()
        {
            ViewBag.EmployeeList = new SelectList(_context.TravelSummaries, "Id", "EmpName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult MultipleItineary(TravelItinenaryDetail Data)
        {
            _context.Add(Data);
            _context.SaveChanges();
            return Json(new { status = true, id = Data.Id, Remarks = "Data Saved successfully." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TravelItinenaryDetail travelItinenaryDetail, IFormFile Img)
        {

            if (Img.Length > 0)
            {
                var filename = Guid.NewGuid().ToString();
                var extension = Path.GetExtension(Img.FileName);
                var paths = Path.GetFullPath(webHostEnvironment.WebRootPath);
                var file = Path.Combine(paths, "images", filename + extension);

                using (var stream = System.IO.File.Create(file))
                {
                    Img.CopyTo(stream);
                }

                travelItinenaryDetail.HotelPhoto = "/images/" + filename + extension;
            }
            /*     if (ModelState.IsValid)
             {*/
            _context.Add(travelItinenaryDetail);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
            /*     }
             ViewBag.EmployeeList = new SelectList(_context.TravelSummaries, "Id", "EmpName");*/
            /*return View(travelItinenaryDetail);*/

        }

        // GET: TravelItinenaryDetail/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TravelItinenaryDetail == null)
            {
                return NotFound();
            }

            var travelItinenaryDetail = await _context.TravelItinenaryDetail.FindAsync(id);
            if (travelItinenaryDetail == null)
            {
                return NotFound();
            }
            ViewBag.EmployeeList = new SelectList(_context.TravelSummaries, "Id", "EmpName");
            //ViewData["EmpolyeeId"] = new SelectList(_context.TravelSummaries, "Id", "CreatedName", travelItinenaryDetail.EmpolyeeId);
            return View(travelItinenaryDetail);
        }

        // POST: TravelItinenaryDetail/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TravelItinenaryDetail travelItinenaryDetail, IFormFile Picture)
        {
            var oldData = _context.TravelItinenaryDetail.Find(travelItinenaryDetail.Id);
            var filename = Guid.NewGuid().ToString();
            if (Picture != null)
            {
                var extension = Path.GetExtension(Picture.FileName);
                var paths = Path.GetFullPath(webHostEnvironment.WebRootPath);
                var file = Path.Combine(paths, "images", filename + extension);

                using (var stream = System.IO.File.Create(file))
                {
                    Picture.CopyTo(stream);
                }

                travelItinenaryDetail.HotelPhoto = "/images/" + filename + extension;
            }
            else
            {
                travelItinenaryDetail.HotelPhoto = oldData.HotelPhoto;
            }

            ViewBag.EmployeeList = new SelectList(_context.TravelSummaries, "Id", "EmpName");
            _context.Entry(oldData).CurrentValues.SetValues(travelItinenaryDetail);

            //new add 
            //old remove
            //old update

            _context.Update(oldData);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: TravelItinenaryDetail/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TravelItinenaryDetail == null)
            {
                return NotFound();
            }

            var travelItinenaryDetail = await _context.TravelItinenaryDetail
                .Include(t => t.Empolyee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (travelItinenaryDetail == null)
            {
                return NotFound();
            }

            return View(travelItinenaryDetail);
        }

        // POST: TravelItinenaryDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_context.TravelItinenaryDetail == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TravelItinenaryDetail'  is null.");
            }
            var travelItinenaryDetail = _context.TravelItinenaryDetail.Find(id);
            if (travelItinenaryDetail != null)
            {
                _context.TravelItinenaryDetail.Remove(travelItinenaryDetail);
            }
            travelItinenaryDetail.Deleted = true;
            _context.Update(travelItinenaryDetail);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool TravelItinenaryDetailExists(int id)
        {
            return (_context.TravelItinenaryDetail?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPost]
        public JsonResult AjaxDeleteConfirmed(string checkedItems, int _id)
        {
            string[] arr = checkedItems.Split(',');
            foreach (var id in arr)
            {
                var currentId = Convert.ToInt32(id);
                var travelItinenaryDetail = _context.TravelItinenaryDetail.Find(currentId);
                
                travelItinenaryDetail.Deleted = true;
                _context.Update(travelItinenaryDetail);
                _context.SaveChanges();
            }
            return Json(new { status = true, id = _id, Remarks = "Data Edited successfully." });
            
        }

        public IActionResult ItinenaryDetailsList(int id)
        {
            var DataList = _context.TravelItinenaryDetail.Where(x => x.EmpolyeeId == id && x.Deleted==false).ToList();
              return PartialView(DataList);
        }

    }
}
