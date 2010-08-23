using System;
using System.Collections.Generic;
using System.Threading;

namespace SimpleCQRS
{
    public class FakeBus : ICommandSender, IEventPublisher
    {
        private readonly Dictionary<Type, List<object>> _routes = new Dictionary<Type, List<object>>();

        public void RegisterHandler<T>(Handles<T> handler) where T : Message
        {
            List<object> handlers;
            if(!_routes.TryGetValue(typeof(T), out handlers))
            {
                handlers = new List<object>();
                _routes.Add(typeof(T), handlers);
            }
            handlers.Add(handler);
        }

        public void Send<T>(T command) where T : Command
        {
            List<object> handlers;
            if (_routes.TryGetValue(typeof(T), out handlers))
            {
                if(handlers.Count != 1) throw new InvalidOperationException("cannot send to more than one handler");
                ((Handles<T>)handlers[0]).Handle(command);
            }
            throw new InvalidOperationException("no handler registered");
        }

        public void Publish<T>(T @event) where T : Event
        {
            List<object> handlers;
            if (!_routes.TryGetValue(typeof (T), out handlers)) return;
            foreach(Handles<T> handler in handlers)
            {
                //dispatch on thread pool for added awesomeness
                var handler1 = handler;
                ThreadPool.QueueUserWorkItem(x => handler1.Handle(@event));
            }
        }
    }

    public interface Handles<T>
    {
        void Handle(T message);
    }

    public interface ICommandSender
    {
        void Send<T>(T command) where T : Command;

    }
    public interface IEventPublisher
    {
        void Publish<T>(T @event) where T : Event;
    }
}
