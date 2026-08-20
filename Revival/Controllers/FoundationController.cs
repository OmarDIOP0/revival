using Microsoft.AspNetCore.Mvc;

namespace Revival.Controllers;

public class FoundationController : Controller
{
    public IActionResult Index() => View();
}
