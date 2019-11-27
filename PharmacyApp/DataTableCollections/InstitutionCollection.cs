using jQuery.DataTables.Mvc;
using PharmacyApp.DAL;
using PharmacyApp.jQuery.DataTables.Mvc;
using PharmacyApp.Models;
using PharmacyApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace PharmacyApp.DataTableCollections
{
    public class InstitutionCollection : BaseEntityCollection, IEntityCollection<InstitutionViewModel>
    {
        public InstitutionCollection(ApplicationDbContext context):base(context)
        {

        }

        public IList<InstitutionViewModel> GetEntityData(int startIndex, int pageSize, ReadOnlyCollection<SortedColumn> sortedColumns, out int totalRecordCount, out int searchRecordCount, string searchString)
        {
            var institutions = DataContext.Institutions.AsQueryable();

            totalRecordCount = institutions.Count();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                institutions = institutions.Where(x => x.Name.ToLower().Contains(searchString.ToLower()) ||x.Title.StartsWith(searchString)||
                x.Phone.StartsWith(searchString) || x.Email.ToLower().Contains(searchString.ToLower()));
            }

            searchRecordCount = institutions.Count();

            IOrderedEnumerable<Institution> sortedCollection = null;
            foreach (var sortedColumn in sortedColumns)
            {
                switch (sortedColumn.PropertyName)
                {
                    case "Name":
                        sortedCollection = sortedCollection == null ? institutions.CustomSort(sortedColumn.Direction, x => x.Name)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.Name);
                        break;
                    case "Title":
                        sortedCollection = sortedCollection == null ? institutions.CustomSort(sortedColumn.Direction, x => x.Title)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.Title);
                        break;

                    case "Phone":
                        sortedCollection = sortedCollection == null ? institutions.CustomSort(sortedColumn.Direction, x => x.Phone)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.Phone);
                        break;
                    case "Email":
                        sortedCollection = sortedCollection == null ? institutions.CustomSort(sortedColumn.Direction, x => x.Email)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.Email);
                        break;
                }
            }

            return sortedCollection.Skip(startIndex).Take(pageSize)
                .Select(x => new InstitutionViewModel
                {
                    InstitutionId = x.InstitutionId,
                    Phone = x.Phone,
                    Name = x.Name,
                    Title =x.Title,
                    Email = x.Email
                }).ToList();
        }
    }
}