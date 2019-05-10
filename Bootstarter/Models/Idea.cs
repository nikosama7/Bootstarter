using Bootstarter.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bootstarter.Models
{
    public class Idea
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Image { get; set; }          
        public decimal Goal { get; set; }
        [Display(Name = "Money Gathered")]
        public decimal MoneyGathered { get; set; }
        public bool Cancelled {get; set;}
        [Display(Name = "Submission Date")]
        public DateTime SubmissionDate { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Cancellation Date")]
        public DateTime? CancellationDate { get; set; }

        [Required]
        public string FounderId { get; set; }    
        public ApplicationUser Founder { get; set; }

        public ICollection<Interest> Interested { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public Idea()
        {
            Interested = new Collection<Interest>();
            Transactions = new Collection<Transaction>();
            Comments = new Collection<Comment>();
        }

        public Idea(IdeaFormViewModel viewModel, string userId) : this()
        {
            Name = viewModel.Name;
            Description = viewModel.Description;
            Goal = viewModel.Goal;
            MoneyGathered = 0.0m;
            SubmissionDate = DateTime.Now;
            StartDate = DateTime.Parse(viewModel.StartDate);
            EndDate = DateTime.Parse(viewModel.EndDate);
            Cancelled = false;
            FounderId = userId;
            Image = viewModel.Image;
        }

        public void Modify(IdeaFormViewModel viewModel)
        {
            Name = viewModel.Name;
            Description = viewModel.Description;
            Image = viewModel.Image;
            Goal = viewModel.Goal;
            StartDate = DateTime.Parse(viewModel.StartDate);
            EndDate = DateTime.Parse(viewModel.EndDate);
        }

        public void Cancel()
        {
            Cancelled = true;
            CancellationDate = DateTime.Now;
        }

        public void Deposit(decimal money)
        {
            MoneyGathered += money;
        }
    }
/*
--------------
|    TODO    |
--------------

Requirements:
StartDate Constrains
EndDate
*/
}

