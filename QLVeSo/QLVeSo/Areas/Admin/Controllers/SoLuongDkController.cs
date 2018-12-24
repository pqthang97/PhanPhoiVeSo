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
    public class SoLuongDkController : Controller
    {
        // GET: Admin/TbDaily
        private QLVSDbContext db = new QLVSDbContext();
        public string getMaDL()
        {
            var countRow = db.SoLuongDKs.Count();
            int getCount = countRow + 1;
            string newMaDL = "DK";
            if (getCount < 10) newMaDL += "00" + getCount.ToString();
            else if (getCount < 100) newMaDL += "0" + getCount.ToString();
            return newMaDL;
        }
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            var Dailys = (from p in db.SoLuongDKs where p.Flag == true select p);
            Dailys = Dailys.OrderByDescending(s => s.MaDaiLy);

            if (searchString != null)
                page = 1;
            else searchString = currentFilter;
            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                Dailys = Dailys.Where(s => s.MaDaiLy.ToUpper().Contains(searchString.ToUpper()));
                if (Dailys.Count() > 0)
                {
                    TempData["notice"] = "Have result";
                    TempData["dem"] = Dailys.Count();
                }
                else
                {
                    TempData["notice"] = "No result";
                }
            }
            switch (sortOrder)
            {
                case "tendl":
                    Dailys = Dailys.OrderBy(s => s.MaDaiLy);
                    break;
                case "tendl_desc":
                    Dailys = Dailys.OrderByDescending(s => s.MaDaiLy);
                    break;

            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(Dailys.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SoLuongDK sldk = db.SoLuongDKs.Find(id);
            if (sldk == null)
            {
                return HttpNotFound();
            }
            return View(sldk);
        }
        public ActionResult Create()
        {
            SoLuongDK a = new SoLuongDK();
            a.ID = getMaDL();
            ViewBag.MaDaiLy = new SelectList(db.DaiLies, "MaDaiLy", "MaDaiLy");
            return View(a);
        }

        // POST: Admin/TbSanPham/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MaDaiLy,NgayDK,SoLuongDK1,Flag")] SoLuongDK sldk)
        {
            if (ModelState.IsValid)
            {

                sldk.Flag = true;
                db.SoLuongDKs.Add(sldk);
                TempData["notice"] = "Successfully create";
                TempData["tensanpham"] = sldk.ID;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sldk);
        }
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SoLuongDK sldk = db.SoLuongDKs.Find(id);
            if (sldk == null)
            {
                return HttpNotFound();
            }
            return View(sldk);
        }

        // POST: Admin/TbSanPham/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MaDaiLy,NgayDK,SoLuongDK1,Flag")] SoLuongDK sldk)
        {
            if (ModelState.IsValid)
            {
                TempData["notice"] = "Successfully edit";
                TempData["tensanpham"] = sldk.ID;
                db.Entry(sldk).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sldk);
        }
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SoLuongDK sldk = db.SoLuongDKs.Find(id);
            if (sldk == null)
            {
                return HttpNotFound();
            }
            return View(sldk);
        }

        // POST: Admin/TbSanPham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SoLuongDK dl = db.SoLuongDKs.Find(id);
            dl.Flag = false;
            TempData["notice"] = "Successfully delete";
            TempData["tensanpham"] = dl.ID;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}