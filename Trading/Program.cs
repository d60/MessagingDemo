using System;
using Messages;
using Messages.Trade;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Logging;
using Rebus.Persistence.SqlServer;
using Rebus.Transport.Msmq;

namespace Trading
{
    class Program
    {
        static void Main()
        {
            using (var adapter = new BuiltinHandlerActivator())
            {
                Configure.With(adapter)
                    .Logging(l => l.ColoredConsole(minLevel: LogLevel.Warn))
                    .Transport(t => t.UseMsmq("trading"))
                    .Subscriptions(s => s.StoreInSqlServer("server=.;database=demo;trusted_connection=true", "subscriptions", isCentralized: true))
                    .Start();

                var bus = adapter.Bus;

                Console.WriteLine("----------------- TRADING -----------------");

                while (true)
                {
                    Console.Write("Input trade > ");
                    var text = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(text)) break;

                    bus.Publish(new TradeFinalized(string.Format("trade-{0:yyMMdd}-{0:hhmmss}", DateTime.Now), text)).Wait();
                }
            }
        }
    }
}
