using Microsoft.AspNetCore.Mvc;

namespace BikeRental.DDD.API.Controllers;

[ApiController]
[Route("{*url}", Order = int.MaxValue)]
public class FallbackController : Controller
{
    public ActionResult Index()
    {
        return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(),
            "wwwroot", "index.html"), "text/HTML");
    }
}