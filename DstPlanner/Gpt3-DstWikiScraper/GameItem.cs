namespace DstPlanner.WikiScraper.Gpt3;

public record GameItem
{
    /// <summary>
    /// The game item identifier.
    /// </summary>
    public string GameItemId { get; set; }

    /// <summary>
    /// The name of the GameItem.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Link to the image of the game item.
    /// </summary>
    public string ImgUrl { get; set; }

    /// <summary>
    /// List of game items required to craft this game item.
    /// </summary>
    public Dictionary<string, int>? Ingredients { get; set; }

    /// <summary>
    /// Required tools to craft this game item.
    /// </summary>
    public GameItem? Prerequisite { get; set; }
}
