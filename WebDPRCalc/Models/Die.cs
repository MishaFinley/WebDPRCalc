﻿using System;
using System.Collections.Generic;
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
    }
    //2d6(r2)+1d8(m2)+4d4+1d12(r2,m2)
}
