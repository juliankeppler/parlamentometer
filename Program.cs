using System;
using System.Collections.Generic;

namespace sweproject {
    class Program {
        static void Main(string[] args) {
            DIP dip = new DIP();
            GroupMode mode = GroupMode.Year;
            var (term, periods) = GetUserInput(dip);
            if (periods.Length == 1) {
                mode = GroupMode.Month;
            }

            SortedDictionary<string, int> buckets = new SortedDictionary<string, int>();
            try {
                buckets = dip.GetRelevance(term, mode, periods);
            } catch (ArgumentException) {
                Console.WriteLine("Invalid search term (1)");
            } catch (IndexOutOfRangeException) {
                Console.WriteLine("Für diesen Suchbegriff wurden im gegebenen Zeitraum keine Ergebnisse gefunden.");
            }

            Plotter.Plot(buckets, mode);
        }
        static (string, int[]) GetUserInput(DIP dip) {

            string term = "";
            int[] periods = null;
            bool validInput = false;

            while (!validInput) {
                Console.WriteLine("Bitte geben sie einen Suchbegriff ein: ");
                term = Console.ReadLine();
                Console.WriteLine("Bitte geben sie eine oder mehrere Wahlperioden ein (Bsp. 13, 18, 19): ");
                string speriod = Console.ReadLine();

                if (speriod != "") {
                    string[] arr = speriod.Split(",");
                    periods = Array.ConvertAll(arr, s => int.Parse(s));
                } else {
                    periods = new int[]{};
                }

                SortedDictionary<string, int> buckets = new SortedDictionary<string, int>();
                int results = 0;
                try {
                    results = dip.GetResults(term, periods);
                    Console.WriteLine(results);
                } catch (ArgumentException) {
                    Console.WriteLine("Invalid search term (2)");
                }

                Console.WriteLine("Durchsuche DIP nach Redebeiträgen mit dem Wort {0}...", term);
                switch (results) {
                case int n when n > 10000:
                    Console.WriteLine("Für diese Parameter gibt es zu viele Treffer,\nbitte wählen Sie einen anderen Suchbegriff oder ändern sie den Suchzeitraum (Wahlperioden).");
                    break;
                case int n when n > 2000:
                    Console.WriteLine("Viele Treffer, dies kann einen Moment dauern.");
                    validInput = true;
                    break;
                default: 
                    validInput = true;
                    break;
                }
            }
            return (term, periods);
        }
    }
}
