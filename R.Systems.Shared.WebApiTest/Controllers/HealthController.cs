using Microsoft.AspNetCore.Mvc;
using R.Systems.Shared.WebApi.Jwt;
using R.Systems.Shared.WebApiTest.Models.Responses;
using System.Security.Claims;

namespace R.Systems.Shared.WebApiTest.Controllers
{
    [ApiController, Route("health")]
    public class HealthController : ControllerBase
    {
        public HealthController(UserClaimsService userClaimsService)
        {
            UserClaimsService = userClaimsService;
        }

        public UserClaimsService UserClaimsService { get; }

        [HttpGet, Route("get-logged-user-id")]
        public IActionResult GetLoggedUserId()
        {
            long loggedUserId = UserClaimsService.GetUserId();
            return Ok(new GetLoggedUserIdResponse
            {
                LoggedUserId = loggedUserId
            });
        }

        [HttpGet, Route("get-roles")]
        public IActionResult GetLoggedUserRoles()
        {
            List<string> loggedUserRoles = UserClaimsService.GetClaim(ClaimTypes.Role);
            return Ok(new GetLoggedUserRoles
            {
                LoggedUserRoles = loggedUserRoles
            });
        }
    }
}
