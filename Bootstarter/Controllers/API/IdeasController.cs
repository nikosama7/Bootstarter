using Bootstarter.Dtos;
using Bootstarter.Models;
using Bootstarter.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http;

namespace Bootstarter.Controllers.API
{
    [Authorize(Roles = "Admin, Founder")]
    public class IdeasController : ApiController
    {
        private readonly IdeaRepository _ideaRepository;

        public IdeasController()
        {
            _ideaRepository = new IdeaRepository();
        }

        [AllowAnonymous]
        public List<Idea> GetIdeas(int page=1, int itemsPerPage=6)
        {
            // WebApiConfig.cs:  
            //config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; 
            var ideas = _ideaRepository.GetNotCancelledIdeasOrderedBy(i => i.EndDate)
                            .Skip((page - 1) * itemsPerPage)
                            .Take(itemsPerPage).ToList(); 
            
            return ideas;
        }

        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var user = User.Identity;

            if(!_ideaRepository.CancelIdea(id, user))
            {
                return BadRequest();
            }

            return Ok();
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
