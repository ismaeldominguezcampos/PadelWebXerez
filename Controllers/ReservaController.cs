using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PadelWebXerez.Controllers
{
    public class ReservaController : Controller
    {
        public IActionResult Reserva()
        {
            return View();
        }
    }
}
