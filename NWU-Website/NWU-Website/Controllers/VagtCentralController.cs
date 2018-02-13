using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NWU_Website.Models;
using System.Threading;

namespace NWU_Website.Controllers
{
    public class VagtCentralController : Controller
    {
        // GET: VagtCentral
        public ActionResult Index()
        {
            // Obejct who holds data we send to View and can use with @Model.? - Claus
            VagtCentralChartsValues vcObj = new VagtCentralChartsValues();

            // Create a context who controlls access to the DB.
            // The using block close and run dispose after - Claus 
            // TalTabelContext er forbindelsen på Claus's computer til lokal DB
           
            using (var context = new TalTabelContext())
            {
                // LINQ statement who find and put all values in the row where 
                // id is 1 into the var q - Claus
                var q = context.TalTabels.Where(s => s.id == 1);

                // Convert LINQ query result into object values - Claus 
                vcObj.Tal1 = (int)q.First().Tal1;
                vcObj.Tal2 = (int)q.First().Tal2;
                vcObj.Tal3 = (int)q.First().Tal3;
                vcObj.Tal4 = (int)q.First().Tal4;
                vcObj.Tal5 = (int)q.First().Tal5;
                vcObj.Tal6 = (int)q.First().Tal6;
                vcObj.Tal7 = (int)q.First().Tal7;
                vcObj.Tal8 = (int)q.First().Tal8;

            }
            // Pause the method for 5 seconds - Claus
            Thread.Sleep(5000);

            // return the object to the View - Claus
            return View(vcObj);
        }


        public ActionResult RunningDemo()
        {

            return View();
        }
    }
}