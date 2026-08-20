using Microsoft.AspNetCore.Mvc;

namespace Revival.Controllers;

public class TeamController : Controller
{
    public IActionResult Index() => View();
}
