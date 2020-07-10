using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebDPRCalc.Models;

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

        [HttpPost]
        public IActionResult EditAttack(FormCollection fc)
        {

            AttackRoll atkroll = new AttackRoll();

            DamageRoll dmgRoll = new DamageRoll();


            Attack attack = new Attack();
            attack.name = fc["atkname"];
            attack.id = int.Parse(fc["id"]);
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