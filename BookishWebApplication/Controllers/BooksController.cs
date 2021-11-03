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
        public IActionResult Search(string searchString)
        {
            var books = _booksService.SearchBooks(searchString);
            var viewModel = new SearchViewModel {Books = books, SearchString = searchString};
            return View(viewModel);
        }
    }
}