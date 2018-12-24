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
    public class LoaiVesoesController : Controller
    {
        private QLVSDbContext db = new QLVSDbContext();
        
        // GET: Admin/LoaiVesoes
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;



            var loaiveso = (from p in db.LoaiVesoes where p.Flag == true select p).ToList();
            

            if (searchString != null)
                page = 1;
            else searchString = currentFilter;
            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
               
                if (loaiveso.Count() > 0)
                {
                    TempData["notice"] = "Have result";
                    TempData["dem"] = loaiveso.Count();
                }
                else
                {
                    TempData["notice"] = "No result";
                }
            }
            

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(loaiveso.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/LoaiVesoes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiVeso loaiVeso = db.LoaiVesoes.Find(id);
            if (loaiVeso == null)
            {
                return HttpNotFound();
            }
            return View(loaiVeso);
        }

        // GET: Admin/LoaiVesoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/LoaiVesoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaLoaiVeSo,Tinh,Flag")] LoaiVeso loaiVeso)
        {
            if (ModelState.IsValid)
            {

                loaiVeso.Flag = true;
                db.LoaiVesoes.Add(loaiVeso);
                TempData["notice"] = "Successfully create";
                TempData["tensanpham"] = loaiVeso.MaLoaiVeSo;
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View(loaiVeso);
        }

        // GET: Admin/LoaiVesoes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiVeso loaiVeso = db.LoaiVesoes.Find(id);
            if (loaiVeso == null)
            {
                return HttpNotFound();
            }
            return View(loaiVeso);
        }

        // POST: Admin/LoaiVesoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaLoaiVeSo,Tinh,Flag")] LoaiVeso loaiVeso)
        {
            if (ModelState.IsValid)
            {
                TempData["notice"] = "Successfully edit";
                TempData["tensanpham"] = loaiVeso.MaLoaiVeSo;
                loaiVeso.Flag = true;
                db.Entry(loaiVeso).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(loaiVeso);
        }

        // GET: Admin/LoaiVesoes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiVeso loaiVeso = db.LoaiVesoes.Find(id);
            if (loaiVeso == null)
            {
                return HttpNotFound();
            }
            return View(loaiVeso);
        }

        // POST: Admin/LoaiVesoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            LoaiVeso dl = db.LoaiVesoes.Find(id);
            dl.Flag = false;
            TempData["notice"] = "Successfully delete";
            TempData["tensanpham"] = dl.MaLoaiVeSo;
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
