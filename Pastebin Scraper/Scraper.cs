using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Pastebin_Scraper
{
    internal class Scraper
    {
        private readonly List<Pastebin.Paste> _pastesList = new List<Pastebin.Paste>();

        // Function to perform get request to Pastebin API, retrieving either valid JSON or failure
        public void FetchPasteList()
        {
            // Initialize jsonData string that will later store valid json
            var jsonData = "";

            // Creating new HttpClient and then disposing once operation is complete
            using (var client = new HttpClient())
            {
                // Performs asynchronous get request to Pastebin scraping API and returns response message
                var pastebinRes = client.GetAsync("https://scrape.pastebin.com/api_scraping.php").Result;

                // Parsing content from response, Pastebin returns status 200 even if IP is unauthorized
                jsonData = pastebinRes.Content.ReadAsStringAsync().Result;

                // Checks if response code indicates failure
                if (!pastebinRes.IsSuccessStatusCode)
                    Console.WriteLine("There was an error processing your request to Pastebin, status code: " +
                                      pastebinRes.StatusCode);
                else if (jsonData.Contains("DOES NOT HAVE ACCESS"))
                    Console.WriteLine(
                        "Your IP has not been whitelisted by Pastebin, please visit: https://pastebin.com/doc_scraping_api to get access.");
                else
                    StorePastes(Pastebin.BuildPasteList(jsonData));
            }
        }

        // Stores [] of Paste objects in a central paste list
        public void StorePastes(Pastebin.Paste[] pastes)
        {
            // Clears contents of current paste list
            _pastesList.Clear();

            // Iterates over Paste array passed and assigns to current paste list
            _pastesList.AddRange(pastes);
        }
    }
}