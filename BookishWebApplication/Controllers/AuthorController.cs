using BookishWebApplication.Models.Database.Create;
using BookishWebApplication.Models.View;
using BookishWebApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookishWebApplication.Controllers
{
    [Route("/author")]
    public class AuthorController : Controller
    {
        private readonly IBooksService _booksService;
        private readonly IAuthorService _authorService;
        public AuthorController(IBooksService booksService, IAuthorService authorService)
        {
            _booksService = booksService;
            _authorService = authorService;
        }
        
        [HttpGet("")]
        public IActionResult ViewAllAuthorsPage()
        {
            var authors = _authorService.GetAllAuthors();
            var viewModel = new AuthorsViewModel {Authors = authors};
            return View(viewModel);
        }

        [HttpGet("{authorId}")]
        public IActionResult ViewAuthorPage(int authorId)
        {
            var allBooks = _booksService.GetAllBooks();
            var booksByAuthor = _booksService.GetBooksByAuthor(authorId);
            var author = _authorService.GetAuthor(authorId);
            var bookAuthor = new BookAuthor {AuthorId = authorId};
            var viewModel = new AuthorViewModel {Author = author, BooksByAuthor = booksByAuthor, AllBooks = allBooks, BookAuthor = bookAuthor};
            return View(viewModel);
        }
        
        [HttpGet("create")]
        public IActionResult CreateAuthorPage()
        {
            return View();
        }
        
        [HttpPost("create")]
        public IActionResult CreateAuthor(CreateAuthorModel newAuthor)
        {
            var authorId = _authorService.CreateAuthor(newAuthor);
            return RedirectToAction("ViewAuthorPage", new {authorid = authorId});
        }
        
        [HttpPost("delete")]
        public IActionResult DeleteAuthor(DeleteAuthorModel author)
        {
            _authorService.DeleteAuthor(author);
            return RedirectToAction("ViewAllAuthorsPage");
        }
        
        [HttpPost("create/addBookToAuthor")]
        public IActionResult AddBookToAuthor(BookAuthor bookAuthor)
        {
            _booksService.AddBookToAuthor(bookAuthor);
            return RedirectToAction("ViewAuthorPage", new { authorId = bookAuthor.AuthorId });
        }
        
        [HttpPost("delete/removeBookFromAuthor")]
        public IActionResult RemoveBookFromAuthor(BookAuthor bookAuthor)
        {
            _booksService.RemoveBookFromAuthor(bookAuthor);
            return RedirectToAction("ViewAuthorPage", new { authorId = bookAuthor.AuthorId });
        }
    }
}