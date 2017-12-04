using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShareDrive.Controllers
{
    public class DriveController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}