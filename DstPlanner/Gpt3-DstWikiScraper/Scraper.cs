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

        private Dictionary<string, int> ExtractIngredients(IElement containerElement)
        {
            var ingredients = new Dictionary<string, int>();

            // Find the <div> element with class "pi-data-value" within the container element
            var ingredientsDivElement = containerElement.QuerySelector("div.pi-data-value");

            if (ingredientsDivElement != null)
            {
                // Get the text content of the <div> element
                var ingredientsText = ingredientsDivElement.TextContent;

                // Split the text by space to separate individual ingredients
                var ingredientTokens = ingredientsText.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < ingredientTokens.Length; i += 2)
                {
                    // Extract the ingredient name from the token (remove leading/trailing whitespace)
                    var ingredientName = ingredientTokens[i].Trim();

                    // Extract the quantity from the next token, skipping "×" and parsing as integer
                    if (i + 1 < ingredientTokens.Length && ingredientTokens[i + 1].StartsWith("×"))
                    {
                        var quantityText = ingredientTokens[i + 1].Substring(1);
                        if (int.TryParse(quantityText, out var quantity))
                        {
                            ingredients.Add(ingredientName, quantity);
                        }
                    }
                }
            }

            return ingredients;
        }


    }
}
