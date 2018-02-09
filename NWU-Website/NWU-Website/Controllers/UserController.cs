using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NWU_Website.Models;
using NWU_Website.ViewModel;

namespace NWU_Website.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            UserViewModel userViewModel = new UserViewModel();
            userViewModel.personale = new personale();
            return View();
        }

       
        public ActionResult CreateUser()
        {
            return View();
        }
    }
}