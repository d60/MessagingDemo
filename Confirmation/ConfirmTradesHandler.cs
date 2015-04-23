using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Messages;
using Messages.Approval;
using Messages.Trade;
using Rebus.Bus;
using Rebus.Handlers;

namespace Confirmation
{
    class ConfirmTradesHandler : IHandleMessages<TradeFinalized>
    {
        readonly IBus _bus;
        readonly ConcurrentQueue<char> _characterQueue;

        public ConfirmTradesHandler(IBus bus, ConcurrentQueue<char> characterQueue)
        {
            _bus = bus;
            _characterQueue = characterQueue;
        }

        public async Task Handle(TradeFinalized message)
        {
            Console.Write("Got trade: {0} - approve? (y/n) ", message);

            var done = false;

            while (!done)
            {
                var key = await GetNextChar();

                switch (key)
                {
                    case 'y':
                        Console.WriteLine("Approved!");
                        await _bus.Publish(new TradeApproved(message.Id));
                        done = true;
                        break;

                    case 'n':
                        Console.WriteLine("Rejected....");
                        await _bus.Publish(new TradeRejected(message.Id));
                        done = true;
                        break;
                }
            }
        }

        async Task<char> GetNextChar()
        {
            char c;

            while (!_characterQueue.TryDequeue(out c))
                await Task.Delay(100);

            return c;
        }
    }
}