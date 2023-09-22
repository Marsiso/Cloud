namespace Cloud.Web.Controllers;

using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[Controller]/[Action]")]
public class CultureController : ControllerBase
{
    public IActionResult SetCulture(string? culture, string? redirectUri)
    {
        if (!string.IsNullOrWhiteSpace(culture))
        {
            var cultureInfo = new RequestCulture(culture);
            this.HttpContext.Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(cultureInfo));
        }

        return this.LocalRedirect(redirectUri ?? "/");
    }
}
