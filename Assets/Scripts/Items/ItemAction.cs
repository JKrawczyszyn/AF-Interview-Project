namespace AFSInterview.Items
{
    using System;
    using UnityEngine;

    [Serializable]
    public class ItemAction
    {
        public enum ActionType
        {
            Money,
            Item
        }
        [field: SerializeField] public ActionType Type { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int Value { get; private set; }
    }
}
