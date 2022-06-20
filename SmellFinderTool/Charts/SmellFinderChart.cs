using Spectre.Console;

namespace SmellFinderTool.Charts
{
    public class SmellFinderChart : IBarChartItem
    {
        public string Label { get; set; }
        public double Value { get; set; }
        public Color? Color { get; set; }

        public SmellFinderChart(string label, double value, Color? color = null)
        {
            Label = label;
            Value = value;
            Color = color;
        }
    }
}
