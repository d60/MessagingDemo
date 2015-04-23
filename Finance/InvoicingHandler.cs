using System.Threading.Tasks;
using Messages.Approval;
using Messages.Trade;
using Rebus.Sagas;

namespace Finance
{
    public class InvoicingHandler : Saga<InvoicingSagaData>,
        IAmInitiatedBy<TradeFinalized>,
        IAmInitiatedBy<TradeApproved>,
        IAmInitiatedBy<TradeRejected>
    {
        protected override void CorrelateMessages(ICorrelationConfig<InvoicingSagaData> config)
        {
            config.Correlate<TradeFinalized>(m => m.Id, s => s.TradeId);
            config.Correlate<TradeApproved>(m => m.Id, s => s.TradeId);
            config.Correlate<TradeRejected>(m => m.Id, s => s.TradeId);
        }

        public async Task Handle(TradeFinalized message)
        {
            if (Data.IsNew)
            {
                Data.TradeId = message.Id;
                Data.Description = string.Format("Trade: {0}", message);
            }

            Data.IsNew = false;

            Data.PrintStatus();
        }

        public async Task Handle(TradeApproved message)
        {
            Data.ConfirmationStatus = "Approved!       :)";

            Data.IsNew = false;

            Data.PrintStatus();
        }

        public async Task Handle(TradeRejected message)
        {
            Data.ConfirmationStatus = "Rejected....        :'(";

            Data.IsNew = false;

            Data.PrintStatus();
        }
    }
}