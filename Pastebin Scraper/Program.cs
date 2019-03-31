using System;

namespace Pastebin_Scraper
{
    // This class servers as our main entry point and will provide the outline for the CLI the end user will interact with
    internal class Program
    {
        //Entry Point
        private static void Main(string[] args)
        {
            var scraper = new Scraper();
            scraper.FetchPasteList();
            Console.WriteLine("lol");
        }
    }
}