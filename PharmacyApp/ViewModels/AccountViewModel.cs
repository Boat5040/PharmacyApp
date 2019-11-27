using PharmacyApp.Constants;
using PharmacyApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PharmacyApp.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [StringLength(18, MinimumLength = 6)]
        //[RegularExpression(@"(?=^.{6,18}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\s).*$)")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,16}", ErrorMessage = "Password must contain at least an uppercase, lowercase, digit and a special character and must be 8-16 long.")]
        [System.Web.Mvc.Remote(AccountControllerRoute.GetValidatePassword, HttpMethod = "POST", ErrorMessage = "Invalid password", AdditionalFields = "__RequestVerificationToken")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }

    public class ProfileViewModel
    {
        public byte[] UserImage { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "Institution")]
        public string Institution { get; set; }

        [EnumDataType(typeof(UserStatus))]
        [Display(Name = "Status")]
        public UserStatus Status { get; set; }

        [Display(Name = "Roles")]
        public string Roles { get; set; }
    }
}