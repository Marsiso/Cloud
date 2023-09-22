namespace Cloud.Web.Controllers;

using System.Security.Claims;
using Cloud.Application.ViewModels;
using Cloud.Domain.Application.DataTransferObjects;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class AccountController : ControllerBase
{
    [HttpPost("~/account/sign-in")]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginForm? loginForm)
    {
        var claims = new List<Claim>();

        if (loginForm is { Email: not null })
        {
            var emailClaim = new Claim(ClaimTypes.Email, loginForm.Email);

            claims.Add(emailClaim);
        }

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await this.HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));

        return this.LocalRedirect(Routes.Home);
    }

    [HttpPost("~/account/sign-out")]
    public async Task<IActionResult> Logout()
    {
        await this.HttpContext.SignOutAsync();

        return this.LocalRedirect(Routes.Home);
    }
}
