namespace WebDPRCalc.Models
{
    public class Attack
    {
        public int id { get; set; }
        public string name { get; set; }
        public AttackRoll attackRoll { get; set; }
        public DamageRoll damageRoll { get; set; }

        public AttackDPRCaclulation DPRCaclulation()
        {
            var calc = new AttackDPRCaclulation
            {
                averageDamagePerHit = damageRoll.averageNormalHitDamage(),
                averageDamagePerCrit = damageRoll.averageCriticalHitDamage(),
                dPRFragments = new AttackDPRFragment[41]
            };
            for (int i = 0; i < 41; i++)
            {
                calc.dPRFragments[i] = new AttackDPRFragment
                {
                    ac = i + 5,
                    hitChance = attackRoll.hitChance(i + 5),
                    critChance = attackRoll.critChance(i + 5),
                    averageDamage = damageRoll.averageDamage(attackRoll.hitChance(i + 5), attackRoll.critChance(i + 5))
                };
            }
            return calc;
        }
    }
}
