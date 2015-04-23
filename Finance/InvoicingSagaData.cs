using System;
using Rebus.Sagas;

namespace Finance
{
    public class InvoicingSagaData : ISagaData
    {
        public InvoicingSagaData()
        {
            IsNew = true;
            Description = "<no details available>";
            ConfirmationStatus = "<awaiting confirmation>";
        }

        public Guid Id { get; set; }
        public int Revision { get; set; }

        public bool IsNew { get; set; }

        public string TradeId { get; set; }
        public string Description { get; set; }
        public string ConfirmationStatus { get; set; }

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