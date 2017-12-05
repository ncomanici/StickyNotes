using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StickyNotes.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace StickyNotes.Controllers
{
    public class DashboardNotesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DashboardNotes
        public ActionResult Index()
        {
            // get current logged user
            ApplicationUser currentUser = this.CurrentUser;

            if (currentUser != null)
            {
                // load notes for logged in user
                string userId = User.Identity.GetUserId();
                var dashboardNotes = db.DashboardNotes.Include(d => d.ApplicationUser).Where(d => d.ApplicationUserId == userId);
                return View(dashboardNotes.ToList());
            }
            else
            {
                // if no user is logged in, then redirect to login
                return Redirect("/Account/Login");
            }
        }

        // GET: DashboardNotes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DashboardNote dashboardNote = db.DashboardNotes.Find(id);
            if (dashboardNote == null)
            {
                return HttpNotFound();
            }
            return View(dashboardNote);
        }

        private ApplicationUser CurrentUser
        {
            get
            {
                UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                return userManager.FindById(User.Identity.GetUserId());
            }
        }

        [HttpPost]
        public ActionResult Save(int id, string title, string description)
        {
            // get current logged user
            ApplicationUser currentUser = this.CurrentUser;

            if (currentUser != null)
            {
                DashboardNote dashboardNote;
                if (id == -1)
                {
                    dashboardNote = new DashboardNote();
                    db.DashboardNotes.Add(dashboardNote);
                }
                else
                {
                    dashboardNote = db.DashboardNotes.Find(id);
                }

                // populate the fields
                dashboardNote.Title = title;
                dashboardNote.Description = description;
                dashboardNote.ApplicationUser = currentUser;

                // update changes in database
                db.SaveChanges();

                return Json(dashboardNote);
            }
            else
            {
                return new HttpUnauthorizedResult("You are not logged in.");
            }
        }

        // GET: DashboardNotes/Create
        public ActionResult Create()
        {
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: DashboardNotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,ApplicationUserId")] DashboardNote dashboardNote)
        {
            if (ModelState.IsValid)
            {
                db.DashboardNotes.Add(dashboardNote);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", dashboardNote.ApplicationUserId);
            return View(dashboardNote);
        }

        // GET: DashboardNotes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DashboardNote dashboardNote = db.DashboardNotes.Find(id);
            if (dashboardNote == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", dashboardNote.ApplicationUserId);
            return View(dashboardNote);
        }

        // POST: DashboardNotes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,ApplicationUserId")] DashboardNote dashboardNote)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dashboardNote).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", dashboardNote.ApplicationUserId);
            return View(dashboardNote);
        }

        // GET: DashboardNotes/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            // get current logged user
            ApplicationUser currentUser = this.CurrentUser;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dashboardNote =
                this.db.DashboardNotes.FirstOrDefault(
                    d => (d.Id == id && d.ApplicationUserId == currentUser.Id));

            if (dashboardNote == null)
            {
                return this.HttpNotFound();
            }

            db.DashboardNotes.Remove(dashboardNote);
            db.SaveChanges();

            return View(dashboardNote);
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
