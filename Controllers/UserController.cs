using DatabaseLibrary.Models;
using DatabaseLibrary.DBContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web.Http.Results;

namespace TestTask.Controllers
{
    [Authorize(Roles ="user")]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private LibraryContext db;

        public UserController(LibraryContext db)
        {
            this.db = db;
        }

        [HttpPost("AddBookList")]
        public async Task<ActionResult> AddUserBook(int idbook)
        {
            int count = 0;
            Book? book = await db.Books.FirstOrDefaultAsync(u => u.Id == idbook);           
            User? user = await db.Users.FirstOrDefaultAsync(u => u.Login == User.Identity.Name);
            if (book.IdUser != null && book.IdUser != user.Id)
            {
                return NotFound("This book someone took");
            }
            else if (book.IdUser != null && book.IdUser == user.Id)
            {
                return NotFound("This book you already taken");
            }
            if (user != null)
            {
                book.TakenDate = DateTime.Now;
                book.IdUser = user.Id;
            }
            
            foreach (var item in db.Books)
            {
                if (item.IdUser == user.Id)
                {                    
                        count++;                    
                }
                if (count > 3)
                {
                    return NotFound("User can have no more than 3 books");
                }
                else
                {
                    db.Update(book);
                }
            }
            db.SaveChanges();
            return Ok(book);
        }

        [HttpDelete("DelBookList")]
        public async Task<ActionResult> DelBook(int delBook)
        {
            Book? book = await db.Books.FirstOrDefaultAsync(u => u.Id == delBook);
            User? user = await db.Users.FirstOrDefaultAsync(u => u.Login == User.Identity.Name);
            if (book.IdUser == user.Id)
            {
                book.IdUser = null;
                book.TakenDate = null;
            }
            else
            {
                return NotFound(new { message = "Book is not find" });
            }

            db.Books.Update(book);
            db.SaveChanges();
            return Ok(book);
        }

        [HttpGet("User`sBooks")]
        public  ActionResult<IEnumerable<Book>> UserBooks()
        {
            User? user =  db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);

            IEnumerable<Book> memberModels = db.Books.Where(x=>x.IdUser==user.Id).ToList();
            return Ok(memberModels);
        }

        [HttpGet("User`sBook")]
        public async Task<ActionResult<Book>> UserBook(int idBook)
        {
            Book? book = await db.Books.FirstOrDefaultAsync(u => u.Id == idBook);
            User? user =  await db.Users.FirstOrDefaultAsync(u => u.Login == User.Identity.Name);
            if (book.IdUser == user.Id)
            {
                return Ok(book);
            }
            else
            {
                return NotFound();
            }            
        }

    }
}
