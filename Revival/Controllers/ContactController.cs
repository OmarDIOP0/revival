using Microsoft.AspNetCore.Mvc;

namespace Revival.Controllers;

public class ContactController : Controller
{
    public IActionResult Index() => View();
}
