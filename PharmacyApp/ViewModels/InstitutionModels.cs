using PharmacyApp.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PharmacyApp.ViewModels
{
    public class InstitutionViewModel
    {
        [Key]
        [Display(Name = "Actions")]
        public int InstitutionId { get; set; }

        [Display(Name = "Institution Name")]
        public string Name { get; set; }

        public byte[] LogoImage { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }


        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Display(Name = "Email Address")]
        public string Email { get; set; }

    }

    public class NewInstitutionViewModel
    {
        [Required(AllowEmptyStrings = false)]
        [Remote(InstitutionControllerRoute.GetVerifyInstitutionName, HttpMethod = "POST", ErrorMessage = "Institution already exist", AdditionalFields = "__RequestVerificationToken")]
        public string Name { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please enter a valid phone number")]
        public string Phone { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter prompt email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Intitution Logo")]
        public HttpPostedFileBase File { get; set; }

    }

    public class EditInstitutionViewModel
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int InstitutionId { get; set; }

        public string Name { get; set; }

        public byte[] LogoImage { get; set; }

        public string Title { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please enter a valid phone number")]
        public string Phone { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter prompt email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
    }

}