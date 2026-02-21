using Application.DTOs;
using Application.Features.User.Commands;
using Application.Features.User.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace MRP.Web.Controllers
{
    public class AccountController : Controller
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        public IActionResult Login(string returnUrl = null)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            TempData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginUserQuery loginUserQuery)
        {
            var response = await Mediator.Send(loginUserQuery);
            if (response.Succeeded)
            { 
                if (response.Message != null)
                {
                    await HttpContext.Session.LoadAsync();
                    await LoginProcess(response.Data);
                    await HttpContext.Session.CommitAsync();
                }
                else
                {
                    try
                    {
                        await LoginProcess(response.Data);

                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return Ok(response);

        }
        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserCommand command)
        {
            var response = await Mediator.Send(command);
            return Json(response);
        }
        private async Task LoginProcess(UserLoginDto user)
        {
            //await Mediator.Send(new LoginUserCommand(user));

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
            HttpContext.Session.SetString("currentUser",
                JsonSerializer.Serialize(user));
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Account");
        }
    }

}
