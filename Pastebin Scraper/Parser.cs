using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Pastebin_Scraper
{
    // The parser class will provide the basic functionality for parsing individual pastes for our predefined search terms
    internal static class Parser
    {
        // Static function to parse a list of paste objects, along with a string to search for
        public static void ParsePastes(List<Pastebin.Paste> pastes, string pattern, bool isRegex = false)
        {
            // Initializes new List of strings, for storing URLs later
            var urlList = new List<string>();

            // Iterates over each paste 
            foreach (var paste in pastes)
                // Separates Regex logic from normal string search logic
                if (!isRegex)
                {
                    // Checks if individual paste contains search pattern string
                    if (paste.content.Contains(pattern))
                        urlList.Add(paste.scrape_url);
                }
                else
                {
                    // Checks if individual paste has regex pattern that matches, the one provided one function call
                    if (Regex.IsMatch(paste.content, pattern, RegexOptions.IgnoreCase))
                        urlList.Add(paste.scrape_url);
                }

            // Passes list of URLS to function to be saved to text file
            SavePasteUrls(urlList);
        }

        // Function to save a list of URLs to a text file
        private static void SavePasteUrls(IEnumerable<string> urls)
        {
            // Initializes a new disposable stream writer
            using (var writer = new StreamWriter("output.txt"))
            {
                // Iterates over the list of URL strings passed to function
                foreach (var url in urls)
                    // Writes the the individual URL to the text file line
                    writer.WriteLine(url);
            }
        }
    }
}