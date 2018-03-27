using SciChart.Charting.Common.Extensions;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Axes;
using SciChart.Core.Helpers;
using SciChart.Data.Model;
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

namespace Test
{
    /// <summary>
    /// Interaction logic for ShakingScreen.xaml
    /// </summary>
    public partial class ShakingScreen : UserControl
    {
        private Timer _timer;
        private uint _timerInterval = 200;// Interval of the timer to generate data in ms        
        private int _bufferSize = 100; // Number of points to append to each channel each timer tick

        // X, Y buffers used to buffer data into the Scichart instances in blocks of BufferSize
        private double[] xBuffer;
        private double[] yBuffer;
        private bool isBusy;

        private IXyDataSeries<double, double> dataSeries;
        public IXyDataSeries<double, double> DataSeries
        {
            get { return dataSeries; }
            set { dataSeries = value; }
        }

        private IRange range;
        public IRange Range
        {
            get { return range; }
            set { range = value; }
        }

        public ShakingScreen()
        {
            InitializeComponent();
            DataContext = this;
            DataSeries = new XyDataSeries<double, double>();
            range = new DoubleRange(0, 60);
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            DataSeries.Clear();
            Range.SetMinMax(0, 60);
            xBuffer = new double[_bufferSize];
            yBuffer = new double[_bufferSize];
            _timer = new Timer(_timerInterval);
            _timer.Elapsed += OnTick;
            _timer.AutoReset = true;
            _timer.Start();

        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
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
                    xValue = xValue + 0.002;

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
            if (DataSeries.XMax.CompareTo(Range.Max) > 0)
            { var dif = (double)DataSeries.XMax - (double)Range.Max;
                Range.Max=(double)Range.Max + dif;
                Range.Min=(double)Range.Min + dif;
            }
        }
    }

}
