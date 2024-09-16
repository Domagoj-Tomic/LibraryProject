using System.Web.Http;
using LibraryBackend.Models;
using LibraryBackend.Repositories;

namespace LibraryBackend.Controllers
{
	public class LoginController : ApiController
	{
		private readonly UserRepository _userRepository;

		public LoginController()
		{
			_userRepository = new UserRepository(new LibraryEntities());
		}

		[HttpPost]
		[Route("api/login")]
		public IHttpActionResult Login(LoginModel login)
		{
			if (decimal.TryParse(login.Pin, out decimal pin))
			{
				var user = _userRepository.ValidateUser(login.Username, pin);
				if (user != null)
				{
					return Ok();
				}
				return Unauthorized();
			}
			return BadRequest("Invalid PIN format.");
		}
	}
}
