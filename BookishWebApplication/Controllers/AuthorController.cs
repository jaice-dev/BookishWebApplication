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
        public IActionResult ViewAuthorPage()
        {
            var author = _authorService.GetAllAuthors();
            var viewModel = new AuthorViewModel {Authors = author};
            return View(viewModel);
        }
        
        // [HttpPost("create")]
        // public IActionResult CreateAuthor(CreateAuthorModel newAuthor)
        // {
        //     _authorService.CreateAuthor(newAuthor);
        //     return RedirectToAction("CreateBookAuthorPage");
        // }
    }
}