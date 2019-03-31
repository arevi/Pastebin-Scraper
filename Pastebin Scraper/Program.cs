using System.Collections.Generic;
using System.Net.Http;

namespace Pastebin_Scraper
{
    // This class servers as our main entry point and will provide the outline for the CLI the end user will interact with
    internal class Program
    {
        private static readonly HttpClient Client = new HttpClient();

        //Entry Point
        private static void Main(string[] args)
        {
            // Initializes a list of pastes to the return of the function scrape
            var pastes = Scrape();

            // Passes the return from the scrape function, a list of pastes, to the parser to be processed
            Parser.ParsePastes(pastes, "");
        }

        // Function to perform the general scraping of the latest public pastes from Pastebin
        private static List<Pastebin.Paste> Scrape()
        {
            // Creates scraper object that will be disposed upon completion
            using (var scraper = new Scraper(Client))
            {
                // Fetches list of pastes from scraper object
                scraper.FetchPasteList();

                // Returns the list of fetched pastes to be later parsed
                return scraper.RetrievePastes();
            }
        }
    }
}