using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPProject4.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASPProject4.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new List<UserModel>());
        }
    }
}