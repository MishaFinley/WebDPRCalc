namespace WebDPRCalc.Models
{
    public class Die
    {
        public int sidesCount { get; set; }
        public int rerollAtBelow { get; set; }
        public int mimumumNumber { get; set; }

        public double averageResult()
        {
            int runningTotal = 0;
            for (int i = 1; i <= sidesCount; i++)
            {
                runningTotal += i < mimumumNumber ? mimumumNumber : i;
            }
            double workingAverage = runningTotal * 1.0 / sidesCount;
            double total = 0;
            for (int i = 1; i <= sidesCount; i++)
            {
                if (i <= rerollAtBelow)
                {
                    total += workingAverage < mimumumNumber ? mimumumNumber : workingAverage;
                }
                else
                {
                    total += i < mimumumNumber ? mimumumNumber : i;
                }
            }
            return total / sidesCount;
        }
        public double chanceToMeetTarget(double target)
        {
            if (target <= mimumumNumber)
            {
                return 1;
            }
            else if (target > sidesCount)
            {
                return 0;
            }
            else
            {
                double successChance = 0;
                for (int i = 1; i <= sidesCount; i++)
                {
                    double number = i < rerollAtBelow ? averageResult() : i;
                    if (number < mimumumNumber)
                        number = mimumumNumber;
                    if (number >= target)
                        successChance += 1;
                }
                successChance = successChance / sidesCount;
                return successChance;
            }
        }
    }
}
