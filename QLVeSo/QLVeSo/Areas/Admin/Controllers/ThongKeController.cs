using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLVESO.Areas.Admin.Controllers
{
    public class ThongKeController : Controller
    {
        // GET: Admin/ThongKe
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DoanhThu(string date = null)
        {
            
            return View();
        }

        public ActionResult LoiNhuan()
        {
            return View();
        }

        public ActionResult TinhHinhPhatHanh()
        {
            return View();
        }
        public ActionResult CongNo()
        {
            return View();
        }
    }
}