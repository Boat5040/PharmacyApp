using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PharmacyApp.Models
{
    [Serializable]

    public class SecurityPolicy
    {
        public bool RequireDigit { get; set; }
        public bool RequireUppercase { get; set; }
        public bool RequireLowercase { get; set; }
        public bool RequireSpecialCharacter { get; set; }
        public string SpecialCharacters { get; set; }
        public int MinimumPasswordLength { get; set; }
        public int MaximumPasswordLength { get; set; }

    }
}