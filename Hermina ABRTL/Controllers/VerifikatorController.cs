using Hermina_ABRTL.DAL;
using Hermina_ABRTL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rotativa;
using Rotativa.Options;

namespace Hermina_ABRTL.Controllers
{
    [Authorize]
    public class VerifikatorController : Controller
    {
        // GET: Vefirikator
        public ActionResult Layout()
        {
            var dtSesion = (loginVM)Session["USER"];
            var periode = DateTime.Now.ToString("yyyyMM");
            string Err = "";
            DashboardVM model = new DashboardVM();
            DtReportDAL.getDataDashboaard(periode, dtSesion.IDRS, out model, out Err);
            model.NamaRS = dtSesion.NamaRS;
            return View(model);
        }
        public ActionResult Round(string Periode)
        {
            RABListVM data = new RABListVM();
            List<RABCheck> model = new List<RABCheck>();
            List<CheckRound> model2 = new List<CheckRound>();
            var dtSesion = (loginVM)Session["USER"];
            string Err = "";
            RSDAL.ValidasiRound(dtSesion.IDRS, dtSesion.KodeAkses, Periode, out model, out model2, out Err);
            data.listCheckerVal = model2;
            data.Akses = dtSesion.KodeAkses;
            return View(data);
        }
        [HttpGet]
        public ActionResult Penilaian(string Round, string Periode)
        {
            var dtSesion = (loginVM)Session["USER"];
            ExcReportVM model = new ExcReportVM();
            string strErr = "";
            if (RSDAL.validasiVerifyReport(dtSesion.KodeAkses, dtSesion.IDRS, Round, out strErr))
            {
                var data = RSDAL.getDataPenilaianVer(dtSesion.IDRS, Round, Periode, out model, out strErr);
                model.Periode = Periode;
                return PartialView(model);
            }
            else
            {
                TempData["Message"] = strErr;
                return RedirectToAction("Round", new { Periode});
            }
            
        }
        public ActionResult Execute(string Round, string Submit, string Comment, string Periode)
        {
            string Err = "";
            var dtSesion = (loginVM)Session["USER"];
            if (RSDAL.authVerifikator(dtSesion.IDRS, dtSesion.IDRegister, dtSesion.KodeAkses, Round,
                                      Submit, Comment, Periode, out Err))
            {
                if (EmailDAL.SendingEmail(out Err, dtSesion.KodeAkses, dtSesion.IDRS, Submit, Round))
                {
                    TempData["Message"] = "Authorisasi and send email is success";
                    return RedirectToAction("Round", new { Periode });
                }
                else
                {
                    TempData["Message"] = "Authorisasi is success, send email failed : " + Err;
                    return RedirectToAction("Round", new { Periode });
                }
                
            }
            else
            {
                TempData["Message"] = Err;
                return RedirectToAction("Round", new { Periode });
            }
            //return RedirectToAction("Round");
        }
        [HttpGet]
        public ActionResult ExcReport(string Round, string Periode)
        {
            var dtSesion = (loginVM)Session["USER"];
            ExcReportVM model = new ExcReportVM();
            string Err = "";
            //if (RSDAL.getDataLogForVer(dtSesion.IDRS, Round, Periode, out model, out Err))
            if (RSDAL.getDataExecReport(dtSesion.IDRS, Round, Periode, out model, out Err))
            {
                model.Periode = Periode;
                return PartialView(model);
            }
            else
            {
                TempData["Message"] = Err;
                return RedirectToAction("Round", new { Periode });
            }
            //return View();
        }

