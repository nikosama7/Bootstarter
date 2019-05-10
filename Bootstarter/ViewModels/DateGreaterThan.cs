using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bootstarter.ViewModels
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DateGreaterThan : ValidationAttribute
    {
        private string DateToCompareToFieldName { get; set; }

        public DateGreaterThan(string dateToCompareToFieldName)
        {
            DateToCompareToFieldName = dateToCompareToFieldName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty((string)value))
            {
                return new ValidationResult("No date imput");
            }

            if (string.IsNullOrEmpty((string)validationContext.ObjectType.GetProperty(DateToCompareToFieldName).GetValue(validationContext.ObjectInstance, null)))
            {
                return new ValidationResult("No date imput");
            }

            DateTime earlierDate = DateTime.Parse((string)validationContext.ObjectType.GetProperty(DateToCompareToFieldName).GetValue(validationContext.ObjectInstance, null));

            DateTime laterDate = DateTime.Parse((string)value);

            if (laterDate > earlierDate)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("End Date should be later than the Start Date");
            }
        }

    }
}