using Bootstarter.Models;
using Bootstarter.Repositories;
using Bootstarter.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Bootstarter.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private readonly IdeaRepository _ideaRepository;
        private readonly TransactionRepository _transactionRepository;

        public TransactionsController()
        {
            _transactionRepository = new TransactionRepository();
            _ideaRepository = new IdeaRepository();
        }

        public ActionResult MyBackings()
        {
            var userId = User.Identity.GetUserId();
            var myBackings = _transactionRepository.GetMyBackings(userId)
                            .ToList();

            return View(myBackings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BackingUp(IdeaDetailsViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", "Ideas", new { id = vm.TransactionDto.IdeaId });
            }

            var userId = User.Identity.GetUserId();

            if (_ideaRepository.IsMyIdea(vm.TransactionDto.IdeaId, userId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            _transactionRepository.BackingUp(vm.TransactionDto.IdeaId, vm.TransactionDto.Backing, userId);
            
            return RedirectToAction("Details", "Ideas", new { id = vm.TransactionDto.IdeaId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _ideaRepository.Dispose();
                _transactionRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}