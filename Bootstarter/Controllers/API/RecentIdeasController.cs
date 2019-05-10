using Bootstarter.Models;
using Bootstarter.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Bootstarter.Controllers.API
{
    public class RecentIdeasController : ApiController
    {
        private readonly IdeaRepository _ideaRepository;

        public RecentIdeasController()
        {
            _ideaRepository = new IdeaRepository();
        }

        [AllowAnonymous]
        public List<Idea> GetRecentIdeas(int page = 1, int itemsPerPage = 6)
        {
            // WebApiConfig.cs:  
            //config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; 
            var ideas = _ideaRepository.GetRecentIdeas()
                            .Skip((page - 1) * itemsPerPage)
                            .Take(itemsPerPage).ToList();

            return ideas;
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
