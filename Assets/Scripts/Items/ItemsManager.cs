namespace AFSInterview.Items
{
	using TMPro;
	using UnityEngine;

	public class ItemsManager : MonoBehaviour
	{
		[SerializeField] private InventoryController inventoryController;
		[SerializeField] private int itemSellMaxValue;
		[SerializeField] private Transform itemSpawnParent;
		[SerializeField] private GameObject itemPrefab;
		[SerializeField] private BoxCollider itemSpawnArea;
		[SerializeField] private float itemSpawnInterval;
		[SerializeField] private TextMeshProUGUI moneyText;

		private Camera cameraMain;
		private int layerMask;
		private ItemActionResolver itemActionResolver;


		private void Start()
		{
			cameraMain = Camera.main;
			layerMask = LayerMask.GetMask("Item");
			itemActionResolver = new ItemActionResolver(inventoryController);

			UpdateMoneyText();

			InvokeRepeating(nameof(SpawnNewItem), 0f, itemSpawnInterval);
		}

		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
				TryPickUpItem();

			if (Input.GetMouseButtonDown(1))
				TryUseItem();
			
			if (Input.GetKeyDown(KeyCode.Space))
			{
				inventoryController.SellAllItemsUpToValue(itemSellMaxValue);

				UpdateMoneyText();
			}
		}

		private void UpdateMoneyText()
		{
			moneyText.text = "Money: " + inventoryController.Money;
		}

		private void SpawnNewItem()
		{
			var spawnAreaBounds = itemSpawnArea.bounds;
			var position = new Vector3(
				Random.Range(spawnAreaBounds.min.x, spawnAreaBounds.max.x),
				0f,
				Random.Range(spawnAreaBounds.min.z, spawnAreaBounds.max.z)
			);
			
			// Could be optimized by using object pooling if necessary
			Instantiate(itemPrefab, position, Quaternion.identity, itemSpawnParent);
		}

		private void TryPickUpItem()
		{
			if (TryGetItemHolderOnPosition(Input.mousePosition, out IItemHolder itemHolder))
				return;
			
			var item = itemHolder.GetItem(true);
            inventoryController.AddItem(item);
            Debug.Log("Picked up " + item.Name + " with value of " + item.Value + " and now have " + inventoryController.ItemsCount + " items");
		}

		private void TryUseItem()
		{
			if (TryGetItemHolderOnPosition(Input.mousePosition, out IItemHolder itemHolder))
				return;

			var item = itemHolder.GetItem(true);
			ItemAction action = item.Use();
			itemActionResolver.Resolve(action);
			UpdateMoneyText();
			Debug.Log("Used " + item.Name + " item and now have " + inventoryController.Money + " money and " + inventoryController.ItemsCount + " items");
		}

		private bool TryGetItemHolderOnPosition(Vector3 position, out IItemHolder itemHolder)
		{
			itemHolder = null;

			var ray = cameraMain.ScreenPointToRay(position);
			if (!Physics.Raycast(ray, out var hit, 100f, layerMask) || !hit.transform.gameObject.TryGetComponent<IItemHolder>(out itemHolder))
				return true;

			return false;
		}
	}
}