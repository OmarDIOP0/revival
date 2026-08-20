using Microsoft.AspNetCore.Mvc;

namespace Revival.Controllers;

public class CenterController : Controller
{
    public IActionResult Index() => View();
}
