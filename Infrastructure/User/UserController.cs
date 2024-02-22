using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.User
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
		private readonly UserContext userContext;

		public UserController(
			UserContext userContext
		)
		{
			this.userContext = userContext;
		}

		[HttpGet("currentData")]
		public ActionResult<UserContext> GetFiltered()
		{
			return Ok(userContext);
		}
	}
}
