using DstPlanner.WikiScraper.Gpt3;

// See https://aka.ms/new-console-template for more information

Console.WriteLine("Starting Wiki Scraper");

var scraper = new Scraper();
var crockPot = scraper.ParseGameItem("https://dontstarve.wiki.gg/wiki/Crock_Pot");

Console.WriteLine("Result:");

Console.WriteLine($"\tID: {crockPot.GameItemId}");
Console.WriteLine($"\tName: {crockPot.Name}");
Console.WriteLine($"\tImage: {crockPot.ImgUrl ?? "None"}");

if (crockPot.Ingredients != null)
{
    Console.WriteLine($"\tIngredients: {crockPot.ImgUrl}");
    foreach (var ingredient in crockPot.Ingredients)
    {
        Console.WriteLine($"\t\t{ingredient.Key} x{ingredient.Value}");
    }
}
else
{
    Console.WriteLine($"\tIngredients: None");
}

