using Microsoft.AspNetCore.Mvc;
using ShareDrive.Services.Contracts;
using ShareDrive.ViewModels.Admin.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareDrive.Areas.Admin.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public IActionResult Index()
        {
            IEnumerable<ShareDrive.ViewModels.Admin.User.IndexViewModel> users = this.usersService.GetAllAdmin();
            return this.View(users);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            DetailsViewModel model = this.usersService.GetDetailsModel(id);
            return this.PartialView("_Details", model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            // bool result = this.usersService.Delete(id);

            bool result = false;
            
            if (result)
            {
                this.ViewData["SuccessMessage"] = "User successfully dleted.";
            }
            else
            {
                this.ViewData["ErrorMessage"] = "Cannot delete this user.";
            }
            
            return this.RedirectToIndex();
        }

        private IActionResult RedirectToIndex()
        {
            var users = this.usersService.GetAllAdmin();
            return this.PartialView("_IndexPartial", users);
        }
    }
}
