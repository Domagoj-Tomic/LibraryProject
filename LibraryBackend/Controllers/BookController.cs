using LibraryBackend.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LibraryBackend.Controllers
{
	public class BookController : GenericController<Book>
	{
		public BookController() : base(new LibraryEntities())
		{
		}

		[HttpPost]
		[Route("api/books/uploadCoverImage/{id}")]
		public IHttpActionResult UploadCoverImage(int id, [FromBody] byte[] image)
		{
			var book = _dbSet.Find(id);
			if (book == null)
			{
				return NotFound();
			}

			book.CoverImage = image;
			_context.SaveChanges();

			return Ok();
		}

		[HttpGet]
		[Route("api/books/getCoverImage/{id}")]
		public HttpResponseMessage GetCoverImage(int id)
		{
			var book = _dbSet.Find(id);
			if (book == null || book.CoverImage == null)
			{
				return Request.CreateResponse(HttpStatusCode.NotFound);
			}

			var result = new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new ByteArrayContent(book.CoverImage)
			};
			result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
			return result;
		}
	}
}