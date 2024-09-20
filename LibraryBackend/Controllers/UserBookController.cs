using LibraryShared.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace LibraryBackend.Controllers
{
	public class UserBookController : GenericController<UserBook>
	{
		public UserBookController() : base(new LibraryEntities())
		{
		}

		// POST: api/UserBook/AddBookToUser
		[HttpPost]
		[Route("AddBooksToUser")]
		public IHttpActionResult AddBooksToUser(int userId, List<int> bookIds, DateTime borrowedDate, int numberOfDays)
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
	}
}