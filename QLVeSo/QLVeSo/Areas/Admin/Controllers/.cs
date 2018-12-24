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

namespace STSShop.Areas.Admin.Controllers
{
    public class KetQuaSoXoesController : Controller
    {
        private QLVSDbContext db = new QLVSDbContext();

        // GET: Admin/KetQuaSoXoes
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
            var ketQuaSoXoes = (from p in db.KetQuaSoXoes where p.Flag == true select p).ToList();

            //var ketQuaSoXoes = db.KetQuaSoXoes.Include(k => k.Giai).Include(k => k.LoaiVeso).ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                ketQuaSoXoes = ketQuaSoXoes.Where(s => s.SoTrung.ToUpper().Contains(searchString) || s.Giai.TenGiai.ToUpper().Contains(searchString.ToUpper()) || s.LoaiVeso.Tinh.ToUpper().Contains(searchString.ToUpper())).ToList();

                if (ketQuaSoXoes.Count() > 0)
                {
                    TempData["notice"] = "Have result";
                    TempData["dem"] = ketQuaSoXoes.Count();
                }
                else
                {
                    TempData["notice"] = "No result";
                }
            }
            ketQuaSoXoes.OrderByDescending(v => v.MaGiai);
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(ketQuaSoXoes.ToPagedList(pageNumber, pageSize));
            //return View(ketQuaSoXoes.ToList());
        }
        public string getNextId(string ID = null)
        {
            var list = db.KetQuaSoXoes.Select(p => p.ID).ToList();
            int i = 1;
            if (list.Count == 0) return ID.ToString() + i.ToString();
            while (list.Exists(p => p == i))
            {
                i++;
            }
            return ID.ToString() + i.ToString();
        }
        public int getMaDL()
        {
            var countRow = db.KetQuaSoXoes.Count();
            int getCount = countRow + 1;
            
            return getCount;
        }
        // GET: Admin/KetQuaSoXoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KetQuaSoXo ketQuaSoXo = db.KetQuaSoXoes.Find(id);
            if (ketQuaSoXo == null)
            {
                return HttpNotFound();
            }
            return View(ketQuaSoXo);
        }

        // GET: Admin/KetQuaSoXoes/Create
        public ActionResult Create()
        {
            ViewBag.MaGiai = new SelectList(db.Giais, "MaGiai", "TenGiai");
            ViewBag.MaLoaiVeSo = new SelectList(db.LoaiVesoes, "MaLoaiVeSo", "Tinh");
            KetQuaSoXo a = new KetQuaSoXo();
            a.ID = getMaDL();
            return View(a);
        }

        // POST: Admin/KetQuaSoXoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MaLoaiVeSo,MaGiai,NgaySo,SoTrung,Flag")] KetQuaSoXo ketQuaSoXo)
        {
            if (ModelState.IsValid)
            {
                var tran = db.Database.BeginTransaction();
                try
                {
                    ketQuaSoXo.Flag = true;
                    TempData["notice"] = "Successfully create";
                    TempData["makqsx"] = ketQuaSoXo.ID.ToString();
                    db.KetQuaSoXoes.Add(ketQuaSoXo);
                    db.SaveChanges();
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                }
                tran.Dispose();
                return RedirectToAction("Index");
            }

            ViewBag.MaGiai = new SelectList(db.Giais, "MaGiai", "TenGiai", ketQuaSoXo.MaGiai);
            ViewBag.MaLoaiVeSo = new SelectList(db.LoaiVesoes, "MaLoaiVeSo", "Tinh", ketQuaSoXo.MaLoaiVeSo);
            return View(ketQuaSoXo);
        }

        // GET: Admin/KetQuaSoXoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KetQuaSoXo ketQuaSoXo = db.KetQuaSoXoes.Find(id);
            if (ketQuaSoXo == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaGiai = new SelectList(db.Giais, "MaGiai", "TenGiai", ketQuaSoXo.MaGiai);
            ViewBag.MaLoaiVeSo = new SelectList(db.LoaiVesoes, "MaLoaiVeSo", "Tinh", ketQuaSoXo.MaLoaiVeSo);
            return View(ketQuaSoXo);
        }

        // POST: Admin/KetQuaSoXoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MaLoaiVeSo,MaGiai,NgaySo,SoTrung")] KetQuaSoXo ketQuaSoXo)
        {
            if (ModelState.IsValid)
            {
                TempData["notice"] = "Successfully edit";
                TempData["tensanpham"] = ketQuaSoXo.ID;
                ketQuaSoXo.Flag = true;
                db.Entry(ketQuaSoXo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ketQuaSoXo);
        }

        // GET: Admin/KetQuaSoXoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KetQuaSoXo ketQuaSoXo = db.KetQuaSoXoes.Find(id);
            if (ketQuaSoXo == null)
            {
                return HttpNotFound();
            }
            return View(ketQuaSoXo);
        }

        // POST: Admin/KetQuaSoXoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var tran = db.Database.BeginTransaction();
            try
            {
                var kqsx = db.KetQuaSoXoes.Find(id);
                TempData["notice"] = "Successfully delete";
                TempData["makqsx"] = kqsx.ID.ToString();
                kqsx.Flag = false;
                db.SaveChanges();
                tran.Commit();
            }
            catch
            {
                tran.Rollback();
            }
            tran.Dispose();
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
