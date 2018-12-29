using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SOPD.Models;
using SOPD.Infrastructure;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using RotativaHQ.MVC5;
using PagedList;


namespace SOPD.Controllers
{
    public class ThesesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Theses
        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 3;
            var theses = db.Theses.Include(t => t.Author).Include(t => t.OrganizationalUnit).Include(t => t.Promoter).Include(t => t.Reviewer).OrderBy(t=>t.Title);
            return View(theses.ToPagedList(pageNumber,pageSize));
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
            ViewBag.reviewExists = thesis.Reviews.Count != 0;
            ViewBag.CanReview = thesis.Reviews.Count == 0 && thesis.ReviewerID == User.Identity.GetUserId();
            ViewBag.CanApproveProposition = thesis.State == ThesisState.Proposition && thesis.PromoterID == User.Identity.GetUserId();
            ViewBag.isPromoter= thesis.PromoterID == User.Identity.GetUserId();
            ViewBag.CanEdit = thesis.AuthorID == User.Identity.GetUserId() || thesis.PromoterID == User.Identity.GetUserId();
            return View(thesis);
        }
        [PromoterAuth]
        public ActionResult Propositions(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 3;
            var id = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var thesis = db.Theses.Where(t => t.State == ThesisState.Proposition && t.PromoterID == id).OrderBy(t=>t.Title).ToPagedList(pageNumber,pageSize);
            if (thesis == null)
            {
                return HttpNotFound();
            }
            return View("Index", thesis);
        }
        [ReviewerAuth]
        public ActionResult WaitingForReview(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 3;
            var id = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var thesis = db.Theses.Where(t => t.Reviews.Count == 0 && t.ReviewerID == id).OrderBy(t=>t.Title).ToPagedList(pageNumber,pageSize);
            if (thesis == null)
            {
                return HttpNotFound();
            }
            return View("Index", thesis);
        }

        public ActionResult MyTheses()
        {
            var userId = User.Identity.GetUserId();
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            List<List<Thesis>> viewModel = new List<List<Thesis>>();
            viewModel.Add(db.Theses.Where(t => t.AuthorID == userId).ToList());
            viewModel.Add(db.Theses.Where(t => t.PromoterID == userId).ToList());
            viewModel.Add(db.Theses.Where(t => t.ReviewerID == userId).ToList());
            if (viewModel.Count == 0)
            {
                return HttpNotFound();
            }
            return View(viewModel);
        }

        // GET: Theses/Create
        [ThesisAuth]
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
                if (User.IsInRole("Promotor"))
                {
                    thesis.AuthorID =null;
                    thesis.PromoterID = User.Identity.GetUserId();
                    thesis.State = ThesisState.UnApproved;
                }
                db.Theses.Add(thesis);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrganizationalUnitID = new SelectList(db.OrganizationalUnits, "OrganizationalUnitID", "UnitName", thesis.OrganizationalUnitID);
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
            ViewBag.ReviewerID = new SelectList(UsersController.GetReviewers(db), "Id", "FullName", thesis.ReviewerID);
            ViewBag.AuthorID = new SelectList(UsersController.GetDiplomants(db), "Id", "FullName", thesis.AuthorID);
            bool isPromoter = thesis.PromoterID == User.Identity.GetUserId();
            bool canEdit = thesis.AuthorID == User.Identity.GetUserId() || isPromoter;
            if (!canEdit)
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
            ViewBag.ReviewerID = new SelectList(UsersController.GetReviewers(db), "Id", "FullName", thesis.ReviewerID);
            ViewBag.AuthorID = new SelectList(UsersController.GetDiplomants(db), "Id", "FullName", thesis.AuthorID);
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
        public ActionResult UnapprovedTheses(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 3;
            var userId = User.Identity.GetUserId();
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var theses = db.Theses.Where(t => t.State == ThesisState.UnApproved).OrderBy(t=>t.Title);
            return View("Index", theses.ToPagedList(pageNumber,pageSize));
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
            return View("Details", thesis);

        }
        [PromoterAuth]
        public async Task<ActionResult> ApproveProposition(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thesis thesis = db.Theses.Find(id);
            if (thesis == null || thesis.PromoterID != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }
            thesis.State = ThesisState.UnApproved;
            db.SaveChanges();
            await ApproveEmail(thesis);
            return View("Details", thesis);

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ApproveEmail(Thesis model)
        {

            if (model == null)
            {
                return RedirectToAction("Details");
            }
            var apiKey = System.Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
            var client = new SendGridClient(apiKey);
            var body = "\t \t \t \t Powiadomienie SOPD! \n \t Praca o tytule: {0} zmieniła status z \"Propozycja\" na \"Niezatwierdzona\"";
            var message = new SendGridMessage();
            var from = new EmailAddress("skrzypapiotr144@gmail.com", "System obsługi prac dyplomowych");
            message.SetFrom(from);
            var to = new EmailAddress(model.Author.Email, model.Author.FullName);
            message.AddTo(to);
            message.SetSubject("Zmiana statusu tematu pracy");
            message.AddContent(MimeType.Text, string.Format(body, model.Title));
            var response = await client.SendEmailAsync(message);
            return RedirectToAction("Details");
        }


        [PromoterAuth]
        public ActionResult GeneratePDF(int? id)
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
            string footer = "--footer-right \"Date: [date] [time]\" " + "--footer-center \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\"";
            return new ViewAsPdf(thesis) { FileName = "TestViewAsPdf.pdf",
                                                          CustomSwitches=footer};
        }
    }
}
