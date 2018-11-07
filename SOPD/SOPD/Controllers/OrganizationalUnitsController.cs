using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SOPD.Models;

namespace SOPD.Controllers
{
    public class OrganizationalUnitsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: OrganizationalUnits
        public ActionResult Index()
        {
            return View(db.OrganizationalUnits.ToList());
        }

        // GET: OrganizationalUnits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrganizationalUnit organizationalUnit = db.OrganizationalUnits.Find(id);
            if (organizationalUnit == null)
            {
                return HttpNotFound();
            }
            return View(organizationalUnit);
        }

        // GET: OrganizationalUnits/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrganizationalUnits/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrganizationalUnitID,UnitName")] OrganizationalUnit organizationalUnit)
        {
            if (ModelState.IsValid)
            {
                db.OrganizationalUnits.Add(organizationalUnit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(organizationalUnit);
        }

        // GET: OrganizationalUnits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrganizationalUnit organizationalUnit = db.OrganizationalUnits.Find(id);
            if (organizationalUnit == null)
            {
                return HttpNotFound();
            }
            return View(organizationalUnit);
        }

        // POST: OrganizationalUnits/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrganizationalUnitID,UnitName")] OrganizationalUnit organizationalUnit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(organizationalUnit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(organizationalUnit);
        }

        // GET: OrganizationalUnits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrganizationalUnit organizationalUnit = db.OrganizationalUnits.Find(id);
            if (organizationalUnit == null)
            {
                return HttpNotFound();
            }
            return View(organizationalUnit);
        }

        // POST: OrganizationalUnits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrganizationalUnit organizationalUnit = db.OrganizationalUnits.Find(id);
            db.OrganizationalUnits.Remove(organizationalUnit);
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
