using PharmacyApp.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace PharmacyApp.ViewModels
{
    public class InstitutionUserViewModel
    {
        [Key]
        [Display(Name = "Actions")]
        public int UserId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }
        public byte[] UserImage { get; set; }
        public string Email { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        public string Roles { get; set; }

    }

    public class NewInstitutionUserViewModel
    {
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public byte[] UserImage { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Username")]
        [Remote(InstitutionUserControllerRoute.GetVerifyUserName, HttpMethod = "POST", ErrorMessage = "Username already exist", AdditionalFields = "__RequestVerificationToken")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Branch")]
        public int? BranchId { get; set; }

        [Display(Name = "Profile Photo")]
        public HttpPostedFileBase File { get; set; }


        [Display(Name = "Roles")]
        [Required(ErrorMessage = "Please select at least one role")]
        public List<int> RoleIds { get; set; }

    }

    public class EditInstitutionUserViewModel
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int UserId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        public string UserName { get; set; }

        public byte[] UserImage { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Roles")]
        [Required(ErrorMessage = "Please select at least one role")]
        public List<int> RoleIds { get; set; }

        [Display(Name = "Account Disabled?")]
        public bool IsDisabled { get; set; }
    }

}