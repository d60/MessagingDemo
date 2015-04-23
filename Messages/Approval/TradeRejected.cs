namespace Messages.Approval
{
    public class TradeRejected
    {
        public TradeRejected(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }

        public override string ToString()
        {
            return string.Format("trade {0} not ok...", Id);
        }
    }
}