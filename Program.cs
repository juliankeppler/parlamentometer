using System;
using ScottPlot;

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
                dip.GetRelevance(term);
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
