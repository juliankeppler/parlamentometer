using System;
using ScottPlot;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Drawing;

public static class Plotter {

    // Diagram colors
    static private Color graphColor = Color.FromArgb(255, 33, 150, 243);
    static private Color labelColor = Color.FromArgb(150, Color.Black);
    static private Color gridColor = Color.FromArgb(40, Color.Black);

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
    private static (double[], double[]) ConvertInput(SortedDictionary<string, int> buckets, GroupMode mode) {
        double[] xarray = XToDouble(buckets, mode);
        double[] yarray = YToDouble(buckets);
    
        return (xarray, yarray);
    }
    
    //Printing Graph
    public static void Plot(string term, SortedDictionary<string, int> buckets, GroupMode mode){
        
        var (dataX, dataY) = ConvertInput(buckets, mode);

        // Diagram dimensions
        var plt = new ScottPlot.Plot(1080, 400);
        plt.SetAxisLimits(dataX.Min(), dataX.Max(), dataY.Min(), dataY.Max()+0.1*dataY.Max());
        plt.AddScatter(dataX, dataY, lineWidth: 4, markerSize: 0, color:graphColor);

        // Diagram styling
        plt.XAxis.DateTimeFormat(true);
        plt.XAxis.Grid(false);
        plt.XAxis.TickLabelStyle(color: labelColor);
        plt.XAxis.TickDensity(0.5);
        plt.XAxis.TickMarkColor(Color.FromArgb(0, Color.Black));
        plt.YAxis.Ticks(true, false, true);
        plt.YAxis.Label($"Reden in denen {term} erwähnt wurde");
        plt.YAxis.TickLabelStyle(color: labelColor);
        plt.YAxis.TickMarkColor(gridColor);
        plt.YAxis.TickDensity(0.8);
        plt.SetCulture(CultureInfo.CreateSpecificCulture("de-DE"));
        plt.Grid(color: gridColor);
        plt.Frame(left: false, bottom: true, top: false, right: false);
        plt.XAxis2.Label($"{term} im Bundestag", bold: false, size: 22);
        // Export diagram
        plt.SaveFig("graph.png");
    }
}