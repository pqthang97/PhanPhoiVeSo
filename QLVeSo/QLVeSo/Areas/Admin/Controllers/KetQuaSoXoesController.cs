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
        public ActionResult matchingLottery(string tinh,DateTime? datepiker,string sove)
        {
            ViewBag.lstTinh = db.LoaiVesoes.Select(p => p.Tinh.ToString()).ToList();
            if (tinh != null && datepiker != null)
            {
                var lst = db.KetQuaSoXoes.Where(p => p.LoaiVeso.Tinh.ToUpper().Contains(tinh.ToUpper()) && p.NgaySo == datepiker).ToList();
                   var giai=  xetGiai(sove, lst);
                if (giai != null)
                {
                    TempData["message"] = "trúng " + giai.TenGiai;
                    TempData["sove"] = sove;
                }
                else
                {
                    TempData["message"] = "không trúng";
                    TempData["sove"] = sove;
                }
              
            }


            return View();
        }
        public Giai xetGiai(string sove,List<KetQuaSoXo> lst)
        {
            if(sove.Length < 6)
            {
                return null;
            }
            string tmp = sove;
            //giai 8 - 1
            var giai8 = db.Giais.Where(p => p.MaGiai.Contains("GI08")).FirstOrDefault();
            var lst8 = lst.Where(p => p.MaGiai.Contains(giai8.MaGiai)).Select(o => o.SoTrung).ToList();
            if (lst8.Exists(p => p.Contains(tmp.Clone().ToString().Substring(4)))) return giai8;
            //giai7 - 1
            var giai7 = db.Giais.Where(p => p.MaGiai.Contains("GI07")).FirstOrDefault();
            var lst7 = lst.Where(p => p.MaGiai.Contains(giai7.MaGiai)).Select(o => o.SoTrung).ToList();
            if (lst7.Exists(p => p.Contains(tmp.Clone().ToString().Substring(3)))) return giai7;
            //giai6 - 3
            var giai6 = db.Giais.Where(p => p.MaGiai.Contains("GI06")).FirstOrDefault();
            var lst6 = lst.Where(p => p.MaGiai.Contains(giai6.MaGiai)).Select(o => o.SoTrung).ToList();
            if (lst6.Exists(p => p.Contains(tmp.Clone().ToString().Substring(2)))) return giai6;
            //giai5 - 1
            var giai5 = db.Giais.Where(p => p.MaGiai.Contains("GI05")).FirstOrDefault();
            var lst5 = lst.Where(p => p.MaGiai.Contains(giai5.MaGiai)).Select(o => o.SoTrung).ToList();
            if (lst5.Exists(p => p.Contains(tmp.Clone().ToString().Substring(2)))) return giai5;
            //giai4 - 7
            var giai4 = db.Giais.Where(p => p.MaGiai.Contains("GI04")).FirstOrDefault();
            var lst4 = lst.Where(p => p.MaGiai.Contains(giai4.MaGiai)).Select(o => o.SoTrung).ToList();
            if (lst4.Exists(p => p.Contains(tmp.Clone().ToString().Substring(1)))) return giai4;
            //giai3 - 2
            var giai3 = db.Giais.Where(p => p.MaGiai.Contains("GI03")).FirstOrDefault();
            var lst3 = lst.Where(p => p.MaGiai.Contains(giai3.MaGiai)).Select(o => o.SoTrung).ToList();
            if (lst3.Exists(p => p.Contains(tmp.Clone().ToString().Substring(1)))) return giai3;
            //giai2 - 1
            var giai2 = db.Giais.Where(p => p.MaGiai.Contains("GI02")).FirstOrDefault();
            var lst2 = lst.Where(p => p.MaGiai.Contains(giai2.MaGiai)).Select(o => o.SoTrung).ToList();
            if (lst2.Exists(p => p.Contains(tmp.Clone().ToString().Substring(1)))) return giai2;
            //giai1 - 1
            var giai1 = db.Giais.Where(p => p.MaGiai.Contains("GI01")).FirstOrDefault();
            var lst1 = lst.Where(p => p.MaGiai.Contains(giai1.MaGiai)).Select(o => o.SoTrung).ToList();
            if (lst1.Exists(p => p.Contains(tmp.Clone().ToString().Substring(1)))) return giai1;

            //giai datbiet
            var giaidb = db.Giais.Where(p => p.MaGiai.Contains("GIDB")).FirstOrDefault();
            var lstdb = lst.Where(p => p.MaGiai.Contains(giaidb.MaGiai)).Select(o => o.SoTrung).ToList();
            if (lstdb.Exists(p => p.Contains(tmp.Clone().ToString()))) return giaidb;
            //giaiPhuDb
            var giaipdb = db.Giais.Where(p => p.MaGiai.Contains("GIPDB")).FirstOrDefault();
            var lstpdb = lst.Where(p => p.MaGiai.Contains("GIDB")).Select(o => o.SoTrung).ToList();
            if (lstdb.Exists(p => p.Contains(tmp.Clone().ToString().Substring(1)))) return giaipdb;
            //giaiKK
            var giaikk = db.Giais.Where(p => p.MaGiai.Contains("GIKK")).FirstOrDefault();
            if (ktraKK(sove, lst)) return giaikk;

            return null;
        }
        public bool ktraKK(string veso, List<KetQuaSoXo> lst)
        {
            
            var lstkk = lst.Where(p => p.MaGiai.Contains("GIDB")).Select(o => o.SoTrung).ToList();
            foreach (string item in lstkk)
            {
                if (strcompare(item, veso)) return true;
            }
            return false;
        }
        public bool strcompare(string a,string b)
        {
            int c = 0;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i]) c++;
            }
            if (c > 1) return false;
            return true;
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
