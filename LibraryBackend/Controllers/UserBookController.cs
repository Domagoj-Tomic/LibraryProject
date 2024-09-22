using LibraryShared.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;

namespace LibraryBackend.Controllers
{
	public class UserBookController : GenericController<UserBook>
	{
		public UserBookController() : base(new LibraryEntities())
		{
		}

		// POST: api/UserBook
		[HttpPost]
		[Route("api/UserBook")]
		public override IHttpActionResult Post(UserBook entity)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			// Check if the UserBook record already exists
			var existingUserBook = _context.Set<UserBook>()
				.FirstOrDefault(ub => ub.UserID == entity.UserID && ub.BookID == entity.BookID);

			if (existingUserBook != null)
			{
				return Conflict();
			}

			_dbSet.Add(entity);
			_context.SaveChanges();

			var key = GetPrimaryKeyValue(entity);
			return CreatedAtRoute("DefaultApi", new { id = key }, entity);
		}

		// GET: api/UserBook/{bookId}/Users
		[HttpGet]
		[Route("api/UserBook/{bookId}/Users")]
		public IHttpActionResult GetUsersByBook(int bookId)
		{
			var users = from ub in _context.Set<UserBook>()
						where ub.BookID == bookId
						select new
						{
							ub.User.UserID,
							ub.User.FirstName,
							ub.User.LastName,
							ub.User.Email,
							ub.User.Username,
							ub.User.PhoneNumber
						};

			return Ok(users.ToList());
		}

		// GET: api/UserBook/{userId}/Books
		[HttpGet]
		[Route("api/UserBook/{userId}/Books")]
		public IHttpActionResult GetBooksByUser(int userId)
		{
			var books = from ub in _context.Set<UserBook>()
						where ub.UserID == userId
						select new
						{
							ub.Book.BookID,
							ub.Book.Title,
							ub.Book.Author,
							ub.Book.ISBN,
							ub.Book.Category,
							ub.Book.BorrowingAllowed
						};

			return Ok(books.ToList());
		}
	}
}