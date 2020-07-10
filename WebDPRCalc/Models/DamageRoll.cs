namespace WebDPRCalc.Models
{
    public class DamageRoll
    {
        private Die[] dice { get; set; }
        private int numericalAddition { get; set; }
        private bool resisted { get; set; }
        private Die[] additionalCritDice { get; set; }
        private int rerollCountOfDie { get; set; }
        private int rollUseHighest { get; set; }

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
            //TODO
            return damage;
        }
        private double averageAfterRollUseHighest(double damage, Die[] dice)
        {
            //TODO
            return damage;
        }

        public double averageCriticalHitDamage()
        {
            double damage = 0;
            Die[] combinedDice = new Die[dice.Length * 2 + additionalCritDice.Length];
            dice.CopyTo(combinedDice, 0);
            dice.CopyTo(combinedDice, dice.Length);
            additionalCritDice.CopyTo(combinedDice, dice.Length + additionalCritDice.Length);
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
            return 0;
        }
        public double averageDamage(double hitChance, double critChance)
        {
            return (averageNormalHitDamage() * hitChance) + ((averageCriticalHitDamage() - averageNormalHitDamage()) * critChance);
        }
    }
}
