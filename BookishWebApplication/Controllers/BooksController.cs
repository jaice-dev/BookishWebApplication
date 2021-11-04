using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IAuthorService _authorService;
        public BooksController(IBooksService booksService, IAuthorService authorService)
        {
            _booksService = booksService;
            _authorService = authorService;
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
            var authors = _authorService.GetAllAuthors();
            var bookAuthor = new BookAuthor {BookId = book.BookId};
            var viewModel = new BookViewModel {Book = book, AllAuthors = authors, BookAuthor = bookAuthor};
            return View(viewModel);
        }

        [HttpGet("search")]
        public IActionResult SearchBooksResultsPage(string searchString)
        {
            var authors = _authorService.GetAllAuthors();
            var books = _booksService.SearchBooks(searchString);
            var bookAuthors= books.Select(book => new BookAuthor() {BookId = book.BookId});
            var viewModel = new SearchBooksViewModel {Books = books, AllAuthors = authors, BookAuthors = bookAuthors, SearchString = searchString};
            return View(viewModel);
        }
        
        [HttpGet("create")]
        public IActionResult CreateBookPage()
        {
            return View();
        }
        
        [HttpPost("create/book")]
        public IActionResult CreateBook(CreateBookModel newBook)
        {
            var bookId =  _booksService.CreateBook(newBook);
            return RedirectToAction("ViewBookPage", new { id = bookId});
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
        public IActionResult AddAuthorToBook(BookAuthor bookAuthor)
        {
            _booksService.AddAuthorToBook(bookAuthor);
            return RedirectToAction("ViewBookPage", new { id = bookAuthor.BookId });
        }
    }
}

//TODO When you add an author go to author page