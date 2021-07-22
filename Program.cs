using System;
using ScottPlot;

namespace sweproject {
    class Program {
        static void Main(string[] args) {
            DIP d = new DIP();
            int n;
            try {
                n = d.GetResults("Corona");
            } catch (ArgumentException) {
                // Ask user to provide a search term
                Console.WriteLine("Invalid search term");
            }

        }
    }
}
