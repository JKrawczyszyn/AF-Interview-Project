namespace AFSInterview.Items
{
	using System;
	using UnityEngine;

	[Serializable]
	public class Item
	{
		[SerializeField] private string name;
		[SerializeField] private int value;
		[SerializeField] private ItemAction action;

		public string Name => name;
		public int Value => value;

		public Item(string name, int value)
		{
			this.name = name;
			this.value = value;
		}

		public ItemAction Use()
		{
			Debug.Log("Using " + Name);

			return action;
		}
	}
}