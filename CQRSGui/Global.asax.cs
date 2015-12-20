using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using SimpleCQRS;

namespace CQRSGui
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static List<IDisposable> _unsubscribeTokens;

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);

            var bus = new CommandBus();

            var eventStore = new EventStore();
            var repository = new Repository<InventoryItem>(eventStore);

            var commandHandlers = new InventoryCommandHandlers(repository);
            bus.RegisterHandler<CheckInItemsToInventory>(commandHandlers);
            bus.RegisterHandler<CreateInventoryItem>(commandHandlers);
            bus.RegisterHandler<DeactivateInventoryItem>(commandHandlers);
            bus.RegisterHandler<RemoveItemsFromInventory>(commandHandlers);
            bus.RegisterHandler<RenameInventoryItem>(commandHandlers);

            var detailsViewSubscriber = new InventoryItemDetailView();
            var listViewSubscriber = new InventoryListView();

            _unsubscribeTokens = new List<IDisposable>
            {
                eventStore.Subscribe<InventoryItemCreated>(detailsViewSubscriber),
                eventStore.Subscribe<InventoryItemDeactivated>(detailsViewSubscriber),
                eventStore.Subscribe<InventoryItemRenamed>(detailsViewSubscriber),
                eventStore.Subscribe<ItemsCheckedInToInventory>(detailsViewSubscriber),
                eventStore.Subscribe<ItemsRemovedFromInventory>(detailsViewSubscriber),
                eventStore.Subscribe<InventoryItemCreated>(listViewSubscriber),
                eventStore.Subscribe<InventoryItemDeactivated>(listViewSubscriber),
                eventStore.Subscribe<InventoryItemRenamed>(listViewSubscriber)
            };

            ServiceLocator.Bus = bus;
        }

        protected void Application_End()
        {
            //technically dont need to do this, but for demo purposes...
 
            if (_unsubscribeTokens != null)
                _unsubscribeTokens.ForEach(t => t.Dispose());
        }
    }
}