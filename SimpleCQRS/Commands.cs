using System;
namespace SimpleCQRS
{
	public class Command : Message
	{
	    public int Version;
	}
	
	public class DeactivateInventoryItem : Command {
		public readonly Guid InventoryItemId;
		public DeactivateInventoryItem(Guid inventoryItemId) {
			InventoryItemId = inventoryItemId;
		}
	}
	
	public class CreateInventoryItem : Command {
		public readonly Guid InventoryItemId;
		public readonly string Name;
		public CreateInventoryItem(Guid inventoryItemId, string name) {
			InventoryItemId = inventoryItemId;
			Name = name;
		}
	}
	
	public class RenameInventoryItem : Command {
		public readonly Guid InventoryItemId;
		public readonly string NewName;
		public RenameInventoryItem(Guid inventoryItemId, string newName) {
			InventoryItemId = inventoryItemId;
			NewName = newName;
		}
	}

	public class CheckInItemsToInventory : Command {
		public Guid InventoryItemId;
		public readonly int Count;
		public CheckInItemsToInventory(Guid inventoryItemId, int count) {
			InventoryItemId = inventoryItemId;
			Count = count;
		}
	}
	
	public class RemoveItemsFromInventory : Command {
		public Guid InventoryItemId;
		public readonly int Count;
		
		public RemoveItemsFromInventory(Guid inventoryItemId, int count) {
			InventoryItemId = inventoryItemId;
			Count = count;
		}
	}
}
