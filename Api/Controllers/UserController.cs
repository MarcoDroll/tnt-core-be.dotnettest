using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetUserRoles")]
        [Authorize]
        public IEnumerable<dynamic> Get()
        {
            return User.Claims.Select(claim => new
            {
                RoleType = claim.Type,
                RoleValue = claim.Value
            }).ToArray();
        }
    }
}