namespace Messages.Trade
{
    public class TradeFinalized
    {
        public string Id { get; private set; }
        public string Commodity { get; private set; }

        public TradeFinalized(string id, string commodity)
        {
            Id = id;
            Commodity = commodity;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Id, Commodity);
        }
    }
}
