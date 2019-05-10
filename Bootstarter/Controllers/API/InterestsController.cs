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
    [Authorize]
    public class InterestsController : ApiController
    {
        private readonly InterestRepository _interestRepository;

        public InterestsController()
        {
            _interestRepository = new InterestRepository();
        }

        [HttpGet]
        public IEnumerable<Idea> InterestedIdeas()
        {
            var followerId = User.Identity.GetUserId();

            return _interestRepository.GetInterestedIdeas(followerId);
        }

        [HttpPost]
        public IHttpActionResult Follow(InterestDto dto)
        {
            var followerId = User.Identity.GetUserId();

            if (_interestRepository.IsInterested(dto.IdeaId, followerId))
            {
                return BadRequest("You are already interested in this Idea");
            }

            _interestRepository.Add(dto.IdeaId, followerId);

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Unfollow(int id)
        {
            var userId = User.Identity.GetUserId();

            if (!_interestRepository.IsInterested(id, userId))
            {
                return NotFound();
            }

            _interestRepository.Remove(id, userId);

            return Ok(id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _interestRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
