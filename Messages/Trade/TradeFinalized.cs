namespace Messages.Trade
{
    public class TradeFinalized
    {
        public TradeFinalized(string id, string commodity)
        {
            Id = id;
            Commodity = commodity;
        }

        public string Id { get; private set; }
        public string Commodity { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Id, Commodity);
        }
    }
}
