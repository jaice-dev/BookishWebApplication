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
        private readonly IAuthorService _authorService;
        
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }
        [HttpGet("")]
        public IActionResult ViewAllAuthorsPage()
        {
            var authors = _authorService.GetAllAuthors();
            var viewModel = new AuthorViewModel {Authors = authors};
            return View(viewModel);
        }

        [HttpGet("{id}")]
        public IActionResult ViewAuthorPage(int id)
        {
            var author = _authorService.GetAuthor(id);
            var viewModel = new AuthorViewModel {Authors = author};
            return View(viewModel);
        }
        
        [HttpPost("create")]
        public IActionResult CreateAuthor(CreateAuthorModel newAuthor)
        {
            var authorId = _authorService.CreateAuthor(newAuthor);
            return RedirectToAction("ViewAuthorPage", new {id = authorId});
        }
    }
}