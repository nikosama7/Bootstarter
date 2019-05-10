using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Bootstarter.ViewModels
{
    public class CVVValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            Regex expression = new Regex(@"\d{3}");

            //Return if it was a match or not
            return expression.IsMatch(((int)value).ToString());
        }
    }
}