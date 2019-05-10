using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bootstarter.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string InspirationWord { get; set; }

        public DateTime CommentTime {get; set;}

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        public int IdeaId { get; set; }
        public Idea Idea { get; set; }
    }
}