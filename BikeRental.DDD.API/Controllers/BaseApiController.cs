using BikeRental.DDD.Domain.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BikeRental.DDD.API.Controllers;

[ServiceFilter(typeof(LogUserActivity))]
[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{

}
