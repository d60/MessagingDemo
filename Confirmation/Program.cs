using System;
using System.Collections.Concurrent;
using Messages;
using Messages.Trade;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Logging;
using Rebus.Persistence.SqlServer;
using Rebus.Transport.Msmq;

namespace Confirmation
{
    class Program
    {
        const string ConnectionString = "server=.;database=demo;trusted_connection=true";

        static readonly ConcurrentQueue<char> CharacterQueue = new ConcurrentQueue<char>();

        static void Main()
        {
            using (var adapter = new BuiltinHandlerActivator())
            {
                adapter.Register(() => new ConfirmTradesHandler(adapter.Bus, CharacterQueue));

                Configure.With(adapter)
                    .Logging(l => l.ColoredConsole(minLevel: LogLevel.Warn))
                    .Transport(t => t.UseMsmq("confirmation"))
                    .Subscriptions(s => s.StoreInSqlServer(ConnectionString, "subscriptions", isCentralized: true))
                    .Options(o =>
                    {
                        o.SetNumberOfWorkers(1);
                        o.SetMaxParallelism(1);
                    })
                    .Start();

                var bus = adapter.Bus;

                bus.Subscribe<TradeFinalized>().Wait();

                Console.WriteLine("----------------- CONFIRMATION -----------------");

                Console.WriteLine("Press q to quit");
                while (true)
                {
                    var c = char.ToLowerInvariant(Console.ReadKey(true).KeyChar);

                    if (c == 'q') break;

                    CharacterQueue.Enqueue(c);
                }
            }
        }
    }
}
