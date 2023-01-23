using DatabaseLibrary.DBContext;
using DatabaseLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;

namespace TestTask.Controllers
{
    [Authorize(Roles = "admin")]
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private LibraryContext db;

        public AdminController(LibraryContext db)
        {
            this.db = db;
        }

        [HttpPost("AddBookLibrary")]
        public ActionResult AddBook( BookModel model)
        {
            Book book = new Book { Name = model.Name, Year = model.Year, Author = model.Author, Genre = model.Genre };
            db.Books.Add(book);
            db.SaveChanges();
            return Ok(book);
        }

        [HttpDelete("DelBookLibrary")]
        public ActionResult DeleteBook(int idBook)
        {
            Book? book = db.Books.FirstOrDefault(u => u.Id == idBook);

            if (book == null)
            {
                return NotFound(new { message = "Book is not find" });
            }

            db.Books.Remove(book);
            db.SaveChanges();
            return Ok(book);
        } 
        
        [HttpGet("InfoUser")]
        public ActionResult<Book> InfoUser(int iduser)
        {
            var books = db.Books.Where(x => x.IdUser == iduser && x.TakenDate < DateTime.Now.AddMonths(-1)).ToList();
            return Ok(books);
        }
    }
}
