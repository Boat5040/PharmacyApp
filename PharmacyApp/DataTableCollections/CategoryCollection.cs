using PharmacyApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jQuery.DataTables.Mvc;
using System.Collections.ObjectModel;
using PharmacyApp.DAL;
using PharmacyApp.Models;
using PharmacyApp.jQuery.DataTables.Mvc;

namespace PharmacyApp.DataTableCollections
{
    public class CategoryCollection : BaseEntityCollection, IEntityCollection<CategoryViewModel>
    {
        private int _institutionId;
        public CategoryCollection(ApplicationDbContext context, int institutionId) : base(context)
        {
            _institutionId = institutionId;
        }

        public IList<CategoryViewModel> GetEntityData(int startIndex, int pageSize, ReadOnlyCollection<SortedColumn> sortedColumns, out int totalRecordCount, out int searchRecordCount, string searchString)
        {
            var categories = DataContext.DrugCategories.Where(x => x.InstitutionId == _institutionId);

            totalRecordCount = categories.Count();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                categories = categories.Where(x => x.Name.ToLower().Contains(searchString.ToLower()));
            }

            searchRecordCount = categories.Count();

            IOrderedEnumerable<DrugCategory> sortedCollection = null;
            foreach (var sortedColumn in sortedColumns)
            {
                switch (sortedColumn.PropertyName)
                {
                    case "Name":
                        sortedCollection = sortedCollection == null ? categories.CustomSort(sortedColumn.Direction, x => x.Name)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.Name);
                        break;
                }
            }

            return sortedCollection.Skip(startIndex).Take(pageSize)
                .Select(x => new CategoryViewModel
                {
                    CategoryId = x.CategoryId,
                    Name = x.Name,
                    Description = x.Description
                }).ToList();

        }
    }
}