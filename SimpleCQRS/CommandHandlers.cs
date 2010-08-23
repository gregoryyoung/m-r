using System;

namespace SimpleCQRS
{
    public class LoggingHandler<T> :Handles<T> where T:Message
    {
        private readonly Handles<T> _next;

        public LoggingHandler(Handles<T> next)
        {
            _next = next;
        }

        public void Handle(T message)
        {
            Console.WriteLine("Logging Handler caught [" + typeof(T) + "]");
            _next.Handle(message);
        }
    }

    public class CreateInventoryItemCommandHandler : Handles<CreateInventoryItem>
    {
        private readonly IRepository<InventoryItem> _repository;
        public CreateInventoryItemCommandHandler(IRepository<InventoryItem> repository)
        {
            _repository = repository;
        }
        public void Handle(CreateInventoryItem message)
        {
            var item = new InventoryItem(message.InventoryItemId, message.Name);
            _repository.Save(item, message.Version);
        }
    }

    public class DeactivateInventoryItemCommandHandler : Handles<DeactivateInventoryItem>
    {
        private readonly IRepository<InventoryItem> _repository;
        public DeactivateInventoryItemCommandHandler(IRepository<InventoryItem> repository)
        {
            _repository = repository;
        }
        public void Handle(DeactivateInventoryItem message)
        {
            var item = _repository.GetById(message.InventoryItemId);
            item.Deactivate();
            _repository.Save(item, message.Version);
        }
    }

    public class RemoveItemsFromInventoryCommandHandler : Handles<RemoveItemsFromInventory>
    {
        private readonly IRepository<InventoryItem> _repository;
        public RemoveItemsFromInventoryCommandHandler(IRepository<InventoryItem> repository)
        {
            _repository = repository;
        }
        public void Handle(RemoveItemsFromInventory message)
        {
            var item = _repository.GetById(message.InventoryItemId);
            item.Remove(message.Count);
            _repository.Save(item, message.Version);
        }
    }
    public class CheckInToInventoryCommandHandler : Handles<CheckInItemsToInventory>
    {
        private readonly IRepository<InventoryItem> _repository;
        public CheckInToInventoryCommandHandler(IRepository<InventoryItem> repository)
        {
            _repository = repository;
        }
        public void Handle(CheckInItemsToInventory message)
        {
            var item = _repository.GetById(message.InventoryItemId);
            item.CheckIn(message.Count);
            _repository.Save(item, message.Version);
        }
    }
    public class RenameInventoryItemCommandHandler : Handles<RenameInventoryItem>
    {
        private readonly IRepository<InventoryItem> _repository;
        public RenameInventoryItemCommandHandler(IRepository<InventoryItem> repository)
        {
            _repository = repository;
        }
        public void Handle(RenameInventoryItem message)
        {
            var item = _repository.GetById(message.InventoryItemId);
            item.ChangeName(message.NewName);
            _repository.Save(item, message.Version);
        }
    }
    
}
