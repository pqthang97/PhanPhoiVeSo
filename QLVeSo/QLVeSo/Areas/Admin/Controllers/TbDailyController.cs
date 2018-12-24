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
    public class TbDailyController : Controller
    {
        // GET: Admin/TbDaily
        private QLVSDbContext db = new QLVSDbContext();
        public string getMaDL()
        {
            var countRow = db.DaiLies.Count();
            int getCount = countRow + 1;
            string newMaDL = "DL";
            if (getCount < 10) newMaDL += "0" + getCount.ToString();
            else if (getCount < 100) newMaDL += "0" + getCount.ToString();
            return newMaDL;
        }
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            


            var Dailys = (from p in db.DaiLies where p.Flag==true select p);
            Dailys = Dailys.OrderByDescending(s => s.MaDaiLy);

            if (searchString != null)
                page = 1;
            else searchString = currentFilter;
            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                Dailys = Dailys.Where(s => s.TenDaiLy.ToUpper().Contains(searchString.ToUpper()));
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
                    Dailys = Dailys.OrderBy(s => s.TenDaiLy);
                    break;
                case "tendl_desc":
                    Dailys = Dailys.OrderByDescending(s => s.TenDaiLy);
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
            DaiLy dly = db.DaiLies.Find(id);
            if (dly == null)
            {
                return HttpNotFound();
            }
            return View(dly);
        }
        public ActionResult Create()
        {
            DaiLy dl = new DaiLy();
            dl.MaDaiLy = getMaDL();
            
            return View(dl);
        }

        // POST: Admin/TbSanPham/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDaiLy,TenDaiLy,DiaChi,SDT,Flag")] DaiLy dl)
        {
            

            if (ModelState.IsValid)
            {
                
                dl.Flag =true;
                db.DaiLies.Add(dl);
                TempData["notice"] = "Successfully create";
                TempData["tensanpham"] = dl.TenDaiLy;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            
            return View(dl);
        }
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DaiLy dl = db.DaiLies.Find(id);
            if (dl == null)
            {
                return HttpNotFound();
            }
            
            return View(dl);
        }

        // POST: Admin/TbSanPham/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDaiLy,TenDaiLy,DiaChi,SDT,Flag")] DaiLy dl)
        {
            

            if (ModelState.IsValid)
            {
                TempData["notice"] = "Successfully edit";
                TempData["tensanpham"] = dl.TenDaiLy;
                db.Entry(dl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(dl);
        }
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DaiLy dl = db.DaiLies.Find(id);
            if (dl == null)
            {
                return HttpNotFound();
            }
            return View(dl);
        }

        // POST: Admin/TbSanPham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DaiLy dl = db.DaiLies.Find(id);
            dl.Flag = false;
            TempData["notice"] = "Successfully delete";
            TempData["tensanpham"] = dl.TenDaiLy;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}