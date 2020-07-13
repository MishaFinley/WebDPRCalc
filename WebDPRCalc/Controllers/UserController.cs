using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebDPRCalc.Models;

namespace WebDPRCalc.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            User check = UserDatabaseInterface.readUser(username);
            if (!(check is null) && check.validPassword(password, username))
            {
                HttpContext.Session.SetString("username", username);

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult CreateAccount()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateAccount(string username, string password, string passwordcon)
        {
            User check = UserDatabaseInterface.readUser(username);
            if ((check is null) && password.Equals(passwordcon))
            {
                UserDatabaseInterface.createUser(new Models.User { username = username, password = Models.User.hashPassword(password, username) });
                HttpContext.Session.SetString("username", username);

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult Profile()
        {
            string username = HttpContext.Session.GetString("username");
            User user = UserDatabaseInterface.readUser(username);
            if (username is null || user is null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.profile = user;
                return View();
            }
        }

    }
}
