using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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
        public static Die[] fromString(string data)
        {
            try
            {
                data = Regex.Replace(data, @"\s+", "");
                var dieGroups = data.Split("+");
                List<Die> result = new List<Die>();
                foreach (var dieGroup in dieGroups)
                {
                    result.AddRange(dieGroupFromString(dieGroup));
                }
                return result.ToArray();
            }
            catch (Exception)
            {
                return new Die[0];
            }
        }
        private static Die[] dieGroupFromString(string dieGroupData)
        {
            int dieCount;
            string[] dataSplit = dieGroupData.Split('d');
            int.TryParse(dataSplit[0], out dieCount);
            Die[] result = new Die[dieCount];
            Die die = parseDieFromString(dataSplit[1]);
            for (int i = 0; i < dieCount; i++)
            {
                result[i] = die;
            }
            return result;
        }
        private static Die parseDieFromString(string dieData)
        {
            System.Console.WriteLine($"Die Data: {dieData}");
            int sides = 4;
            int reroll = 0;
            int min = 0;
            Regex sidesFind = new Regex("(\\d*)\\D?.*");
            Regex rerollFind = new Regex(".*?r(\\d*)\\D?.*");
            Regex minFind = new Regex(".*?m(\\d*)\\D?.*");
            var m = sidesFind.Match(dieData);
            if (m.Success)
            {
                int.TryParse(m.Groups[1].Value, out sides);
                System.Console.WriteLine("sides success");
            }
            m = rerollFind.Match(dieData);
            if (m.Success)
            {
                int.TryParse(m.Groups[1].Value, out reroll);
                System.Console.WriteLine("reroll success");
            }
            m = minFind.Match(dieData);
            if (m.Success)
            {
                int.TryParse(m.Groups[1].Value, out min);
                System.Console.WriteLine("min success");
            }
            return new Die { sidesCount = sides, rerollAtBelow = reroll, mimumumNumber = min };
        }
        public override bool Equals(object obj)
        {
            if (obj is Die)
            {
                Die that = (Die)obj;
                return that.sidesCount.Equals(this.sidesCount) && that.rerollAtBelow.Equals(this.rerollAtBelow) && that.mimumumNumber.Equals(this.mimumumNumber);
            }
            return base.Equals(obj);
        }
        public static string ToString(Die[] dice)
        {
            List<DieGroup> dieGroups = new List<DieGroup>();
            foreach (var die in dice)
            {
                bool added = false;
                for (int i = 0; i < dieGroups.Count; i++)
                {
                    if (dieGroups[i].die.Equals(die))
                    {
                        dieGroups[i].count++;
                        added = true;
                    }
                }
                if (!added)
                {
                    dieGroups.Add(new DieGroup { die = die, count = 1 });
                }
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dieGroups.Count; i++)
            {
                sb.Append(dieGroups[i].ToString());
                if (i != dieGroups.Count - 1)
                {
                    sb.Append('+');
                }
            }
            return sb.ToString();
        }
    }
    class DieGroup
    {
        public int count; public Die die;
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(count);
            sb.Append('d');
            sb.Append(die.sidesCount);
            if (die.rerollAtBelow != 0 || die.mimumumNumber != 0)
            {
                sb.Append('(');
                if (die.rerollAtBelow != 0)
                {
                    sb.Append('r');
                    sb.Append(die.rerollAtBelow);
                }
                if (die.rerollAtBelow != 0 && die.mimumumNumber != 0)
                    sb.Append(',');
                if (die.mimumumNumber != 0)
                {
                    sb.Append('m');
                    sb.Append(die.mimumumNumber);
                }
                sb.Append(')');
            }
            return sb.ToString();
        }
    }

    //2d6(r2)+1d8(m2)+4d4+1d12(r2,m2)
}
