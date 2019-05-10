using Bootstarter.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Bootstarter.Repositories
{
    public class CommentRepository : IDisposable
    {
        private readonly ApplicationDbContext db;

        public CommentRepository()
        {
            db = new ApplicationDbContext();
        }

        public IEnumerable<Comment> GetCommentsOfIdea(int id)
        {
            return db.Comments.Where(c => c.IdeaId == id).Include(i => i.User).OrderBy(c => c.CommentTime);
        }

        public void Comment(string comment, int ideaId, string userId)
        {
            db.Comments.Add(new Models.Comment()
            {
                InspirationWord = comment,
                IdeaId = ideaId,
                UserId = userId,
                CommentTime = DateTime.Now
            });
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}