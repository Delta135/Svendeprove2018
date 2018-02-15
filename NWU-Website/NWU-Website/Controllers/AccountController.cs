using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NWU_Website.Models;

namespace NWU_Website.Controllers
{
    //Alt på Account controller har Samet Lavet
    public class AccountController : Controller
    {
        private nwuDB1Entities1 db = new nwuDB1Entities1();

        // GET: Account
        public ActionResult Index()
        {
            return View(db.Personales.ToList());
        }

        // GET: Account/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Account/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "personaleID,fornavn,efternavn,brugernavn,adgangskode,rolleID")] Personale personale)
        {
            if (ModelState.IsValid)
            {
                personale.adgangskode = AESCryptography.Encryption(personale.adgangskode);
                db.Personales.Add(personale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ModelState.Clear();
            ViewBag.Message = "Successfully Registered MR. " + personale.fornavn + " " + personale.efternavn;

            return View(personale);
        }

        // GET: Account/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personale personale = db.Personales.Find(id);
            if (personale == null)
            {
                return HttpNotFound();
            }
            return View(personale);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "personaleID,fornavn,efternavn,brugernavn,adgangskode,rolleID")] Personale personale)
        {
            if (ModelState.IsValid)
            {
                personale.adgangskode = AESCryptography.Encryption(personale.adgangskode);
                db.Entry(personale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(personale);
        }

        // GET: Account/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personale personale = db.Personales.Find(id);
            if (personale == null)
            {
                return HttpNotFound();
            }
            return View(personale);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Personale personale = db.Personales.Find(id);
            db.Personales.Remove(personale);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Autherize(NWU_Website.Models.Personale userModel)
        {
            using (nwuDB1Entities1 db = new nwuDB1Entities1())
            {
                var userDetails = db.Personales.SingleOrDefault(p => p.brugernavn == userModel.brugernavn && p.adgangskode == userModel.adgangskode);
                //var userDetails = db.Personales.Single(p => p.brugernavn == userModel.brugernavn && p.adgangskode == userModel.adgangskode);
                
                //checking if userdetails are the same as null if credentials i
                if (userDetails == null)
                {
                    //If your login information is incorrect, you will receive a warning
                    userModel.LoginErrorMessage = "Wrong username or password.";
                    return View("Index", userModel);
                }

                else
                {
                    //If its correct you will be redirectet to Index view in accountcontroller
                    Session["userID"] = userDetails.personaleID;
                    Session["userName"] = userDetails.brugernavn;
                    return RedirectToAction("Index", "Account");
                }
             
               
            }

         

        }
    }
}
