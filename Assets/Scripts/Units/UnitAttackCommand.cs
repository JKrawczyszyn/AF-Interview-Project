namespace AFSInterview.Units
{
    /// <summary>
    /// Represents a command to attack another unit.
    /// </summary>
    public class UnitAttackCommand : IUnitCommand
    {
        public Unit Attacker { get; }
        public Unit Defender { get; }
        public bool IsDefenderDead { get; private set; }

        public UnitAttackCommand(Unit attacker, Unit defender)
        {
            Attacker = attacker;
            Defender = defender;
        }

        public void Execute()
        {
            var damage = Attacker.GetAttackDamageAgainst(Defender);

            Defender.TakeDamage(damage);

            IsDefenderDead = Defender.IsDead;
        }
    }
}
