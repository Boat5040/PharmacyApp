using PharmacyApp.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PharmacyApp.Areas.Administration.Models
{
    public class SuperAdminViewModel
    {
        [Key]
        [Display(Name = "Actions")]
        public int UserId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public string Institution { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

    }

    public class NewSuperAdminViewModel
    {
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "User name")]
        [Remote(SuperAdminAccountControllerRoute.GetVerifyUserName, HttpMethod = "POST", ErrorMessage = "Username already exist", AdditionalFields = "__RequestVerificationToken")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [System.Web.Mvc.Remote(SuperAdminAccountControllerRoute.GetValidatePassword, HttpMethod = "POST", ErrorMessage = "Invalid password", AdditionalFields = "__RequestVerificationToken")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Profile Photo")]
        public HttpPostedFileBase File { get; set; }

    }

    public class EditSuperAdminViewModel
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int UserId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "Account Disabled?")]
        public bool IsDisabled { get; set; }

    }

    public class ResetSuperAdminViewModel
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}