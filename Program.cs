using System;
using ScottPlot;

namespace sweproject {
    class Program {
        static void Main(string[] args) {
            DIP d = new DIP();
            int results;
            try {
                results = d.GetResults("Corona", new int[] {18,19});
                Console.WriteLine(results);
            } catch (ArgumentException) {
                // Ask user to provide a search term
                Console.WriteLine("Invalid search term");
            }
        }
    }
}
