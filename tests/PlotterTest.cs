using System;
using Xunit;
using sweproject;
using System.IO;
using System.Collections.Generic;

namespace tests {
    public class PlotterTest {
        
        [Theory]
        [MemberData(nameof(TestPlotCreatesFileData))]
        public void TestPlotCreatesFile(string term, SortedDictionary<string, int> data, GroupMode mode) {
            Plotter.Plot(term, data, mode);
            Assert.True(File.Exists("graph.png"));
        }

        public static IEnumerable<object[]> TestPlotCreatesFileData => new List<object[]>{
            new object[] {
                "Corona",
                new SortedDictionary<string, int>(){
                    {"2017", 3},{"2018",0},{"2019",0},{"2020",0},{"2021",4}
                },
                GroupMode.Year,
            }, // Test plotting of GroupMode.Year
            new object[] {
                "Corona",
                new SortedDictionary<string, int>(){
                    {"2017-11", 3},{"2017-12", 0},{"2018-01", 0},{"2018-02", 0},{"2018-03", 0},{"2018-04", 0},{"2018-05", 123},{"2018-06", 0},
                    {"2018-07", 0},{"2018-08", 0},{"2018-09", 0},{"2018-10", 5},{"2018-11", 0},{"2018-12", 8},{"2019-01", 0},{"2019-02", 0},
                    {"2019-03", 0},{"2019-04", 123},{"2019-05", 0},{"2019-06", 0},{"2019-07", 0},{"2019-08", 0},{"2019-09", 0},{"2019-10", 0},
                    {"2019-11", 0},{"2019-12", 0},{"2020-01", 0},{"2020-02", 0},{"2020-03", 0},{"2020-04", 0},{"2020-05", 0},{"2020-06", 0},
                    {"2020-07", 0},{"2020-08", 123},{"2020-09", 0},{"2020-10", 0},{"2020-11", 7},{"2020-12", 0},{"2021-01", 0},{"2021-02", 0},
                    {"2021-03", 0},{"2021-04", 0},{"2021-05", 0},{"2021-06", 0},{"2021-07", 0},{"2021-08", 0},{"2021-09", 0}
                }, 
                GroupMode.Month
            }, // Test plotting of GroupMode.Month
        };
    }
}
