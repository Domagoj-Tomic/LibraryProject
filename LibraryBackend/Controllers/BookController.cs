using LibraryBackend.Models;

namespace LibraryBackend.Controllers
{
	public class BookController : GenericController<Book>
	{
		public BookController() : base(new LibraryEntities())
		{
		}
	}
}