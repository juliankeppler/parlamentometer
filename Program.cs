using System;
using ScottPlot;
using System.Collections.Generic;

namespace sweproject {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Start");
            DateTime starttime = DateTime.Now;

            //////
            string term = "corona";
            DIP dip = new DIP();
            int results;
            try {
                results = dip.GetResults(term);
                Console.WriteLine(results);
            } catch (ArgumentException) {
                // Ask user to provide a search term
                Console.WriteLine("Invalid search term");
            }
            try {
                Dictionary<string, int> buckets = dip.GetRelevance(term, GroupMode.Month);
                foreach (KeyValuePair<string, int> kvp in buckets) {
                    Console.WriteLine("Month={0}, Mentions={1}", kvp.Key, kvp.Value);
                }

            } catch (ArgumentException) {
                // Ask user to provide a search term
                Console.WriteLine("Invalid search term");
            }
            /////
            
            DateTime endtime = DateTime.Now;
            TimeSpan runtime = endtime - starttime;
            Console.WriteLine(runtime.ToString());
        }
    }
}
