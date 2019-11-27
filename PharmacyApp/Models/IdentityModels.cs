using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;
using PharmacyApp.DAL;

namespace PharmacyApp.Models
{
    public enum UserStatus { Active, Disabled, Expired, Deleted }

    [Table("User")]
    public class ApplicationUser : IdentityUser<int,ApplicationUserLogin,ApplicationUserRole,ApplicationUserClaim>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? InstitutionId { get; set; }
        public int? BranchId { get; set; }
        public bool ForceChangePassword { get; set; }
        public bool HasPasswordChange { get; set; }
        [Column(TypeName = "date")]
        public DateTime? LastPasswordChangedDate { get; set; }

        [Column(TypeName = "xml")]
        public string PasswordHistory { get; set; }

        public UserStatus Status { get; set; }

        [Column(TypeName = "image")]
        public byte[] UserImage { get; set; }
        [Required]
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public virtual Institution Institution { get; set; }
        


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser,int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    [Table("UserLogin")]
    public class ApplicationUserLogin : IdentityUserLogin<int> { }
    [Table("UserClaim")]
    public class ApplicationUserClaim : IdentityUserClaim<int> { }
    [Table("UserRole")]
    public class ApplicationUserRole : IdentityUserRole<int> { }

    [Table("Role")]
    public class ApplicationRole:IdentityRole<int,ApplicationUserRole>
    {
        public ApplicationRole() { }
        public ApplicationRole(string name)
        {
            Name = name;
        }
        public ApplicationRole(string name,string description)
        {
            Name = name;
            Description = description;
        }

        public string Description { get; set; }
    }


    public class ApplicationUserStore:UserStore<ApplicationUser,ApplicationRole,int,ApplicationUserLogin,ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationUserStore(ApplicationDbContext context):base(context)
        {

        }
    }

    public class ApplicationRoleStore:RoleStore<ApplicationRole,int,ApplicationUserRole>
    {
        public ApplicationRoleStore(ApplicationDbContext context):base(context)
        {

        }
    }
}