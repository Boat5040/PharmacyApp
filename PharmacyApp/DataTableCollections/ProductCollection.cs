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

namespace PharmacyApp.DataTableCollections
{
    public class ProductCollection: BaseEntityCollection, IEntityCollection<ProductViewModel>
    {
        private int _institutionId;
        public ProductCollection(ApplicationDbContext context, int institutionId) : base(context)
        {
            _institutionId = institutionId;
        }

        public IList<ProductViewModel> GetEntityData(int startIndex, int pageSize, ReadOnlyCollection<SortedColumn> sortedColumns, out int totalRecordCount, out int searchRecordCount, string searchString)
        {
            var products = DataContext.Products.Where(x => x.InstitutionId == _institutionId);

            totalRecordCount = products.Count();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                products = products.Where(x => x.Name.ToLower().Contains(searchString.ToLower()) ||
                x.DrugCategory.Name.ToLower().Contains(searchString.ToLower()) ||
                x.GenericName.ToLower().Contains(searchString.ToLower()) ||
                x.CompanyName.ToLower().Contains(searchString.ToLower())
                );
            }

            searchRecordCount = products.Count();

            IOrderedEnumerable<Product> sortedCollection = null;
            foreach (var sortedColumn in sortedColumns)
            {
                switch (sortedColumn.PropertyName)
                {
                    case "Name":
                        sortedCollection = sortedCollection == null ? products.CustomSort(sortedColumn.Direction, x => x.Name)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.Name);
                        break;
                    case "Category":
                        sortedCollection = sortedCollection == null ? products.CustomSort(sortedColumn.Direction, x => x.CategoryId)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.CategoryId);
                        break;
                    case "CompanyName":
                        sortedCollection = sortedCollection == null ? products.CustomSort(sortedColumn.Direction, x => x.CompanyName)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.CompanyName);
                        break;
                    case "GenericName":
                        sortedCollection = sortedCollection == null ? products.CustomSort(sortedColumn.Direction, x => x.GenericName)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.GenericName);
                        break;
                    case "Effect":
                        sortedCollection = sortedCollection == null ? products.CustomSort(sortedColumn.Direction, x => x.Effect)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.Effect);
                        break;
                    case "PurchasePrice":
                        sortedCollection = sortedCollection == null ? products.CustomSort(sortedColumn.Direction, x => x.PurchasedPrice)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.PurchasedPrice);
                        break;
                    case "SellingPrice":
                        sortedCollection = sortedCollection == null ? products.CustomSort(sortedColumn.Direction, x => x.SellingPrice)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.SellingPrice);
                        break;
                    case "MufDate":
                        sortedCollection = sortedCollection == null ? products.CustomSort(sortedColumn.Direction, x => x.MufDate)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.MufDate);
                        break;
                    case "ExpiredDate":
                        sortedCollection = sortedCollection == null ? products.CustomSort(sortedColumn.Direction, x => x.ExpiredDate)
                            : sortedCollection.CustomSort(sortedColumn.Direction, x => x.ExpiredDate);
                        break;




                }
            }

            return sortedCollection.Skip(startIndex).Take(pageSize)
                .Select(x => new ProductViewModel
                {
                    ProductId = x.ProductId,
                    Name = x.Name,
                    CategoryId = x.CategoryId.Value,
                    CompanyName = x.CompanyName,
                    Effect = x.Effect,
                    GenericName = x.GenericName,
                    PurchasedPrice = x.PurchasedPrice,
                    SellingPrice = x.SellingPrice,
                    MufDate = x.MufDate.Value,
                    ExpiredDate = x.ExpiredDate.Value
                }).ToList();

        }
    }
}