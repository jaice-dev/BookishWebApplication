using System;
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

        [HttpGet("search")]
        public IActionResult SearchBooksPage(string searchString)
        {
            var books = _booksService.SearchBooks(searchString);
            var viewModel = new SearchViewModel {Books = books, SearchString = searchString};
            return View(viewModel);
        }
        
        [HttpGet("edit")]
        public IActionResult EditBooksPage()
        {
            return View();
        }
        
        [HttpPost("create")]
        public IActionResult CreateBook(CreateBookAuthorModel newBook)
        {
            _booksService.CreateBook(newBook);
            return RedirectToAction("ViewAllBooksPage");
        }
    }
}