using System;
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
            // Variables to be used later
            var searchQuery = "";
            var isRunning = false;

            // While not running the scraping logic, loop menu
            while (!isRunning)
            {
                // Initial menu prompt + assignment of input selection
                Console.Clear();
                Console.WriteLine("-----Pastebin Scraper-----");
                Console.WriteLine("1) Define Search Query");
                Console.WriteLine("2) Start Scraping");
                Console.Write("Please enter your selection: ");
                var input = int.Parse(Console.ReadLine());

                // Switching on the input provided by the user
                switch (input)
                {
                    // Search query entry input
                    case 1:
                        Console.Clear();
                        Console.WriteLine("-----Pastebin Scraper-----");
                        Console.Write("Please enter your Query: ");
                        searchQuery = Console.ReadLine();
                        break;
                    // Start parsing input
                    case 2:
                        // Check to make sure we have a valid searchQuery
                        if (!string.IsNullOrWhiteSpace(searchQuery))
                        {
                            isRunning = true;
                            // Initializes a list of pastes to the return of the function scrape
                            var pastes = Scrape();
                            // Passes the return from the scrape function, a list of pastes, to the parser to be processed
                            Parser.ParsePastes(pastes, searchQuery);
                        }
                        else
                        {
                            // Search query must be invalid, or undefined, so return to main menu
                            Console.Clear();
                            Console.WriteLine("-----Pastebin Scraper-----");
                            Console.WriteLine("Error: You did not specify a search query.");
                            Console.Write("Press any key to return to the main menu...");
                            Console.ReadKey();
                        }

                        break;
                    // Catching invalid input
                    default:
                        Console.Clear();
                        break;
                }
            }
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