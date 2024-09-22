using LibraryShared.Models;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;

namespace LibraryBackend.Controllers
{
	public class UserWorkshopController : GenericController<UserWorkshop>
	{
		public UserWorkshopController() : base(new LibraryEntities())
		{
		}

		// POST: api/UserWorkshop/AddUserToWorkshop
		[HttpPost]
		[Route("AddUserToWorkshop")]
		public IHttpActionResult AddUserToWorkshop(int workshopId, List<int> userIds)
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

		// GET: api/UserWorkshop/Workshop/{workshopId}/Users
		[HttpGet]
		[Route("Workshop/{workshopId}/Users")]
		public IHttpActionResult GetUsersByWorkshop(int workshopId)
		{
			var users = from uw in _context.Set<UserWorkshop>()
						where uw.WorkshopID == workshopId
						select uw.User;

			return Ok(users.ToList());
		}

		// GET: api/UserWorkshop/User/{userId}/Workshops
		[HttpGet]
		[Route("User/{userId}/Workshops")]
		public IHttpActionResult GetWorkshopsByUser(int userId)
		{
			var workshops = from uw in _context.Set<UserWorkshop>()
							where uw.UserID == userId
							select uw.Workshop;

			return Ok(workshops.ToList());
		}
	}
}
