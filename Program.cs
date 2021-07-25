using System;
using System.Collections.Generic;

namespace sweproject {
    class Program {
        static void Main(string[] args) {
            DIP dip = new DIP();
            var (term, gmode, periods) = GetUserInput(dip);
            SortedDictionary<string, int> buckets = new SortedDictionary<string, int>();
            try {
                buckets = dip.GetRelevance(term, gmode, periods);
                // foreach (KeyValuePair<string, int> kvp in buckets) {
                //    Console.WriteLine("Month={0}, Mentions={1}", kvp.Key, kvp.Value);
                // }
            } catch (ArgumentException) {
                // Ask user to provide a search term
                Console.WriteLine("Invalid search term (1)");
            } catch (IndexOutOfRangeException) {
                Console.WriteLine("Für diesen Suchbegriff wurden im gegebenen Zeitraum keine Ergebnisse gefunden.");
            }
            
            Plotter.Plot(buckets, gmode);
        }
        static (string, GroupMode, int[]) GetUserInput(DIP dip) {

            GroupMode gmode = GroupMode.Year;
            string term = "";
            int[] periods = null;
            bool validInput = false;

            while (!validInput) {
                Console.WriteLine("Bitte geben sie einen Suchbegriff ein: ");
                term = Console.ReadLine();
                Console.WriteLine("Bitte geben sie eine oder mehrere Wahlperioden ein (Bsp. 13, 18, 19): ");
                string speriod = Console.ReadLine();
                string[] arr = speriod.Split(",");
                periods = Array.ConvertAll(arr, s => int.Parse(s));

                bool quitloop = false;
                while (!quitloop) {
                    Console.WriteLine("Bitte wählen sie einen Modus: 'M' für Monat, 'J' für Jahr.");
                    string modeInput = Console.ReadLine().ToLower();
                    switch (modeInput) {
                    case "m":
                        gmode = GroupMode.Month;
                        quitloop = true;
                        break;
                    case "j": 
                        gmode = GroupMode.Year;
                        quitloop = true;
                        break;
                    default:
                        //Console.WriteLine("");
                        break;
                    }
                }
                SortedDictionary<string, int> buckets = new SortedDictionary<string, int>();
                int results = 0;
                try {
                    results = dip.GetResults(term, periods);
                    Console.WriteLine(results);
                } catch (ArgumentException) {
                    // Ask user to provide a search term
                    Console.WriteLine("Invalid search term (2)");
                }
                switch (results) {
                case int n when n > 10000:
                    Console.WriteLine("Für diese Parameter gibt es zu viele Treffer,\nbitte wählen Sie einen anderen Suchbegriff oder ändern sie den Suchzeitraum (Wahlperioden).");
                    break;
                case int n when n > 2000:
                    Console.WriteLine("Viele Treffer, dies kann einen Moment dauern.");
                    validInput = true;
                    break;
                default: 
                    Console.WriteLine("Durchsuche DIP nach Redebeiträgen mit dem Wort {0}...", term);
                    validInput = true;
                    break;
                }
            }
            return (term, gmode, periods);
        }
    }
}