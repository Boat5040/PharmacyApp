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
    public class TaxSettingCollection:BaseEntityCollection, IEntityCollection<TaxViewModel>
    {
        private int _taxId;
        public TaxSettingCollection(ApplicationDbContext context, int taxId) : base(context)
        {
            _taxId = taxId;
        }

        public IList<TaxViewModel> GetEntityData(int startIndex, int pageSize, ReadOnlyCollection<SortedColumn> sortedColumns, out int totalRecordCount, out int searchRecordCount, string searchString)
        {
            var taxes = DataContext.TaxSettings.Where(x => x.TaxId == _taxId);

            totalRecordCount = taxes.Count();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                taxes = taxes.Where(x => x.Name.ToLower().Contains(searchString.ToLower()));
            }

            searchRecordCount = taxes.Count();

            IOrderedEnumerable<TaxSetting> sortedCollection = null;
            foreach (var sortedColumn in sortedColumns)
            {
                switch (sortedColumn.PropertyName)
                {
                    case "Name":
                        sortedCollection = sortedCollection == null ? taxes.CustomSort(sortedColumn.Direction, x => x.Name)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.Name);
                        break;
                    case "Percentge":
                        sortedCollection = sortedCollection == null ? taxes.CustomSort(sortedColumn.Direction, x => x.Percentage)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.Percentage);
                        break;
                    case "status":
                        sortedCollection = sortedCollection == null ? taxes.CustomSort(sortedColumn.Direction, x => x.Status)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.Status);
                        break;

                }
            }

            return sortedCollection.Skip(startIndex).Take(pageSize)
                .Select(x => new TaxViewModel
                {
                    TaxId = x.TaxId,
                    Name = x.Name,
                    Percentage= x.Percentage,
                    Status=x.Status
                }).ToList();

        }
    }
}