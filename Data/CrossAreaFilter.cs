namespace SignalRFilterPlay.Data
{
    /// <summary>
    /// Filter mode for cross-area trade matching.
    /// </summary>
    public enum CrossAreaFilter
    {
        /// <summary>
        /// Include trades where either Buy or Sell area matches.
        /// </summary>
        Include,

        /// <summary>
        /// Include only trades where both Buy and Sell areas match.
        /// </summary>
        Exclude
    }
}
