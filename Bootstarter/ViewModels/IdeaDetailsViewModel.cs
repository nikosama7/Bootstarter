using Bootstarter.Models;
using Bootstarter.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bootstarter.ViewModels
{
    public class IdeaDetailsViewModel
    {
        public Idea Idea { get; set; }
        public bool IsInterested { get; set; }
        public bool VisitedByFounder { get; set; } 
        public TransactionDto TransactionDto { get; set; }
    }
}