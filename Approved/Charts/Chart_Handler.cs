using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Approved.Charts
{
    class Chart_Handler
    {
        private LineChart lineChart;
        private PieChart pieChart;

        public void createLineChart(string product, Panel panel)
        {
            lineChart = new LineChart();
            lineChart.createLineChart(product, panel);
        }

        public void createPieChart(string product, Panel panel)
        {
            pieChart = new PieChart();
            pieChart.createPieChart(product, panel);
        }

        public void dispose()
        {
            lineChart.Dispose();
            pieChart.Dispose();
        }
    }
}
