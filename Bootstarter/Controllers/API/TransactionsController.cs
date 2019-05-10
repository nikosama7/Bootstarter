using Bootstarter.Dtos;
using Bootstarter.Models;
using Bootstarter.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Bootstarter.Controllers.API
{
    //[Authorize]
    public class TransactionsController : ApiController
    {
        private readonly IdeaRepository _ideaRepository;
        private readonly TransactionRepository _transactionRepository;

        public TransactionsController()
        {
            _transactionRepository = new TransactionRepository();
            _ideaRepository = new IdeaRepository();
        }

        [HttpPost]
        public IHttpActionResult BackingUp(TransactionDto dto)
        {
            var userId = User.Identity.GetUserId();

            if (_ideaRepository.IsMyIdea(dto.IdeaId, userId))
            {
                return BadRequest("Cheater");
            }

            _transactionRepository.BackingUp(dto.IdeaId, dto.Backing, userId);

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var userId = User.Identity.GetUserId();

            if (!_transactionRepository.CancelTransaction(id, userId))
            {
                return NotFound();
            }

            return Ok();
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
