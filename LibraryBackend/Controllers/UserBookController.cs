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

		// POST: api/UserBook/AddBookToUser
		[HttpPost]
		[Route("AddBookToUser")]
		public IHttpActionResult AddBookToUser(int userId, List<int> bookIds, DateTime borrowedDate, int numberOfDays)
		{
			foreach (var bookId in bookIds)
			{
				var userBook = new UserBook
				{
					UserID = userId,
					BookID = bookId,
					BorrowedDate = borrowedDate,
					NumberOfDays = numberOfDays
				};

				Post(userBook);
			}

			return Ok();
		}

		// GET: api/UserBook/Book/{bookId}/Users
		[HttpGet]
		[Route("Book/{bookId}/Users")]
		public IHttpActionResult GetUsersByBook(int bookId)
		{
			var users = from ub in _context.Set<UserBook>()
						where ub.BookID == bookId
						select ub.User;

			return Ok(users.ToList());
		}

		// GET: api/UserBook/User/{userId}/Books
		[HttpGet]
		[Route("User/{userId}/Books")]
		public IHttpActionResult GetBooksByUser(int userId)
		{
			var books = from ub in _context.Set<UserBook>()
						where ub.UserID == userId
						select ub.Book;

			return Ok(books.ToList());
		}
	}
}