using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ShareDrive.ViewModels.Drive;
using Microsoft.AspNetCore.Http;
using ShareDrive.Helpers;
using ShareDrive.Services.Contracts;
using ShareDrive.ViewModels.Car;

namespace ShareDrive.Controllers
{
    public class DriveController : Controller
    {
        private int userId;
        private readonly IDrivesService drivesService;
        private readonly IDriveHelperService driveHelperService;
        private readonly ICarsService carsService;

        public DriveController(IHttpContextAccessor contextAccessor, IDrivesService drivesService, IDriveHelperService driveHelperService, ICarsService carsService)
        {
            this.drivesService = drivesService;
            this.driveHelperService = driveHelperService;
            this.SetUserId(contextAccessor);
            this.carsService = carsService;
        }

        public IActionResult Index()
        {
            // TODO: Select all drives
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            List<SelectViewModel> carsSelect = this.carsService.GetSelectionList();

            if (carsSelect.Count == 0)
            {
                // TODO: Notify that the drive cannot be created

            }

            if (carsSelect.Count > 1)
            {
                carsSelect.Insert(0, new SelectViewModel(0, "Select"));
            }           

            CreateViewModel model = new CreateViewModel()
            {
                Cars = carsSelect
            };

            return this.PartialView("_CreatePartial", model);
        }

        [HttpPost]
        public IActionResult Create(CreateViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                this.driveHelperService.ProcessCreateDrive(model, this.userId);
            }

            return this.RedirectToIndex();
        }

        private void SetUserId(IHttpContextAccessor contextAccessor)
        {
            this.userId = IdentityHelper.GetUserId(contextAccessor);
        }

        private IActionResult RedirectToIndex()
        {
            // TODO: Get all drives as IndexViewModel

            return this.View("Index");
        }
    }
}