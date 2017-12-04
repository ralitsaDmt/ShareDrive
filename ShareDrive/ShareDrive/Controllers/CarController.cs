using Microsoft.AspNetCore.Mvc;
using ShareDrive.Models;
using ShareDrive.ViewModels.CarViewModels;
using System.IO;
using System.Threading.Tasks;

namespace ShareDrive.Controllers
{
    public class CarController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {

            return this.View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            var tempPath = Path.GetTempPath();

            foreach (var formFile in model.Images)
            {
                if (formFile.Length > 0)
                {
                    var image = new Image();
                    using (var stream = new MemoryStream())
                    {
                        await formFile.CopyToAsync(stream);
                        image.File = stream.ToArray();
                    }
                }
            }

            return null;
        }
    }
}
