using DatabaseLibrary.Models;
using DatabaseLibrary.DBContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TestTask.Controllers
{
    
    [Route("[controller]")]
    [ApiController]
    public class AnonymousController: ControllerBase
    {
        private LibraryContext db;

        public AnonymousController(LibraryContext db)
        {
            this.db = db;
        }
        [HttpGet("AllBooks")]
        public ActionResult<IEnumerable<Book>> GetBooks()
        {
            var books = db.Books.ToList();
            return Ok(books);
        }
        [HttpGet("BookByTitle")]
        public ActionResult<Book> GetBook(string title)
        {
            var book = db.Books.Where(x => x.Name.Contains(title));
            if (book==null)
            {
                return NotFound("There is no book with that name");
            }
            return Ok(book);
        }
    }
}
