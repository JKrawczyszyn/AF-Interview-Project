namespace AFSInterview.Items
{
    using System;

    public class ItemActionResolver
    {
        private InventoryController inventoryController;

        public ItemActionResolver(InventoryController inventoryController)
        {
            this.inventoryController = inventoryController;
        }

        public void Resolve(ItemAction action)
        {
            switch (action.Type)
            {
                case ItemAction.ActionType.Money:
                    inventoryController.AddMoney(action.Value);
                    break;
                case ItemAction.ActionType.Item:
                    inventoryController.AddItem(new Item(action.Name, action.Value));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action.Type, "Unknown action type");
            }
        }
    }
}
