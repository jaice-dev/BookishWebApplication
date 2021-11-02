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
        
        [HttpGet("search")]
        public IActionResult Search()
        {
            var books = _booksService.SearchBooks("Potter");
            var viewModel = new BooksViewModel {Books = books};
            
            return View(viewModel);
        }
    }
}