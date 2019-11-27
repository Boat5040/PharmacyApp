using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PharmacyApp.ViewModels
{
    public class SecurityPolicyViewModel
    {
        [Display(Name = "Require Digit")]
        public bool RequireDigit { get; set; }

        [Display(Name = "Require Uppercase")]
        public bool RequireUppercase { get; set; }

        [Display(Name = "Require Lowercase")]
        public bool RequireLowercase { get; set; }

        [Display(Name = "Require Special Character")]
        public bool RequireSpecialCharacter { get; set; }

        [RequiredIfTrue("RequireSpecialCharacter")]
        [Display(Name = "Special Characters")]
        public string SpecialCharacters { get; set; }

        [Display(Name = "Min Pass Length")]
        public int MinimumPasswordLength { get; set; }

        [Display(Name = "Max Pass Length")]
        public int MaximumPasswordLength { get; set; }


    }
}