using System.Text.Json.Serialization;

namespace Pastebin_Scraper.JSON_Classes
{
    internal class Pastebin
    {
        // Accepts json string, returns Paste[]
        internal static Paste[] BuildPasteList(string json)
        {
            // Uses native JsonSerializer to parse json data passed into an array of Paste[] objects
            return JsonSerializer.Parse<Paste[]>(json);
        }

        // Individual paste objects from Pastebin JSON
        public class Paste
        {
            public string scrape_url { get; set; }
            public string full_url { get; set; }
            public string date { get; set; }
            public string key { get; set; }
            public string size { get; set; }
            public string expire { get; set; }
            public string title { get; set; }
            public string syntax { get; set; }
            public string user { get; set; }
        }
    }
}