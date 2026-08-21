using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Revival.Models;
using Revival.Services;

namespace Revival.Controllers;

public class ContactController(IEmailSender emailSender) : Controller
{
    private static readonly Regex EmailPattern = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    [HttpGet]
    public IActionResult Index()
    {
        var model = new ContactPageModel { Submitted = TempData["ContactSent"] is true };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ContactFormModel form)
    {
        var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

        if (!string.IsNullOrWhiteSpace(form.Website))
        {
            // Honeypot tripped: behave as if it succeeded, send nothing.
            TempData["ContactSent"] = true;
            return RedirectToRoute("localized", new { culture, controller = "Contact", action = "Index" });
        }

        var errors = Validate(form);
        if (errors.Count > 0)
        {
            return View(new ContactPageModel { Form = form, FieldErrors = errors });
        }

        var subject = $"Revival Medical Link — nouveau message ({form.Reason})";
        var body = $"Nom : {form.Name}\nTéléphone : {form.Phone}\nEmail : {form.Email}\nMotif : {form.Reason}\n\n{form.Message}";
        await emailSender.SendAsync(subject, body);

        TempData["ContactSent"] = true;
        return RedirectToRoute("localized", new { culture, controller = "Contact", action = "Index" });
    }

    private static HashSet<string> Validate(ContactFormModel form)
    {
        var errors = new HashSet<string>();
        if (string.IsNullOrWhiteSpace(form.Name)) errors.Add("Name");
        if (string.IsNullOrWhiteSpace(form.Phone)) errors.Add("Phone");
        if (string.IsNullOrWhiteSpace(form.Email) || !EmailPattern.IsMatch(form.Email)) errors.Add("Email");
        if (form.Reason is not ("new_patient" or "holiday_dialysis" or "other")) errors.Add("Reason");
        if (string.IsNullOrWhiteSpace(form.Message)) errors.Add("Message");
        return errors;
    }
}
