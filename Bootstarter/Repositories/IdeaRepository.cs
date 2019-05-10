using Bootstarter.Models;
using Bootstarter.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Bootstarter.Repositories
{
    public class IdeaRepository : IDisposable
    {
        private readonly ApplicationDbContext db;

        public IdeaRepository()
        {
            db = new ApplicationDbContext();
        }

        public IEnumerable<Idea> GetAllIdeas()
        {
            return db.Ideas.Include(i => i.Founder);
        }

        public IEnumerable<Idea> GetNotCancelledIdeas()
        {
            return db.Ideas.Where(i => !i.Cancelled).Include(i => i.Founder);
        }

        public IEnumerable<Idea> GetNotCancelledIdeasBySearch(string search)
        {
            return db.Ideas
                .Where(x => String.IsNullOrEmpty(search) || x.Name.ToLower().Contains(search.ToLower()) || x.Founder.CompanyName.ToLower().Contains(search.ToLower()))
                .Where(i => !i.Cancelled)
                .Include(i => i.Founder);
        }

        public IEnumerable<Idea> GetNotCancelledIdeasOrderedBy<T>(Func<Idea, T> orderby, bool desc=false)
        {
            if (desc)
            {
                return db.Ideas.Where(i => !i.Cancelled && (i.MoneyGathered < i.Goal)).Include(i => i.Founder).OrderByDescending(orderby);
            }
            return db.Ideas.Where(i => !i.Cancelled && (i.MoneyGathered < i.Goal)).Include(i => i.Founder).OrderBy(orderby);
        }

        public IEnumerable<Idea> GetRecentIdeas()
        {
            return db.Ideas.Where(i => !i.Cancelled && i.StartDate < DateTime.Now).Include(i => i.Founder).OrderByDescending(i => i.StartDate);
        }

        public IEnumerable<Idea> GetMyIdeas(string userId)
        {
            return db.Ideas
                    .Where(i => i.FounderId == userId)
                    .Include(i => i.Founder)
                    .OrderBy(i => i.StartDate)
                    .ToList();
        }

        public bool IsMyIdea(int id, string userId)
        {
            return db.Ideas.SingleOrDefault(i => i.FounderId == userId && i.Id == id) != null;
        }

        public Idea GetAnIdea(int id)
        {
            return db.Ideas.Include(i => i.Founder).FirstOrDefault(i => i.Id == id);
        }

        public Idea GetAnIdea(int id, string userId)
        {
            return db.Ideas.Include(i => i.Founder).SingleOrDefault(i => i.Id == id && i.FounderId == userId);
        }

        public Idea GetIdeaForEdit(int id, string userId, bool isAdmin)
        {
            return db.Ideas.SingleOrDefault(i => i.Id == id && (i.FounderId == userId || isAdmin));
        }

        public Idea GetIdeaForEdit(int id, IIdentity user)
        {
            bool isAdmin = ApplicationUser.IsAdminUser(user);
            var userId = user.GetUserId();
            return db.Ideas.SingleOrDefault(i => i.Id == id && (i.FounderId == userId || isAdmin));
        }

        public Idea GetMyIdeaForEdit(int id, IIdentity user)
        {
            bool isAdmin = ApplicationUser.IsAdminUser(user);
            var userId = user.GetUserId();
            return db.Ideas.Single(i => i.Id == id && (i.FounderId == userId || isAdmin));
        }

        public void Add(Idea idea)
        {
            db.Ideas.Add(idea);
            db.SaveChanges();
        }

        public void Edit(IdeaFormViewModel viewModel, IIdentity user)
        {
            var idea = GetMyIdeaForEdit(viewModel.Id, user);
            idea.Modify(viewModel);
            db.SaveChanges();
        }


        public bool CancelIdea(int id, IIdentity user)
        {
            bool isAdmin = ApplicationUser.IsAdminUser(user);
            string userId = user.GetUserId();
            Idea idea = db.Ideas.SingleOrDefault(i => i.Id == id && (i.FounderId == userId || isAdmin));

            if (idea == null)
            {
                return false;
            }

            if (!idea.Cancelled)
            {
                //TransactionRepository rep = new TransactionRepository();
                //rep.CancelIdea(idea.Id);
                var transactions = db.Transactions.Where(t => t.IdeaId == id).ToList();

                foreach (var transaction in transactions)
                {
                    db.Transactions.Remove(transaction);
                }
                idea.Cancel();
                db.SaveChanges();               
            }
            return true;
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}