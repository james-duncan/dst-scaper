using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AngleSharp;
using AngleSharp.Dom;

namespace DstPlanner.WikiScraper.Gpt3
{
    public class Scraper
    {
        public GameItem ParseGameItem(string pageUrl)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = context.OpenAsync(pageUrl).GetAwaiter().GetResult();

            var gameItem = new GameItem();

            // Find the <h2> element with attribute "data-source"="box title"
            var nameElement = document.QuerySelector($"h2[data-source='Box title']");
            if (nameElement != null)
            {
                gameItem.Name = nameElement.TextContent.Trim();
            }
            else
            {
                throw new InvalidOperationException("Name element not found on the page.");
            }

            // Find the <img> element within the <figure> element with class "pi-image"
            var imgElement = document.QuerySelector("figure.pi-image img");
            if (imgElement != null)
            {
                gameItem.ImgUrl = imgElement.GetAttribute("src");
            }
            else
            {
                throw new InvalidOperationException("Image element not found on the page.");
            }

            // Find the <div> element with attribute "data-source"="spawnCode"
            var codeElement = document.QuerySelector($"div[data-source='spawnCode'] code");
            if (codeElement != null)
            {
                gameItem.GameItemId = codeElement.TextContent.Trim('"');
            }
            else
            {
                throw new InvalidOperationException("Code element not found on the page.");
            }

            // Find the <div> element with attribute "data-source"="ingredient1"
            var ingredientsElement = document.QuerySelector($"div[data-source='ingredient1']");
            if (ingredientsElement != null)
            {
                gameItem.Ingredients = ExtractIngredients(ingredientsElement);
            }

            // Additional logic to extract other properties like Prerequisite

            return gameItem;
        }

        private Dictionary<string, int> ExtractIngredients(IElement ingredientsElement)
        {
            var ingredients = new Dictionary<string, int>();

            // Find all <a> elements within the <div> element
            var ingredientLinks = ingredientsElement.QuerySelectorAll("div.pi-data-value a");

            foreach (var ingredientLink in ingredientLinks)
            {
                // Get the URL from the href attribute
                var ingredientUrl = ingredientLink.GetAttribute("href");

                // Get the quantity from the text content (e.g., "×3")
                var quantityText = ingredientLink.TextContent;

                // Extract the quantity from the text (assuming it's always in the format "×{number}")
                var quantityMatch = Regex.Match(quantityText, @"×(\d+)");
                if (quantityMatch.Success && int.TryParse(quantityMatch.Groups[1].Value, out var quantity))
                {
                    ingredients.Add(ingredientUrl, quantity);
                }
            }

            return ingredients;
        }

    }
}
