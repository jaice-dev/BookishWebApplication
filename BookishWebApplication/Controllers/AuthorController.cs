using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BookishWebApplication.Models;
using BookishWebApplication.Models.Database.Create;
using BookishWebApplication.Models.View;
using BookishWebApplication.Services;

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
            var booksByAuthor = _booksService.GetBooksByAuthor(authorId);
            var author = _authorService.GetAuthor(authorId);
            var viewModel = new AuthorViewModel {Author = author, BooksByAuthor = booksByAuthor};
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
            return RedirectToAction("ViewAuthorPage", new {id = authorId});
        }
    }
}