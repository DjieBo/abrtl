using Hermina_ABRTL.DAL;
using Hermina_ABRTL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hermina_ABRTL.Controllers
{
    public class PeerGroupController : Controller
    {        
        public ActionResult AutoSubmit(string IDRS, string Round)
        {
            string Err = "";
            string Periode = "";
            RSDAL.saveDataSubmit(IDRS, Round, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), out Periode, out Err);
            return RedirectToAction("Report");
        }
        [HttpGet]
        public ActionResult Layout() {
            PeerGroupVM model = new PeerGroupVM();
            string Err = "";
            var data = DtReportDAL.getDataAuto(out model, out Err);
            return View(model);
        }
        [HttpGet]
        public ActionResult RealTime()
        {
            PeerGroupVM model = new PeerGroupVM();
            string Err = "";
            var data = DtReportDAL.getDataAuto(out model, out Err);
            return View(model);
        }
        [HttpPost]
        public ActionResult AutoVerify(string IDRS, string Round, string Periode, string Akses) //ctt : Akses = Verifikator 1 atau Verifikator 2
        {
            string Err = "";
            DtReportDAL.authVerifikator(IDRS, "Sistem", Akses, Round, "Verify", "Auto Verify", Periode, out Err);
            return View();
        }
    }
}