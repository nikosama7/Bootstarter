using Bootstarter.Models;
using Bootstarter.Repositories;
using Bootstarter.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Bootstarter.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ApplicationDbContext db;
        private readonly IdeaRepository _ideaRepository;

        public HomeController()
        {
            //db = new ApplicationDbContext();
            _ideaRepository = new IdeaRepository();
        }

        public ActionResult Index()
        {
            int productsPerPage = 6;

            var ideas = _ideaRepository.GetNotCancelledIdeasOrderedBy(i => i.EndDate, true)     
                            .Take(productsPerPage)
                            .ToList();

            var viewModel = new IdeaIndexViewModel()
            {
                Ideas = ideas,
            };

            return View(viewModel);
        }

        public ActionResult Search(int page = 1, ItemsPerPage itemsPerPage = ItemsPerPage.p05, string Search = "")
        {
            if (page <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Parameter 'page' must be positive.");
            }
            int productsPerPage = (int)itemsPerPage;

            var filters = _ideaRepository.GetNotCancelledIdeasBySearch(Search);

            int totalPages = (int)Math.Ceiling((double)filters.Count() / (double)productsPerPage);

            var ideas = filters
                            .OrderByDescending(x => x.EndDate)
                            .Skip((page - 1) * productsPerPage)
                            .Take(productsPerPage)
                            .ToList();

            var viewModel = new IdeaIndexViewModel()
            {
                Ideas = ideas,
                Search = Search,
                TotalPages = totalPages,
                CurrentPage = page,
                ItemsPerPage = itemsPerPage,
            };

            return View(viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _ideaRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}