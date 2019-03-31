using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;

namespace Pastebin_Scraper
{
    // The scraper class provides the necessary functions for scraping the Pastebin API and storing individual pastes into a list
    internal class Scraper : IDisposable
    {
        private readonly HttpClient _client;

        // Private variable declarations
        private readonly List<Pastebin.Paste> _pastesList = new List<Pastebin.Paste>();

        // Constructor for the Scraper class, allows us to pass in a reusable client rather than initializing a new one repeatedly.
        public Scraper(HttpClient client)
        {
            _client = client;
        }

        // Allows our class to become disposable
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        // Function to perform get request to Pastebin API, retrieving either valid JSON or failure
        public void FetchPasteList()
        {
            // Assigns our response from the requestHandler function to a string, should return a string of either JSON or NULL
            var jsonData = RequestHandler("https://scrape.pastebin.com/api_scraping.php");

            // If the response string is valid, it'll be passed to StorePastes which will extract individual pastes and store them
            if (jsonData != null) StorePastes(Pastebin.BuildPasteList(jsonData));
        }

        // Function to take an array of paste objects, pass them to a parser to extract each paste
        // The extracted paste string for each paste object is then stored globally in a list of strings
        public void StorePastes(Pastebin.Paste[] pastes)
        {
            // Clears contents of current paste list
            _pastesList.Clear();

            // Iterates over the passed array of Paste objects
            foreach (var paste in pastes)
            {
                // Using ExtractPaste function to return a string of either JSON data or null
                // If JSON is returned, the JSON will be saved to our list of pastes
                var data = ExtractPaste(paste);
                if (data != null) _pastesList.Add(data);

                // THIS IS CRITICAL
                // To respect Pastebins API rules, we will make roughly 1 request per second
                // By default this delay is set to 800ms, to account for req/res time.
                Thread.Sleep(800);
            }
        }

        // Function to accept a Paste object and return the string resolved from the individual paste body of content
        public Pastebin.Paste ExtractPaste(Pastebin.Paste paste)
        {
            paste.content = RequestHandler(paste.scrape_url);
            return paste;
        }

        // Provides a public method to extract the list of individual paste strings from the scraper object
        public List<Pastebin.Paste> RetrievePastes()
        {
            return _pastesList;
        }

        public string RequestHandler(string url)
        {
            // Initializes the string that will store our data to return
            var data = "";

            // Performs get request to Pastebin server and returns server response
            var pastebinRes = _client.GetAsync(url).Result;

            // Sets our data string to the body content of the response
            data = pastebinRes.Content.ReadAsStringAsync().Result;

            // To avoid inefficiencies, error handling structure assumes a valid response in the first case
            if (pastebinRes.IsSuccessStatusCode && !data.Contains("DOES NOT HAVE ACCESS"))
                return data;

            // Response indicates user has not whitelisted their IP, per Pastebin requirements
            if (data.Contains("DOES NOT HAVE ACCESS"))
            {
                Console.WriteLine(
                    "Your IP has not been whitelisted by Pastebin, please visit: https://pastebin.com/doc_scraping_api to get access.");
                return null;
            }

            // If neither case above are true, there must be some kind of status code indicating failure
            Console.WriteLine("There was an error processing your request to Pastebin, status code: " +
                              pastebinRes.StatusCode);
            return null;
        }
    }
}