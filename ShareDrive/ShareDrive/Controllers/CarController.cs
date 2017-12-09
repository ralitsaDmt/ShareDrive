using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShareDrive.Helpers;
using ShareDrive.Models;
using ShareDrive.Services.Contracts;
using ShareDrive.ViewModels.CarViewModels;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShareDrive.Controllers
{
    public class CarController : Controller
    {   
        private readonly ICarsService carsService;

        private int userId;

        public CarController(IHttpContextAccessor contextAccessor, ICarsService carsService)
        {
            this.carsService = carsService;
            this.SetUserId(contextAccessor);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return this.RedirectToIndex();
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            return this.PartialView("_CreateModal");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                await this.carsService.Create(model, this.userId);
                return this.RedirectToIndex();
            }
            else
            {
                return this.PartialView("_CreateModal", model);
            }           
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
            if (this.ModelState.IsValid)
            {
                await this.carsService.Edit(id, model);
                return this.RedirectToIndex();
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

            this.carsService.Delete(id);
            return this.RedirectToIndex();
        }

        private void SetUserId(IHttpContextAccessor contextAccessor)
        {
            this.userId = IdentityHelper.GetUserId(contextAccessor);
        }

        private IActionResult RedirectToIndex()
        {
            var cars = this.carsService.GetAllCarsIndex().ToList();
            return this.View("Index", cars);
        }


    }
}
