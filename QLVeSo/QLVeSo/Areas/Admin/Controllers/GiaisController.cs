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
    public class GiaisController : Controller
    {
        private QLVSDbContext db = new QLVSDbContext();
        //cus



        // GET: Admin/Giais
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

            var giai = (from p in db.Giais where p.Flag == true select p).ToList();
           
            if (!String.IsNullOrEmpty(searchString))
            {
                giai = giai.Where(s => s.TenGiai.ToUpper().Contains(searchString.ToUpper())).ToList();
            }

            giai.OrderByDescending(v => v.MaGiai);
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(giai.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Giais/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Giai giai = db.Giais.Find(id);
            if (giai == null)
            {
                return HttpNotFound();
            }
            return View(giai);
        }

        // GET: Admin/Giais/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Giais/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaGiai,TenGiai,SoTienNhan,Flag")] Giai giai)
        {
            if (ModelState.IsValid)
            {
                var tran = db.Database.BeginTransaction();
                try
                {
                    TempData["notice"] = "Successfully create";
                    TempData["tengiai"] = giai.TenGiai;
                    db.Giais.Add(giai);
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

            return View(giai);
        }

        // GET: Admin/Giais/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Giai giai = db.Giais.Find(id);
            if (giai == null)
            {
                return HttpNotFound();
            }
            return View(giai);
        }

        // POST: Admin/Giais/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaGiai,TenGiai,SoTienNhan,Flag")] Giai giai)
        {
            if (ModelState.IsValid)
            {
                DbContextTransaction tran = db.Database.BeginTransaction();
                try
                {
                    giai.Flag = true;
                    TempData["notice"] = "Successfully edit";
                    TempData["tengiai"] = giai.TenGiai;
                    db.Entry(giai).State = EntityState.Modified;
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
            return View(giai);
        }

        // GET: Admin/Giais/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Giai giai = db.Giais.Find(id);
            if (giai == null)
            {
                return HttpNotFound();
            }
            return View(giai);
        }

        // POST: Admin/Giais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {

            DbContextTransaction tran = db.Database.BeginTransaction();
            try
            {
                var giai = db.Giais.Find(id);
                TempData["notice"] = "Successfully delete";
                TempData["tengiai"] = giai.TenGiai;
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
