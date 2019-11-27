using PharmacyApp.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace PharmacyApp.Areas.Administration.Models
{
    public class AdminAccountViewModel
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
        public string Institution { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }
    }

    public class NewAdminAccountViewModel
    {
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please select an institution")]
        [Display(Name = "Institution")]
        public int? InstitutionId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Username")]
        [Remote(AdminAccountControllerRoute.GetVerifyUserName, HttpMethod = "POST", ErrorMessage = "Username already exist", AdditionalFields = "__RequestVerificationToken")]
        public string UserName { get; set; }

        [Display(Name = "Profile Photo")]
        public HttpPostedFileBase File { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class EditAdminAccountViewModel
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

        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public byte[] UserImage { get; set; }
        public string Institution { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Display(Name = "Account Disabled?")]
        public bool IsDisabled { get; set; }
    }
}