using Microsoft.AspNetCore.Mvc;

namespace BookishWebApplication.Controllers
{
    [Route("/books")]
    public class BooksController : Controller
    {
        [HttpGet("")]
        public IActionResult ViewAll()
        {
            return View();
        }
    }
}