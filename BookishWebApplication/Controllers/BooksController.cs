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
        public IActionResult ViewAll()
        {
            var books = _booksService.GetAllBooks();
            var viewModel = new BooksViewModel {Books = books};
            
            return View(viewModel);
        }
        
        [HttpGet("search/{bookTitle}")]
        public IActionResult Search(string bookTitle)
        {
            var books = _booksService.SearchBooks(bookTitle);
            var viewModel = new SearchViewModel {Books = books, BookTitle = bookTitle};
            return View(viewModel);
        }
    }
}