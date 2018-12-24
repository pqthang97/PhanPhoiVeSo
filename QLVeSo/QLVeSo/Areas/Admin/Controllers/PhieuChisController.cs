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
    public class PhieuChisController : Controller
    {
        private QLVSDbContext db = new QLVSDbContext();

        // GET: Admin/PhieuChis
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
            var phieu = db.PhieuChis.ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                phieu = phieu.Where(s => s.MaPhieuChi.ToUpper().Contains(searchString.ToUpper())).ToList();
            }

            phieu.OrderByDescending(v => v.MaPhieuChi);
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(phieu.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/PhieuChis/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuChi phieuChi = db.PhieuChis.Find(id);
            if (phieuChi == null)
            {
                return HttpNotFound();
            }
            return View(phieuChi);
        }
        public string getMaDL()
        {
            var countRow = db.PhieuChis.Count();
            int getCount = countRow + 1;
            string newMaDL = "PCH";
            if (getCount < 10) newMaDL += "000" + getCount.ToString();
            else if (getCount < 100) newMaDL += "00" + getCount.ToString();
            return newMaDL;
        }
        // GET: Admin/PhieuChis/Create
        public ActionResult Create()
        {
            PhieuChi a = new PhieuChi();
            a.MaPhieuChi = getMaDL();
            return View(a);
        }

        // POST: Admin/PhieuChis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaPhieuChi,Ngay,NoiDung,SoTien")] PhieuChi phieuChi)
        {
            if (ModelState.IsValid)
            {

                
                db.PhieuChis.Add(phieuChi);
                TempData["notice"] = "Successfully create";
                TempData["tensanpham"] = phieuChi.MaPhieuChi;
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View(phieuChi);
        }

        // GET: Admin/PhieuChis/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuChi phieuChi = db.PhieuChis.Find(id);
            if (phieuChi == null)
            {
                return HttpNotFound();
            }
            return View(phieuChi);
        }

        // POST: Admin/PhieuChis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaPhieuChi,Ngay,NoiDung,SoTien")] PhieuChi phieuChi)
        {
            if (ModelState.IsValid)
            {
                DbContextTransaction tran = db.Database.BeginTransaction();
                try
                {
                    TempData["notice"] = "Successfully edit";
                    TempData["ma"] = phieuChi.MaPhieuChi;
                    db.Entry(phieuChi).State = EntityState.Modified;
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
            return View(phieuChi);
        }

        // GET: Admin/PhieuChis/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuChi phieuChi = db.PhieuChis.Find(id);
            if (phieuChi == null)
            {
                return HttpNotFound();
            }
            return View(phieuChi);
        }

        // POST: Admin/PhieuChis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            PhieuChi pc = db.PhieuChis.Find(id);
            db.PhieuChis.Remove(pc);
            TempData["notice"] = "Successfully delete";
            TempData["tensanpham"] = pc.MaPhieuChi;
            db.SaveChanges();
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
