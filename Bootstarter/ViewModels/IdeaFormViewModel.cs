using Bootstarter.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Bootstarter.ViewModels
{
    public class IdeaFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Upload File")]
        [Required(ErrorMessage = "Please choose file to upload.")]
        public string Image { get; set; }

        public decimal Goal { get; set; }
        
        [FutureDate]
        [Display(Name = "Start Date")]
        public string StartDate { get; set; }

        [FutureDate]
        [DateGreaterThan("StartDate")]
        [Display(Name = "End Date")]
        public string EndDate { get; set; }

        public string Heading { get; set; }

        public string Action
        {
            get
            {
                Expression<Func<IdeasController, ActionResult>> edit = (c => c.Edit(this.Id));

                Expression<Func<IdeasController, ActionResult>> create = (c => c.Create());

                var action = (Id != 0) ? edit : create;
                var actionName = (action.Body as MethodCallExpression).Method.Name;

                return actionName;
            }
        }

    }
}