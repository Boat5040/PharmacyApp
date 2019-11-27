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
    [Authorize(Roles = PharmacyUserRoles.Administrator)]
    public class CategoryController : BaseController
    {
        public CategoryController() { }

        public CategoryController(ApplicationUserManager userManager) : base(userManager, context: new ApplicationDbContext())
        {

        }
        [Route("categories", Name = CategoryControllerRoute.GetIndex)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("categories")]
        public ActionResult Index(JQueryDataTablesModel viewModel)
        {
            int totalRecordCount;
            int searchRecordCount;

            var branches = new CategoryCollection(DataContext, CurrentUser.InstitutionId.Value)
            .GetEntityData(startIndex: viewModel.iDisplayStart,
            pageSize: viewModel.iDisplayLength, sortedColumns: viewModel.GetSortedColumns(),
            totalRecordCount: out totalRecordCount, searchRecordCount: out searchRecordCount,
            searchString: viewModel.sSearch);


            return Json(new JQueryDataTablesResponse<CategoryViewModel>(items: branches,
            totalRecords: totalRecordCount,
            totalDisplayRecords: searchRecordCount,
            sEcho: viewModel.sEcho));
        }

        [Route("new-category", Name = CategoryControllerRoute.GetCreate)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("new-category")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(NewCategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var category = new DrugCategory
                {
                    InstitutionId = CurrentUser.InstitutionId.Value,
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    CreatedBy = User.Identity.GetUserId<int>(),
                    CreatedDate = DateTime.Now,
                };
                DataContext.DrugCategories.Add(category);
                await DataContext.SaveChangesAsync();
                return RedirectToAction(CategoryControllerAction.Index);
            }

            return View(viewModel);
        }

        [Route("update-category", Name = CategoryControllerRoute.GetEdit)]
        public async Task<ActionResult> Edit(int? categoryId = null)
        {
            if (categoryId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var category = await DataContext.DrugCategories.FindAsync(categoryId);

            if (category == null)
                return new HttpNotFoundResult();

            if (category.InstitutionId != CurrentUser.InstitutionId)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            return View(new EditCategoryViewModel
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Description = category.Description
            });
        }

        [HttpPost]
        [Route("update-category")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditCategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var category = await DataContext.DrugCategories.FindAsync(viewModel.CategoryId);

                if (category == null)
                    return new HttpNotFoundResult();

                if (category.InstitutionId != CurrentUser.InstitutionId)
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

                category.Name = viewModel.Name;
                category.Description = viewModel.Description;
                category.ModifiedBy = User.Identity.GetUserId<int>();
                category.ModifiedDate = DateTime.Now;

                await DataContext.SaveChangesAsync();
                return RedirectToAction(CategoryControllerAction.Index);
            }
            return View(viewModel);
        }

        [Route("delete-category", Name = CategoryControllerRoute.GetDelete)]
        public async Task<ActionResult> Delete(int? categoryId = null)
        {
            if (categoryId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var category = await DataContext.DrugCategories.FindAsync(categoryId);

            if (category == null)
                return new HttpNotFoundResult();

            if (category.InstitutionId != CurrentUser.InstitutionId)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            return View(new CategoryViewModel
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Description = category.Description
            });

        }

        [Route("delete-category")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> DeleteConfirmed(int? categoryId = null)
        {
            if (categoryId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var category = await DataContext.DrugCategories.FindAsync(categoryId);

            if (category == null)
                return new HttpNotFoundResult();

            if (category.InstitutionId != CurrentUser.InstitutionId)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            DataContext.DrugCategories.Remove(category);
            await DataContext.SaveChangesAsync();

            return RedirectToAction(CategoryControllerAction.Index);
        }

    }
}