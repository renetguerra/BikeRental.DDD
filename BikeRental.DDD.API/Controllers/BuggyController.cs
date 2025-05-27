using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BikeRental.DDD.Domain;
using BikeRental.DDD.Domain.Entities;

namespace BikeRental.DDD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class BuggyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BuggyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        [HttpGet("not-found")]
        public ActionResult<User> GetNotFound()
        {
            var thing = _unitOfWork.UserRepository.GetUserByIdAsync(-1).Result;

            if (thing == null) return NotFound();

            return thing;
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var thing = _unitOfWork.UserRepository.GetUserByIdAsync(-1).Result;

            var thingToReturn = thing.ToString();

            return thingToReturn;
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("This was not a good request");
        }
    }
}
