using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LiveCharts; //Core of the library
using LiveCharts.Wpf; //The WPF controls
using LiveCharts.WinForms; //the WinForm wrappers
using LiveCharts.Defaults;
using System.Windows.Forms;
using System.Drawing;
using Approved.DatabaseConnection;

namespace Approved.Charts
{
    class PieChart
    {
        LiveCharts.WinForms.PieChart chart;
        private List<productPreference> preference;
        private Connection_Handler connection;

        public PieChart()
        {
            chart = new LiveCharts.WinForms.PieChart();
            connection = new Connection_Handler();
        }

        public void createPieChart(string product, Panel panel)
        {
            panel.Controls.Add(chart);

            chart.Width = 300;
            chart.Height = 300;
            chart.Location = new Point(850, 75);
            
            Func<ChartPoint, string> labelPoint = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            preference = connection.GetPreference(product);
            List<double> likes = new List<double>();
            List<double> dislikes = new List<double>();

            for (int i = 0; i < preference.Count; i++)
            {
                if (preference[i].likePreference == "0")
                    dislikes.Add(double.Parse(preference[i].likePreference));
                else if (preference[i].likePreference == "1")
                    likes.Add(double.Parse(preference[i].likePreference));
            }

            chart.Series = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Likes",
                    Values = new ChartValues<double> { likes.Count },
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
                new PieSeries
                {
                    Title = "Dislikes",
                    Values = new ChartValues<double> { dislikes.Count },
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
            };

            chart.DataTooltip.Opacity = 0;
            chart.LegendLocation = LegendLocation.Bottom;
        }

        public void Dispose()
        {
            chart.Dispose();
        }
    }
}
