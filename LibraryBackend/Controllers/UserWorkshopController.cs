using LibraryShared.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace LibraryBackend.Controllers
{
	public class UserWorkshopController : GenericController<UserWorkshop>
	{
		public UserWorkshopController() : base(new LibraryEntities())
		{
		}

		// POST: api/UserWorkshop/AddUserToWorkshop
		[HttpPost]
		[Route("AddUsersToWorkshop")]
		public IHttpActionResult AddUsersToWorkshop(int workshopId, List<int> userIds)
		{
			foreach (var userId in userIds)
			{
				var userWorkshop = new UserWorkshop
				{
					UserID = userId,
					WorkshopID = workshopId
				};

				Post(userWorkshop);
			}

			return Ok();
		}
	}
}
