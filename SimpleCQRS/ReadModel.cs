using System;
using System.Collections.Generic;

namespace SimpleCQRS
{
    public interface IReadModelFacade
    {
        IEnumerable<InventoryItemListDto> GetInventoryItems();
        InventoryItemDetailsDto GetInventoryItemDetails(Guid id);
    }

    public class InventoryItemDetailsDto
    {
        public Guid Id;
        public string Name;
        public int CurrentCount;
        public int Version;

        public InventoryItemDetailsDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class InventoryItemListDto
    {
        public Guid Id;
        public string Name;

        public InventoryItemListDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class InventoryListView : ISubscriber<InventoryItemCreated>, ISubscriber<InventoryItemRenamed>, ISubscriber<InventoryItemDeactivated>
    {
        public void OnEvent(InventoryItemCreated message)
        {
            BullShitDatabase.list.Add(new InventoryItemListDto(message.Id, message.Name));
        }

        public void OnEvent(InventoryItemRenamed message)
        {
            var item = BullShitDatabase.list.Find(x => x.Id == message.Id);
            item.Name = message.NewName;
        }

        public void OnEvent(InventoryItemDeactivated message)
        {
            BullShitDatabase.list.RemoveAll(x => x.Id == message.Id);
        }
    }

    public class InventoryItemDetailView : ISubscriber<InventoryItemCreated>, ISubscriber<InventoryItemDeactivated>, ISubscriber<InventoryItemRenamed>, ISubscriber<ItemsRemovedFromInventory>, ISubscriber<ItemsCheckedInToInventory>
    {
        public void OnEvent(InventoryItemCreated message)
        {
            BullShitDatabase.details.Add(message.Id, new InventoryItemDetailsDto(message.Id, message.Name));
        }

        public void OnEvent(InventoryItemDeactivated message)
        {
            BullShitDatabase.details.Remove(message.Id);
        }

        public void OnEvent(InventoryItemRenamed message)
        {
            InventoryItemDetailsDto d = GetDetailsItem(message.Id);
            d.Name = message.NewName;
            d.Version = message.Version;
        }

        public void OnEvent(ItemsRemovedFromInventory message)
        {
            InventoryItemDetailsDto d = GetDetailsItem(message.Id);
            d.CurrentCount -= message.Count;
            d.Version = message.Version;
        }

        public void OnEvent(ItemsCheckedInToInventory message)
        {
            InventoryItemDetailsDto d = GetDetailsItem(message.Id);
            d.CurrentCount += message.Count;
            d.Version = message.Version;
        }

        private static InventoryItemDetailsDto GetDetailsItem(Guid id)
        {
            InventoryItemDetailsDto dto;

            if (!BullShitDatabase.details.TryGetValue(id, out dto))
            {
                throw new InvalidOperationException("did not find the original inventory this shouldnt happen");
            }

            return dto;
        }
    }

    public class ReadModelFacade : IReadModelFacade
    {
        public IEnumerable<InventoryItemListDto> GetInventoryItems()
        {
            return BullShitDatabase.list;
        }

        public InventoryItemDetailsDto GetInventoryItemDetails(Guid id)
        {
            return BullShitDatabase.details[id];
        }
    }

    public static class BullShitDatabase
    {
        public static Dictionary<Guid, InventoryItemDetailsDto> details = new Dictionary<Guid, InventoryItemDetailsDto>();
        public static List<InventoryItemListDto> list = new List<InventoryItemListDto>();
    }
}
