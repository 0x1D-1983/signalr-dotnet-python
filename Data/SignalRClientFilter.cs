namespace SignalRFilterPlay.Data;

/// <summary>
/// Filter for the SignalR client.
/// </summary>
public class SignalRClientFilter
{
    /// <summary>
    /// Filter trades by Parent Market Area.
    /// </summary>
    public string? ParentMarketArea { get; set; }

    /// <summary>
    /// Filter trades by Buy and Sell Areas. The behaviour of this filter is controlled by the Cross Area Filter parameter. For a list of area values, please refer to this document: <see href="https://confluence.edftrading.com/spaces/DP/pages/259327388/Delivery+Areas">Delivery Areas - Data: Documentation and Glossaries - Confluence</see> 
    /// </summary>
    // public string? Area { get; set; }

    /// <summary>
    /// Cross Area Filter. Possible values are Include and Exclude. Default is Include. Include: Filter trades where either the Buy or Sell Areas match the Area parameter. Exclude: Filter trades where both Buy and Sell Areas are matching the Area parameter.
    /// </summary>
    // public CrossAreaFilter CrossAreaFilter { get; set; }
}