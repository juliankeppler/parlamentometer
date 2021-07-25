using System;
using ScottPlot;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;

public static class Plotter {

    //X-Value Conversion
    private static double[] XToDouble(SortedDictionary<string, int> buckets, GroupMode mode) {

        string format = "";
        switch (mode) {
        case GroupMode.Year:
            format = "yyyy";
            break;
        case GroupMode.Month:
            format = "yyyy-MM";
            break;
        }

        DateTime[] dates = new DateTime[]{};
        foreach (string key in buckets.Keys) {
            dates = dates.Append(DateTime.ParseExact(key, format, CultureInfo.InvariantCulture)).ToArray();
        }

        double[] doubles = dates.Select(x => x.ToOADate()).ToArray();
        return doubles;
    }

    //Y-Value Conversion
    private static double[] YToDouble(SortedDictionary<string, int> buckets) {
        int[] ys = new int[]{};
        foreach (int value in buckets.Values) {
            ys = ys.Append(value).ToArray();
        }

        double[] doubles = ys.Select(Convert.ToDouble).ToArray();

        return doubles;
    }      
    
    //Calling Conversion method depending on mode
    private static Tuple<double[], double[]> ConvertInput(SortedDictionary<string, int> buckets, GroupMode mode) {
        double[] xarray = XToDouble(buckets, mode);
        double[] yarray = YToDouble(buckets);
    
        return Tuple.Create(xarray, yarray);
    }
    
    //Printing Graph
    public static void Plot(string term, SortedDictionary<string, int> buckets, GroupMode mode){
        var (dataX, dataY) = ConvertInput(buckets, mode);
        // foreach(double x in dataX) {
        //     Console.WriteLine(x);
        // }
        var plt = new ScottPlot.Plot(1080, 400);
        plt.SetAxisLimits(dataX.Min(), dataX.Max(), dataY.Min(), dataY.Max()+0.1*dataY.Max());
        // var psi = new ScottPlot.Statistics.Interpolation.NaturalSpline(dataX, dataY, resolution: 5);
        // plt.AddScatter(psi.interpolatedXs, psi.interpolatedYs, lineWidth: 2, markerSize: 0);
        plt.XAxis.DateTimeFormat(true);
        plt.YAxis.Label($"Reden in denen {term} erwähnt wurde");
        plt.XAxis.Grid(false);
        plt.YAxis.Ticks(true, false, true);
        plt.YAxis.TickDensity(0.8);
        plt.Grid(color: Color.FromArgb(40, Color.Black));
        plt.Frame(left: false, bottom: true, top: false, right: false);
        plt.Title($"{term} im Bundestag");
        plt.AddScatter(dataX, dataY, lineWidth: 3, markerSize: 0, color:Color.FromArgb(255, 33, 150, 243));
        plt.SaveFig("graph.png");
    }
}