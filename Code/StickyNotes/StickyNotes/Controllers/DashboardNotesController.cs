using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StickyNotes.Models;

namespace StickyNotes.Controllers
{
    public class DashboardNotesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DashboardNotes
        public ActionResult Index()
        {
            var dashboardNotes = db.DashboardNotes.Include(d => d.ApplicationUser);
            return View(dashboardNotes.ToList());
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
        public ActionResult Delete(int? id)
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

        // POST: DashboardNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DashboardNote dashboardNote = db.DashboardNotes.Find(id);
            db.DashboardNotes.Remove(dashboardNote);
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
