using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SOPD.Models;
using SOPD.Infrastructure;

namespace SOPD.Controllers
{
    public class ThesesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Theses
        public ActionResult Index()
        {
            var theses = db.Theses.Include(t => t.Author).Include(t => t.OrganizationalUnit).Include(t => t.Promoter).Include(t => t.Reviewer);
            return View(theses.ToList());
        }

        // GET: Theses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thesis thesis = db.Theses.Find(id);
            if (thesis == null)
            {
                return HttpNotFound();
            }
            return View(thesis);
        }

        // GET: Theses/Create
        [Authorize(Roles="Promotor,Dyplomant")]
        public ActionResult Create()
        {
            ViewBag.AuthorID = new SelectList(db.Users, "Id", "FullName");
            ViewBag.OrganizationalUnitID = new SelectList(db.OrganizationalUnits, "OrganizationalUnitID", "UnitName");
            ViewBag.PromoterID = new SelectList(db.Users, "Id", "FullName");
            ViewBag.ReviewerID = new SelectList(db.Users, "Id", "FullName");
            return View();
        }

        // POST: Theses/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ThesisAuth]
        public ActionResult Create([Bind(Include = "ThesisID,Title,EnglishTitle,Abstract,EnglishAbstract,ApprovalDate,KeyWords,EnglishKeyWords,State,PromoterID,AuthorID,OrganizationalUnitID,ReviewerID")] Thesis thesis)
        {
            if (ModelState.IsValid)
            {
                db.Theses.Add(thesis);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AuthorID = new SelectList(db.Users, "Id", "FullName", thesis.AuthorID);
            ViewBag.OrganizationalUnitID = new SelectList(db.OrganizationalUnits, "OrganizationalUnitID", "UnitName", thesis.OrganizationalUnitID);
            ViewBag.PromoterID = new SelectList(db.Users, "Id", "FullName", thesis.PromoterID);
            ViewBag.ReviewerID = new SelectList(db.Users, "Id", "FullName", thesis.ReviewerID);
            return View(thesis);
        }

        // GET: Theses/Edit/5
        [ThesisAuth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thesis thesis = db.Theses.Find(id);
            if (thesis == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorID = new SelectList(db.Users, "Id", "FullName", thesis.AuthorID);
            ViewBag.OrganizationalUnitID = new SelectList(db.OrganizationalUnits, "OrganizationalUnitID", "UnitName", thesis.OrganizationalUnitID);
            ViewBag.PromoterID = new SelectList(db.Users, "Id", "FullName", thesis.PromoterID);
            ViewBag.ReviewerID = new SelectList(db.Users, "Id", "FullName", thesis.ReviewerID);
            return View(thesis);
        }

        // POST: Theses/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ThesisAuth]
        public ActionResult Edit([Bind(Include = "ThesisID,Title,EnglishTitle,Abstract,EnglishAbstract,ApprovalDate,KeyWords,EnglishKeyWords,State,PromoterID,AuthorID,OrganizationalUnitID,ReviewerID")] Thesis thesis)
        {
            if (ModelState.IsValid)
            {
                db.Entry(thesis).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorID = new SelectList(db.Users, "Id", "FullName", thesis.AuthorID);
            ViewBag.OrganizationalUnitID = new SelectList(db.OrganizationalUnits, "OrganizationalUnitID", "UnitName", thesis.OrganizationalUnitID);
            ViewBag.PromoterID = new SelectList(db.Users, "Id", "FullName", thesis.PromoterID);
            ViewBag.ReviewerID = new SelectList(db.Users, "Id", "FullName", thesis.ReviewerID);
            return View(thesis);
        }

        // GET: Theses/Delete/5
        [AdminAuth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thesis thesis = db.Theses.Find(id);
            if (thesis == null)
            {
                return HttpNotFound();
            }
            return View(thesis);
        }

        // POST: Theses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AdminAuth]
        public ActionResult DeleteConfirmed(int id)
        {
            Thesis thesis = db.Theses.Find(id);
            db.Theses.Remove(thesis);
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
