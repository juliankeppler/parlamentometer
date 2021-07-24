using System;
using System.Collections.Generic;

namespace sweproject {
    class Program {
        static void Main(string[] args) {

            string term = "Bier";
            DIP dip = new DIP();
            SortedDictionary<string, int> buckets = new SortedDictionary<string, int>();
            try {
                int results = dip.GetResults(term, new int[] {19});
                Console.WriteLine(results);
            } catch (ArgumentException) {
                // Ask user to provide a search term
                Console.WriteLine("Invalid search term");
            }
            try {
                buckets = dip.GetRelevance(term, GroupMode.Month, new int[] {19});
                foreach (KeyValuePair<string, int> kvp in buckets) {
                    Console.WriteLine("Month={0}, Mentions={1}", kvp.Key, kvp.Value);
                }
            } catch (ArgumentException) {
                // Ask user to provide a search term
                Console.WriteLine("Invalid search term");
            }

            foreach (string date in buckets.Keys) {
                Console.WriteLine(date);
            }
        }
    }
}