using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LibraryBackend.Models;

public class BookController : ApiController
{
    private LibraryEntities db = new LibraryEntities();

    // GET: api/Book
    public IQueryable<Book> GetBooks()
    {
        return db.Book;
    }

    // GET: api/Book/5
    public IHttpActionResult GetBook(int id)
    {
        Book book = db.Book.Find(id);
        if (book == null)
        {
            return NotFound();
        }

        return Ok(book);
    }

    // PUT: api/Book/5
    public IHttpActionResult PutBook(int id, Book book)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != book.BookID)
        {
            return BadRequest();
        }

        db.Entry(book).State = EntityState.Modified;

        try
        {
            db.SaveChanges();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BookExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return StatusCode(HttpStatusCode.NoContent);
    }

    // POST: api/Book
    public IHttpActionResult PostBook(Book book)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        db.Book.Add(book);
        db.SaveChanges();

        return CreatedAtRoute("DefaultApi", new { id = book.BookID }, book);
    }

    // DELETE: api/Book/5
    public IHttpActionResult DeleteBook(int id)
    {
        Book book = db.Book.Find(id);
        if (book == null)
        {
            return NotFound();
        }

        db.Book.Remove(book);
        db.SaveChanges();

        return Ok(book);
    }

    private bool BookExists(int id)
    {
        return db.Book.Count(e => e.BookID == id) > 0;
    }
}
   