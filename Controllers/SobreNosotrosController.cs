using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PadelWebXerez.Controllers
{
    public class SobreNosotrosController : Controller
    {
        public IActionResult SobreNosotros()
        {
            return View();
        }
    }
}
