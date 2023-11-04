using AuthenticationSystem.Client.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace AuthenticationSystem.Client.Controllers
{
    public class BaseController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> AccessDeniedAsync()
        {
            return View("AccessDenied", new AccessDeniedViewModel(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Role).Value));
        }
    }
}
