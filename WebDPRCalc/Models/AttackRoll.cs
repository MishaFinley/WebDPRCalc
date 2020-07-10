namespace WebDPRCalc.Models
{
    public class AttackRoll
    {
        public int numericalAddition { get; set; }
        private bool advantage { get; set; }
        public bool disadvantage { get; set; }
        public bool luckyDie { get; set; }
        public bool elvenAccuracy { get; set; }
        public int critRangeCount { get; set; }
        public bool halfingLuck { get; set; }
        public Die[] diceAddition = new Die[0];
        public bool rerollMiss { get; set; }



        private double averageDiceAddition()
        {
            double average = 0;
            foreach (Die die in diceAddition)
            {
                average += die.averageResult();
            }
            return average;
        }
        private double averageAddition()
        {
            return averageDiceAddition() + numericalAddition;
        }
        private double d20Chance(int ac)
        {
            Die d20 = new Die { sidesCount = 20, rerollAtBelow = halfingLuck ? 1 : 0, mimumumNumber = 0 };
            double d20Chance = d20.chanceToMeetTarget(ac - averageAddition());
            if (!halfingLuck)
            {
                if (d20Chance > 0.95)
                    d20Chance = 0.95;
                if (d20Chance < 0.05)
                    d20Chance = 0.05;
            }
            else
            {
                if (d20Chance > 0.9975)
                    d20Chance = 0.9975;
                if (d20Chance < 0.0525)
                    d20Chance = 0.0525;
            }
            return d20Chance;
        }
        private double bestOfCount(int count, double chance)
        {
            if (count == 1)
                return chance;
            else if (count < 0)
                return 0;
            else
            {
                double countMinus1 = bestOfCount(count - 1, chance);
                return ((1 - countMinus1) * chance) + countMinus1;
            }
        }
        public double hitChance(int ac)
        {
            double rollChance = d20Chance(ac);
            if (disadvantage && !advantage && !luckyDie)
            {
                double chance = rollChance * rollChance;
                return rerollMiss ? bestOfCount(2, chance) : chance;
            }
            else
            {
                int bestCount = 1;
                if (advantage && !disadvantage) bestCount++;
                if (disadvantage && !advantage && luckyDie) bestCount = 3;
                else if (luckyDie) bestCount++;
                if (advantage && !disadvantage && elvenAccuracy) bestCount++;
                if (rerollMiss) bestCount = bestCount * 2;
                return bestOfCount(bestCount, rollChance);
            }
        }
        private double baseCritChance()
        {
            return critRangeCount / 20.0;
        }
        public double critChance(int ac)
        {
            double chance = baseCritChance();
            if (disadvantage && !advantage && !luckyDie)
            {
                chance = chance * chance;
            }
            else
            {
                int bestCount = 1;
                if (advantage && !disadvantage) bestCount++;
                if (disadvantage && !advantage && luckyDie) bestCount = 3;
                else if (luckyDie) bestCount++;
                if (advantage && !disadvantage && elvenAccuracy) bestCount++;
                chance = bestOfCount(bestCount, chance);
            }
            if (rerollMiss)
            {
                chance = chance + ((1 - hitChance(ac)) * chance);
            }
            return chance > hitChance(ac) ? hitChance(ac) : chance;
        }
    }
}
