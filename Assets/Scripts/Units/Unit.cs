namespace AFSInterview.Units
{
    using System;
    using UnityEngine;

    [Serializable]
    public class Unit
    {
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

        public void TakeDamage(int damage)
        {
            health -= damage;
        }
    }
}
