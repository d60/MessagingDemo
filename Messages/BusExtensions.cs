using System;
using System.Threading.Tasks;
using Rebus.Bus;

namespace Messages
{
    public static class BusExtensions
    {
        public static async Task Publish<TEvent>(this IBus bus, TEvent eventMessage)
        {
            await bus.Publish(TypeToTopic(typeof (TEvent)), eventMessage);
        }

        public static async Task Subscribe<TEvent>(this IBus bus)
        {
            await bus.Subscribe(TypeToTopic(typeof (TEvent)));
        }

        static string TypeToTopic(Type type)
        {
            return type.Name;
        }
    }
}