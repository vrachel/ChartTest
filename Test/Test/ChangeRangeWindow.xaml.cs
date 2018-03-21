using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Axes;
using SciChart.Core.Helpers;

namespace Test
{
    /// <summary>
    /// Interaction logic for ChangeRangeWindow.xaml
    /// </summary>
    public partial class ChangeRangeWindow : UserControl
    {
        private readonly Timer _timerNewDataUpdate;
        private IXyDataSeries<double, double> dataSeries;
        public IXyDataSeries<double, double> DataSeries
        {
            get { return dataSeries; }
            set { dataSeries = value; }
        }
        private readonly FasterRandom _random = new FasterRandom();
        private int _bufferSize = 600; // Number of points to append to each channel each timer tick
        private double[] xBuffer;
        private double[] yBuffer;
        private bool isBusy = false;

        public ChangeRangeWindow()
        {
            InitializeComponent();
            _timerNewDataUpdate = new Timer(100);
            _timerNewDataUpdate.AutoReset = true;
            _timerNewDataUpdate.Elapsed += OnTick;
            DataContext = this;
            DataSeries = new XyDataSeries<double, double>();
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (!isBusy)
            {
                isBusy = true;

                // Preload previous value with k-1 sample, or 0.0 if the count is zero
                double xValue = DataSeries.Count > 0 ? DataSeries.XValues[DataSeries.Count - 1] : 0.0;

                // Add points 10 at a time for efficiency   
                for (int j = 0; j < _bufferSize; j++)
                {
                    // Generate a new X,Y value in the random walk
                    xValue = xValue + 0.001;
                    //double yValue = _random.NextDouble();

                    xBuffer[j] = xValue;
                    yBuffer[j] = Math.Sin(xValue);
                }
                // Append block of values
                DataSeries.Append(xBuffer, yBuffer);

                Dispatcher.BeginInvoke(new System.Threading.ThreadStart(() => UpdateRange()));
                isBusy = false;
            }

        }

        private void UpdateRange()
        {

            var x = chart1.XAxis as NumericAxis;
            if (DataSeries.XMax.CompareTo(x.VisibleRange.Max) > 0)
                x.VisibleRange = SciChart.Data.Model.RangeFactory.NewWithMinMax(x.VisibleRange, (double)x.VisibleRange.Min + 2, (double)x.VisibleRange.Max + 2);
        }

        private void btnStopTimer_Click(object sender, RoutedEventArgs e)
        {
            _timerNewDataUpdate.Stop();
        }

        private void btnStartTimer_Click(object sender, RoutedEventArgs e)
        {
            xBuffer = new double[_bufferSize];
            yBuffer = new double[_bufferSize];

            _timerNewDataUpdate.Start();
        }
        private void btnPrevRange_Click(object sender, RoutedEventArgs e)
        {
            if ((double)xAxis2.VisibleRange.Min > 0)
                xAxis2.VisibleRange = SciChart.Data.Model.RangeFactory.NewWithMinMax(xAxis2.VisibleRange, (double)xAxis2.VisibleRange.Min - 20, (double)xAxis2.VisibleRange.Max - 20);
        }

        private void btnNextRange_Click(object sender, RoutedEventArgs e)
        {
            xAxis2.VisibleRange = SciChart.Data.Model.RangeFactory.NewWithMinMax(xAxis2.VisibleRange, (double)xAxis2.VisibleRange.Min + 20, (double)xAxis2.VisibleRange.Max + 20);

        }
    }
}
