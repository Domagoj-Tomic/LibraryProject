using LibraryShared.Models;

namespace LibraryBackend.Controllers
{
	public class MemberController : GenericController<Member>
	{
		public MemberController() : base(new LibraryEntities())
		{
		}
	}
}
