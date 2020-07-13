using System.Collections.Generic;

namespace WebDPRCalc.Models
{
    public class DamageRoll
    {
        public Die[] dice { get; set; }
        public int numericalAddition { get; set; }
        public bool resisted { get; set; }
        public Die[] additionalCritDice { get; set; }
        public int rerollCountOfDie { get; set; }
        public int rollUseHighest { get; set; }

        public double averageNormalHitDamage()
        {
            double damage = 0;
            //Damage Die
            for (int i = 0; i < dice.Length; i++)
                damage += dice[i].averageResult();

            //reroll count of die
            damage = averageAfterRerollCountOfDie(damage, dice);

            //roll use highest
            damage = averageAfterRollUseHighest(damage, dice);

            //modifier
            damage = damage + numericalAddition;

            //Reistance
            if (resisted && dice.Length > 0)
                damage = (damage / 2) > 0.5 ? (damage / 2) - 0.5 : (damage / 2);
            else if (resisted)
                damage = damage / 2;
            //Result
            return damage;
        }
        private double averageAfterRerollCountOfDie(double damage, Die[] dice)
        {
            List<double> Increases = new List<double>();
            for (int i = 0; i < dice.Length; i++)
            {
                double dieIncrease = 0;
                for (int side = 1; side <= dice[i].sidesCount / 2; side++)
                {
                    double chanceToRollNumber = dice[i].chanceToMeetTarget(side);
                    double increase = dice[i].averageResult() - side;
                    double increseTimesChance = increase * chanceToRollNumber;
                    dieIncrease += increseTimesChance;
                }
                Increases.Add(dieIncrease);
            }
            Increases.Sort();
            Increases.Reverse();
            double averageIncrease = 0;
            for (int i = 0; i < rerollCountOfDie && i < Increases.Count; i++)
            {
                averageIncrease += Increases[i];
            }
            return damage + averageIncrease;
        }
        private double averageAfterRollUseHighest(double damage, Die[] dice)
        {
            //TODO
            return damage;
        }
        public double averageCriticalHitDamage()
        {
            double damage = 0;
            Die[] combinedDice = new Die[(dice.Length * 2) + additionalCritDice.Length];
            dice.CopyTo(combinedDice, 0);
            dice.CopyTo(combinedDice, dice.Length);
            additionalCritDice.CopyTo(combinedDice, dice.Length * 2);
            //Damage Die
            foreach (Die die in combinedDice)
                damage += die.averageResult();


            //reroll count of die
            damage = averageAfterRerollCountOfDie(damage, combinedDice);

            //roll use highest
            damage = averageAfterRollUseHighest(damage, combinedDice);

            //modifier
            damage = damage + numericalAddition;

            //Reistance
            if (resisted && dice.Length > 0)
                damage = (damage / 2) > 0.5 ? (damage / 2) - 0.5 : (damage / 2);
            else if (resisted)
                damage = damage / 2;
            return damage;
        }
        public double averageDamage(double hitChance, double critChance)
        {
            return (averageNormalHitDamage() * hitChance) + ((averageCriticalHitDamage() - averageNormalHitDamage()) * critChance);
        }
    }
}
