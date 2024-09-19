using LibraryShared.Models;
using System;
using System.Diagnostics;
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
		[Route("api/Book/uploadCoverImage/{id}")]
		public IHttpActionResult UploadCoverImage(int id, [FromBody] ImageUploadModel model)
		{
			Debug.WriteLine($"Entering UploadCoverImage with id: {id}");

			var book = _dbSet.Find(id);
			if (book == null)
			{
				Debug.WriteLine("Book not found");
				return NotFound();
			}

			if (model == null || string.IsNullOrEmpty(model.Image))
			{
				Debug.WriteLine("Image is null or empty");
				return BadRequest("Image data is required");
			}

			try
			{
				book.CoverImage = Convert.FromBase64String(model.Image);
				_context.SaveChanges();
				Debug.WriteLine("Image saved successfully");
				return Ok();
			}
			catch (FormatException)
			{
				Debug.WriteLine("Invalid base64 string");
				return BadRequest("Invalid image data");
			}
		}

		public class ImageUploadModel
		{
			public string Image { get; set; }
		}

		[HttpGet]
		[Route("api/Book/getCoverImage/{id}")]
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