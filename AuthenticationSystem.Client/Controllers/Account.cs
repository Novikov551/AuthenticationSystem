using AuthenticationSystem.Client.Controllers;
using AuthenticationSystem.Client.Models;
using AuthenticationSystem.Client.Models.Login;
using AuthenticationSystem.Client.Services;
using AuthenticationSystem.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace UserAuthenticationSystem.Client.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAuthorizationService _authorizationService;

        public AccountController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromForm] LoginViewModel loginInfo)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            var authResult = await _authorizationService.LoginAsync(loginInfo.Email, loginInfo.Password);

            if (authResult.IsFailed)
            {
                return View("Index");
            }

            return RedirectToAction("Index", "Home");
        }

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
