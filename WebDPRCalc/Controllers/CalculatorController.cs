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
        public IActionResult EditAttack(IFormCollection fc)
        {

            AttackRoll atkroll = new AttackRoll();
            atkroll.numericalAddition = int.Parse(fc["atkmod"]);
            atkroll.critRangeCount = int.Parse(fc["critrange"]);
            atkroll.luckyDie = fc["lucky"] == "on" ? true : false;
            atkroll.elvenAccuracy = fc["elvenacc"] == "on" ? true : false;
            atkroll.halfingLuck = fc["halflucky"] == "on" ? true : false;
            atkroll.rerollMiss = fc["missreroll"] == "on" ? true : false;
            atkroll.diceAddition = Die.fromString(fc["tohit"]);

            DamageRoll dmgRoll = new DamageRoll();
            dmgRoll.numericalAddition = int.Parse(fc["dmgmod"]);
            dmgRoll.resisted = fc["resist"] == "on" ? true : false;
            dmgRoll.dice = Die.fromString(fc["dmgdice"]);
            dmgRoll.rerollCountOfDie = int.Parse(fc["rerolldice"]);
            dmgRoll.additionalCritDice = Die.fromString(fc["addcritdice"]);

            Attack attack = new Attack();
            attack.name = fc["atkname"];
            attack.id = int.Parse(fc["id"]);
            attack.attackRoll = atkroll;
            attack.damageRoll = dmgRoll;
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