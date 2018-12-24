using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace QLVESO.Areas.Admin.Controllers
{
    public class PhatHanhController : Controller
    {
        // GET: Admin/TbDaily
        private QLVSDbContext db = new QLVSDbContext();
        public string getMaDL()
        {
            var countRow = db.PhatHanhs.Count();
            int getCount = countRow + 1;
            string newMaDL = "DL";
            if (getCount < 10) newMaDL += "0" + getCount.ToString();
            else if (getCount < 100) newMaDL += "0" + getCount.ToString();
            return newMaDL;
        }
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            var PhatHanhs = (from p in db.PhatHanhs where p.Flag == true select p);
            PhatHanhs = PhatHanhs.OrderByDescending(s => s.MaDaiLy);

            if (searchString != null)
                page = 1;
            else searchString = currentFilter;
            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                PhatHanhs = PhatHanhs.Where(s => s.MaDaiLy.ToUpper().Contains(searchString.ToUpper()));
                if (PhatHanhs.Count() > 0)
                {
                    TempData["notice"] = "Have result";
                    TempData["dem"] = PhatHanhs.Count();
                }
                else
                {
                    TempData["notice"] = "No result";
                }
            }
            switch (sortOrder)
            {
                case "tendl":
                    PhatHanhs = PhatHanhs.OrderBy(s => s.MaPH);
                    break;
                case "tendl_desc":
                    PhatHanhs = PhatHanhs.OrderByDescending(s => s.MaPH);
                    break;
                default:
                    PhatHanhs = PhatHanhs.OrderByDescending(s => s.MaPH);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(PhatHanhs.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PhatHanh ph = db.PhatHanhs.FirstOrDefault(m => m.MaPH == id);
            if (ph == null)
            {
                return HttpNotFound();
            }
            return View(ph);
        }

        public ActionResult Create()
        {
            ViewBag.MaLoaiVeSo = new SelectList(db.LoaiVesoes, "MaLoaiVeSo", "MaLoaiVeSo");
            ViewBag.MaDaiLy = new SelectList(db.DaiLies, "MaDaiLy", "MaDaiLy");
            
            return View();
        }

        // POST: Admin/TbSanPham/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDaiLy,MaLoaiVeSo,SoLuong,NgayNhan,SLBan,DoanhThuDPH,HoaHong,TienThanhToan,Flag")] PhatHanh ph)
        {
            if (ModelState.IsValid)
            {

                ph.Flag = true;
                db.PhatHanhs.Add(ph);
                TempData["notice"] = "Successfully create";
                TempData["tensanpham"] = ph.MaLoaiVeSo;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ph);
        }
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhatHanh ph = db.PhatHanhs.FirstOrDefault(m => m.MaPH == id);
            if (ph == null)
            {
                return HttpNotFound();
            }
            return View(ph);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PhatHanh ph)
        {
            if (ModelState.IsValid)
            {
                ph.HoaHong = Convert.ToInt32(ph.HoaHong);
                TempData["notice"] = "Successfully edit";
                TempData["tensanpham"] = ph.MaLoaiVeSo;
                db.Entry(ph).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ph);
        }


        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhatHanh ph = db.PhatHanhs.FirstOrDefault(m => m.MaPH == id);
            if (ph == null)
            {
                return HttpNotFound();
            }
            return View(ph);
        }

        // POST: Admin/TbSanPham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhatHanh ph = db.PhatHanhs.FirstOrDefault(m => m.MaPH == id);
            ph.Flag = false;
            TempData["notice"] = "Successfully delete";
            TempData["tensanpham"] = ph.MaLoaiVeSo;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}