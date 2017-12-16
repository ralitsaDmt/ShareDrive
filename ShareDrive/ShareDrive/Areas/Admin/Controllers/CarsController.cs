using Microsoft.AspNetCore.Mvc;
using ShareDrive.Services.Contracts;
using ShareDrive.ViewModels.Admin.Car;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareDrive.Areas.Admin.Controllers
{
    public class CarsController : BaseController
    {
        private readonly ICarsService carsService;

        public CarsController(ICarsService carsService)
        {
            this.carsService = carsService;
        }

        public IActionResult Index()
        {
            IEnumerable<IndexViewModel> cars = this.carsService.GetAllAdmin();
            return this.View("CarsIndex", cars);
        }
        
        [HttpGet]
        public IActionResult Details(int id)
        {
            var model = this.carsService.GetDetailsAdmin(id);
            return this.PartialView("_CarDetails", model);
        }
        

        [HttpPost]
        public IActionResult Delete(int id)
        {
            //bool result = this.carsService.Delete(id);
            bool result = true;

            if (result)
            {
                this.ViewData["SuccessMessage"] = "Car successfully deleted.";
            } 
            else
            {
                this.ViewData["ErrorMessage"] = "Cannot delete this car.";
            }
            
            IEnumerable<IndexViewModel> cars = this.carsService.GetAllAdmin();
            return this.PartialView("_CarsIndexPartial", cars);
        }
    }
}
