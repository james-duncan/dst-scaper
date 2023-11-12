using System;
using System.Collections.Generic;
using System.Linq;
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
                // Handle the case when the element is not found
                throw new InvalidOperationException("Name element not found on the page.");
            }

            // Additional logic to extract other properties like ImgUrl, Ingredients, and Prerequisite

            return gameItem;
        }
    }
}
