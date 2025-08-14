namespace SignalRFilterPlay
{
    /// <summary>
    /// Represents an EPEX trade
    /// </summary>
    public class EpexTrade
    {
        /// <summary>
        /// The contract identifier
        /// </summary>
        public string ContractId { get; set; } = string.Empty;

        /// <summary>
        /// The parent market area
        /// </summary>
        public string ParentMarketArea { get; set; } = string.Empty;

        /// <summary>
        /// The buy area
        /// </summary>
        public string BuyArea { get; set; } = string.Empty;

        /// <summary>
        /// The sell area
        /// </summary>
        public string SellArea { get; set; } = string.Empty;

        /// <summary>
        /// The trade price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The trade volume
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        /// The time of the trade
        /// </summary>
        public DateTime TradeTime { get; set; }

        /// <summary>
        /// Checks if the trade matches the area filters
        /// </summary>
        public bool WhereAreaMatch(string? parentMarketArea, string? area, CrossAreaFilter crossAreaFilter)
        {
            if (string.IsNullOrEmpty(parentMarketArea) && string.IsNullOrEmpty(area))
                return true;

            if (!string.IsNullOrEmpty(parentMarketArea) && ParentMarketArea != parentMarketArea)
                return false;

            if (string.IsNullOrEmpty(area))
                return true;

            return crossAreaFilter switch
            {
                CrossAreaFilter.Include => BuyArea == area || SellArea == area,
                CrossAreaFilter.Exclude => BuyArea == area && SellArea == area,
                _ => true
            };
        }
    }
}
