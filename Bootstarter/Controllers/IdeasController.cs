using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Bootstarter.Models;
using Bootstarter.Repositories;
using Bootstarter.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Bootstarter.Controllers
{
    public class IdeasController : Controller
    {
        private readonly IdeaRepository _ideaRepository;
        private readonly InterestRepository _interestRepository;

        public IdeasController()
        {
            _ideaRepository = new IdeaRepository();
            _interestRepository = new InterestRepository();
        }    

        // GET: Ideas
        public ActionResult Index()
        {
            var ideas = _ideaRepository.GetNotCancelledIdeas();
            return View("Ideas", ideas.ToList());
        }

        // GET: Ideas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var idea = _ideaRepository.GetAnIdea((int)id);
            if (idea == null)
            {
                return HttpNotFound();
            }
            var userId = User.Identity.GetUserId();
            var viewModel = new IdeaDetailsViewModel()
            {
                Idea = idea,
                VisitedByFounder = (userId == idea.FounderId),
                IsInterested = _interestRepository.IsInterested((int)id, userId)
            };
            return View("Details", viewModel);
        }

        [Authorize(Roles = "Founder,Admin")]
        public ActionResult Create()
        {
            // Fill ViewModel
            var viewModel = new IdeaFormViewModel
            {
                Heading = "Add your Project"
            };
            // Send ViewModel to view
            return View("IdeaForm", viewModel);
        }

        // POST: Ideas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Founder,Admin")]
        public ActionResult Create( IdeaFormViewModel viewModel, HttpPostedFileBase Image)
        {
            if (!ModelState.IsValid)
            {
                return View("IdeaForm", viewModel);
            }

            if (viewModel.Image != null)
            {
                string p = $"{Guid.NewGuid()}{Path.GetExtension(Image.FileName)}";
                string path = Path.Combine(Server.MapPath("~/UploadedFiles"), p);
                Image.SaveAs(path);
                viewModel.Image = "/UploadedFiles/" + p;
            }
            else
            {
                return View("IdeaForm", viewModel);
            }

            Idea idea = new Idea(viewModel, User.Identity.GetUserId());
            _ideaRepository.Add(idea);

            return RedirectToAction("Index", "Home");
        }

        //GET: Ideas/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = User.Identity;
            Idea idea = _ideaRepository.GetIdeaForEdit((int)id, user);
            if (idea == null)
            {
                return HttpNotFound();
            }
            var viewModel = new IdeaFormViewModel()
            {
                Id = idea.Id,
                Name = idea.Name,
                Description = idea.Description,
                Image = idea.Image,
                Goal = idea.Goal,
                StartDate = String.Format($"{idea.StartDate:d/M/yyyy}"),
                EndDate = String.Format($"{idea.EndDate:d/M/yyyy}"),
                Heading = "Edit an Idea"
            };

            //ViewBag.FounderId = new SelectList(db.ApplicationUsers, "Id", "CompanyName", idea.FounderId);
            return View("IdeaForm", viewModel);
        }

        // POST: Ideas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(IdeaFormViewModel viewModel, HttpPostedFileBase Image)
        {
            if (!ModelState.IsValid)
            {
                return View("IdeaForm", viewModel);
            }

            if (Image != null)
            {
                string p = $"{Guid.NewGuid()}{Path.GetExtension(Image.FileName)}";
                string path = Path.Combine(Server.MapPath("~/UploadedFiles"), p);
                Image.SaveAs(path);
                viewModel.Image = "/UploadedFiles/" + p;
            }

            var user = User.Identity;
            _ideaRepository.Edit(viewModel, user);

            //ViewBag.FounderId = new SelectList(db.ApplicationUsers, "Id", "CompanyName", idea.FounderId);
            return RedirectToAction("MyIdeas", "Ideas");
        }

        public ActionResult MyIdeas(string message)
        {
            var userId = User.Identity.GetUserId();
            var myIdeas = _ideaRepository.GetMyIdeas(userId);

            ViewBag.Message = String.IsNullOrEmpty(message) ? "" : message;
            return View(myIdeas);
        }

        public ActionResult UserIdeas(string id)
        {
            var myIdeas = _ideaRepository.GetMyIdeas(id);

            return View(myIdeas);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AllIdeas()
        {
            var myIdeas = _ideaRepository.GetAllIdeas();

            return View(myIdeas);
        }

        // GET: Ideas/Delete/5
        [Authorize(Roles = "Founder,Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = User.Identity;

            if (!_ideaRepository.CancelIdea((int)id, user))
            {
                return HttpNotFound();
            }

            return RedirectToAction("Index");
        }

        public ActionResult RecentIdeas()
        {

            return View();
        }

        // POST: Ideas/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Idea idea = db.Ideas.Find(id);
        //    db.Ideas.Remove(idea);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _interestRepository.Dispose();
                _ideaRepository.Dispose();
            }
            base.Dispose(disposing);
        }

        //public bool IsAdminUser(IIdentity user)
        //{
        //    if (user.IsAuthenticated)
        //    {
        //        //var user = User.Identity;
        //        ApplicationDbContext context = new ApplicationDbContext();
        //        var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
        //        var s = UserManager.GetRoles(user.GetUserId());
        //        if (s[0].ToString() == "Admin")
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    return false;
        //}
    }
}
