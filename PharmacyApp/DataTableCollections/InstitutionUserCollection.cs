using PharmacyApp.DAL;
using PharmacyApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jQuery.DataTables.Mvc;
using System.Collections.ObjectModel;
using PharmacyApp.Models;
using PharmacyApp.jQuery.DataTables.Mvc;
using PharmacyApp.Constants;

namespace PharmacyApp.DataTableCollections
{
    public class InstitutionUserCollection:BaseEntityCollection,IEntityCollection<InstitutionUserViewModel>
    {
        private int _currentUserId;

        public InstitutionUserCollection(ApplicationDbContext context, int currentUserId):base(context)
        {
            _currentUserId = currentUserId;
        }

        private string GetRoles(ApplicationUser user)
        {
            string roles = string.Empty;
            foreach (var role in user.Roles)
            {
                roles += DataContext.Roles.Find(role.RoleId).Name + ",";
            }

            return roles = roles.Substring(0, roles.Length - 2);
        }
        public IList<InstitutionUserViewModel> GetEntityData(int startIndex, int pageSize, ReadOnlyCollection<SortedColumn> sortedColumns, out int totalRecordCount, out int searchRecordCount, string searchString)
        {
            int superUserRoleId = DataContext.Roles.FirstOrDefault(x => x.Name.Equals(PharmacyUserRoles.SuperAdministrator)).Id;
            int institutionId = DataContext.Users.Find(_currentUserId).InstitutionId.Value;
            var users = DataContext.Users.Where(x => x.Status != UserStatus.Deleted &&
            x.Id != _currentUserId && x.InstitutionId == institutionId && x.Roles.All(y => y.RoleId != superUserRoleId));

            totalRecordCount = users.Count();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                users = users.Where(x => x.FirstName.ToLower().Contains(searchString.ToLower()) ||
                x.LastName.ToLower().Contains(searchString.ToLower())||
                x.UserName.ToLower().Contains(searchString.ToLower()) ||
                x.Email.ToLower().Contains(searchString.ToLower()));
            }

            searchRecordCount = users.Count();

            IOrderedEnumerable<ApplicationUser> sortedCollection = null;

            foreach (var sortedColumn in sortedColumns)
            {
                switch (sortedColumn.PropertyName)
                {
                    case "FirstName":
                        sortedCollection = sortedCollection == null ? users.CustomSort(sortedColumn.Direction, x => x.FirstName)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.FirstName);
                        break;
                    case "LastName":
                        sortedCollection = sortedCollection == null ? users.CustomSort(sortedColumn.Direction, x => x.LastName)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.LastName);
                        break;

                    case "UserName":
                        sortedCollection = sortedCollection == null ? users.CustomSort(sortedColumn.Direction, x => x.UserName)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.UserName);
                        break;
                    case "Email":
                        sortedCollection = sortedCollection == null ? users.CustomSort(sortedColumn.Direction, x => x.Email)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.Email);
                        break;
                }
            }

            return sortedCollection.Skip(startIndex).Take(pageSize)
                .Select(x => new InstitutionUserViewModel
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Status = x.Status.ToString(),
                    UserId = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                    Roles = GetRoles(x)
                }).ToList();
        }
    }
}