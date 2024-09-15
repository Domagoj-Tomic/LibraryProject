using LibraryBackend.Models;

namespace LibraryBackend.Controllers
{
	public class EmployeeController : GenericController<Employee>
	{
		public EmployeeController() : base(new LibraryEntities())
		{
		}
	}
}
