using jQuery.DataTables.Mvc;
using Microsoft.AspNet.Identity;
using PharmacyApp.Areas.Administration.DataTableCollections;
using PharmacyApp.Areas.Administration.Models;
using PharmacyApp.Constants;
using PharmacyApp.Controllers;
using PharmacyApp.DAL;
using PharmacyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PharmacyApp.Areas.Administration.Controllers
{
    [Authorize(Roles = PharmacyUserRoles.Administrator)]
    public class BranchController : BaseController
    {

        public BranchController()
        {

        }

        public BranchController(ApplicationUserManager userManager) : base(userManager, context: new ApplicationDbContext())
        {

        }
        [Route("branches", Name = BranchControllerRoute.GetIndex)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("branches")]
        public ActionResult Index(JQueryDataTablesModel viewModel)
        {
            int totalRecordCount;
            int searchRecordCount;

            var branches = new BranchCollection(DataContext, CurrentUser.InstitutionId.Value)
                .GetEntityData(startIndex: viewModel.iDisplayStart,
                pageSize: viewModel.iDisplayLength, sortedColumns: viewModel.GetSortedColumns(),
                totalRecordCount: out totalRecordCount, searchRecordCount: out searchRecordCount,
                searchString: viewModel.sSearch);

            return Json(new JQueryDataTablesResponse<BranchViewModel>(items: branches,
                totalRecords: totalRecordCount,
                totalDisplayRecords: searchRecordCount,
                sEcho: viewModel.sEcho));
        }

        [Route("new-branch", Name = BranchControllerRoute.GetCreate)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("new-branch")]
        public async Task<ActionResult> Create(NewBranchViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var branch = new Branch
                {
                    CreatedBy = User.Identity.GetUserId<int>(),
                    CreatedDate = DateTime.Now,
                    Name = viewModel.Name,
                    Phone = viewModel.Phone,
                    InstitutionId = CurrentUser.InstitutionId.Value
                };

                DataContext.Branches.Add(branch);
                await DataContext.SaveChangesAsync();
                return RedirectToAction(BranchControllerAction.Index);
            }

            return View(viewModel);
        }

        [Route("update-branch", Name = BranchControllerRoute.GetEdit)]
        public async Task<ActionResult> Edit(int? branchId = null)
        {
            if (branchId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var branch = await DataContext.Branches.FindAsync(branchId);

            if (branch == null)
                return new HttpNotFoundResult();

            if (branch.InstitutionId != CurrentUser.InstitutionId)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            return View(new EditBranchViewModel
            {
                BranchId = branch.BranchId,
                Name = branch.Name,
                Phone = branch.Phone
            });

        }

        [HttpPost]
        [Route("update-branch")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditBranchViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var branch = await DataContext.Branches.FindAsync(viewModel.BranchId);

                if (branch == null)
                    return new HttpNotFoundResult();

                if (branch.InstitutionId != CurrentUser.InstitutionId)
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

                branch.Name = viewModel.Name;
                branch.Phone = viewModel.Phone;
                branch.ModifiedBy = User.Identity.GetUserId<int>();
                branch.ModifiedDate = DateTime.Now;

                await DataContext.SaveChangesAsync();
                return RedirectToAction(BranchControllerAction.Index);
            }

            return View(viewModel);
        }


        [Route("delete-branch", Name = BranchControllerRoute.GetDelete)]
        public async Task<ActionResult> Delete(int? branchId = null)
        {
            if (branchId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var branch = await DataContext.Branches.FindAsync(branchId);

            if (branch == null)
                return new HttpNotFoundResult();

            if (branch.InstitutionId != CurrentUser.InstitutionId)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            return View(new BranchViewModel
            {
                BranchId = branch.BranchId,
                Name = branch.Name,
                Phone = branch.Phone
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        [Route("delete-branch")]
        public async Task<ActionResult> DeleteConfirmed(int? branchId = null)
        {
            if (branchId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var branch = await DataContext.Branches.FindAsync(branchId);

            if (branch == null)
                return new HttpNotFoundResult();

            if (branch.InstitutionId != CurrentUser.InstitutionId)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            DataContext.Branches.Remove(branch);
            await DataContext.SaveChangesAsync();
            return RedirectToAction(BranchControllerAction.Index);
        }

    }
}