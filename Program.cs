using System;
using System.Collections.Generic;
using System.Linq;

namespace sweproject {
    class Program {
        static void Main(string[] args) {

            DIP dip = new DIP();
            GroupMode mode = GroupMode.Year;

            // Ask user to provide a search term and search time frame
            var (term, periods) = GetUserInput(dip);
            if (periods.Length == 1) {
                mode = GroupMode.Month;
            }


            // Parse DIP API for mentions of the term
            SortedDictionary<string, int> buckets = new SortedDictionary<string, int>();
            try {
                buckets = dip.GetRelevance(term, mode, periods);
            } catch (ArgumentException) {
                Console.WriteLine("Invalid search term (1)");
            } catch (IndexOutOfRangeException) {
                Console.WriteLine("Für diesen Suchbegriff wurden im gegebenen Zeitraum keine Ergebnisse gefunden.");
            }
            
            // Create the diagram
            Plotter.Plot(term, buckets, mode);
        }

        static (string, int[]) GetUserInput(DIP dip) {

            // declare return variables
            string term = "";
            int[] periods = null;


            ReadTerm:
            // Ask the user to provide a search term
            Console.WriteLine("Bitte geben sie einen Suchbegriff ein: ");
            term = Console.ReadLine();

            if (term == "") {
                // User didn't provide a search term
                goto ReadTerm;
            }


            ReadPeriods:
            // Ask the user to provide a time frame
            Console.WriteLine("Bitte geben sie eine oder mehrere Legislaturperioden ein (Bsp. '19' oder '17, 18, 19'). Wir befinden uns derzeit in der 19. Legislaturperiode: ");
            string speriod = Console.ReadLine();

            if (speriod == "") {
                periods = new int[]{}; // if left blank all electionperiods are queried
            } else {
                string[] arr = speriod.Split(",");
                try {
                    periods = Array.ConvertAll(arr, s => int.Parse(s));
                } catch (FormatException) {
                    // User entered some silly non integer values
                    Console.WriteLine("Der eingegebene Wert bezeichnet keine gültigen Legislaturperioden.");
                    goto ReadPeriods;
                }
            }

            // Check if user provided valid election periods
            if (periods.Min() < 1) {
                Console.WriteLine("Der eingegebene Wert bezeichnet keine gültigen Legislaturperioden.");
                goto ReadPeriods;
            } else if (periods.Max() > 19) {
                Console.WriteLine("Der eingegebene Wert bezeichnet keine gültigen Legislaturperioden.");
                goto ReadPeriods;
            }


            // Query the DIP API for amount of results for given term in given time frame
            Console.WriteLine("Durchsuche DIP nach Redebeiträgen mit dem Wort {0}...", term);
            SortedDictionary<string, int> buckets = new SortedDictionary<string, int>();
            int results = 0;
            try {
                results = dip.GetResults(term, periods);
                Console.WriteLine(results);
            } catch (ArgumentException) {
                Console.WriteLine("Der eingebene Wert ist kein gültiger Suchbegriff.");
                goto ReadTerm;
            }

            switch (results) {
            case int n when n > 10000:
                Console.WriteLine("Für diese Parameter gibt es zu viele Treffer. Bitte wählen Sie einen anderen Suchbegriff oder ändern Sie den Suchzeitraum (Wahlperioden).");
                goto ReadTerm;
            case 0:
                Console.WriteLine("Für den angegebenen Suchbegriff konnten im gewählten Zeitraum keine Suchergebnisse gefunden werden. Versuchen Sie es mit einem anderen Suchbegriff oder Zeitraum erneut.");
                goto ReadTerm;
            case int n when n > 2000:
                Console.WriteLine("Für den Suchbegriff wurden viele Treffer gefunden, dies kann einen Moment dauern.");
                break;
            default:
                break;
            }
            return (term, periods);
        }
    }
}
