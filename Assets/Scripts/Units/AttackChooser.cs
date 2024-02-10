namespace AFSInterview.Units
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine.Assertions;

    public class AttackChooser
    {
        private readonly Dictionary<Unit, float> unitToPriority = new();

        public Unit Get(Unit attacker, IEnumerable<Unit> defenders)
        {
            unitToPriority.Clear();

            foreach (var defender in defenders)
            {
                if (defender.IsDead)
                    continue;

                unitToPriority[defender] = CalculatePriority(attacker, defender);
            }

            return unitToPriority.OrderByDescending(kvp => kvp.Value).FirstOrDefault().Key;
        }

        private float CalculatePriority(Unit attacker, Unit defender)
        {
            // AI is not very smart.
            // The priority is the ratio of the attack damage to the current health of the defender.
            var priority = (float)attacker.GetAttackDamageAgainst(defender) / defender.CurrentHealth;

            Assert.IsTrue(priority >= 0f, "Priority must be non-negative");

            return priority;
        }
    }
}
