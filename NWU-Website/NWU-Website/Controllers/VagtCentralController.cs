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
            // Showing what our program will look like in real life
            return View();
        }

        public ActionResult ShowHistoryData(int? area, string dato)
        {
            
            HistoryData ttc = new HistoryData();
            nwuDB1Entities3  db = new nwuDB1Entities3();
           
            // if area is assigned a value
            if (area.HasValue)
            {
                // put all data from the table checkIND where kortlaeserID is the same as area into test
                var test = db.checkINDs.Where(o => o.kortlaeserID == area);

                // sort after date and time
                test = test.OrderBy(o => o.ckeckIND);

                // we transform a var result to a list<checkIND> result
                List<checkIND> testRedultater = test.ToList<checkIND>();


                ttc.Tal1 = (int)testRedultater.ElementAt(0).antalPersoner; //first result
                ttc.Tal2 = (int)testRedultater.ElementAt(1).antalPersoner;
                ttc.Tal3 = (int)testRedultater.ElementAt(2).antalPersoner;
                ttc.Tal4 = (int)testRedultater.ElementAt(3).antalPersoner;
                ttc.Tal5 = (int)testRedultater.ElementAt(4).antalPersoner;
                ttc.Tal6 = (int)testRedultater.ElementAt(5).antalPersoner;
                ttc.Tal7 = (int)testRedultater.ElementAt(6).antalPersoner;
                ttc.Tal8 = (int)testRedultater.ElementAt(7).antalPersoner;
                ttc.Tal9 = (int)testRedultater.ElementAt(8).antalPersoner;
                ttc.Tal10 = (int)testRedultater.ElementAt(9).antalPersoner;
                ttc.Tal11 = (int)testRedultater.ElementAt(10).antalPersoner;
            }

            return View(ttc);            
        }
    }
}