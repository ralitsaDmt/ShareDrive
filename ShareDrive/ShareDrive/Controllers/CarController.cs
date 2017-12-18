namespace ShareDrive.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using ShareDrive.Exceptions.Car;
    using ShareDrive.Infrastructure.Filters;
    using ShareDrive.Models;
    using ShareDrive.Services.Contracts;
    using ShareDrive.ViewModels.CarViewModels;

    public class CarController : Controller
    {
        [HttpGet]
        public IActionResult Index([FromServices] ICarsService carsService,
            [FromServices] UserManager<ApplicationUser> userManager)
        {
            int userId = int.Parse(userManager.GetUserId(User));
            var cars = carsService.GetAllCarsIndex(userId);
            return this.View("Index", cars);
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult IndexPartial([FromServices] ICarsService carsService,
            [FromServices] UserManager<ApplicationUser> userManager)
        {
            int userId = int.Parse(userManager.GetUserId(User));
            var cars = carsService.GetAllCarsIndex(userId);
            return this.View("_IndexPartial", cars);
        }

        [HttpGet]
        [Authorize]
        [AjaxOnly]
        public IActionResult Create()
        {
            return this.PartialView("_CreateModal");
        }

        [HttpPost]
        [Authorize]
        [AjaxOnly]
        public async Task<IActionResult> Create(CarCreateViewModel model, 
            [FromServices] ICarsService carsService,
            [FromServices] UserManager<ApplicationUser> userManager)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    int userId = int.Parse(userManager.GetUserId(User));
                    await carsService.Create(model, userId);
                    return Json(new { success = "true", message = "Car successfully created." });
                }
                catch(CarCreateException ex)
                {
                    return Json(new { success = "false", message = ex.Message });
                }
            }

            return this.PartialView("_CreateModal", model);
        }
        
        [HttpGet]
        [Authorize]
        [AjaxOnly]
        public IActionResult Edit(int id, [FromServices] ICarsService carsService)
        {
            CarEditViewModel model = carsService.GetEditViewModel(id);

            if (model != null)
            {
                return this.PartialView("_EditModal", model);
            }

            return this.RedirectToAction("Error", "Home");
        }

        [HttpPost]
        [Authorize]
        [AjaxOnly]
        public async Task<IActionResult> Edit(int id, CarEditViewModel model,
            [FromServices] ICarsService carsService)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    await carsService.Edit(id, model);
                    return Json(new { success = "true", message = "Car successfully edited." });
                }
                catch (CarEditException ex)
                {
                    return Json(new { success = "false", message = ex.Message });
                }
            }

            return this.PartialView("_EditModal", model);
        }

        [HttpGet]
        [Authorize]
        [AjaxOnly]
        public IActionResult Delete(int id, [FromServices] ICarsService carsService)
        {
            CarDeleteViewModel model = carsService.GetDeleteViewModel(id);

            if (model != null)
            {
                return this.PartialView("_DeleteConfirmation", model);
            }

            return this.RedirectToAction("Error", "Home");
        }

        [HttpPost]
        [AjaxOnly]
        public IActionResult DeleteConfirm(int id, [FromServices] ICarsService carsService)
        {
            try
            {
                bool result = carsService.Delete(id);

                if (result)
                {
                    return Json(new { success = "true", message = "Car successfully deleted." });
                }
                else
                {
                    return this.RedirectToAction("Error", "Home");
                }
            }
            catch (CarDeleteException ex)
            {
                return Json(new { success = "false", message = ex.Message });
            }
        }
    }
}
