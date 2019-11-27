using jQuery.DataTables.Mvc;
using Microsoft.AspNet.Identity;
using PharmacyApp.Constants;
using PharmacyApp.DAL;
using PharmacyApp.DataTableCollections;
using PharmacyApp.Models;
using PharmacyApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PharmacyApp.Controllers
{
    public class ProductController : BaseController
    {
        public ProductController() { }

        public ProductController(ApplicationUserManager userManager) : base(userManager, context: new ApplicationDbContext())
        {

        }

        [Route("products", Name = ProductControllerRoute.GetIndex)]
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        [Route("products")]
        public ActionResult Index(JQueryDataTablesModel viewModel)
        {
            int totalRecordCount;
            int searchRecordCount;

            var products = new ProductCollection(DataContext, CurrentUser.InstitutionId.Value)
            .GetEntityData(startIndex: viewModel.iDisplayStart,
            pageSize: viewModel.iDisplayLength, sortedColumns: viewModel.GetSortedColumns(),
            totalRecordCount: out totalRecordCount, searchRecordCount: out searchRecordCount,
            searchString: viewModel.sSearch);


            return Json(new JQueryDataTablesResponse<ProductViewModel>(items: products,
            totalRecords: totalRecordCount,
            totalDisplayRecords: searchRecordCount,
            sEcho: viewModel.sEcho));

        }

        [Route("new-product", Name = ProductControllerRoute.GetCreate)]
        public async Task<ActionResult> Create()
        {
            ViewBag.products = new SelectList(await Task.Run(() => DataContext.DrugCategories.ToList()), "CategoryId", "Name");

            return View();
        }

        [HttpPost]
        [Route("new-product")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(NewProductViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                byte[] uploadedFile = new byte[viewModel.File.InputStream.Length];
                viewModel.File.InputStream.Read(uploadedFile, 0, uploadedFile.Length);

                var product = new Product
                {
                    InstitutionId = CurrentUser.InstitutionId.Value,
                    Name = viewModel.Name,
                    CategoryId = viewModel.CategoryId,
                    PurchasedPrice = viewModel.PurchasedPrice,
                    SellingPrice = viewModel.SellingPrice,
                    Quantity = viewModel.Quantity,
                    GenericName = viewModel.GenericName,
                    CompanyName = viewModel.CompanyName,
                    DrugImage = uploadedFile,
                    Effect = viewModel.Effect,
                    MufDate = viewModel.MufDate,
                    ExpiredDate = viewModel.ExpiredDate,
                    CreatedBy = User.Identity.GetUserId<int>(),
                    CreatedDate = DateTime.Now

                };
                DataContext.Products.Add(product);
                await DataContext.SaveChangesAsync();
                return RedirectToAction(ProductControllerAction.Index);
            }
            return View(viewModel);

        }

        [Route("update-product", Name = ProductControllerRoute.GetEdit)]
        public async Task<ActionResult> Edit(int? productId = null)
        {
            if (productId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var product = await DataContext.Products.FindAsync(productId);

            if (product == null)
                return new HttpNotFoundResult();

            if (product.InstitutionId != CurrentUser.InstitutionId)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            return View(new EditProductViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                CategoryId = product.CategoryId.Value,
                CompanyName = product.CompanyName,
                Effect = product.Effect,
                GenericName = product.GenericName,
                Quantity =product.Quantity.Value,
                PurchasedPrice = product.PurchasedPrice,
                SellingPrice = product.SellingPrice,
                MufDate = product.MufDate.Value,
                ExpiredDate = product.ExpiredDate.Value
            });

        }

        [HttpPost]
        [Route("update-product")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditProductViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var product = await DataContext.Products.FindAsync(viewModel.ProductId);

                if (product == null)
                    return new HttpNotFoundResult();

                if (product.InstitutionId != CurrentUser.InstitutionId)
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

                product.Name = viewModel.Name;
                product.CategoryId = viewModel.CategoryId;
                product.CompanyName = viewModel.CompanyName;
                product.GenericName = viewModel.GenericName;
                product.Quantity = viewModel.Quantity;
                product.PurchasedPrice = viewModel.PurchasedPrice;
                product.SellingPrice = viewModel.SellingPrice;
                product.Effect = viewModel.Effect;
                product.ExpiredDate = viewModel.ExpiredDate;
                product.MufDate = viewModel.MufDate;

                await DataContext.SaveChangesAsync();
                return RedirectToAction(ProductControllerAction.Index);
            }
            return View(viewModel);
        }

        [Route("delete-product", Name = ProductControllerRoute.GetDelete)]
        public async Task<ActionResult> Delete(int? productId = null)
        {
            if (productId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var product = await DataContext.Products.FindAsync(productId);

            if (product == null)
                return new HttpNotFoundResult();

            if (product.InstitutionId != CurrentUser.InstitutionId)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            return View(new ProductViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                CategoryId = product.CategoryId.Value,
                CompanyName = product.CompanyName,
                Effect = product.Effect,
                GenericName = product.GenericName,
                Quantity = product.Quantity.Value,
                PurchasedPrice = product.PurchasedPrice,
                SellingPrice = product.SellingPrice,
                MufDate = product.MufDate.Value,
                ExpiredDate = product.ExpiredDate.Value
            });

        }

        [Route("delete-product")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> DeleteConfirmed(int? productId = null)
        {
            if (productId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var product = await DataContext.Products.FindAsync(productId);

            if (product == null)
                return new HttpNotFoundResult();

            if (product.InstitutionId != CurrentUser.InstitutionId)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            DataContext.Products.Remove(product);
            await DataContext.SaveChangesAsync();

            return RedirectToAction(ProductControllerAction.Index);

        }





    }
}