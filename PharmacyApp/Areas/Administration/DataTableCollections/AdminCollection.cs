using jQuery.DataTables.Mvc;
using PharmacyApp.Areas.Administration.Models;
using PharmacyApp.Constants;
using PharmacyApp.DAL;
using PharmacyApp.DataTableCollections;
using PharmacyApp.jQuery.DataTables.Mvc;
using PharmacyApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace PharmacyApp.Areas.Administration.DataTableCollections
{
    public class AdminCollection : BaseEntityCollection, IEntityCollection<AdminAccountViewModel>
    {
        private int _currentUserId;
        public AdminCollection(ApplicationDbContext context, int currentUserId) : base(context)
        {
            _currentUserId = currentUserId;
        }

        public IList<AdminAccountViewModel> GetEntityData(int startIndex, int pageSize, ReadOnlyCollection<SortedColumn> sortedColumns, out int totalRecordCount, out int searchRecordCount, string searchString)
        {
            int adminRoleId = DataContext.Roles.FirstOrDefault(m => m.Name.Equals(PharmacyUserRoles.Administrator)).Id;
            var user = DataContext.Users.Where(m => m.Status != UserStatus.Deleted && m.Id != _currentUserId && m.Roles.Any(c => c.RoleId == adminRoleId));

            totalRecordCount = user.Count();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                user = user.Where(m => m.FirstName.ToLower().Contains(searchString.ToLower()) ||
                m.LastName.ToLower().Contains(searchString.ToLower()) ||
                m.Institution.Name.ToLower().Contains(searchString.ToLower()) ||
                m.UserName.ToLower().Contains(searchString.ToLower()) ||
                m.Email.ToLower().Contains(searchString.ToLower()));
            }

            searchRecordCount = user.Count();
            IOrderedEnumerable<ApplicationUser> sortedCollection = null;

            foreach (var sortedColumn in sortedColumns)
            {
                switch (sortedColumn.PropertyName)
                {
                    case "FirstName":
                        sortedCollection = sortedCollection == null ? user.CustomSort(sortedColumn.Direction, c => c.FirstName)
                            : sortedCollection.CustomSort(sortedColumn.Direction, c => c.FirstName);
                        break;
                    case "LastName":
                        sortedCollection = sortedCollection == null ? user.CustomSort(sortedColumn.Direction, c => c.LastName)
                            : sortedCollection.CustomSort(sortedColumn.Direction, c => c.LastName);
                        break;

                    case "UserName":
                        sortedCollection = sortedCollection == null ? user.CustomSort(sortedColumn.Direction, x => x.UserName)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.UserName);
                        break;
                    case "Email":
                        sortedCollection = sortedCollection == null ? user.CustomSort(sortedColumn.Direction, x => x.Email)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.Email);
                        break;
                    case "Institution":
                        sortedCollection = sortedCollection == null ? user.CustomSort(sortedColumn.Direction, x => x.Institution.Name)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.Institution.Name);
                        break;

                }
            }

            return sortedCollection.Skip(startIndex).Take(pageSize)
                .Select(m => new AdminAccountViewModel
                {
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    Status = m.Status.ToString(),
                    UserId = m.Id,
                    UserName = m.UserName,
                    Email = m.Email,
                    Institution = m.Institution.Name
                }).ToList();
        }

    }
}