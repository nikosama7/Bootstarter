using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Bootstarter.ViewModels
{
    public class CreditCardValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            Regex expression = new Regex(@"^(\d{4}[- ]){3}\d{4}|\d{16}$");

            //Return if it was a match or not
            return expression.IsMatch((string)value);
        }
    }
}