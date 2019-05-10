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
    public class CommentsController : ApiController
    {
        private readonly CommentRepository _commentRepository;

        public CommentsController()
        {
            _commentRepository = new CommentRepository();
        }

        [AllowAnonymous]
        public List<Comment> GetComments(int ideaId)
        {
            // WebApiConfig.cs:  
            //config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; 
            var comments = _commentRepository.GetCommentsOfIdea(ideaId)
                            .ToList();

            return comments;
        }

        [HttpPost]
        public IHttpActionResult Comment(CommentDto dto)
        {
            var userId = User.Identity.GetUserId();

            _commentRepository.Comment(dto.Comment, dto.IdeaId, userId);

            return Ok();
        }
    }
}
