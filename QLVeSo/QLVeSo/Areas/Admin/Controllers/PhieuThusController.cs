using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models.EF;
using PagedList;

namespace QLVESO.Areas.Admin.Controllers
{
    public class PhieuThusController : Controller
    {
        private QLVSDbContext db = new QLVSDbContext();
        public string getMaDL()
        {
            var countRow = db.PhieuThus.Count();
            int getCount = countRow + 1;
            string newMaDL = "PTH";
            if (getCount < 10) newMaDL += "000" + getCount.ToString();
            else if (getCount < 100) newMaDL += "00" + getCount.ToString();
            return newMaDL;
        }
        // GET: Admin/PhieuThus
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var phieu = (from p in db.PhieuThus where p.Flag == true select p).ToList();
            
            
            if (!String.IsNullOrEmpty(searchString))
            {
                phieu = phieu.Where(s => s.MaPhieuThu.ToUpper().Contains(searchString.ToUpper())).ToList();
            }

            phieu.OrderByDescending(v => v.MaPhieuThu);
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(phieu.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/PhieuThus/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuThu phieuThu = db.PhieuThus.Find(id);
            if (phieuThu == null)
            {
                return HttpNotFound();
            }
            return View(phieuThu);
        }

        // GET: Admin/PhieuThus/Create
        public ActionResult Create()
        {
            ViewBag.MaDaiLy = new SelectList(db.DaiLies, "MaDaiLy", "TenDaiLy");
            PhieuThu a = new PhieuThu();
            a.MaPhieuThu = getMaDL();
            a.Flag = true;
            return View(a);
        }

        // POST: Admin/PhieuThus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaPhieuThu,MaDaiLy,NgayNop,SoTienNop,Flag")] PhieuThu phieuThu)
        {
            if (ModelState.IsValid)
            {

                phieuThu.Flag = true;
                db.PhieuThus.Add(phieuThu);
                TempData["notice"] = "Successfully create";
                TempData["tensanpham"] = phieuThu.MaPhieuThu;
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View(phieuThu);
        }

        // GET: Admin/PhieuThus/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuThu phieuThu = db.PhieuThus.Find(id);
            if (phieuThu == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDaiLy = new SelectList(db.DaiLies, "MaDaiLy", "TenDaiLy", phieuThu.MaDaiLy);
            return View(phieuThu);
        }

        // POST: Admin/PhieuThus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaPhieuThu,MaDaiLy,NgayNop,SoTienNop")] PhieuThu phieuThu)
        {
            if (ModelState.IsValid)
            {
                DbContextTransaction tran = db.Database.BeginTransaction();
                try
                {
                    TempData["notice"] = "Successfully edit";
                    TempData["ma"] = phieuThu.MaPhieuThu;
                    phieuThu.Flag = true;
                    db.Entry(phieuThu).State = EntityState.Modified;
                    db.SaveChanges();
                    tran.Commit();
                    tran.Dispose();
                }
                catch
                {
                    tran.Rollback();
                    tran.Dispose();

                }
                return RedirectToAction("Index");
            }
            ViewBag.MaDaiLy = new SelectList(db.DaiLies, "MaDaiLy", "TenDaiLy", phieuThu.MaDaiLy);
            return View(phieuThu);
        }

        // GET: Admin/PhieuThus/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuThu phieuThu = db.PhieuThus.Find(id);
            if (phieuThu == null)
            {
                return HttpNotFound();
            }
            return View(phieuThu);
        }

        // POST: Admin/PhieuThus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DbContextTransaction tran = db.Database.BeginTransaction();
            try
            {
                var giai = db.PhieuThus.Find(id);
                TempData["notice"] = "Successfully delete";
                TempData["ma"] = giai.MaPhieuThu;
                giai.Flag = false;
                db.SaveChanges();
                tran.Commit();
                tran.Dispose();
            }
            catch
            {
                tran.Rollback();
                tran.Dispose();
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
