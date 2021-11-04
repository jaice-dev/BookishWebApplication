using System;
using System.Threading.Tasks;
using BookishWebApplication.Models.Database;
using BookishWebApplication.Models.Database.Create;
using BookishWebApplication.Models.View;
using BookishWebApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookishWebApplication.Controllers
{
    [Route("/books")]
    public class BooksController : Controller
    {
        private readonly IBooksService _booksService;
        public BooksController(IBooksService booksService)
        {
            _booksService = booksService;
        }

        [HttpGet("")]
        public IActionResult ViewAllBooksPage()
        {
            var books = _booksService.GetAllBooks();
            Console.WriteLine(books);
            var viewModel = new BooksViewModel {Books = books};
            
            return View(viewModel);
        }
        
        [HttpGet("{id}")]
        public IActionResult ViewBookPage(int id)
        {
            var book = _booksService.GetBook(id);
            var viewModel = new BooksViewModel {Books = book};
            return View(viewModel);
        }

        [HttpGet("search")]
        public IActionResult SearchBooksPage(string searchString)
        {
            var books = _booksService.SearchBooks(searchString);
            var viewModel = new SearchViewModel {Books = books, SearchString = searchString};
            return View(viewModel);
        }
        
        [HttpGet("create")]
        public IActionResult CreateBookAuthorPage()
        {
            return View();
        }
        
        [HttpPost("create/book")]
        public IActionResult CreateBook(CreateBookModel newBook)
        {
            var bookId =  _booksService.CreateBook(newBook);
            return RedirectToAction("ViewBookPage", new { id = bookId});
        }
        
        [HttpPost("create/author")]
        public IActionResult CreateAuthor(CreateAuthorModel newAuthor)
        {
            _booksService.CreateAuthor(newAuthor);
            return RedirectToAction("CreateBookAuthorPage");
        }
        
        [HttpGet("create/copy")]
        public IActionResult CreateBookCopyPage()
        {
            return View();
        }
        
        [HttpPost("create/copy")]
        public IActionResult CreateBookCopy(CreateBookCopyModel newCopy)
        {
            _booksService.CreateBookCopy(newCopy);
            return RedirectToAction("CreateBookCopyPage");
        }
        
        [HttpGet("create/addAuthorToBook")]
        public IActionResult AddAuthorToBookPage()
        {
            return View();
        }
        
        [HttpPost("create/addAuthorToBook")]
        public IActionResult AddAuthorToBook(CreateBookCopyModel newCopy)
        {
            _booksService.AddAuthorToBook(newCopy);
            return RedirectToAction("AddAuthorToBookPage");
        }
        
        
        
    }
}