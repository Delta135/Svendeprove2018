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
        public ActionResult Authorize(NWU_Website.Models.personale userModel)
        {
            using(nwuDBEntities db = new nwuDBEntities())
            {
                var userDetails = db.personales.Where(x => x.brugernavn == userModel.brugernavn && x.password == userModel.password).FirstOrDefault();
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