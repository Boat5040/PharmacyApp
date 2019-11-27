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
    public class SuperAdminCollection : BaseEntityCollection, IEntityCollection<SuperAdminViewModel>
    {
        private int _currentUserId;
        public SuperAdminCollection(ApplicationDbContext context, int currentUserId) : base(context)
        {
            _currentUserId = currentUserId;
        }

        public IList<SuperAdminViewModel> GetEntityData(int startIndex, int pageSize, ReadOnlyCollection<SortedColumn> sortedColumns, out int totalRecordCount, out int searchRecordCount, string searchString)
        {
            int SuperAdminRoleId = DataContext.Roles.FirstOrDefault(m => m.Name.Equals(PharmacyUserRoles.SuperAdministrator)).Id;
            var user = DataContext.Users.Where(m => m.Status != UserStatus.Deleted && m.Id != _currentUserId && m.Roles.Any(c => c.RoleId == SuperAdminRoleId));

            totalRecordCount = user.Count();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                user = user.Where(m => m.FirstName.ToLower().Contains(searchString.ToLower()) ||
                m.LastName.ToLower().Contains(searchString.ToLower()) ||
                m.UserName.ToLower().Contains(searchString.ToLower()));
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
                }
            }

            return sortedCollection.Skip(startIndex).Take(pageSize)
                .Select(m => new SuperAdminViewModel
                {
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    Status = m.Status.ToString(),
                    UserId = m.Id,
                    UserName = m.UserName
                }).ToList();
        }

    }
}