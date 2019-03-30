using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using Microsoft.Win32.SafeHandles;
using Pastebin_Scraper.JSON_Classes;

namespace Pastebin_Scraper
{
    internal class Scraper
    {
        List<Pastebin.Paste> PastesList = new List<Pastebin.Paste>();
        public void FetchPasteList()
        {
            // Creating new HttpClient and then disposing once operation is complete
            using (var client = new HttpClient())
            {
                // Performs asynchronous get request to Pastebin scraping API and returns response message
                var pastebinRes = client.GetAsync("https://scrape.pastebin.com/api_scraping.php").Result;

                // Checks if response code indicates failure
                if (!pastebinRes.IsSuccessStatusCode)
                {
                    // Prints error message and prints status code of error
                    Console.WriteLine("There was an error processing your request to Pastebin, status code: " + pastebinRes.StatusCode);
                }
                else
                {
                    // Upon success, reads and stores response body
                    var jsonData = pastebinRes.Content.ReadAsStringAsync().Result;

                    storePastes(Pastebin.BuildPasteList(jsonData));
                }
            }
        }

        public void storePastes(Pastebin.Paste[] pastes)
        {
            // Clears contents of current paste list
            PastesList.Clear();

            // Iterates over Paste array passed and assigns to current paste list
            PastesList.AddRange(pastes);
        }


    }
}
