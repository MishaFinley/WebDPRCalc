using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebDPRCalc.Controllers
{
    public class CalculatorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AttackList()
        {
            return View();
        }
        public IActionResult EditAttack()
        {
            return View();
        }
        public IActionResult ViewAttack()
        {
            return View();
        }
        public IActionResult Tutorial()
        {
            return View();
        }
    }
}