using PharmacyApp.Constants;
using PharmacyApp.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PharmacyApp.Controllers
{
    public class POSController : BaseController
    {
        public POSController()
        {

        }

        public POSController(ApplicationUserManager userManager):base(userManager,context:new ApplicationDbContext())
        {

        }


        [Route("pos", Name = POSControllerRoute.GetIndex)]
        public ActionResult Index()
        {
            return View();
        }
    }
}