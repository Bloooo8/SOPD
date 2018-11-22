using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SOPD.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using SOPD.Infrastructure;

namespace SOPD.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.OrganizationalUnit);
            return View(users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            List<string> rolesIDs = user.Roles.Select(r => r.RoleId).ToList();
            List<IdentityRole> allRoles = db.Roles.ToList();
            List<string> rolesNames = new List<string>();
            foreach(IdentityRole r in allRoles)
            {
                if (rolesIDs.Contains(r.Id))
                {
                    rolesNames.Add(r.Name);
                }
            }
            EditAccountViewModel editModel = new EditAccountViewModel
            {
                NewUser = user,
                AllRoles = allRoles,
                RolesNames = rolesNames
            };
            editModel.AllRoles = db.Roles.ToList();
            return View(editModel);
        }

        // GET: Users/Create
        [AdminAuth]
        public ActionResult Create()
        {
            CreateAccountViewModel model = new CreateAccountViewModel()
            {
                NewUser = new User(),
                AllRoles = db.Roles.ToList(),
                RolesNames = new List<string>(),
            };
            ViewBag.UnitIDs = new SelectList(db.OrganizationalUnits, "OrganizationalUnitID", "UnitName", model.NewUser.OrganizationalUnitID);
            return View(model);
        }

        // POST: Users/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuth]
        public async Task<ActionResult> Create(CreateAccountViewModel account)
        {

            ApplicationUserManager userManager = HttpContext.GetOwinContext().Get<ApplicationUserManager>();
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = account.NewUser.UserName,
                    Email = account.NewUser.Email,
                    FirstName = account.NewUser.FirstName,
                    LastName = account.NewUser.LastName,
                    PhoneNumber=account.NewUser.PhoneNumber,
                    OrganizationalUnitID =account.NewUser.OrganizationalUnitID
                };

                var result = await userManager.CreateAsync(user, account.Password);
                if (result.Succeeded)
                {
                    foreach(var role in account.RolesNames)
                    {
                        userManager.AddToRole(user.Id, role);
                    }
                    
                    return RedirectToAction("Index", "Users");
                }
                AddErrors(result);
            }

            // Dotarcie do tego miejsca wskazuje, że wystąpił błąd, wyświetl ponownie formularz
            account.AllRoles = db.Roles.ToList();
            ViewBag.UnitIDs = new SelectList(db.OrganizationalUnits, "OrganizationalUnitID", "UnitName", account.NewUser.OrganizationalUnitID);
            return View(account);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        // GET: Users/Edit/5
        [AdminAuth]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            List<string> rolesIDs = user.Roles.Select(r => r.RoleId).ToList();
            List<IdentityRole> allRoles = db.Roles.ToList();
            List<string> rolesNames = new List<string>();
            foreach (IdentityRole r in allRoles)
            {
                if (rolesIDs.Contains(r.Id))
                {
                    rolesNames.Add(r.Name);
                }
            }
            EditAccountViewModel editModel = new EditAccountViewModel
            {
                NewUser = user,
                AllRoles = allRoles,
                RolesNames = rolesNames
            };
            ViewBag.UnitIDs = new SelectList(db.OrganizationalUnits, "OrganizationalUnitID", "UnitName", user.OrganizationalUnitID);
            editModel.AllRoles = db.Roles.ToList();
            return View(editModel);
        }

        // POST: Users/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AdminAuth]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditAccountViewModel model)
        {
            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            if (ModelState.IsValid)
            {
                var userID = model.NewUser.Id;
                var roles = userManager.GetRoles(userID);
                userManager.RemoveFromRoles(userID, roles.ToArray());
                userManager.AddToRoles(userID, model.RolesNames.ToArray());
                db.Entry(model.NewUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UnitIDs = new SelectList(db.OrganizationalUnits, "OrganizationalUnitID", "UnitName", model.NewUser.OrganizationalUnitID);
            model.AllRoles = db.Roles.ToList();
            return View(model);
        }

        // GET: Users/Delete/5
        [AdminAuth]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AdminAuth]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public static List<User> GetPromotors(ApplicationDbContext db)
        {
            string roleID = db.Roles.First(r=>r.Name=="Promotor").Id;
            var users = db.Users.Include(u => u.Roles).Where(u=>u.Roles.Any(r=>r.RoleId==roleID));

            return users.ToList();
        }
        public static List<User> GetReviewers(ApplicationDbContext db)
        {
            string roleID = db.Roles.First(r => r.Name == "Recenzent").Id;
            var users = db.Users.Include(u => u.Roles).Where(u => u.Roles.Any(r => r.RoleId == roleID));

            return users.ToList();
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
