namespace AFSInterview.Units
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Utilities;

    public class UnitAnimator
    {
        private ArmyView army1View;
        private ArmyView army2View;

        public UnitAnimator(ArmyView army1View, ArmyView army2View)
        {
            this.army1View = army1View;
            this.army2View = army2View;
        }

        public async Task AnimateCommands(IEnumerable<IUnitCommand> commands)
        {
            foreach (IUnitCommand command in commands)
            {
                if (command is UnitAttackCommand attack)
                {
                    var attackerPresenter = GetUnitPresenter(attack.Attacker);
                    var defenderPresenter = GetUnitPresenter(attack.Defender);

                    if (attackerPresenter != null && defenderPresenter != null)
                        await AnimateAttack(attackerPresenter, defenderPresenter, attack.IsDefenderDead);
                }
            }
        }

        private UnitPresenter GetUnitPresenter(Unit unit)
        {
            return army1View.GetUnitPresenter(unit) ?? army2View.GetUnitPresenter(unit);
        }

        private async Task AnimateAttack(UnitPresenter attackerPresenter, UnitPresenter defenderPresenter, bool isDefenderDead)
        {
            var startPosition = attackerPresenter.transform.position;

            await attackerPresenter.transform.AnimateMove(defenderPresenter.transform.position, 0.2f);

            if (isDefenderDead)
                defenderPresenter.DestroySelf();

            await attackerPresenter.transform.AnimateMove(startPosition, 0.2f);
        }
    }
}
