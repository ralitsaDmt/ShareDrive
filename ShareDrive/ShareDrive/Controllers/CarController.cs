using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using ShareDrive.Models;
using ShareDrive.Services.Contracts;
using ShareDrive.ViewModels.CarViewModels;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShareDrive.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarsService carsService;

        public CarController(ICarsService carsService)
        {
            this.carsService = carsService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var cars = this.carsService.GetAllCarsIndex().ToList();
            return this.View(cars);
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            return this.PartialView("_CreateModal");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            this.carsService.Create(model);

            var cars = this.carsService.GetAllCarsIndex().ToList();

            return this.View("Index", cars);
        }  
        
        [HttpGet]
        public IActionResult Edit(int id)
        {
            EditViewModel model = this.carsService.GetEditViewModel(id);
                        
            return this.PartialView("_EditModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditViewModel model)
        {
            bool result = await this.carsService.Edit(id, model);
            
            if (result)
            {
                var cars = this.carsService.GetAllCarsIndex().ToList();
                return this.View("Index", cars);
            }
            else
            {
                return this.PartialView("_EditModal", model);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            IQueryable<Car> car = this.carsService.GetById(id);
            DeleteViewModel model = car.ProjectTo<DeleteViewModel>().FirstOrDefault();

            return this.PartialView("_DeleteConfirmation", model);
        }

        [HttpPost]
        public IActionResult DeleteConfirm(int id)
        {
            bool result = this.carsService.Delete(id);
            if (result)
            {
                var cars = this.carsService.GetAllCarsIndex().ToList();
                return this.View("Index", cars);
            }
            else
            {
                return null;
            }
        }
    }
}
