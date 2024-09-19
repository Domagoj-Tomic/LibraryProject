using LibraryShared.Models;

namespace LibraryBackend.Controllers
{
	public class UserController : GenericController<User>
	{
		public UserController() : base(new LibraryEntities())
		{
		}
	}
}