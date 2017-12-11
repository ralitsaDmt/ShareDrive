using System;
using System.Collections.Generic;
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
            return this.RedirectToIndex();
        }
        
        public IActionResult IndexFilter(string sort, string from, string to, string date)
        {
            var drives = this.drivesService.GetAll(sort, from, to, date);
            return this.PartialView("_Index", drives);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            List<SelectViewModel> carsSelect = this.GetCarsSelectionList();
                        
            // TODO: Check if there аre cars and if no cars selected, return to index with error message

            EditViewModel model = new EditViewModel()
            {
                Cars = carsSelect
            };

            return this.PartialView("_EditPartial", model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EditViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                this.driveHelperService.ProcessCreateDrive(model, this.userId);

                ViewData["SuccessMessage"] = "Drive successfully created";
                return this.RedirectToIndex("");
            }
            
            return this.PartialView("_EditPartial", model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            EditViewModel model = this.drivesService.GetEditModelById(id);
            model.Cars = this.GetCarsSelectionList();

            return this.PartialView("_EditPartial", model);
        }

        [HttpPost]
        public IActionResult Edit(int? id, EditViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                if (id == 0)
                {
                    this.CreateDrive(model);
                }
                else
                {
                    this.UpdateDrive((int)id, model);
                }

                this.ViewData["SuccessMessage"] = "Drive successfully updated.";
                return this.RedirectToIndex();
            }

            this.ViewData["ErrorMessage"] = "Please, fill the form corectly";
            return this.PartialView("_EditPartial", model);
        }

        public IActionResult Delete(int id)
        {
            DeleteViewModel model = this.drivesService.GetDeleteModelById(id);
            return this.PartialView("_DeletePartial", model);
        }

        public IActionResult DeleteConfirm(int id)
        {
            this.drivesService.Delete(id);

            this.ViewData["SuccessMessage"] = "Drive successfully deleted";
            return this.RedirectToIndex();
        }

        public IActionResult Details(int id)
        {
            DetailsViewModel model = this.drivesService.GetDetailsModel(id);
            return this.PartialView("_Details", model);
        }

        private void UpdateDrive(int id, EditViewModel model)
        {
            this.driveHelperService.ProcessEditDrive(model, id);
        }

        private void CreateDrive(EditViewModel model)
        {
            this.driveHelperService.ProcessCreateDrive(model, this.userId);
            ViewData["SuccessMessage"] = "Drive successfully created";
        }

        private void SetUserId(IHttpContextAccessor contextAccessor)
        {
            if (this.User != null)
            {
                this.userId = IdentityHelper.GetUserId(contextAccessor);
            }            
        }

        private List<SelectViewModel> GetCarsSelectionList()
        {
            List<SelectViewModel> carsSelect = this.carsService.GetSelectionListByDriver(this.userId);

            if (carsSelect.Count > 1)
            {
                carsSelect.Insert(0, new SelectViewModel(0, "Select"));
            }

            return carsSelect;
        }

        private IActionResult RedirectToIndex(string sort = "date_desc")
        {
            List<IndexViewModel> drives = this.drivesService.GetAll(sort);
            
            return this.View("Index", drives);
        }
    }
}