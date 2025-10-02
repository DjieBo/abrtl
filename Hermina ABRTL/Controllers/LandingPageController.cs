using Hermina_ABRTL.DAL;
using Hermina_ABRTL.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Hermina_ABRTL.Controllers
{
    [Authorize]
    public class LandingPageController : Controller
    {
        // GET: LandingPage
        public ActionResult Index()
        {
            return RedirectToAction("Logout");
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            if (this.Request.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string username, string password)
        {
            string Err = "";
            int dt = 0;
            var data = UserDAL.cekUser(username, password, out Err, out dt);
            if (Err == "")
            {
                if (UserDAL.UpdateDataLogin(data.ID, Session.SessionID.ToString(), out Err))
                {
                    SignInAsync(username);
                    Session["USER"] = data;
                    //var timeOut = Session.Timeout.ToString(); setingan bawaan 20 menit.
                    if (data.KodeAkses == "Checker")
                    {
                        if (dt == 0)
                        {
                            return RedirectToAction("SetFacility", "Checker");
                        }
                        else
                        {
                            return RedirectToAction("Layout", "Checker");
                        }                        
                    }
                    if (data.KodeAkses == "Validator 1" || data.KodeAkses == "Validator 2" || data.KodeAkses == "Validator 3")
                    {
                        return RedirectToAction("Layout", "Validator");
                    }
                    if (data.KodeAkses == "Verifikator 1" || data.KodeAkses == "Verifikator 2" )
                    {
                        return RedirectToAction("Layout", "Verifikator");
                    }
                    if (data.KodeAkses == "Admin")
                    {
                        return RedirectToAction("Layout", "PeerGroup");
                    }
                }
                else
                {
                    TempData["Message"] = Err;

                    return View();
                }
            }
            else
            {
                if (Err == "Maaf ID anda berstatus Aktif, silahkan Reset terlebih dahulu.!")
                {
                    TempData["MsgPass"] = Err;
                    return View();
                }
                else
                {
                    TempData["Message"] = Err;
                    return View();
                }     
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult SessionClear(string IDReg) {

            UserDAL.resertLogin(IDReg);
            return RedirectToAction("Login");
        }
        [AllowAnonymous]
        public ActionResult ResetPassword() {

            return View();
        }
        [AllowAnonymous]
        public ActionResult CheckRegister(string IDRegister)
        {
            string Err = "", Mesage = "";            
            UserDAL.CheckDataUser(IDRegister, out Mesage, out Err);
            return Json(new { success = true, Values = Mesage, JsonRequestBehavior.AllowGet });
        }
        [AllowAnonymous]
        public ActionResult ChangePassword(string IDRegister, string Password)
        {
            string Message = "", Err = "";
            UserDAL.ChangePassword(IDRegister, Password, out Message, out Err);
            return Json(new { success = true, Values = Message, JsonRequestBehavior.AllowGet });
        }
        public ActionResult Logout()
        {
            var dtSesion = (loginVM)Session["USER"];
            if (dtSesion != null)
            {
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;
                UserDAL.resertLogin(dtSesion.IDRegister);
                authenticationManager.SignOut();
                Session.Clear();
                Session.Abandon();
                FormsAuthentication.SignOut();
                return RedirectToAction("Login");
            }
            else
            {
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;
                authenticationManager.SignOut();
                Session.Clear();
                Session.Abandon();
                FormsAuthentication.SignOut();
                return RedirectToAction("Login");
            }            
        }
        private void SignInAsync(string IDRegister)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, IDRegister));
            var id = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignIn(id);

        }
    }
}