        [AllowAnonymous]
        public ActionResult PrintLayout(string IDRS, string Round, string Periode)
        {
            string Err = "";
            ExcReportVM model = new ExcReportVM();
            //if (RSDAL.getDataLogForVer(IDRS, Round, Periode, out model, out Err))
            if (RSDAL.getDataExecReport(IDRS, Round, Periode, out model, out Err))
            {
                return View(model);
            }
            return View(model);
        }
        public ActionResult PrintPDF(string IDRS, string Round, string Periode)
        {
            var q = new ActionAsPdf("PrintLayout", new { IDRS, Round, Periode})
            { PageSize = Size.A4, PageOrientation = Orientation.Landscape, PageMargins = { Left = 0, Right = 0 } };
            return q;
        }
        public ActionResult Dashboard()
        {
            var dtSesion = (loginVM)Session["USER"];
            var periode = DateTime.Now.ToString("yyyyMM");
            string Err = "";
            DashboardVM model = new DashboardVM();
            DtReportDAL.getDataDashboaard(periode, dtSesion.IDRS, out model, out Err);
            return View(model);
        }
        public ActionResult RAB(string Periode) {
            RABListVM data = new RABListVM();
            List<RABCheck> model = new List<RABCheck>();
            List<CheckRound> model2 = new List<CheckRound>();
            var dtSesion = (loginVM)Session["USER"];
            string Err = "";
            RSDAL.ValidasiRound(dtSesion.IDRS, dtSesion.KodeAkses,Periode, out model, out model2, out Err);
            data.listCheck = model;
            return View(data);
        }
        public ActionResult PrintRAB() {
            return View();
        }
        public ActionResult VerifikatorRABForm(string Round, string Periode) {
            var dtSesion = (loginVM)Session["USER"];
            string strErr; ExtReportRABVM model = new ExtReportRABVM();
            RSDAL.getExtReportRAB(dtSesion.IDRS, Round, Periode, out strErr, out model);
            //model.NamaRS = dtSesion.NamaRS;
            model.Round = Round;
            model.Periode = Periode;
            return View(model);
        }
        public ActionResult RABExecute(string Round, string Submit, string Comment, string Periode) {
            var dtSesion = (loginVM)Session["USER"];
            string Err = "";
            if (Submit == "Verify")
            {
                RSDAL.saveVerifyRAB(dtSesion.IDRS, dtSesion.KodeAkses, dtSesion.IDRegister, Comment, Round, Periode, Submit, out Err);
                return RedirectToAction("VerifikatorRABForm", new { Round, Periode });
            }
            else
            {
                RSDAL.saveVerifyRAB(dtSesion.IDRS, dtSesion.KodeAkses, dtSesion.IDRegister, Comment, Round, Periode, Submit, out Err);
                return RedirectToAction("RAB", new { Round, Periode });
            }
        }
        public ActionResult SPKLayout() {
            DataSPK data = new DataSPK();
            var dtSesion = (loginVM)Session["USER"];
            string Err = "";
            RSDAL.getListSPK(dtSesion.IDRS, out data, out Err);
            data.NamaRS = dtSesion.NamaRS;
            return View(data);
        }
        public ActionResult SPK(string Round, string Periode)
        {
            DataSPK data = new DataSPK();
            var dtSesion = (loginVM)Session["USER"];
            string Err = "";
            RSDAL.getDataSPK(dtSesion.IDRS, Round, Periode, out data, out Err);
            return View(data);
        }
        public ActionResult SPKImage(string IDUnix, string Round, string Periode)
        {
            var dtSesion = (loginVM)Session["USER"];
            ImageRABBVM data = new ImageRABBVM();
            if (RSDAL.getImageSPK(dtSesion.IDRS, IDUnix, Round, Periode, out data))
            {
                return View(data);
            }
            return View();
        }
        [AllowAnonymous]
        public ActionResult SPKReportVLayout(string IDRS, string NamaRS, string Round, string Periode)
        {
            string strErr;
            ExtReportRABVM model = new ExtReportRABVM();
            RSDAL.getExtReportRAB(IDRS, Round, Periode, out strErr, out model);
            model.NamaRS = NamaRS;
            model.Round = Round;
            model.Periode = Periode;
            return View(model);
        }
        public ActionResult SPKVPdf(string Round, string Periode)
        {
            var dtSesion = (loginVM)Session["User"];
            var m = new ActionAsPdf("SPKReportVLayout", new { dtSesion.IDRS, dtSesion.NamaRS, Round, Periode })
            { PageSize = Size.A4, PageOrientation = Orientation.Landscape, PageMargins = { Left = 0, Right = 0 } };
            return m;

        }
        public ActionResult BRABImage(string ID, string Round, string Periode)
        {
            var dtSesion = (loginVM)Session["USER"];
            ImageRABBVM data = new ImageRABBVM();
            if (RSDAL.getRABBeforeImage(dtSesion.IDRS, ID, Round, Periode, out data))
            {
                return View(data);
            }
            return View();
        }
        [AllowAnonymous]
        public ActionResult PrintLayoutRAB(string IDRS, string NamaRS, string Round, string Periode)
        {
            string strErr; ExtReportRABVM model = new ExtReportRABVM();
            RSDAL.getExtReportRAB(IDRS, Round, Periode, out strErr, out model);
            model.NamaRS = NamaRS;
            model.Round = Round;
            model.Periode = Periode;
            return View(model);
        }
        public ActionResult PrintDataRAB(string Round, string Periode) {
            var dtSesion = (loginVM)Session["User"];
            var q = new ActionAsPdf("PrintLayoutRAB", new { dtSesion.IDRS, dtSesion.NamaRS, Round, Periode })
            { PageSize = Size.A4, PageOrientation = Orientation.Landscape, PageMargins = { Left = 0, Right = 0 } };
            return q;
        }
    }
}