using AuthenticationSystem.Client.Controllers;
using AuthenticationSystem.Client.Models;
using AuthenticationSystem.Client.Models.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserAuthenticationSystem.Client.Controllers
{
    public class AccountController : BaseController
    {
        private readonly AuthenticationSystem.Client.Services.IAuthorizationService _authorizationService;

        public AccountController(AuthenticationSystem.Client.Services.IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            return View("Index");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromForm] LoginViewModel loginInfo)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            var authResult = await _authorizationService.LoginAsync(loginInfo);

            if (authResult.IsFailed)
            {
                return View("Index");
            }

            return Redirect("/Home/Index");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromForm] RegistrationViewModel registrationInfo)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            var authResult = await _authorizationService.RegisterAsync(registrationInfo);

            if (authResult.IsFailed)
            {
                return View("Index");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> LogoutAsync()
        {
            var authResult = await _authorizationService.LogoutAsync();
            
            if (authResult.IsFailed)
            {
                return View("Error", new ErrorViewModel());
            }

            return Redirect("/Account/Index");
        }
    }
}
