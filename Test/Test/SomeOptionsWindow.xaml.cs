using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SciChart.Charting;
using SciChart.Charting.Model.DataSeries;
using SciChart.Core.Helpers;

namespace Test
{
    /// <summary>
    /// Interaction logic for SomeOptionsWindow.xaml
    /// </summary>
    public partial class SomeOptionsWindow : UserControl
    {
        private readonly FasterRandom _random = new FasterRandom();
        private XyDataSeries<double, double> series;
        public SomeOptionsWindow()
        {
            InitializeComponent();
            DrawGraph();
        }

        private void DrawGraph()
        {
            double xValue = 0;
            double[] xBuffer = new double[100];
            double[] yBuffer = new double[100];
                for (int j = 0; j < 100; j++)
            {
                // Generate a new X,Y value in the random walk
                xValue = xValue + 0.01;
                double yValue = _random.NextDouble();

                xBuffer[j] = xValue;
                yBuffer[j] = yValue;
            }
            yBuffer = GetYValues().ToArray();
            // Append block of values
            series = new XyDataSeries<double, double>();
            series.Append (xBuffer,yBuffer);
            fastLineRenderableSeries.DataSeries = series;
        }

        #region Change GridLine visibility
        private void cbDrawMajorGridLinesX_Checked(object sender, RoutedEventArgs e)
        {
            chartXAxis.DrawMajorGridLines = (bool)cbDrawMajorGridLinesX.IsChecked;
        }

        private void cbDrawMajorGridLinesY_Checked(object sender, RoutedEventArgs e)
        {
            chartYAxis.DrawMajorGridLines = (bool)cbDrawMajorGridLinesY.IsChecked;

        }

        private void cbDrawMinorGridLinesX_Checked(object sender, RoutedEventArgs e)
        {
            chartXAxis.DrawMinorGridLines = (bool)cbDrawMinorGridLinesX.IsChecked;
        }

        private void cbDrawMinorGridLinesY_Checked(object sender, RoutedEventArgs e)
        {
            chartYAxis.DrawMinorGridLines = (bool)cbDrawMinorGridLinesY.IsChecked;
        }
        #endregion
        private void btnChangeColor_Click(object sender, RoutedEventArgs e)
        {
            if (fastLineRenderableSeries.Stroke == Colors.Yellow)
                fastLineRenderableSeries.Stroke = Colors.Red;
            else if(fastLineRenderableSeries.Stroke == Colors.Red)
                fastLineRenderableSeries.Stroke = Colors.Green;
            else 
                fastLineRenderableSeries.Stroke = Colors.Yellow;
        }

        private List<double> GetYValues()
        {
            List<double> valueList = new List<double>();
            double d = 0.002;
            for (int i = 0; i < 10; i++)
            {
                valueList.Add(d += 0.4);
            }

            for (int i = 0; i < 30; i++)
            {
                valueList.Add(d -= 0.4);
            }
            for (int i = 0; i < 30; i++)
            {
                valueList.Add(d += 0.5);
            }

            for (int i = 0; i < 20; i++)
            {
                valueList.Add(d -= 0.1);
            }

            for (int i = 0; i < 10; i++)
            {
                valueList.Add(d += 0.1);
            }
           
            return valueList;
        }

        private void cbTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ThemeManager.SetTheme(chart, (string)cbTheme.SelectedItem);
        }
    }
}
