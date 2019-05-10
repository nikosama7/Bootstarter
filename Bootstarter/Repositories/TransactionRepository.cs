using Bootstarter.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Bootstarter.Repositories
{
    public class TransactionRepository : IDisposable
    {
        private readonly ApplicationDbContext db;

        public TransactionRepository()
        {
            db = new ApplicationDbContext();
        }

        public IEnumerable<Transaction> GetMyBackings(string userId)
        {
            return db.Transactions
                            .Where(t => t.DonatorId == userId)
                            .Include(t => t.Idea)
                            .OrderByDescending(t => t.DonationDate);
        }

        public void BackingUp(int id, decimal backing, string userId)
        {
            var idea = db.Ideas.Find(id);

            db.Transactions.Add(new Transaction(id, userId, backing));
            idea.Deposit(backing);
            db.SaveChanges();
        }

        public bool CancelTransaction(int id, string userId)
        {
            var transaction = db.Transactions.SingleOrDefault(t => t.DonatorId == userId && t.Id == id);

            if (transaction == null || transaction.DonationDate < DateTime.Now.AddDays(-1))
            {
                return false;
            }
            var idea = db.Ideas.Find(transaction.IdeaId);
            idea.MoneyGathered -= transaction.DonationMoney;
            db.Transactions.Remove(transaction);
            db.SaveChanges();
            return true;
        }

        public void CancelIdea(int ideaId)
        {
            var transactions = db.Transactions.Where(t => t.IdeaId == ideaId).ToList();

            foreach(var transaction in transactions)
            {
                db.Transactions.Remove(transaction);
            }            
            //db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}