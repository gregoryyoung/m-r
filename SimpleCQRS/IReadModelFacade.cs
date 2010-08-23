using System;

namespace SimpleCQRS
{
    public interface ReadModel
    {
        InventoryItemListDto GetInventoryItems();
        InventoryItemDetailsDto GetInventoryItemDetails(Guid id);
    }

    public class InventoryItemDetailsDto
    {
        public readonly Guid Id;
        public readonly string Name;
        public readonly int CurrentCount;

        public InventoryItemDetailsDto(Guid id, string name, int currentCount)
        {
			Id = id;
			Name = name;
            CurrentCount = currentCount;
        }
    }

    public class InventoryItemListDto
    {
        public readonly Guid Id;
        public readonly string Name;

        public InventoryItemListDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
