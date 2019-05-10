using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bootstarter.Models
{
    public class Interest
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; }  
        public ApplicationUser User { get; set; }
        
        [Required]
        public int IdeaId { get; set; }
        public Idea Idea { get; set; }

        public Interest(string userId, int ideaId) : this()
        {
            UserId = userId;
            IdeaId = ideaId;
        }

        protected Interest()
        {

        }

    }
}