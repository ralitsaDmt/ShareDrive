using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareDrive.Areas.Admin.Controllers
{
    public class Home : BaseController
    {
        public IActionResult Index => this.View();
    }
}
