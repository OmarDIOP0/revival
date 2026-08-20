using Microsoft.AspNetCore.Mvc;

namespace Revival.Controllers;

public class CareController : Controller
{
    public IActionResult Index() => View();
}
