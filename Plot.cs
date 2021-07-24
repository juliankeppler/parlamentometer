using System;
using ScottPlot;
using System.Linq;
using System.Collections.Generic;


public static class Plotter {

    //X-Value Conversion Months
    private static double[] MonthToDouble(SortedDictionary<string, int> buckets) {
        string sarray = "";
        foreach (string key in buckets.Keys) {
            sarray+=key+",";
        }
        sarray = sarray.Remove(sarray.Length-1, 1);
        string[] temparr = sarray.Split(",");
        sarray = "";
        foreach (string key in temparr) {
            var x = key.Split("-");
            if (Convert.ToDouble(x[1])/12 == 1) {
                sarray += x[0] + ",";
            } else {
                sarray += Convert.ToDouble(x[0]) -(1-Convert.ToDouble(x[1])/12) + ",";
            }
        }
        sarray = sarray.Remove(sarray.Length-1, 1);
        string[] arr = sarray.Split(",");
        double[] doubles = Array.ConvertAll(arr, new Converter<string, double>(Double.Parse));

        return doubles;
    }

    //X-Value Conversion Years
    private static double[] YearToDouble(SortedDictionary<string, int> buckets) {
        string sarray = "";
        foreach (string key in buckets.Keys) {
            sarray+=key+",";
        }
        sarray = sarray.Remove(sarray.Length-1, 1);
        string[] arr = sarray.Split(",");
        double[] doubles = Array.ConvertAll(arr, new Converter<string, double>(Double.Parse));

        return doubles;
    }

    //Y-Value Conversion
    private static double[] ValuesToDouble(SortedDictionary<string, int> buckets) {
        string sarray = "";
        foreach (int key in buckets.Values) {
            sarray+=key+",";
        }
        sarray = sarray.Remove(sarray.Length-1, 1);
        string[] arr = sarray.Split(",");
        double[] doubles = Array.ConvertAll(arr, new Converter<string, double>(Double.Parse));

        return doubles;
    }      
    
    //Calling Conversion method depending on mode
    private static Tuple<double[], double[]> ConvertInput(SortedDictionary<string, int> buckets, GroupMode mode) {
        double[] xarray;
        if (mode == GroupMode.Month) {
            xarray = MonthToDouble(buckets);
        }
        else {
            xarray = YearToDouble(buckets);
        }
        double[] yarray = ValuesToDouble(buckets);
    
        return Tuple.Create(xarray, yarray);
    }
    
    //Printing Graph
    public static void Plot(SortedDictionary<string, int> buckets, GroupMode mode){
        var (dataX, dataY) = ConvertInput(buckets, mode);
        foreach(double x in dataX) {
            Console.WriteLine(x);
        }
        var plt = new ScottPlot.Plot(1920, 1080);
        plt.SetAxisLimits(dataX.Min(), dataX.Max(), dataY.Min(), dataY.Max()+0.1*dataY.Max());
        plt.AddScatter(dataX, dataY, lineWidth: 3, markerSize: 0);
        plt.SaveFig("Graph0.png");
    }
}