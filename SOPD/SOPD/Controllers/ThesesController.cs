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
using Microsoft.AspNet.Identity;

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

        public ActionResult MyTheses()
        {
            var userId = User.Identity.GetUserId();
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");           
            }
            List<List<Thesis>> viewModel =new List<List<Thesis>>();
            viewModel.Add(db.Theses.Where(t=>t.AuthorID==userId).ToList());
            viewModel.Add(db.Theses.Where(t => t.PromoterID == userId).ToList());
            viewModel.Add(db.Theses.Where(t => t.ReviewerID == userId).ToList());
            if (viewModel.Count==0)
            {
                return HttpNotFound();
            }
            return View(viewModel);
        }

        // GET: Theses/Create
        [Authorize(Roles="Promotor,Dyplomant")]
        public ActionResult Create()
        {
            ViewBag.AuthorID = new SelectList(db.Users, "Id", "FullName");
            ViewBag.OrganizationalUnitID = new SelectList(db.OrganizationalUnits, "OrganizationalUnitID", "UnitName");
            ViewBag.PromoterID = new SelectList(UsersController.GetPromotors(db), "Id", "FullName");
            ViewBag.ReviewerID = new SelectList(UsersController.GetReviewers(db), "Id", "FullName");
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
                if (User.IsInRole("Dyplomant"))
                {
                    thesis.State = ThesisState.Proposition;
                    thesis.AuthorID = User.Identity.GetUserId();
                }
                else
                {
                    thesis.PromoterID = User.Identity.GetUserId();
                    thesis.State = ThesisState.UnApproved;
                }
                db.Theses.Add(thesis);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            ViewBag.AuthorID = new SelectList(db.Users, "Id", "FullName", thesis.AuthorID);
            ViewBag.OrganizationalUnitID = new SelectList(db.OrganizationalUnits, "OrganizationalUnitID", "UnitName", thesis.OrganizationalUnitID);
            ViewBag.PromoterID = new SelectList(UsersController.GetPromotors(db), "Id", "FullName", thesis.PromoterID);
            ViewBag.ReviewerID = new SelectList(UsersController.GetReviewers(db), "Id", "FullName", thesis.ReviewerID);
            return View(thesis);
        }

        [StudentAuth]
        public ActionResult CreateProposition(string id)
        {
            ViewBag.SelectedPromoterID = id;
            ViewBag.OrganizationalUnitID = new SelectList(db.OrganizationalUnits, "OrganizationalUnitID", "UnitName");
            return View("Create");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [StudentAuth]
        public ActionResult CreateProposition(Thesis thesis)
        {

            return Create(thesis);
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
            ViewBag.CanEdit = thesis.AuthorID == User.Identity.GetUserId() && thesis.PromoterID == User.Identity.GetUserId();
            if (!ViewBag.CanEdit)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(thesis);
        }

        // POST: Theses/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PromoterAuth]
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
        [DeanAuth]
        public ActionResult UnapprovedTheses()
        {
            var userId = User.Identity.GetUserId();
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var theses = db.Theses.Where(t => t.State == ThesisState.UnApproved);
            return View("Index",theses.ToList());
        }
        [DeanAuth]
        public ActionResult ApproveThesis(int? id)
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
            thesis.State = ThesisState.Approved;
            thesis.ApprovalDate = DateTime.Now;
            db.SaveChanges();
            return View("Details",thesis);

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
