using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public IActionResult EditAttack(IFormCollection fc)
        {

            AttackRoll atkroll = new AttackRoll();
            atkroll.numericalAddition = int.Parse(fc["atkmod"]);
            try
            {
                atkroll.critRangeCount = int.Parse(fc["critrange"]);
            }
            catch (Exception) { atkroll.critRangeCount = 1; }
            atkroll.luckyDie = fc["lucky"] == "on" ? true : false;
            atkroll.elvenAccuracy = fc["elvenacc"] == "on" ? true : false;
            atkroll.halfingLuck = fc["halflucky"] == "on" ? true : false;
            atkroll.rerollMiss = fc["missreroll"] == "on" ? true : false;
            atkroll.diceAddition = Die.fromString(fc["tohit"]);

            DamageRoll dmgRoll = new DamageRoll();
            dmgRoll.numericalAddition = int.Parse(fc["dmgmod"]);
            dmgRoll.resisted = fc["resist"] == "on" ? true : false;
            dmgRoll.dice = Die.fromString(fc["dmgdice"]);
            try
            {
                dmgRoll.rerollCountOfDie = int.Parse(fc["rerolldice"]);
            }
            catch (Exception) { }
            try
            {
                dmgRoll.additionalCritDice = Die.fromString(fc["addcritdice"]);
            }
            catch (Exception) { }
            Attack attack = new Attack();
            attack.name = fc["atkname"];
            attack.id = int.Parse(fc["id"]);
            attack.attackRoll = atkroll;
            attack.damageRoll = dmgRoll;

            string username = Convert.ToString(HttpContext.Session.Get("username"));
            AttackDPRCaclulation result = attack.DPRCaclulation();
            if (!(HttpContext.Session.Get("username") is null))
            {
                UserDatabaseInterface.createAttack(username, attack);
            }

            return View(attack);
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
