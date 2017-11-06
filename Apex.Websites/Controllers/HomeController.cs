using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Apex.Websites.Models;

namespace Apex.Websites.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
