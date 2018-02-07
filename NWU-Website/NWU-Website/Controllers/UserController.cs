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
    public class UserController : Controller
    {
        private nwuDBEntities db = new nwuDBEntities();

        // GET: User
        public ActionResult Index()
        {
            return View(db.personales.ToList());
        }

        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            personale personale = db.personales.Find(id);
            if (personale == null)
            {
                return HttpNotFound();
            }
            return View(personale);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "personaleID,fornavn,efternavn,brugernavn,password,rolleID")] personale personale)
        {
            if (ModelState.IsValid)
            {
                db.personales.Add(personale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(personale);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            personale personale = db.personales.Find(id);
            if (personale == null)
            {
                return HttpNotFound();
            }
            return View(personale);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "personaleID,fornavn,efternavn,brugernavn,password,rolleID")] personale personale)
        {
            if (ModelState.IsValid)
            {
                db.Entry(personale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(personale);
        }

        // GET: User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            personale personale = db.personales.Find(id);
            if (personale == null)
            {
                return HttpNotFound();
            }
            return View(personale);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            personale personale = db.personales.Find(id);
            db.personales.Remove(personale);
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
    }
}
