using BookishWebApplication.Models.Database;
using BookishWebApplication.Models.Database.Create;
using BookishWebApplication.Models.View;
using BookishWebApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookishWebApplication.Controllers
{
    [Route("/users")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("")]
        public IActionResult ViewAllUsersPage()
        {
            var users = _customerService.GetAllCustomers();
            var viewModel = new CustomersViewModel {AllUsers = users};
            return View(viewModel);
        }
        
        [HttpGet("{userId}")]
        public IActionResult ViewUserPage(int userId)
        {
            var user = _customerService.GetCustomer(userId);
            var viewModel = new CustomerViewModel {User = user};
            return View(viewModel);
        }
        
        [HttpGet("create")]
        public IActionResult CreateUserPage()
        {
            return View();
        }
        
        [HttpPost("create")]
        public IActionResult CreateUser(CreateCustomerModel newUser)
        {
            var id = _customerService.CreateUser(newUser);
            return RedirectToAction("ViewUserPage", new { userId = id});
        }
        
        [HttpPost("delete")]
        public IActionResult DeleteUser(DeleteCustomerModel user)
        {
            _customerService.DeleteUser(user);
            return RedirectToAction("ViewAllUsersPage");
        }

        [HttpGet("edit/{userId}")]
        public IActionResult EditUserPage(int userId, bool error)
        {
            var user = _customerService.GetCustomer(userId);
            var viewModel = new EditCustomerViewModel {User = user, Error = error};
            return View(viewModel);
        }
        
        [HttpPost("edit")]
        public IActionResult EditUserDetails(Customer user)
        {
            if (!user.IsValid())
            {
                return RedirectToAction("EditUserPage", new {userId = user.CustomerId, error = true});
            }
            _customerService.EditUserDetails(user);
            return RedirectToAction("ViewUserPage", new { userId = user.CustomerId});
        }

    }
}