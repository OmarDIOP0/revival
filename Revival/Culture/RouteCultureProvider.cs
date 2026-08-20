using Microsoft.AspNetCore.Localization;

namespace Revival.Culture;

/// <summary>
/// Reads the culture from the {culture} route segment (e.g. /fr/contact) and, when found,
/// persists it as the user's manual choice so bare URLs later resolve to the same language.
/// </summary>
public class RouteCultureProvider : IRequestCultureProvider
{
    public Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
    {
        if (httpContext.GetRouteValue("culture") is not string culture ||
            (culture != "en" && culture != "fr"))
        {
            return Task.FromResult<ProviderCultureResult?>(null);
        }

        httpContext.Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true });

        return Task.FromResult<ProviderCultureResult?>(new ProviderCultureResult(culture));
    }
}
