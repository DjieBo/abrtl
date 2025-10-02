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
    public class CheckerController : Controller
    {
        // GET: Checker
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

        public ActionResult AreaForm(string Round, string IDRoundArea)
        {
            //ViewBag.Round = Round;
            var dtSesion = (loginVM)Session["USER"];
            string Err = "", totalData = "", totalCheck = "";
            if (RSDAL.validasiCheck(dtSesion.IDRS, Round, out Err))
            {
                DataRSVM model = new DataRSVM();

                model.listAreaRound = RSDAL.getDistRoundArea(dtSesion.IDRS, Round, out Err);
                model.listKomponen = RSDAL.getAllDataKomponen(dtSesion.IDRS, Round, IDRoundArea, out totalData, out totalCheck, out Err);
                model.TotalData = totalData;
                model.TotalCheck = totalCheck;
                if (Err == "")
                {
                    return PartialView(model);
                }
                else
                {
                    TempData["Message"] = Err;
                    return RedirectToAction("Layout", "Checker");
                }
            }
            else
            {
                TempData["Message"] = Err;
                return RedirectToAction("FormChecker", "Checker");
            }

        }
        public ActionResult RoundArea(string Round, string RoundArea)
        {
            var dtSesion = (loginVM)Session["USER"];
            string Err = "", totalData = "", totalCheck = "";
            var model = new DataRSVM();
            model.listKomponen = RSDAL.getAllDataKomponen(dtSesion.IDRS, Round, RoundArea, out totalData, out totalCheck, out Err);
            model.TotalData = totalData;
            model.TotalCheck = totalCheck;
            model.round = Round;
            if (Err == "")
            {
                return PartialView(model);
            }
            return View();
        }
        public ActionResult Upload(string ID, string Nilai, string ValParameter, string Keterangan, string base64image) {
            var dtSesion = (loginVM)Session["USER"];
            string IDRoundArea = "", Err = "", Round = "";

            if (ID != null & Nilai == "A" & ValParameter != null)
            {
                RSDAL.saveDataPenilaian(dtSesion.IDRS, dtSesion.IDRegister, ID, Nilai, ValParameter, Keterangan,
                                        base64image, out IDRoundArea, out Round, out Err);
                return RedirectToAction("AreaForm", new { Round, IDRoundArea });
            } else if (ID != null & (Nilai == "B" || Nilai == "C") & ValParameter != null & Keterangan != null & base64image != null)
            {
                RSDAL.saveDataPenilaian(dtSesion.IDRS, dtSesion.IDRegister, ID, Nilai, ValParameter, Keterangan,
                                        base64image, out IDRoundArea, out Round, out Err);
                return RedirectToAction("AreaForm", new { Round, IDRoundArea });
            }
            return View();
        }
        public ActionResult Submit(string Round)
        {
            var dtSesion = (loginVM)Session["USER"];
            string Err = "";
            string Periode = "";
            if (RSDAL.saveDataSubmit(dtSesion.IDRS, Round, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), out Periode, out Err))
            {
                if (EmailDAL.SendingEmail(out Err, "Checker", dtSesion.IDRS, "", Round))
                {
                    TempData["Message"] = "Submit data and send email is success";
                    return RedirectToAction("FormChecker", new { Periode });
                }
                else
                {
                    TempData["Message"] = "Submit data is success, send email failed : " + Err;
                    return RedirectToAction("FormChecker", new { Periode });
                }
            }
            else
            {
                TempData["Message"] = Err;
                return RedirectToAction("FormChecker");
            }
        }
        public ActionResult ImageView(string ID)
        {
            var dtSesion = (loginVM)Session["USER"];
            ViewImageVM data = new ViewImageVM();
            if (RSDAL.getImage(dtSesion.IDRS, ID, out data))
            {
                return View(data);
            }
            return View(data);
        }
        public ActionResult BRABImage(string ID, string Round, string Periode) {
            var dtSesion = (loginVM)Session["USER"];
            ImageRABBVM data = new ImageRABBVM();
            if (RSDAL.getRABBeforeImage(dtSesion.IDRS, ID, Round, Periode, out data))
            {
                return View(data);
            }
            return View();
        }
        public ActionResult ExePort(string Round, string Periode)
        {
            var dtSesion = (loginVM)Session["USER"];
            string Err = "";
            ExcReportVM model = new ExcReportVM();
            if (RSDAL.getDataExecReport(dtSesion.IDRS, Round, Periode, out model, out Err))
            {
                model.Periode = Periode;
                return PartialView(model);
            }
            else
            {
                TempData["Message"] = Err;
                return RedirectToAction("FormChecker", "Checker");
            }

        }
        [AllowAnonymous]
        public ActionResult PrintLayout(string IDRS, string Round, string Periode)
        {
            string Err = "";
            ExcReportVM model = new ExcReportVM();
            if (RSDAL.getDataExecReport(IDRS, Round, Periode, out model, out Err))
            {
                return View(model);
            }
            return View(model);
        }
        public ActionResult PrintPDF(string IDRS, string Round, string Periode)
        {
            var q = new ActionAsPdf("PrintLayout", new { IDRS, Round, Periode })
            { PageSize = Size.A4, PageOrientation = Orientation.Landscape, PageMargins = { Left = 0, Right = 0 } };
            return q;
        }
        public ActionResult Camera() {
            return View();
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
        public ActionResult FormChecker(string Periode)
        {
            RABListVM data = new RABListVM();
            List<RABCheck> model = new List<RABCheck>();
            List<CheckRound> model2 = new List<CheckRound>();
            var dtSesion = (loginVM)Session["USER"];
            string Err = "";
            RSDAL.ValidasiRound(dtSesion.IDRS, dtSesion.KodeAkses, Periode, out model, out model2, out Err);
            data.listCheckerVal = model2;
            return View(data);
        }
        public ActionResult UpdateFacilities()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            var dtSesion = (loginVM)Session["USER"];
            var data = UserDAL.GetIdentitasRS(dtSesion.IDRS);
            RegisterCheckerVM model = new RegisterCheckerVM();
            model.parameter = dtSesion.IDRS;
            model.kota = data.Kota;
            model.province = data.Provinsi;
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult Register(string Provinsi, string Kota, string IDRS, string Nama, string Email, string IDRegister, string Jabatan, string Phone, string Password)
        {
            string Err = "";
            if (UserDAL.registerAccount(Provinsi, Kota, IDRS, Nama, Email, IDRegister, Jabatan, Phone, Password, out Err))
            {
                int dta = 0;
                if (UserDAL.getUserVerify(IDRS, out dta))
                {
                    if (dta < 2)
                    {
                        //TempData["Message"] = "Insert Account : " + IDRegister + " Sukses";

                        ViewBag.RegVer = "Ver1"; //Coba Lempar ini ke View tujuan
                        return RedirectToAction("Register");
                    }
                    else
                    {
                        //TempData["Message"] = "Insert Account : " + IDRegister + " Sukses";

                        ViewBag.RegVer = "Ver1"; //Coba Lempar ini ke View tujuan
                        return RedirectToAction("Dashboard");
                    }
                }
            }
            else
            {
                TempData["Message"] = Err;
                return RedirectToAction("Layout");
            }
            return RedirectToAction("Layout");
        }
        public ActionResult RAB(string Periode)
        {
            RABListVM data = new RABListVM();
            List<RABCheck> model = new List<RABCheck>();
            List<CheckRound> model2 = new List<CheckRound>();
            var dtSesion = (loginVM)Session["USER"];
            string Err = "";
            RSDAL.ValidasiRound(dtSesion.IDRS, dtSesion.KodeAkses, Periode, out model, out model2, out Err);
            data.listCheck = model;
            return View(data);
        }
        public ActionResult RABForm(string Round, string Periode) 
        {
            RABVM model = new RABVM();
            string Err = "";
            var dtSesion = (loginVM)Session["USER"];
            RSDAL.getDataRAB(dtSesion.IDRS, Round, Periode, out model, out Err);
            return View(model);
        }
        public ActionResult RABSubmit(List<RABValue> Data)
        {
            var t = Data;
            int l = 0;
            foreach (var da in t) {
                var Budg = t[l].Budget;
                var Payt = t[l].PayOut;
                var Qrl = t[l].Quareel;
            }
            return RedirectToAction("RAB");
        }
        public ActionResult SPKLayout()
        {
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
        public ActionResult SPKSubmit(string Round, string Periode)
        {
            DataSPK data = new DataSPK();
            var dtSesion = (loginVM)Session["USER"];
            string Err = "";
            RSDAL.submitSPK(dtSesion.IDRS, Round, Periode, out Err);
            return RedirectToAction("SPKLayout");
        }
        public ActionResult SPKImageUpload(string ImageAfter, string Round, string Periode, string IDUnix)
        {
            DataSPK data = new DataSPK();
            var dtSesion = (loginVM)Session["USER"];
            string Err = "";
            RSDAL.uploadIMGSPK(dtSesion.IDRS, Periode, Round, IDUnix, ImageAfter, out data, out Err);
            ViewBag.forSPK = new string[] { Periode, Round };
            return Json(new { success = true, ParamSPK = ViewBag.forSPK, JsonRequestBehavior.AllowGet });
        }
        [AllowAnonymous]
        public ActionResult SPKReportLayout(string IDRS, string NamaRS, string Round, string Periode) {
            string strErr;
            ExtReportRABVM model = new ExtReportRABVM();
            RSDAL.getExtReportRAB(IDRS, Round, Periode, out strErr, out model);
            model.NamaRS = NamaRS;
            model.Round = Round;
            model.Periode = Periode;
            return View(model);
        }
        public ActionResult SPKPdf(string Round, string Periode) {
            var dtSesion = (loginVM)Session["User"];
            var m = new ActionAsPdf("SPKReportLayout", new { dtSesion.IDRS, dtSesion.NamaRS, Round, Periode })
            { PageSize = Size.A4, PageOrientation = Orientation.Landscape, PageMargins = { Left = 0, Right = 0 } };
            return m;

        }
        public ActionResult SPKImage(string IDUnix, string Round, string Periode) {
            var dtSesion = (loginVM)Session["USER"];
            ImageRABBVM data = new ImageRABBVM();
            if (RSDAL.getImageSPK(dtSesion.IDRS, IDUnix, Round, Periode, out data))
            {
                return View(data);
            }
            return View();
        }
        public ActionResult SetFacility() {
            var dtSesion = (loginVM)Session["User"];
            string Err = "";
            var model = new FacilityVM();
            model = FacilityDAL.GetDataMaster(out Err);
            return View(model);
        }
        public ActionResult SaveFacility(string[] Area, string[] SubArea, string[] Type, string[] OptionRoom, string[] OptionRL, string[] QTYRoom)
        {
            var dtSesion = (loginVM)Session["User"];
            if (RSDAL.saveFasilitasRS(Area, SubArea, Type, OptionRoom, OptionRL, QTYRoom, dtSesion.IDRS))
            {
                int dta = 0;
                if (UserDAL.getUserVerify(dtSesion.IDRS, out dta))
                {
                    if (dta < 2)
                    {
                        return RedirectToAction("Register");
                    }
                    else
                    {
                        return RedirectToAction("Dashboard");
                    }
                }
            }
            return RedirectToAction("Dashboard");
        }
        [HttpPost]
        public ActionResult PasstoGet(string IDItem)
        {
            string strErr = "";
            string[,] data;
            RSDAL.getItemHarga(IDItem, out data, out strErr);
            return Json(new { success = true, DataArray = data, JsonRequestBehavior.AllowGet });
        }
        [HttpPost]
        public ActionResult PassItem(string ValItem)
        {
            string strErr = "";
            string[,] data;
            var dtSesion = (loginVM)Session["User"];
            RSDAL.getTypeHarga(ValItem, dtSesion.IDRS, out data, out strErr);
            return Json(new { success = true, DataArrayItem = data, JsonRequestBehavior.AllowGet }); 
        }
        [HttpPost]
        public ActionResult SaveRowRAB(string ID, string Round, string Periode, string Kategori, string Item, string Deskripsi, string Budget, string Payout, string Quarel)
        {
            string Err = "";
            BackRABRowDataVM model = new BackRABRowDataVM();
            var dtSesion = (loginVM)Session["User"];
            RSDAL.saveDataRABRow(dtSesion.IDRS, ID, Round, Periode, Kategori, Item, Deskripsi, Budget, Payout, Quarel, out Err, out model);
            return Json(new { success = true, DataSave = model, JsonRequestBehavior.AllowGet });
        }
        public ActionResult SubmitDataRAB(string Round, string Periode)
        {
            var dtSesion = (loginVM)Session["User"];
            string strErr;
            RSDAL.saveDataSubmitRAB(dtSesion.IDRS, Round, Periode, out strErr);
            return RedirectToAction("PrintRAB", new { Round, Periode });
        }
        public ActionResult PrintRAB(string Round, string Periode)
        {
            var dtSesion = (loginVM)Session["User"];
            string strErr;
            ExtReportRABVM model = new ExtReportRABVM();
            RSDAL.getExtReportRAB(dtSesion.IDRS, Round, Periode, out strErr, out model);
            model.NamaRS = dtSesion.NamaRS;
            model.Round = Round;
            model.Periode = Periode;
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult PrintLayoutRAB(string IDRS, string NamaRS, string Round, string Periode)
        {
            string strErr;
            ExtReportRABVM model = new ExtReportRABVM();
            RSDAL.getExtReportRAB(IDRS, Round, Periode, out strErr, out model);
            model.NamaRS = NamaRS;
            model.Round = Round;
            model.Periode = Periode;
            return View(model);
        }
        public ActionResult PrintDataRAB(string Round, string Periode) {
            var dtSesion = (loginVM)Session["User"];
            var m = new ActionAsPdf("PrintLayoutRAB", new { dtSesion.IDRS, dtSesion.NamaRS, Round, Periode })
            { PageSize = Size.A4, PageOrientation = Orientation.Landscape, PageMargins = { Left = 0, Right = 0 } };
            return m;
        }
        
    }
}