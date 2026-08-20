using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Revival.Configuration;
using Revival.Culture;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

builder.Services.Configure<SiteSettings>(builder.Configuration.GetSection("Site"));
builder.Services.Configure<List<SocialLink>>(builder.Configuration.GetSection("SocialLinks"));
builder.Services.Configure<List<TeamMember>>(builder.Configuration.GetSection("Team"));
builder.Services.Configure<FoundationSettings>(builder.Configuration.GetSection("Foundation"));

var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("fr") };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    RequestCultureProviders = new IRequestCultureProvider[]
    {
        new RouteCultureProvider(),
        new CookieRequestCultureProvider(),
        new AcceptLanguageHeaderRequestCultureProvider()
    }
};

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Bare URLs (no /en or /fr segment) are redirected once to the detected culture so every
// page the user lands on carries a culture prefix. Static assets are left untouched.
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value ?? "/";
    var hasCulturePrefix = path.StartsWith("/en/", StringComparison.Ordinal) || path is "/en"
        || path.StartsWith("/fr/", StringComparison.Ordinal) || path is "/fr";
    var looksLikeAsset = path.Contains('.') || path.StartsWith("/lib/", StringComparison.Ordinal);

    if (!hasCulturePrefix && !looksLikeAsset)
    {
        var culture = DetectCulture(context);
        var target = "/" + culture + (path == "/" ? "" : path) + context.Request.QueryString;
        context.Response.Redirect(target);
        return;
    }

    await next();
});

app.UseRouting();
app.UseRequestLocalization(localizationOptions);
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "localized",
    pattern: "{culture:regex(^(en|fr)$)}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

static string DetectCulture(HttpContext context)
{
    var cookie = context.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
    if (cookie is not null)
    {
        var parsed = CookieRequestCultureProvider.ParseCookieValue(cookie);
        var fromCookie = parsed?.Cultures.FirstOrDefault().Value;
        if (fromCookie is "en" or "fr")
        {
            return fromCookie;
        }
    }

    var acceptLanguage = context.Request.Headers.AcceptLanguage.ToString();
    if (acceptLanguage.Contains("fr", StringComparison.OrdinalIgnoreCase))
    {
        return "fr";
    }

    return "en";
}
