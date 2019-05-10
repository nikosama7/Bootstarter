using Bootstarter.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bootstarter.ViewModels
{
    public enum ItemsPerPage
    {
        [Display(Name = "4")]
        p05 = 4,
        [Display(Name = "8")]
        p8 = 8,
        [Display(Name = "12")]
        p12 = 12
    }

    public class IdeaIndexViewModel
    {
        public IEnumerable<Idea> Ideas { get; set; }
        public string Search { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        [Display(Name = "Ideas per Page")]
        public ItemsPerPage ItemsPerPage { get; set; }
    }
}