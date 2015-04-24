using System.Threading.Tasks;
using Messages.Approval;
using Messages.Trade;
using Rebus.Handlers;
using Rebus.Sagas;

namespace Finance
{
    public class InvoicingProcessManager : Saga<InvoiceData>,
        IAmInitiatedBy<TradeFinalized>,
        IHandleMessages<TradeApproved>,
        IHandleMessages<TradeRejected>
    {
        protected override void CorrelateMessages(ICorrelationConfig<InvoiceData> config)
        {
            config.Correlate<TradeFinalized>(m => m.Id, s => s.TradeId);
            config.Correlate<TradeApproved>(m => m.Id, s => s.TradeId);
            config.Correlate<TradeRejected>(m => m.Id, s => s.TradeId);
        }

        public async Task Handle(TradeFinalized message)
        {
            Data.TradeId = message.Id;
            Data.Description = string.Format("Trade: {0}", message);

            Data.PrintStatus();
        }

        public async Task Handle(TradeApproved message)
        {
            Data.ConfirmationStatus = "Approved!                    :)";

            Data.PrintStatus();
        }

        public async Task Handle(TradeRejected message)
        {
            Data.ConfirmationStatus = "Rejected....                :'(";

            Data.PrintStatus();
        }
    }

    public class VerifyReadyForInvoicing
    {
        public VerifyReadyForInvoicing(string tradeId)
        {
            TradeId = tradeId;
        }

        public string TradeId { get; private set; }
    }
}