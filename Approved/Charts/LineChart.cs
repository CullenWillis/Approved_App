using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LiveCharts; //Core of the library
using LiveCharts.Wpf; //The WPF controls
using LiveCharts.WinForms; //the WinForm wrappers
using LiveCharts.Defaults;
using Approved.DatabaseConnection;
using System.Windows.Forms;
using System.Drawing;

namespace Approved.Charts
{
    class LineChart
    {
        private List<productPreference> preference;
        private Connection_Handler connection;

        private LiveCharts.WinForms.CartesianChart chart;

        private ChartValues<ObservablePoint> valuesLikes { get; set; }
        private ChartValues<ObservablePoint> valuesDislikes { get; set; }

        public LineChart()
        {
            preference = new List<productPreference>();
            connection = new Connection_Handler();

            chart = new LiveCharts.WinForms.CartesianChart();
        }

        public void createLineChart(string product, Panel panel)
        {
            panel.Controls.Add(chart);

            chart.Width = 1200;
            chart.Height = 500;
            chart.Location = new Point(25, 0);

            preference = connection.GetPreference(product);

            valuesLikes = new ChartValues<ObservablePoint>();
            valuesDislikes = new ChartValues<ObservablePoint>();

            // Calculate the values in each section, (0 = dislikes, 1 = likes)
            calculatePoints(preference, 1);
            calculatePoints(preference, 0);

            chart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Likes",
                    LineSmoothness = 0,
                    Values = valuesLikes
                },
                new LineSeries
                {
                    Title = "Dislikes",
                    LineSmoothness = 0,
                    Values = valuesDislikes
                }
            };

            chart.AxisX.Add(new Axis
            {
                Title = "Months",
                Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "Dec"},
                MaxValue = 12,
                BarUnit = 12,
                MaxRange = 12,
            });

            chart.AxisY.Add(new Axis
            {
                Title = "Number of Likes",
                MinValue = 0,
                MaxValue = 25,
                LabelFormatter = value => value + ""
            });

            chart.LegendLocation = LegendLocation.Top;
        }

        private void calculatePoints(List<productPreference> preference, int type)
        {
            double[] value = calculateValues(preference, type);
            double[] months = calculateMonths(preference, type);

            // Start at 0
            if(type == 1)
                valuesLikes.Add(new ObservablePoint(0, 0));
            else
                valuesDislikes.Add(new ObservablePoint(0, 0));

            for (int i = 0; i < months.Length; i++)
            {
                if(type == 1)
                    valuesLikes.Add(new ObservablePoint(months[i], value[i]));
                else
                    valuesDislikes.Add(new ObservablePoint(months[i], value[i]));
            }
        }

        private double[] calculateValues(List<productPreference> preference, int type)
        {
            double[] valuesTemp = new double[preference.Count];

            int count = 0;

            foreach (productPreference o in preference)
            {
                if(int.Parse(o.likePreference) == type)
                {
                    if (count == 0)
                    {
                        valuesTemp[count] = 1;
                    }
                    else
                    {
                        valuesTemp[count] = valuesTemp[count - 1] + 1;
                    }
                    count++;
                }
            }

            double[] values = new double[count];

            for (int i = 0; i < count; i++)
            {
                values[i] = valuesTemp[i];
            }

            return values;
        }

        private double[] calculateMonths(List<productPreference> likes, int type)
        {
            double[] monthsTemp = new double[likes.Count];

            int count = 0;
            foreach (productPreference o in likes)
            {
                if (int.Parse(o.likePreference) == type)
                {
                    double month = o.likeMonth;
                    double day = ((double)o.likeDay) / 100.0;

                    monthsTemp[count] = month + day;
                    count++;
                }
            }

            double[] months = new double[count];

            for (int i = 0; i < count; i++)
            {
                months[i] = monthsTemp[i];
            }

            months = bubbleSortMonths(months);

            return months;
        }

        private double[] bubbleSortMonths(double[] months)
        {
            double temp = 0;

            for (int j = 0; j <= months.Length - 2; j++)
            {
                for (int i = 0; i <= months.Length - 2; i++)
                {
                    if (months[i] > months[i + 1])
                    {
                        temp = months[i + 1];
                        months[i + 1] = months[i];
                        months[i] = temp;

                        
                    }
                }
            }

            return months;
        }

        public void Dispose()
        {
            chart.Dispose();
        }
    }
}
