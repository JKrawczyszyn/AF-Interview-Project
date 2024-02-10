namespace AFSInterview.Units
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using UnityEngine;
    using TMPro;

    public class CombatManager : MonoBehaviour
    {
        [SerializeField] private ArmyView army1View;
        [SerializeField] private ArmyView army2View;
        [SerializeField] private TextMeshProUGUI turnText;

        private bool army1Turn;
        private int turn;
        private bool turnInProgress;

        private AttackChooser attackChooser;
        private UnitAnimator unitAnimator;

        private IEnumerable<Unit> Army1Units => army1View.GetUnits();
        private IEnumerable<Unit> Army2Units => army2View.GetUnits();
        private bool AnyUnitsInArmy1 => Army1Units.Any();
        private bool AnyUnitsInArmy2 => Army2Units.Any();
        private bool AnyUnitsInArmies => AnyUnitsInArmy1 && AnyUnitsInArmy2;

        private void Awake()
        {
            attackChooser = new AttackChooser();
            unitAnimator = new UnitAnimator(army1View, army2View);
        }

        private void Start()
        {
            army1Turn = Random.value > 0.5f;
            turn = 0;
        }

        private async void Update()
        {
            await UpdateAsync();
        }

        // I would use UniTask replacement for Task in real project. It is faster and better suited for Unity.
        private async Task UpdateAsync()
        {
            if (Input.GetMouseButtonDown(0) && !turnInProgress)
            {
                if (!AnyUnitsInArmies)
                    return;

                turn++;

                turnInProgress = true;

                UpdateTurnText();

                var commands = AdvanceTurn();
                await unitAnimator.AnimateCommands(commands);

                if (!AnyUnitsInArmies)
                {
                    Debug.Log("---");
                    Debug.Log("Game over. " + (AnyUnitsInArmy1 ? "Army 1 wins" : "Army 2 wins"));
                }

                ShowState();

                turnInProgress = false;
            }
        }

        private void UpdateTurnText()
        {
            turnText.text = "Turn: " + turn;
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

        private IEnumerable<IUnitCommand> AdvanceTurn()
        {
            var commands = Enumerable.Empty<IUnitCommand>();

            if (!Army1Units.Any() || !Army2Units.Any())
            {
                Debug.Log("Game over");

                return commands;
            }

            Debug.Log("Turn of " + (army1Turn ? "Army 1" : "Army 2") + " started");

            if (army1Turn)
            {
                foreach (Unit unit in Army1Units)
                    unit.AdvanceTurn();

                commands = PerformTurn(Army1Units, Army2Units);
            }
            else
            {
                foreach (Unit unit in Army2Units)
                    unit.AdvanceTurn();

                commands = PerformTurn(Army2Units, Army1Units);
            }

            army1Turn = !army1Turn;

            Debug.Log("Turn ended.");

            return commands;
        }

        private IEnumerable<IUnitCommand> PerformTurn(IEnumerable<Unit> attackers, IEnumerable<Unit> defenders)
        {
            List<IUnitCommand> commands = new();

            var defendersArray = defenders as Unit[] ?? defenders.ToArray();

            foreach (var attacker in attackers)
            {
                if (!attacker.CanAttack())
                    continue;

                var defender = attackChooser.Get(attacker, defendersArray);
                if (defender == null)
                    continue;

                var command = Attack(attacker, defender);
                commands.Add(command);
            }

            return commands;
        }

        private IUnitCommand Attack(Unit attacker, Unit defender)
        {
            var attack = new UnitAttackCommand(attacker, defender);

            attack.Execute();

            Debug.Log("Attack of " + GetUnitName(attacker) + " on " + GetUnitName(defender) + " for "
                + attacker.GetAttackDamageAgainst(defender) + " damage." + (defender.IsDead ? " Defender died." : ""));

            return attack;
        }

        private string GetUnitName(Unit unit)
        {
            return army1View.GetUnitName(unit) ?? army2View.GetUnitName(unit);
        }
    }
}
