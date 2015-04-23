namespace Messages.Approval
{
    public class TradeApproved
    {
        public TradeApproved(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }

        public override string ToString()
        {
            return string.Format("trade {0} OK!", Id);
        }
    }
}