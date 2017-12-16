using Microsoft.AspNetCore.Mvc;
using ShareDrive.Services.Contracts;
using ShareDrive.ViewModels.Admin.Drive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareDrive.Areas.Admin.Controllers
{
    public class DrivesController : BaseController
    {
        private readonly IDrivesService drivesService;

        public DrivesController(IDrivesService drivesService)
        {
            this.drivesService = drivesService;
        }

        public IActionResult Index()
        {
            IEnumerable<IndexViewModel> drives = this.drivesService.GetAllAdmin();
            return this.View("DrivesIndex", drives);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            DetailsViewModel model = this.drivesService.GetDetailsAdminModel(id);
            return this.PartialView("_DriveDetails", model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            //bool result = this.drivesService.Delete(id);

            bool result = true;

            if(result)
            {
                ViewData["SuccessMessage"] = "Drive deleted successfully";
            }
            else
            {
                ViewData["ErrorMessage"] = "Cannot delete this drive.WS";
            }
            

            IEnumerable<IndexViewModel> drives = this.drivesService.GetAllAdmin();
            return this.PartialView("_DrivesIndexPartial", drives);
        }
    }
}
