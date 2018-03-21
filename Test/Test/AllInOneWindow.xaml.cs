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
using SciChart.Core.Helpers;
using SciChart.Data.Model;

namespace Test
{
    /// <summary>
    /// Interaction logic for AllInOneWindow.xaml
    /// </summary>
    public partial class AllInOneWindow : UserControl
    {
        private readonly FasterRandom _random = new FasterRandom();

        private bool isBusy = false;
        private Timer _timer;
        private uint _timerInterval = 100;// Interval of the timer to generate data in ms        
        private int _bufferSize = 600; // Number of points to append to each channel each timer tick

        // X, Y buffers used to buffer data into the Scichart instances in blocks of BufferSize
        private double[] xBuffer;
        private double[] yBuffer;

        private IXyDataSeries<double, double> dataSeries;
        public IXyDataSeries<double, double> DataSeries
        {
            get { return dataSeries; }
            set { dataSeries = value; }
        }

        private IXyDataSeries<double, double> dataSeries2;
        public IXyDataSeries<double, double> DataSeries2
        {
            get { return dataSeries2; }
            set { dataSeries2 = value; }
        }

        private IRange range;
        public IRange Range
        {
            get { return range; }
            set { range = value; }
        }

        private IRange range2;
        public IRange Range2
        {
            get { return range2; }
            set { range2 = value; }
        }
        public AllInOneWindow()
        {
            InitializeComponent();
            DataContext = this;
            DataSeries = new XyDataSeries<double, double>();
            DataSeries2 = new XyDataSeries<double, double>();// { FifoCapacity = 48222 };
            range = new DoubleRange(0, 60);
            range2 = new DoubleRange(0, 10);
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            DataSeries2.Clear();
            DataSeries.Clear();
            Range.SetMinMax(0, 60);
            Range2.SetMinMax(0, 10);
            xBuffer = new double[_bufferSize];
            yBuffer = new double[_bufferSize];
            _timer = new Timer(_timerInterval);
            _timer.Elapsed += OnTick;
            _timer.AutoReset = true;
            _timer.Start();
            scroll.Visibility = Visibility.Collapsed;

        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            scroll.Minimum = 0;
            scroll.Maximum = (double)Range2.Max;
            scroll.Value = (double)Range2.Max;
            scroll.SmallChange = 1;
            scroll.LargeChange = 5;
            scroll.Visibility = Visibility.Visible;
            UpdateLayout();
            scroll.ViewportSize = ((double)Range2.Diff / (double)Range2.Max)*scroll.ActualWidth;
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
                    xValue = xValue + 0.000166;
                    double yValue = _random.NextDouble();

                    xBuffer[j] = xValue;
                    yBuffer[j] = Math.Sin(xValue); ;
                }
                // Append block of values
                DataSeries.Append(xBuffer, yBuffer);
                DataSeries2.Append(xBuffer, yBuffer);
                Dispatcher.BeginInvoke(new System.Threading.ThreadStart(() => UpdateRange()));
                isBusy = false;
            }

        }

        private void UpdateRange()
        {
            if (DataSeries.XMax.CompareTo(Range.Max) > 0)
                Range.Max = (double)Range.Max + 20;
            if (DataSeries2.XMax.CompareTo(Range2.Max) > 0)
            {
                var d = (double)DataSeries2.XMax - (double)Range2.Max;
                Range2.Max = (double)Range2.Max + d;
                Range2.Min = (double)Range2.Min + d;
            }
        }

        private void btnPrevRange_Click(object sender, RoutedEventArgs e)
        {
            Range2.Max = (double)Range2.Max - 2;
            Range2.Min = (double)Range2.Min - 2;
        }

        private void btnNextRange_Click(object sender, RoutedEventArgs e)
        {
            Range2.Max = (double)Range2.Max + 2;
            Range2.Min = (double)Range2.Min + 2;

        }
             

        private void scroll_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (scroll.Visibility == Visibility.Visible)
            {
                if (e.NewValue >= 10)
                {
                    Range2.Max = e.NewValue;
                    Range2.Min = (double)Range2.Max - 10;
                }
                else
                {
                    Range2.Max = 10.0;
                    Range2.Min = 0.0;

                }
            }
        }
    }
}
