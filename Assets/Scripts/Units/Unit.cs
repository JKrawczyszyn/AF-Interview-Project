namespace AFSInterview.Units
{
    using System;
    using System.Linq;
    using UnityEngine;

    [Serializable]
    public class Unit
    {
        public event Action OnDied;

        public enum AttributeType
        {
            None,
            Light,
            Armored,
            Mechanical
        }
        [SerializeField] private AttributeType[] attributes;
        [SerializeField] private int health;
        [SerializeField] private int armor;
        [SerializeField] private int attackInterval;
        [SerializeField] private int attackDamage;
        [SerializeField] private AttributeType attackDamageOverrideType;
        [SerializeField] private int attackDamageOverride;

        private int currentAttackInterval;

        public int CurrentHealth { get; private set; }

        public bool IsDead => CurrentHealth <= 0;

        public void ResetState()
        {
            OnDied = null;

            CurrentHealth = health;
            currentAttackInterval = attackInterval;
        }

        public void AdvanceTurn()
        {
            currentAttackInterval--;

            if (currentAttackInterval <= 0)
                currentAttackInterval = attackInterval;
        }

        public bool CanAttack()
        {
            return currentAttackInterval <= 1;
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;

            if (CurrentHealth <= 0)
                OnDied?.Invoke();
        }

        public int GetAttackDamageAgainst(Unit unit)
        {
            int damage = unit.HasAttribute(attackDamageOverrideType) ? attackDamageOverride : attackDamage - unit.armor;

            if (damage <= 0)
                damage = 1;

            return damage;
        }

        public bool HasAttribute(AttributeType attribute)
        {
            return attributes.Contains(attribute);
        }
    }
}
