using System;
namespace SimpleCQRS
{
    public class Event : Message
    {
        public int Version;
    }
    
    public class InventoryItemDeactivated : Event {
        public readonly Guid Id;

        public InventoryItemDeactivated(Guid id)
        {
            Id = id;
        }
    }

    public class InventoryItemCreated : Event {
        public readonly Guid Id;
        public readonly string Name;
        public readonly int MaxQty;

        public InventoryItemCreated(Guid id, string name, int maxQty)
        {
            Id = id;
            Name = name;
            MaxQty = maxQty;
        }
    }

    public class InventoryItemRenamed : Event
    {
        public readonly Guid Id;
        public readonly string NewName;
 
        public InventoryItemRenamed(Guid id, string newName)
        {
            Id = id;
            NewName = newName;
        }
    }

    public class ItemsCheckedInToInventory : Event
    {
        public Guid Id;
        public readonly int Count;
 
        public ItemsCheckedInToInventory(Guid id, int count) {
            Id = id;
            Count = count;
        }
    }

    public class ItemsRemovedFromInventory : Event
    {
        public Guid Id;
        public readonly int Count;
 
        public ItemsRemovedFromInventory(Guid id, int count) {
            Id = id;
            Count = count;
        }
    }
    public class MaxQtyChanged : Event
    {
        public Guid Id;
        public readonly int NewMaxQty;

        public MaxQtyChanged(Guid id, int newMaxQty)
        {
            Id = id;
            NewMaxQty = newMaxQty;
        }
    }
}

