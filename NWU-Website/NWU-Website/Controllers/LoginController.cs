using NWU_Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NWU_Website.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(NWU_Website.Models.Personale userModel)
        {
            using(nwuDB1Entities1 db = new nwuDB1Entities1())
            {
                var userDetails = db.Personales.Where(x => x.brugernavn == userModel.brugernavn && x.adgangskode == userModel.adgangskode).FirstOrDefault();
                if(userDetails == null)
                {
                    userModel.LoginErrorMessage = "Forkert brugernavn eller adgangskode.";
                    return View("Index", userModel);
                }
                else
                {
                    Session["personaleID"] = userDetails.personaleID;
                    return RedirectToAction("Index", "Home");
                }
            }
                
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");

        }
    }
}