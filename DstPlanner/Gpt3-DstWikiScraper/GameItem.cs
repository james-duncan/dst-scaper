namespace DstPlanner.WikiScraper.Gpt3;

public record GameItem
{
    /// <summary>
    /// The name of the GameItem.
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// Link to the image of the game item.
    /// </summary>
    string ImgUrl { get; set; }

    /// <summary>
    /// List of game items required to craft this game item.
    /// </summary>
    List<GameItem>? Ingredients { get; set; }

    /// <summary>
    /// Required tools to craft this game item.
    /// </summary>
    GameItem? Prerequisite { get; set; }
}
