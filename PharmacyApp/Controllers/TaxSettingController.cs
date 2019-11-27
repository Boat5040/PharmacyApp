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

    public class TaxSettingController : BaseController
    {
        public TaxSettingController()
        {

        }

        public TaxSettingController(ApplicationUserManager userManager) : base(userManager, context: new ApplicationDbContext())
        {

        }
        [Route("tax", Name = TaxSettingControllerRoute.GetIndex)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("tax")]
        public ActionResult Index(JQueryDataTablesModel viewModel)
        {
            int totalRecordCount;
            int searchRecordCount;

            var taxes = new TaxSettingCollection(DataContext, CurrentUser.InstitutionId.Value)
                .GetEntityData(startIndex: viewModel.iDisplayStart,
                pageSize: viewModel.iDisplayLength, sortedColumns: viewModel.GetSortedColumns(),
                totalRecordCount: out totalRecordCount, searchRecordCount: out searchRecordCount,
                searchString: viewModel.sSearch);

            return Json(new JQueryDataTablesResponse<TaxViewModel>(items: taxes,
                totalRecords: totalRecordCount,
                totalDisplayRecords: searchRecordCount,
                sEcho: viewModel.sEcho));

        }

        [Route("new-tax", Name = TaxSettingControllerRoute.GetCreate)]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [Route("new-tax")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(NewTaxViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var tax = new TaxSetting
                {
                    InstitutionId = CurrentUser.InstitutionId.Value,
                    Name = viewModel.Name,
                    Percentage = viewModel.Percentage,
                    Status=viewModel.Status,
                    CreatedBy = User.Identity.GetUserId<int>(),
                    CreatedDate = DateTime.Now,
                };
                DataContext.TaxSettings.Add(tax);
                await DataContext.SaveChangesAsync();
                return RedirectToAction(TaxSettingControllerAction.Index);
            }

            return View(viewModel);
        }


        [Route("update-tax", Name = TaxSettingControllerRoute.GetEdit)]
        public async Task<ActionResult> Edit(int? taxId = null)
        {
            if (taxId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var tax = await DataContext.TaxSettings.FindAsync(taxId);

            if (tax == null)
                return new HttpNotFoundResult();

            if (tax.InstitutionId != CurrentUser.InstitutionId)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            return View(new EditTaxViewModel
            {
                TaxId = tax.TaxId,
                Name = tax.Name,
                Percentage = tax.Percentage,
                Status = tax.Status
            });
        }


        [HttpPost]
        [Route("update-tax")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditTaxViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var tax = await DataContext.TaxSettings.FindAsync(viewModel.TaxId);

                if (tax == null)
                    return new HttpNotFoundResult();

                if (tax.InstitutionId != CurrentUser.InstitutionId)
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

                tax.Name = viewModel.Name;
                tax.Percentage = viewModel.Percentage;
                tax.Status = viewModel.Status;
                tax.ModifiedBy = User.Identity.GetUserId<int>();
                tax.ModifiedDate = DateTime.Now;

                await DataContext.SaveChangesAsync();
                return RedirectToAction(TaxSettingControllerAction.Index);
            }
            return View(viewModel);
        }


        [Route("delete-tax", Name = TaxSettingControllerRoute.GetDelete)]
        public async Task<ActionResult> Delete(int? taxId = null)
        {
            if (taxId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var tax = await DataContext.TaxSettings.FindAsync(taxId);

            if (tax == null)
                return new HttpNotFoundResult();

            if (tax.InstitutionId != CurrentUser.InstitutionId)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            return View(new TaxViewModel
            {
                TaxId = tax.TaxId,
                Name = tax.Name,
                Percentage = tax.Percentage,
                Status = tax.Status
            });

        }


        [Route("delete-tax")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> DeleteConfirmed(int? taxId = null)
        {
            if (taxId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var tax = await DataContext.TaxSettings.FindAsync(taxId);

            if (tax == null)
                return new HttpNotFoundResult();

            if (tax.InstitutionId != CurrentUser.InstitutionId)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            DataContext.TaxSettings.Remove(tax);
            await DataContext.SaveChangesAsync();

            return RedirectToAction(TaxSettingControllerAction.Index);
        }


    }
}