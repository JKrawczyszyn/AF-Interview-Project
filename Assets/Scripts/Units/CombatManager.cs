namespace AFSInterview.Units
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class CombatManager : MonoBehaviour
    {
        [SerializeField] private ArmyView army1View;
        [SerializeField] private ArmyView army2View;

        private bool army1Turn;

        private AttackChooser targetChooser;

        private IEnumerable<Unit> Army1Units => army1View.GetUnits();
        private IEnumerable<Unit> Army2Units => army2View.GetUnits();
        private bool Army1HasUnits => Army1Units.Any();
        private bool Army2HasUnits => Army2Units.Any();
        private bool BothSidesHaveUnits => Army1HasUnits && Army2HasUnits;

        private void Awake()
        {
            targetChooser = new AttackChooser();
        }

        private void Start()
        {
            army1Turn = Random.value > 0.5f;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!BothSidesHaveUnits)
                    return;

                AdvanceTurn();

                if (!BothSidesHaveUnits)
                {
                    Debug.Log("---");
                    Debug.Log("Game over. " + (Army1HasUnits ? "Army 1 wins" : "Army 2 wins"));
                }

                ShowState();
            }
        }

        private void ShowState()
        {
            Debug.Log("  Current state:");
            Debug.Log("    Army 1 units:");

            foreach (Unit unit in Army1Units)
                Debug.Log("    - " + army1View.GetUnitName(unit) + " - health: " + unit.CurrentHealth);
            Debug.Log("    Army 2 units:");
            foreach (Unit unit in Army2Units)
                Debug.Log("    - " + army2View.GetUnitName(unit) + " - health: " + unit.CurrentHealth);
        }

        private void AdvanceTurn()
        {
            if (!Army1Units.Any() || !Army2Units.Any())
            {
                Debug.Log("Game over");

                return;
            }

            Debug.Log("Turn of " + (army1Turn ? "Army 1" : "Army 2") + " started");

            if (army1Turn)
            {
                foreach (Unit unit in Army1Units)
                    unit.AdvanceTurn();

                PerformTurn(Army1Units, Army2Units);
            }
            else
            {
                foreach (Unit unit in Army2Units)
                    unit.AdvanceTurn();

                PerformTurn(Army2Units, Army1Units);
            }

            army1Turn = !army1Turn;

            Debug.Log("Turn ended.");
        }

        private void PerformTurn(IEnumerable<Unit> attackers, IEnumerable<Unit> defenders)
        {
            var defendersArray = defenders as Unit[] ?? defenders.ToArray();

            foreach (var attacker in attackers)
            {
                if (!attacker.CanAttack())
                    continue;

                var target = targetChooser.Get(attacker, defendersArray);
                if (target == null)
                    continue;

                var damage = attacker.GetAttackDamageAgainst(target);

                target.TakeDamage(damage);

                Debug.Log("Attack of " + GetUnitName(attacker) + " on " + GetUnitName(target) + " for " + damage + " damage");
            }
        }

        private string GetUnitName(Unit unit)
        {
            return army1View.GetUnitName(unit) ?? army2View.GetUnitName(unit);
        }
    }
}
