using LibraryShared.Models;

namespace LibraryBackend.Controllers
{
	public class WorkshopController : GenericController<Workshop>
	{
		public WorkshopController() : base(new LibraryEntities())
		{
		}
	}
}