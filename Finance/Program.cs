using System;
using Messages;
using Messages.Approval;
using Messages.Trade;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Logging;
using Rebus.Persistence.SqlServer;
using Rebus.Transport.Msmq;

namespace Finance
{
    class Program
    {
        const string ConnectionString = "server=.;database=demo;trusted_connection=true";

        static void Main(string[] args)
        {
            using (var adapter = new BuiltinHandlerActivator())
            {
                adapter.Register(() => new InvoicingProcessManager());

                Configure.With(adapter)
                    .Logging(l => l.ColoredConsole(minLevel: LogLevel.Warn))
                    .Transport(t => t.UseMsmq("finance"))
                    .Subscriptions(s => s.StoreInSqlServer(ConnectionString, "subscriptions", isCentralized: true))
                    .Sagas(s => s.StoreInSqlServer(ConnectionString, "sagadata", "sagaindex"))
                    .Options(o =>
                    {
                        o.SetNumberOfWorkers(1);
                        o.SetMaxParallelism(1);
                    })
                    .Start();

                var bus = adapter.Bus;

                bus.Subscribe<TradeFinalized>().Wait();
                bus.Subscribe<TradeApproved>().Wait();
                bus.Subscribe<TradeRejected>().Wait();

                Console.WriteLine("----------------- FINANCE -----------------");
                Console.WriteLine("Press ENTER to quit");
                Console.ReadLine();
            }
        }
    }
}
