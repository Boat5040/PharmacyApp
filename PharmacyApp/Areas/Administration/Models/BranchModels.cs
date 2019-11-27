using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PharmacyApp.Areas.Administration.Models
{
    public class BranchViewModel
    {
        [Key]
        [Display(Name = "Actions")]
        public int BranchId { get; set; }

        [Display(Name = "Branch Name")]
        public string Name { get; set; }

        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

    }

    public class NewBranchViewModel
    {
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Branch Name")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please enter a valid phone number")]
        public string Phone { get; set; }
    }

    public class EditBranchViewModel
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int BranchId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Branch Name")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please enter a valid phone number")]
        public string Phone { get; set; }
    }
}