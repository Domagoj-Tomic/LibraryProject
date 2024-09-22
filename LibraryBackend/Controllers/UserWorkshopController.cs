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

		// POST: api/UserWorkshop
		[HttpPost]
		[Route("api/UserWorkshop")]
		public override IHttpActionResult Post(UserWorkshop entity)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			// Check if the UserWorkshop record already exists
			var existingUserWorkshop = _context.Set<UserWorkshop>()
				.FirstOrDefault(uw => uw.UserID == entity.UserID && uw.WorkshopID == entity.WorkshopID);

			if (existingUserWorkshop != null)
			{
				return Conflict(); // Return a conflict response
			}

			_dbSet.Add(entity);
			_context.SaveChanges();

			var key = GetPrimaryKeyValue(entity);
			return CreatedAtRoute("DefaultApi", new { id = key }, entity);
		}

		// GET: api/UserWorkshop/{workshopId}/Users
		[HttpGet]
		[Route("api/UserWorkshop/{workshopId}/Users")]
		public IHttpActionResult GetUsersByWorkshop(int workshopId)
		{
			var users = from uw in _context.Set<UserWorkshop>()
						where uw.WorkshopID == workshopId
						select new
						{
							uw.User.UserID,
							uw.User.FirstName,
							uw.User.LastName,
							uw.User.Email,
							uw.User.Username,
							uw.User.PhoneNumber
						};

			return Ok(users.ToList());
		}

		// GET: api/UserWorkshop/{userId}/Workshops
		[HttpGet]
		[Route("api/UserWorkshop/{userId}/Workshops")]
		public IHttpActionResult GetWorkshopsByUser(int userId)
		{
			var workshops = from uw in _context.Set<UserWorkshop>()
							where uw.UserID == userId
							select new
							{
								uw.Workshop.WorkshopID,
								uw.Workshop.Name,
								uw.Workshop.NumberOfAttendees,
								uw.Workshop.DurationMinutes,
								uw.Workshop.StartDate,
								uw.Workshop.NumberOfTerms,
								uw.Workshop.Monday,
								uw.Workshop.Tuesday,
								uw.Workshop.Wednesday,
								uw.Workshop.Thursday,
								uw.Workshop.Friday
							};

			return Ok(workshops.ToList());
		}
	}
}
