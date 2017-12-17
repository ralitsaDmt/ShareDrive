using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ShareDrive.ViewModels.Drive;
using Microsoft.AspNetCore.Http;
using ShareDrive.Helpers;
using ShareDrive.Services.Contracts;
using ShareDrive.Infrastructure.Filters;
using Microsoft.AspNetCore.Identity;
using ShareDrive.Models;
using System.Threading.Tasks;

namespace ShareDrive.Controllers
{
    public class DriveController : Controller
    {
        private int userId;

        public DriveController(IHttpContextAccessor contextAccessor)
        {
            this.userId = IdentityHelper.GetUserId(contextAccessor);
        }

        [HttpGet]
        [Authorize]
        [AjaxOnly]
        public IActionResult Create([FromServices] IDriveCarsHelperService driveCarsHelperService,
            [FromServices] UserManager<ApplicationUser> userManager)
        {
            int userId = int.Parse(userManager.GetUserId(User));
            DriveCreateEditViewModel model = driveCarsHelperService.GetCreateViewModel(userId);
            return this.PartialView("_CreatePartial", model);
        }


        [HttpPost]
        [Authorize]
        [AjaxOnly]
        public async Task<IActionResult> Create(DriveCreateEditViewModel model, 
            [FromServices] IDriveHelperService driveHelperService, 
            [FromServices] UserManager<ApplicationUser> userManager, 
            [FromServices] IDrivesService drivesService, 
            [FromServices] ICarsService carsService)
        {
            int userId = int.Parse(userManager.GetUserId(User));
            if (this.ModelState.IsValid)
            {
                try
                {
                    await driveHelperService.ProcessCreateDriveAsync(model, userId);
                    return Json(new { success = "true", message = "Drive successfully created." });
                }
                catch (Exception ex)
                {                    
                    return Json(new { success = "false", message = "An error has occured." });
                }               
            }
            
            var cars = carsService.GetSelectionListByDriver(userId);
            model.Cars = cars;
            return this.PartialView("_CreatePartial", model);
        }

        [HttpGet]
        [Authorize]
        [AjaxOnly]
        public IActionResult Edit(int id, [FromServices] IDriveCarsHelperService driveCarsHelperService,
            [FromServices] UserManager<ApplicationUser> userManager)
        {
            int userId = int.Parse(userManager.GetUserId(User));
            DriveCreateEditViewModel model = driveCarsHelperService.GetEditViewModel(id, userId);

            return this.PartialView("_EditPartial", model);
        }

        [HttpPost]
        [Authorize]
        [AjaxOnly]
        public async Task<IActionResult> Edit(int id, 
            DriveCreateEditViewModel model, 
            [FromServices] IDriveHelperService driveHelperService, 
            [FromServices] IDrivesService drivesService,
            [FromServices] UserManager<ApplicationUser> userManager,
            [FromServices] ICarsService carsService)
        {
            int userId = int.Parse(userManager.GetUserId(User));
            if (this.ModelState.IsValid)
            {
                try
                {
                    await driveHelperService.ProcessEditDriveAsync(model, id);
                    return Json(new { success = "true", message = "Drive successfully edited." });
                }
                catch(Exception ex)
                {
                    return Json(new { success = "false", message = "An error has occured." });
                }
            }

            var cars = carsService.GetSelectionListByDriver(userId);
            model.Cars = cars;
            return this.PartialView("_EditPartial", model);
        }

        public IActionResult Index([FromServices] IDrivesService drivesService)
        {
            return this.RedirectToIndex(drivesService);
        }

        public IActionResult IndexPartial([FromServices] IDrivesService drivesService)
        {
            return this.RedirectToIndexPartial(drivesService);
        }

        private IActionResult RedirectToIndexPartial(IDrivesService drivesService)
        {
            IEnumerable<DriveIndexViewModel> drives = drivesService.GetAll();
            return this.PartialView("_IndexPartial", drives);
        }

        private IActionResult RedirectToIndex(IDrivesService drivesService)
        {
            IEnumerable<DriveIndexViewModel> drives = drivesService.GetAll();
            return this.View("Index", drives);
        }

        [HttpGet]
        [Authorize]
        [AjaxOnly]
        public IActionResult Delete(int id, [FromServices] IDrivesService drivesService)
        {
            DriveDeleteViewModel model = drivesService.GetDeleteModelById(id);
            return this.PartialView("_DeletePartial", model);
        }

        [HttpPost]
        [Authorize]
        [AjaxOnly]
        public IActionResult DeleteConfirm(int id, [FromServices] IDrivesService drivesService)
        {
            try
            {
                drivesService.Delete(id);
                return Json(new { success = "true", message = "Drive successfully deleted." });
            }
            catch(Exception ex)
            {
                return Json(new { success = "false", message = "An error has occured." });
            }
        }



        //[Authorize]
        //[AjaxOnly]
        //public IActionResult IndexFilter(string sort, string from, string to, string date, [FromServices] IDrivesService drivesService)
        //{
        //    var drives = drivesService.GetAll(sort, from, to, date);
        //    return this.PartialView("_Index", drives);
        //}








        //[HttpGet]
        //[Authorize]
        //[AjaxOnly]
        //public IActionResult Details(int id)
        //{
        //    DriveCollectionsViewModel model = this.drivesService.GetDetailsModel(id, this.userId);
        //    return this.PartialView("_Details", model);
        //}

        //[HttpPost]
        //[Authorize]
        //[AjaxOnly]
        //public JsonResult Reserve(int id)
        //{
        //    KeyValuePair<bool, string> result = this.drivesService.ReserveSeat(id, this.userId);
        //    return Json(new { result = result.Key, message = result.Value });
        //}

        //[HttpPost]
        //[Authorize]
        //[AjaxOnly]
        //public JsonResult Cancel(int id)
        //{
        //    KeyValuePair<bool, string> result = this.drivesService.CancelReservation(id, this.userId);
        //    return Json(new { result = result.Key, message = result.Value });
        //}




    }
}