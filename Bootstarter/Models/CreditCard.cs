using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bootstarter.Models
{
    public enum CardType
    {
        Visa =1,
        MasterCard,
        Maestro
    }

    public enum Month
    {
        January = 1,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }

    public class CreditCard
    {
        [Required]
        [Display(Name ="Card Number")]
        public string CardNumber { get; set; }
        [Display(Name = "Card Type")]
        public CardType CardType { get; set; }
        public int CVV { get; set; }
        [Required]
        [Display(Name = "Owner Name")]
        public string CardOwner { get; set; }
        public Month Month { get; set; }
        public int Year { get; set; }
    }
}