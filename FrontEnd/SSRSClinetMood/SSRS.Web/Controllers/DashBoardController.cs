﻿using Microsoft.AspNetCore.Mvc;

namespace SSRS.Web.Controllers
{
    public class DashBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
