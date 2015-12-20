using System;

namespace SimpleCQRS
{
    // In a real application where the event store and view model denormalizers 
    // likely live in different runtimes, the ISubscribable interface which is part
    // of the IEventStore contract might accept an offset/timestamp/whatever 
    // parameter to allow denormalizers etc to begin accepting events from the start of
    // time, not just from the end of the stream. This allows new viewmodels to 
    // be added to the application, or viewmodels to be easily rebuilt

    public interface ISubscribable
    {
        IDisposable Subscribe<T>(ISubscriber<T> subscriber) where T : Event;
    }

    public interface ISubscriber<in T>
    {
        void OnEvent(T @event);
    }
}
