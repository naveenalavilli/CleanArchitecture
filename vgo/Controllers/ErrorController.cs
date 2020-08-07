using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace web.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeError(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    return View("404");
                default:
                    return View("Error");
            }
        }

        public IActionResult E404()
        {
            return View();
        }
    }
}