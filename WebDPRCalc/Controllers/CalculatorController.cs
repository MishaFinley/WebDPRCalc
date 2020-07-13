using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
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
            string username = HttpContext.Session.GetString("username");
            User user = UserDatabaseInterface.readUser(username);
            if (username is null || user is null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.attacks = user.attacks;
                return View();
            }
        }
        public IActionResult EditAttack(int id = -1)
        {
            string username = HttpContext.Session.GetString("username");
            try
            {
                var attack = UserDatabaseInterface.readAttack(username, id);
                if (!(username is null) && !(attack is null))
                {
                    ViewBag.attack = attack;
                }
                else { ViewBag.attack = null; }
            }
            catch (Exception) { ViewBag.attack = null; }
            return View();
        }

        [HttpPost]
        public IActionResult EditAttack(IFormCollection fc)
        {

            AttackRoll atkroll = new AttackRoll();
            try
            {
                atkroll.numericalAddition = int.Parse(fc["atkmod"]);
            }
            catch (Exception) { }
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
            try
            {
                dmgRoll.numericalAddition = int.Parse(fc["dmgmod"]);
            }
            catch (Exception) { }
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

            string username = HttpContext.Session.GetString("username");
            //AttackDPRCaclulation result = attack.DPRCaclulation();
            if (!(HttpContext.Session.Get("username") is null))
            {
                if (attack.id == -1)
                {
                    attack.id = 0;
                    UserDatabaseInterface.createAttack(username, attack);
                }
                else
                {
                    UserDatabaseInterface.updateAttack(username, attack);
                }
                return RedirectToAction("ViewAttack", attack.id);
            }
            else
            {
                HttpContext.Session.SetString("attack", new JavaScriptSerializer().Serialize(attack));
                return RedirectToAction("ViewAttack");
            }

        }

        public IActionResult ViewAttack(int id = -1)
        {
            string username = HttpContext.Session.GetString("username");
            try
            {
                var attack = UserDatabaseInterface.readAttack(username, id);
                if (id != -1 && !(username is null))
                {
                    ViewBag.attack = attack;
                    ViewBag.calculation = attack.DPRCaclulation();
                    return View();
                }
            }
            catch (Exception) { }
            try
            {
                var attack = new JavaScriptSerializer().Deserialize<Attack>(HttpContext.Session.GetString("attack"));
                if (!(attack is null))
                {
                    ViewBag.attack = attack;
                    ViewBag.calculation = attack.DPRCaclulation();
                    HttpContext.Session.Remove("attack");
                    return View();
                }
            }
            catch (Exception) { }
            HttpContext.Session.Remove("attack");
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Tutorial()
        {
            return View();
        }
    }
}
