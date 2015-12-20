using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleCQRS
{
    public interface IEventStore : ISubscribable
    {
        void SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion);
        List<Event> GetEventsForAggregate(Guid aggregateId);
    }

    public class EventStore : IEventStore
    {
        private readonly Dictionary<Guid, List<EventDescriptor>> _current;
        private readonly Dictionary<Type, List<Action<Event>>> _subscribers;

        public EventStore()
        {
            _current = new Dictionary<Guid, List<EventDescriptor>>();
            _subscribers = new Dictionary<Type, List<Action<Event>>>();
        }

        public void SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion)
        {
            List<EventDescriptor> eventDescriptors;

            // try to get event descriptors list for given aggregate id
            // otherwise -> create empty dictionary
            if (!_current.TryGetValue(aggregateId, out eventDescriptors))
            {
                eventDescriptors = new List<EventDescriptor>();
                _current.Add(aggregateId, eventDescriptors);
            }
            // check whether latest event version matches current aggregate version
            // otherwise -> throw exception
            else if (eventDescriptors[eventDescriptors.Count - 1].Version != expectedVersion && expectedVersion != -1)
            {
                throw new ConcurrencyException();
            }
            var i = expectedVersion;

            // iterate through current aggregate events increasing version with each processed event
            foreach (var @event in events)
            {
                @event.Version = ++i;

                // push event to the event descriptors list for current aggregate
                eventDescriptors.Add(new EventDescriptor(@event, i));

                //while the below still exhibits pub/sub semantics, it doesn't encourage those 
                //learning how to build this type of architecture to think of the event store 
                //as *requiring* a bus, as an ESB is a poor design choice when using CQRS + ES: 
                //https://github.com/gregoryyoung/m-r/issues/13#issuecomment-165458035

                var eventType = @event.GetType();

                var subscribers = _subscribers
                    .Where(kvp => kvp.Key == eventType)
                    .SelectMany(kvp => kvp.Value);

                foreach (var subscriber in subscribers)
                    subscriber(@event);
            }
        }

        // collect all processed events for given aggregate and return them as a list
        // used to build up an aggregate from its history (Domain.LoadsFromHistory)
        public List<Event> GetEventsForAggregate(Guid aggregateId)
        {
            List<EventDescriptor> eventDescriptors;

            if (!_current.TryGetValue(aggregateId, out eventDescriptors))
            {
                throw new AggregateNotFoundException();
            }

            return eventDescriptors.Select(desc => desc.EventData).ToList();
        }

        public IDisposable Subscribe<T>(ISubscriber<T> subscriber) where T : Event
        {
            Action<Event> subscriberAction = e => subscriber.OnEvent((T)e);

            List<Action<Event>> subscribers;

            if (!_subscribers.TryGetValue(typeof(T), out subscribers))
            {
                subscribers = new List<Action<Event>>();
                _subscribers.Add(typeof(T), subscribers);
            }

            subscribers.Add(subscriberAction);

            return new UnsubscribeToken(subscribers, subscriberAction);
        }

        private struct EventDescriptor
        {
            public readonly Event EventData;
            public readonly int Version;

            public EventDescriptor(Event eventData, int version)
            {
                EventData = eventData;
                Version = version;
            }
        }

        private class UnsubscribeToken : IDisposable
        {
            private readonly List<Action<Event>> _subscribers;
            private readonly Action<Event> _subscriber;

            public UnsubscribeToken(List<Action<Event>> subscribers, Action<Event> subscriber)
            {
                _subscribers = subscribers;
                _subscriber = subscriber;
            }

            public void Dispose()
            {
                if (_subscriber != null && _subscribers.Contains(_subscriber))
                    _subscribers.Remove(_subscriber);
            }
        }
    }

    public class AggregateNotFoundException : Exception { }

    public class ConcurrencyException : Exception { }
}
