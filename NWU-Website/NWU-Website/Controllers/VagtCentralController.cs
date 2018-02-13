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
            //VagtCentralChartsValues vcObj = new VagtCentralChartsValues();
            ChartData chartData = new ChartData();

            // Create a context who controlls access to the DB.
            // The using block close and run dispose after - Claus 
            // TalTabelContext er forbindelsen på Claus's computer til lokal DB

            using (var context = new nwuDB1Entities2())
            {
                //Area 3
                //get max and name
                var area3 = context.omraade.Where(s => s.omraadeID == 1);
                chartData.MaxNumber.Add(area3.First().maxAntal);
                chartData.AreaNames.Add(area3.First().omraadreNavn);

                //get current number in area
                var area3Current = context.checkIND.Where(s => s.checkindID == 3);
                chartData.CurrentNumber.Add(area3Current.First().antalPersoner);


                //Area 4
                //get max and name
                var area4 = context.omraade.Where(s => s.omraadeID == 2);
                chartData.MaxNumber.Add(area4.First().maxAntal);
                chartData.AreaNames.Add(area4.First().omraadreNavn);

                //get current number in area
                var area4Current = context.checkIND.Where(s => s.checkindID == 4);
                chartData.CurrentNumber.Add(area4Current.First().antalPersoner);

            }
            // Pause the method for 5 seconds - Claus
            //Thread.Sleep(5000);

            // return the object to the View - Claus
            return View(chartData);
        }


        public ActionResult RunningDemo()
        {

            return View();
        }
    }
}