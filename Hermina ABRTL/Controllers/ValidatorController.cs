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
    public class ValidatorController : Controller
    {
        // GET: Validator
        public ActionResult Layout()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Account()
        {
            var data = UserDAL.getListDataAcountRS();
            return View(data);
        }
        [HttpGet]
        public ActionResult ExeportLayout(string Round, string IDRS, string Periode) // tampilan setelah milih round di list. belum di setujui. tampilan untuk approv
        {
            var dtSesion = (loginVM)Session["User"];
            string Err = "";
            ExcReportVM model = new ExcReportVM();
            if (DtReportDAL.getDataLogVal(IDRS, Round, Periode, dtSesion.KodeAkses, dtSesion.IDRegister, out model, out Err))            
            {
                model.UserOnLogin = dtSesion.KodeAkses;
                if (dtSesion.KodeAkses == "Validator 1")
                {
                    if (model.Status1 == "")
                    {
                        return PartialView(model);
                    }
                    else
                    {
                        return RedirectToAction("FinalExePort", new { IDRS, Round, model.Periode });
                    }
                }
                else if (dtSesion.KodeAkses == "Validator 2")
                {
                    if (model.Status2 == "")
                    {
                        return PartialView(model);
                    }
                    else
                    {
                        return RedirectToAction("FinalExePort", new { IDRS, Round, model.Periode });
                    }
                }
                else if (dtSesion.KodeAkses == "Validator 3")
                {
                    if (model.Status3 == "")
                    {
                        return PartialView(model);
                    }
                    else
                    {
                        return RedirectToAction("FinalExePort", new { IDRS, Round, model.Periode });
                    }
                }
            }
            else
            {
                TempData["Message"] = Err;
                return RedirectToAction("ExecutiveReport");
            }
            return View(model);
        }
        public ActionResult ExecutiveReport() 
        {
            string Err = "";
            ValidatorVM model = new ValidatorVM();
            if (DtReportDAL.getDataExcRepVal(out model, out Err))
            {
                return PartialView(model);
            }
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult Validation(string Nilai, string IDRS, string Round, string Periode)
        {
            var dtSesion = (loginVM)Session["User"];
            string Err = "";
            if (DtReportDAL.saveApprovVal(dtSesion.KodeAkses, dtSesion.IDRegister, Nilai, IDRS, Round, Periode, out Err))
            {
                return RedirectToAction("FinalExePort", new { IDRS, Round, Periode });
            }
            else
            {
                TempData["Message"] = Err;
                return RedirectToAction("ExecutiveReport");
            }
        }
        public ActionResult FinalExePort(string Round, string IDRS, string Periode)
        {
            var dtSesion = (loginVM)Session["User"];
            string Err = "";
            ExcReportVM model = new ExcReportVM();
            if (DtReportDAL.getDataLogFinal(IDRS, Round, Periode, out model, out Err))
            {
                return PartialView(model);
            }
            return View();
        }
        [AllowAnonymous]
        public ActionResult PrintLayout(string Round, string IDRS, string Periode)
        {
            var dtSesion = (loginVM)Session["User"];
            string Err = "";
            ExcReportVM model = new ExcReportVM();
            if (DtReportDAL.getDataLogFinal(IDRS, Round, Periode, out model, out Err))
            {
                return View(model);
            }
            return View();
        }
        public ActionResult PrintPDF(string IDRS, string Round, string Periode)
        {
            var q = new ActionAsPdf("PrintLayout", new { IDRS, Round, Periode })
            { PageSize = Size.A4, PageOrientation = Orientation.Landscape, PageMargins = { Left = 0, Right = 0 } };
            return q;
        }
        public ActionResult Certificate()
        {
            return View();
        }
        [HttpGet]
        public ActionResult RegisterRS()
        {
            int id = 0;
            ViewBag.ListProvinsi = new SelectList(UserDAL.GetProvinsi(), "IDProvinsi", "Provinsi");
            ViewBag.ListKota = new SelectList(UserDAL.GetKota(id), "IDKota", "Kota");
            return View();
        }
        [HttpPost]
        public ActionResult RegisterRS(string ProvinsiRS, string RegionRS, string CityRS, string AddressRS, string IDRSRS,
            string RSNameRS, string PhoneRS)
        {
            string Err = "";
            if (UserDAL.registerRS(ProvinsiRS, RegionRS, CityRS, AddressRS, IDRSRS, RSNameRS, PhoneRS, out Err))
            {
                TempData["Message"] = "Perdaftaran Rumah Sakit : " + RSNameRS + " Success";
            }
            else
            {
                TempData["Message"] = Err;
            }
            return RedirectToAction("Account");
        }
        public ActionResult ModifyRS(string IDRS)
        {
            var data = UserDAL.GetModifyRS(IDRS);
            return View(data);
        }
        public ActionResult SaveModifyRS(string MProvinsiRS, string MRegionRS, string MCityRS, string MAddressRS, string MIDRSRS,
            string MRSNameRS, string MPhoneRS)
        {
            string Err = "";
            if (UserDAL.updateRegisterRS(MAddressRS, MIDRSRS, MRSNameRS, MPhoneRS, out Err))
            {
                TempData["Message"] = "Update data Rumah Sakit : " + MRSNameRS + " Success";
            }
            else
            {
                TempData["Message"] = Err;
            }
            return RedirectToAction("Account");
        }
        public ActionResult DeleteRS(string IDRS)
        {
            string Err = "";
            if (UserDAL.deleteRegisterRS(IDRS, out Err))
            {
                TempData["Message"] = "Delete Account RS : " + IDRS + " Sukses";
            }
            else
            {
                TempData["Message"] = Err;
            }
            return RedirectToAction("Account");
        }
        [HttpGet]
        public ActionResult RegisterAccount(string IDRS)
        {
            var data = UserDAL.GetIdentitasRS(IDRS);
            RegisterVM model = new RegisterVM();
            model.parameterAC = IDRS;
            model.KotaAC = data.Kota;
            model.provinceAC = data.Provinsi;

            return View(model);
        }
        [HttpPost]
        public ActionResult RegisterAcc(string ProvinsiAC, string KotaAC, string IDRSAC, string NamaAC, string EmailAC,
            string IDRegisterAC, string JabatanAC, string PhoneAC, string PasswordAC)
        {
            string strErr = "";
            JabatanAC = "Checker";
            if (UserDAL.registerAccount(ProvinsiAC, KotaAC, IDRSAC, NamaAC, EmailAC, IDRegisterAC, JabatanAC, PhoneAC, PasswordAC, out strErr))
            {
                TempData["Message"] = "Perdaftaran Account : " + NamaAC + " Sukses";
            }
            else
            {
                TempData["Message"] = strErr;
            }
            return RedirectToAction("Account");
        }
        public ActionResult ModifyAccount(string IDRegister)
        {
            var data = UserDAL.GetModifyAcc(IDRegister);
            return View(data);
        }
        public ActionResult SaveModifyAcc(string ProvinsiMDF, string RegionMDF, string IDRSMDF, string NamaMDF, string EmailMDF,
            string IDRegisterMDF, string JabatanMDF, string PhoneMDF, string PasswordMDF)
        {
            string Err = "";
            if (UserDAL.updateRegisterAccount(EmailMDF, IDRegisterMDF, PhoneMDF, PasswordMDF, out Err))
            {
                TempData["Message"] = "Update Account : " + IDRegisterMDF + " Success";
            }
            else
            {
                TempData["Message"] = Err;
            }
            return RedirectToAction("Account");
        }
        public ActionResult DeleteAccount(string IDReg)
        {
            string Err = "";
            if (UserDAL.deleteRegisterAccount(IDReg, out Err))
            {
                TempData["Message"] = "Delete Account : " + IDReg + " Sukses";
            }
            else
            {
                TempData["Message"] = Err;
            }
            return RedirectToAction("Account");
        }
        public ActionResult RAB() {
            string Err = "";
            ValidatorVM model = new ValidatorVM();
            if (DtReportDAL.getDataExcRepVal(out model, out Err))
            {
                return PartialView(model);
            }
            return PartialView(model);
        }
        public ActionResult SPK() {
            string Err = "";
            ValidatorVM model = new ValidatorVM();
            if (DtReportDAL.getDataExcRepVal(out model, out Err))
            {
                return PartialView(model);
            }
            return PartialView(model);
        }
        public ActionResult LayoutSPK(string IDRS, string Round, string Periode) {
            var dtSesion = (loginVM)Session["USER"];
            string strErr; ExtReportRABVM model = new ExtReportRABVM();
            RSDAL.getExtReportRAB(IDRS, Round, Periode, out strErr, out model);
            model.Round = Round;
            model.Periode = Periode;
            model.UserOnLogin = dtSesion.KodeAkses;
            return View(model);
        }
        [AllowAnonymous]
        public ActionResult LayoutPrintSPK(string IDRS, string NamaRS, string Round, string Periode)
        {
            string strErr;
            ExtReportRABVM model = new ExtReportRABVM();
            RSDAL.getExtReportRAB(IDRS, Round, Periode, out strErr, out model);
            model.NamaRS = NamaRS;
            model.Round = Round;
            model.Periode = Periode;
            return View(model);
        }
        public ActionResult SPKVPdf(string IDRS, string NamaRS,string Round, string Periode)
        {
            var dtSesion = (loginVM)Session["User"];
            var m = new ActionAsPdf("LayoutPrintSPK", new { IDRS, NamaRS, Round, Periode })
            { PageSize = Size.A4, PageOrientation = Orientation.Landscape, PageMargins = { Left = 0, Right = 0 } };
            return m;

        }
        public ActionResult ExeportRABLayout(string IDRS, string Periode, string Round) {
            var dtSesion = (loginVM)Session["USER"];
            string strErr; ExtReportRABVM model = new ExtReportRABVM();
            RSDAL.getExtReportRAB(IDRS, Round, Periode, out strErr, out model);
            model.Round = Round;
            model.Periode = Periode;
            model.UserOnLogin = dtSesion.KodeAkses;
            return View(model);
        }
        public ActionResult SPKImage(string IDRS, string IDUnix, string Round, string Periode) {
            ImageRABBVM data = new ImageRABBVM();
            if (RSDAL.getImageSPK(IDRS, IDUnix, Round, Periode, out data))
            {
                return View(data);
            }
            return View();
        }
        public ActionResult SaveRABValue(string Periode, string IDRS, string Round)
        {
            var dtSesion = (loginVM)Session["USER"];
            string strErr; 
            RSDAL.saveApprovalRAB(Periode, IDRS, Round, dtSesion.KodeAkses, dtSesion.IDRegister, out strErr); 
            return RedirectToAction("ExeportRABLayout", new { IDRS, Periode, Round});
        }
        public ActionResult BRABImage(string ID, string Round, string Periode, string IDRS)
        {
            var dtSesion = (loginVM)Session["USER"];
            ImageRABBVM data = new ImageRABBVM();
            if (RSDAL.getRABBeforeImage(IDRS, ID, Round, Periode, out data))
            {
                return View(data);
            }
            return View();
        }
        [AllowAnonymous]
        public ActionResult FinalExeportRAB(string Periode, string IDRS, string Round) {
            
            string strErr; ExtReportRABVM model = new ExtReportRABVM();
            RSDAL.getExtReportRAB(IDRS, Round, Periode, out strErr, out model);
            model.Round = Round;
            model.Periode = Periode;
            
            return View(model);
        }
        public ActionResult PrintPDFRAB(string IDRS, string Round, string Periode)
        {
            var q = new ActionAsPdf("FinalExeportRAB", new { IDRS, Round, Periode })
            { PageSize = Size.A4, PageOrientation = Orientation.Landscape, PageMargins = { Left = 0, Right = 0 } };
            return q;
        }
    }
}