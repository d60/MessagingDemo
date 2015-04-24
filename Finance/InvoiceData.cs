using System;
using Rebus.Sagas;

namespace Finance
{
    public class InvoiceData : ISagaData
    {
        const string DefaultConfirmationStatus = "<awaiting confirmation>";

        public InvoiceData()
        {
            Description = "<no details available>";
            ConfirmationStatus = DefaultConfirmationStatus;
        }

        public Guid Id { get; set; }
        public int Revision { get; set; }

        public string TradeId { get; set; }
        public string Description { get; set; }
        public string ConfirmationStatus { get; set; }

        public bool ReadyForInvoicing
        {
            get
            {
                if (string.IsNullOrWhiteSpace(TradeId)) return false;

                if (ConfirmationStatus == DefaultConfirmationStatus) return false;

                return true;
            }
        }

        public void PrintStatus()
        {
            Console.WriteLine(@"
-------------------------------------
    Trade ID: {0}
      Status: {1}
 Description: {2}",
                TradeId,
                ConfirmationStatus,
                Description);
        }
    }
}