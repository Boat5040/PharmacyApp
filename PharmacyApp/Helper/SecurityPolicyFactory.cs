using PharmacyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace PharmacyApp.Helper
{
    public class SecurityPolicyFactory
    {
        public static bool EnforcePolicy(string password, SecurityPolicy policy)
        {
            StringBuilder regexBuilder = new StringBuilder("^.*");

            string regex = "";

            if (policy.MinimumPasswordLength > 0)
            {
                regexBuilder.AppendFormat("(?=.{{{0},}})", policy.MinimumPasswordLength);
                regex += "(?=^.{" + policy.MinimumPasswordLength + ",}$)";
            }
            if (policy.RequireDigit)
            {
                regex += @"((?=.*\d)|(?=.*\W+))(?![.\n])";
                regexBuilder.Append(@"(?=(.*\d))");
            }
            if (policy.RequireUppercase)
            {
                regex += "(?=.*[A-Z])";
                regexBuilder.Append("(?=(.*[A-Z]))");
            }
            if (policy.RequireLowercase)
            {
                regex += "(?=.*[a-z])";
                regexBuilder.Append("(?=(.*[a-z])");
            }
            if (policy.RequireSpecialCharacter)
            {
                regex += ("(?=(.*[" + policy.SpecialCharacters + "]))");
                regexBuilder.Append("(?=(.*[{" + policy.SpecialCharacters + "}]))");
            }

            regexBuilder.Append(".*$");
            regex += ".*$";
            var reg = new Regex(regex);
            var result = reg.IsMatch(password) &&
                password.Length <= policy.MaximumPasswordLength;

            return result;
        }

        public static string GenerateValidationMessage(SecurityPolicy policy)
        {
            string message = "Password";
            if (policy.RequireDigit)
                message += " must be alphanumeric";
            if (policy.RequireUppercase)
                message += " must contain at least one Uppercase character";
            if (policy.RequireLowercase)
                message += " must contain at least one Lowercase character";
            if (policy.RequireSpecialCharacter)
                message += " must contain one of these special character(s) (" + policy.SpecialCharacters + ")";

            message += " must be at least " + policy.MinimumPasswordLength.ToString() + " characters long.";
            message += " must not be more than " + policy.MaximumPasswordLength.ToString() + " characters long.";

            return message;
        }

        public static bool? IsUserExpired(ApplicationUser user, int validityPeriod, int delinquentPeriod)
        {
            if (user.LastPasswordChangedDate.HasValue)
            {
                int days = (DateTime.Now.Date - user.LastPasswordChangedDate.Value).Days;

                if (days >= validityPeriod) // x days validity period
                {
                    return true;
                }
                else if (days >= (validityPeriod - delinquentPeriod)) // check delinquent period (y days)
                {
                    return false;
                }
            }

            return null;
        }

    }
}