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
    [Authorize(Roles = PharmacyUserRoles.SuperAdministrator)]
    public class InstitutionController : BaseController
    {
        public InstitutionController() :
            base(context: new ApplicationDbContext())
        {

        }
        [Route("institutions", Name = InstitutionControllerRoute.GetIndex)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("institutions")]
        public ActionResult Index(JQueryDataTablesModel viewModel)
        {
            int totalRecordCount;
            int searchRecordCount;

            var institutions = new InstitutionCollection(DataContext)
                .GetEntityData(startIndex: viewModel.iDisplayStart,
                pageSize: viewModel.iDisplayLength, sortedColumns: viewModel.GetSortedColumns(),
                totalRecordCount: out totalRecordCount, searchRecordCount: out searchRecordCount,
                searchString: viewModel.sSearch);

            return Json(new JQueryDataTablesResponse<InstitutionViewModel>(items: institutions,
                totalRecords: totalRecordCount,
                totalDisplayRecords: searchRecordCount,
                sEcho: viewModel.sEcho));

        }

        [Route("new-institution", Name = InstitutionControllerRoute.GetCreate)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("new-institution")]
        public async Task<ActionResult> Create(NewInstitutionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                byte[] uploadedFile = new byte[viewModel.File.InputStream.Length];
                viewModel.File.InputStream.Read(uploadedFile, 0, uploadedFile.Length);

                var institution = new Institution
                {
                    CreatedBy = User.Identity.GetUserId<int>(),
                    CreatedDate = DateTime.Now,
                    Email = viewModel.Email,
                    Name = viewModel.Name,
                    Title = viewModel.Title,
                    Phone = viewModel.Phone,
                    IntitutionLogo = uploadedFile


                };
                DataContext.Institutions.Add(institution);
                await DataContext.SaveChangesAsync();
                return RedirectToAction(InstitutionControllerAction.Index);
            }

            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("verify-institution-name", Name = InstitutionControllerRoute.GetVerifyInstitutionName)]
        public async Task<ActionResult> InstitutionNameExists(string name)
        {
            return Json(await Task.Run(() => DataContext.Institutions.FirstOrDefault(i => i.Name.ToLower().Equals(name.ToLower()))) == null);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Route("verify-institution-title", Name = InstitutionControllerRoute.GetVerifyInstitustionTitle)]
        //public async Task<ActionResult> InstitutionTitleExists(string name)
        //{
        //    return Json(await Task.Run(() => DataContext.Institutions.FirstOrDefault(i => i.Title.ToLower().Equals(name.ToLower()))) == null);
        //}

        [Route("update-institution", Name = InstitutionControllerRoute.GetEdit)]
        public async Task<ActionResult> Edit(int? institutionId = null)
        {
            if (institutionId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var institution = await DataContext.Institutions.FindAsync(institutionId);

            if (institution == null)
                return new HttpNotFoundResult();

            return View(new EditInstitutionViewModel
            {
                Email = institution.Email,
                InstitutionId = institution.InstitutionId,
                Name = institution.Name,
                Title = institution.Title,
                Phone = institution.Phone
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("update-institution")]
        public async Task<ActionResult> Edit(EditInstitutionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var institution = await DataContext.Institutions.FindAsync(viewModel.InstitutionId);

                if (institution == null)
                    return new HttpNotFoundResult();
                institution.Title = viewModel.Title;
                institution.Phone = viewModel.Phone;
                institution.ModifiedBy = User.Identity.GetUserId<int>();
                institution.ModifiedDate = DateTime.Now;
                institution.Email = viewModel.Email?.Trim();

                await DataContext.SaveChangesAsync();
                return RedirectToAction(InstitutionControllerAction.Index);
            }

            return View(viewModel);
        }

        [Route("delete-institution", Name = InstitutionControllerRoute.GetDelete)]
        public async Task<ActionResult> Delete(int? institutionId = null)
        {
            if (institutionId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var institution = await DataContext.Institutions.FindAsync(institutionId);

            if (institution == null)
                return new HttpNotFoundResult();

            return View(new InstitutionViewModel
            {
                Email = institution.Email,
                InstitutionId = institution.InstitutionId,
                Name = institution.Name,
                Title = institution.Title,
                Phone = institution.Phone
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("delete-institution")]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int? institutionId = null)
        {
            if (institutionId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var institution = await DataContext.Institutions.FindAsync(institutionId);

            if (institution == null)
                return new HttpNotFoundResult();

            DataContext.Institutions.Remove(institution);
            await DataContext.SaveChangesAsync();
            return RedirectToAction(InstitutionControllerAction.Index);
        }




    }
}