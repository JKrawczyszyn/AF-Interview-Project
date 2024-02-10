using UnityEngine.Assertions;

namespace AFSInterview.Units
{
    using System.Collections.Generic;
    using System.Linq;

    public class AttackChooser
    {
        private readonly Dictionary<Unit, float> unitToPriority = new();

        public Unit Get(Unit attacker, IEnumerable<Unit> defenders)
        {
            unitToPriority.Clear();

            var defendersArray = defenders as Unit[] ?? defenders.ToArray();

            foreach (Unit defender in defendersArray)
                unitToPriority[defender] = CalculatePriority(attacker, defender);

            return defendersArray.OrderByDescending(d => unitToPriority[d]).FirstOrDefault();
        }

        private float CalculatePriority(Unit attacker, Unit defender)
        {
            if (defender.CurrentHealth <= 0)
                return -1f;

            var priority = (float)attacker.GetAttackDamageAgainst(defender) / defender.CurrentHealth;

            Assert.IsTrue(priority >= 0f, "Priority must be non-negative");

            return priority;
        }
    }
}
