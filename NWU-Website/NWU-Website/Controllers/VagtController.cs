using NWU_Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NWU_Website.Controllers
{
    public class VagtController : Controller
    {
        // GET: Vagt
        public ActionResult Index()
        {
            //VagtCentralChartsValues vcObj = new VagtCentralChartsValues();
            ChartData chartData = new ChartData();

            // Create a context who controlls access to the DB.
            // The using block close and run dispose after - Claus 
            // TalTabelContext er forbindelsen på Claus's computer til lokal DB

            using (var context = new nwuDB1Entities2())
            {
                //Area 1 Minigolf
                //get max and name
                var area1 = context.omraade.Where(s => s.omraadeID == 1);
                chartData.MaxNumber.Add(area1.First().maxAntal);
                chartData.AreaNames.Add(area1.First().omraadreNavn);

                //get current number in area
                var area1Current = context.checkIND.Where(s => s.checkindID == 3);
                chartData.CurrentNumber.Add(area1Current.First().antalPersoner);


                //Area 2 Abeland
                //get max and name
                var area2 = context.omraade.Where(s => s.omraadeID == 2);
                chartData.MaxNumber.Add(area2.First().maxAntal);
                chartData.AreaNames.Add(area2.First().omraadreNavn);

                //get current number in area
                var area2Current = context.checkIND.Where(s => s.checkindID == 4);
                chartData.CurrentNumber.Add(area2Current.First().antalPersoner);


                //Area 3 Fitness
                //get max and name
                var area3 = context.omraade.Where(s => s.omraadeID == 3);
                chartData.MaxNumber.Add(area3.First().maxAntal);
                chartData.AreaNames.Add(area3.First().omraadreNavn);

                //get current number in area
                var area3Current = context.checkIND.Where(s => s.checkindID == 6);
                chartData.CurrentNumber.Add(area3Current.First().antalPersoner);

                //Area 4 Wellness
                //get max and name
                var area4 = context.omraade.Where(s => s.omraadeID == 4);
                chartData.MaxNumber.Add(area4.First().maxAntal);
                chartData.AreaNames.Add(area4.First().omraadreNavn);

                //get current number in area
                var area4Current = context.checkIND.Where(s => s.checkindID == 7);
                chartData.CurrentNumber.Add(area4Current.First().antalPersoner);


                //Area 5 Vandrutsjebaner
                //get max and name
                var area5 = context.omraade.Where(s => s.omraadeID == 5);
                chartData.MaxNumber.Add(area5.First().maxAntal);
                chartData.AreaNames.Add(area5.First().omraadreNavn);

                //get current number in area
                var area5Current = context.checkIND.Where(s => s.checkindID == 8);
                chartData.CurrentNumber.Add(area5Current.First().antalPersoner);

                //Area 6 River rafting
                //get max and name
                var area6 = context.omraade.Where(s => s.omraadeID == 6);
                chartData.MaxNumber.Add(area6.First().maxAntal);
                chartData.AreaNames.Add(area6.First().omraadreNavn);

                //get current number in area
                var area6Current = context.checkIND.Where(s => s.checkindID == 9);
                chartData.CurrentNumber.Add(area6Current.First().antalPersoner);

                //Area 7 Lazy river
                //get max and name
                var area7 = context.omraade.Where(s => s.omraadeID == 7);
                chartData.MaxNumber.Add(area7.First().maxAntal);
                chartData.AreaNames.Add(area7.First().omraadreNavn);

                //get current number in area
                var area7Current = context.checkIND.Where(s => s.checkindID == 10);
                chartData.CurrentNumber.Add(area7Current.First().antalPersoner);

                //Area 8 Splashzone
                //get max and name
                var area8 = context.omraade.Where(s => s.omraadeID == 8);
                chartData.MaxNumber.Add(area8.First().maxAntal);
                chartData.AreaNames.Add(area8.First().omraadreNavn);

                //get current number in area
                var area8Current = context.checkIND.Where(s => s.checkindID == 11);
                chartData.CurrentNumber.Add(area8Current.First().antalPersoner);

                //Area 9 soppebassin
                //get max and name
                var area9 = context.omraade.Where(s => s.omraadeID == 9);
                chartData.MaxNumber.Add(area9.First().maxAntal);
                chartData.AreaNames.Add(area9.First().omraadreNavn);

                //get current number in area
                var area9Current = context.checkIND.Where(s => s.checkindID == 12);
                chartData.CurrentNumber.Add(area9Current.First().antalPersoner);

                //Area 10 bølgebassin
                //get max and name
                var area10 = context.omraade.Where(s => s.omraadeID == 10);
                chartData.MaxNumber.Add(area10.First().maxAntal);
                chartData.AreaNames.Add(area10.First().omraadreNavn);

                //get current number in area
                var area10Current = context.checkIND.Where(s => s.checkindID == 13);
                chartData.CurrentNumber.Add(area10Current.First().antalPersoner);

                //Area 11 Vandfald
                //get max and name
                var area11 = context.omraade.Where(s => s.omraadeID == 11);
                chartData.MaxNumber.Add(area11.First().maxAntal);
                chartData.AreaNames.Add(area11.First().omraadreNavn);

                //get current number in area
                var area11Current = context.checkIND.Where(s => s.checkindID == 14);
                chartData.CurrentNumber.Add(area11Current.First().antalPersoner);
            }
            // Pause the method for 5 seconds - Claus
            //Thread.Sleep(5000);

            // return the object to the View - Claus
            return View(chartData);
        }
    }
}