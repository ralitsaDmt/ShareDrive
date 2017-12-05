using Microsoft.AspNetCore.Mvc;
using ShareDrive.Models;
using ShareDrive.Services.Contracts;
using ShareDrive.ViewModels.CarViewModels;
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
            var tempPath = Path.GetTempPath();

            Car car = new Car()
            {
                CarModel = model.CarModel,
                Brand = model.Brand,
                Year = model.Year,
                HasAirConditioner = model.HasAirConditioner
            };

            using (var ms = new MemoryStream())
            {
                await model.Image.CopyToAsync(ms);
                car.Image = ms.ToArray();
            }

            this.carsService.Create(car);

            return this.View();
        }  
        
        [HttpGet]
        public IActionResult Delete(int id)
        {
            return this.PartialView("_DeleteConfirmation");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            return null;
        }
    }
}
