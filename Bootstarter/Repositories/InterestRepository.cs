using Bootstarter.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Bootstarter.Repositories
{
    public class InterestRepository : IDisposable
    {
        private readonly ApplicationDbContext db;

        public InterestRepository()
        {
            db = new ApplicationDbContext();
        }

        public IEnumerable<Idea> GetInterestedIdeas(string userId)
        {
            return db.Interests.Where(i => i.UserId == userId).Select(i => i.Idea).Include(i => i.Founder).ToList();
        }

        public bool IsInterested(int id, string userId)
        {
            return (db.Interests.Any(i => i.IdeaId == id && i.UserId == userId));
        }

        public void Add(int id, string userId)
        {
            db.Interests.Add(new Interest(userId, id));
            db.SaveChanges();
        }

        public void Remove(int id, string userId)
        {
            Interest interest = db.Interests.Single(i => i.IdeaId == id && i.UserId == userId);
            db.Interests.Remove(interest);
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}