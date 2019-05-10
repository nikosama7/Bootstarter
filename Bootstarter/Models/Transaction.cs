using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bootstarter.Models
{
    public class Transaction
    {
        public int Id { get; set; }        
        [Required]
        public int IdeaId { get; set; }
        [Required]
        public string DonatorId { get; set; }
        
        public decimal DonationMoney { get; set; }
        public DateTime DonationDate { get; set; }

        public Idea Idea { get; set; }
        public ApplicationUser Donator { get; set; }      
        
        protected Transaction()
        {
            DonationDate = DateTime.Now;
        }

        public Transaction(int idea, string donator, decimal money) : this()
        {
            IdeaId = idea;
            DonatorId = donator;
            DonationMoney = money;
        }

    }
}