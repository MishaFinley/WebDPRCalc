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
            UserDatabaseInterface.createUser(new Models.User { username = username, password = Models.User.hashPassword(password, username) });
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
            return View();
        }

    }
}
