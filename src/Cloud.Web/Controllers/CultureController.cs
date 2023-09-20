using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Cloud.Web.Controllers;

[ApiController]
[Route("[Controller]/[Action]")]
public class CultureController : ControllerBase
{
    public IActionResult SetCulture(string? culture, string? redirectUri)
    {
        if (!string.IsNullOrWhiteSpace(culture))
        {
            HttpContext.Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)));
        }

        return LocalRedirect(redirectUri ?? "/");
    }
}
