using SciChart.Charting;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Charting3D;
using SciChart.Core.Helpers;
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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Members
        // Data Sample Rate (sec)  - 20 Hz
        private const double dt = 0.05;
        private int iteration = 1;

        // FIFO Size is 100 samples, meaning after 100 samples have been appended, each new sample appended
        // results in one sample being discarded
        private const int FifoSize = 100;

        // Timer to process updates
        private readonly Timer _timerNewDataUpdate;

        // The current time
        private double t;
        double d = 0.0;

        // The dataseries to fill
        private IXyDataSeries<double, double> _series0;
        private IXyDataSeries<double, double> _series1;
        private IXyDataSeries<double, double> _series2;

        private IXyDataSeries<double, double> graph7Series;

        private IXyDataSeries<double, double> dataSeries;
        public IXyDataSeries<double, double> DataSeries
        {
            get { return dataSeries; }
            set { dataSeries = value; }
        }

        private readonly FasterRandom _random = new FasterRandom();

        List<double> doubleList1 = new List<double>();
        List<double> doubleList2 = new List<double>();

        private int _bufferSize = 6000; // Number of points to append to each channel each timer tick
        private double[] xBuffer;
        private double[] yBuffer;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            DataSeries = new XyDataSeries<double, double>();


            sciChartSurface.XAxis.TickTextBrush = Brushes.Red;

            sciChartSurface.XAxis.AxisAlignment = SciChart.Charting.Visuals.Axes.AxisAlignment.Top;

            _timerNewDataUpdate = new Timer(1000);// dt *
            _timerNewDataUpdate.AutoReset = true;
            //_timerNewDataUpdate.Elapsed += OnNewData;
            _timerNewDataUpdate.Elapsed += OnTick;

            // Create new Dataseries of type X=double, Y=double
            _series0 = new XyDataSeries<double, double>() { FifoCapacity = FifoSize, SeriesName = "Orange Series", AcceptsUnsortedData = true };
            _series1 = new XyDataSeries<double, double>() { FifoCapacity = FifoSize, SeriesName = "Blue Series", AcceptsUnsortedData = true };
            _series2 = new XyDataSeries<double, double>() { FifoCapacity = FifoSize, SeriesName = "Green Series", AcceptsUnsortedData = true };

            Init();

            // Set the dataseries on the chart's RenderableSeries
            renderableSeries0.DataSeries = _series0;
            renderableSeries1.DataSeries = _series1;
            renderableSeries2.DataSeries = _series2;

        }

        private void Init()
        {
            graph7Series = new XyDataSeries<double, double>();

            //renderableSeriesChart2.DataSeries = graph2Series;
            //renderableSeriesChart3.DataSeries = graph3Series;
            //renderableSeriesChart4.DataSeries = graph4Series;
            //renderableSeriesChart5.DataSeries = graph5Series;
            //renderableSeriesChart6.DataSeries = graph6Series;
            //renderableSeriesChart7.DataSeries = graph7Series;

            for (int i = 0; i < 15; i++)
            {
                doubleList1.AddRange(GetXValues());
                iteration++;
            }
            iteration = 1;
            doubleList2 = GetYValues();

            xBuffer = new double[_bufferSize];
            yBuffer = new double[_bufferSize];

        }

        private void ClearDataSeries()
        {
            if (_series0 == null)
                return;

            using (sciChartSurface.SuspendUpdates())
            {
                _series0.Clear();
                _series1.Clear();
                _series2.Clear();

                DataSeries.Clear();
            }
        }

        private void OnNewData(object sender, EventArgs e)
        {
            // Compute our three series values
            double y1 = 3.0 * Math.Sin(((2 * Math.PI) * 1.4) * t * 0.02);
            double y2 = 2.0 * Math.Cos(((2 * Math.PI) * 0.8) * t * 0.02);
            double y3 = 1.0 * Math.Sin(((2 * Math.PI) * 2.2) * t * 0.02);

            // Suspending updates is optional, and ensures we only get one redraw
            // once all three dataseries have been appended to
            using (chart7.SuspendUpdates())
            {

                // Append x,y data to previously created series
                _series0.Append(t, y1);
                _series1.Append(t, y2);
                _series2.Append(t + 20, y3 + 10);


                var tmp = doubleList1.GetRange(iteration * 6000, 6000);
                graph7Series.Append(tmp, doubleList2.Take(6000));
                //graph7Series.InsertRange(graph7Series.Count,tmp, doubleList2.Take(6000));


            }
            // Increment current time
            t += dt;
            //UpdateStatus();
            if (iteration > 13)
            {
                _timerNewDataUpdate.Stop();
                UpdateStatus(true);

            }
            else { UpdateStatus(false); }
            iteration++;

            ChangeRange();
        }

        private void OnTick(object sender, EventArgs e)
        {
            // Ensure only one timer Tick processed at a time
            //lock (_syncRoot)
            //{
            //    for (int i = 0; i < _channelViewModels.Count; i++)
            //    {
            // Get the dataseries created for this channel
            //var channel = _channelViewModels[i];
            //var dataseries = channel.ChannelDataSeries;

            // Preload previous value with k-1 sample, or 0.0 if the count is zero
            double xValue = DataSeries.Count > 0 ? DataSeries.XValues[DataSeries.Count - 1] : 0.0;

            // Add points 10 at a time for efficiency   
            for (int j = 0; j < _bufferSize; j++)
            {
                // Generate a new X,Y value in the random walk
                xValue = xValue + 0.0001;
                double yValue = _random.NextDouble();

                xBuffer[j] = xValue;
                yBuffer[j] = yValue;
            }
            yBuffer = GetYValues().ToArray();
            // Append block of values
            DataSeries.Append(xBuffer, yBuffer);

            ChangeRange();
            // For reporting current size to GUI
            //_currentSize = dataseries.Count;
            //}
            //}


        }

        private void UpdateStatus(bool isLastTime)
        {
            //Dispatcher.BeginInvoke(
            //         new System.Threading.ThreadStart(() =>
            //tbCounter.Text =
            //  (graph2Series.XValues.Count() +
            //  graph3Series.XValues.Count() +
            //  graph4Series.XValues.Count() +
            //  graph5Series.XValues.Count() +
            //  graph6Series.XValues.Count() +
            //  graph7Series.XValues.Count() +
            //  graph8Series.XValues.Count()).ToString()
            //  + "      Now : " + DateTime.Now + "  Iteration: " + iteration + "   Finished= " + isLastTime
            //  ));
        }


        private void btnHide_Click(object sender, RoutedEventArgs e)
        {
            if (chart4.XAxis.Visibility == System.Windows.Visibility.Hidden)
                chart4.XAxis.Visibility = System.Windows.Visibility.Visible;
            else chart4.XAxis.Visibility = System.Windows.Visibility.Hidden;
        }

        private List<double> GetYValues()
        {
            List<double> valueList = new List<double>();
            double d = 0.002;
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.002);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.001);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.005);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.001);
            }
            //for (int i = 0; i < 10; i++)
            //{
            //    valueList.Add(d += 0.004);
            //}

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.001);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.005);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.001);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.005);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.0001);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.002);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.002);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.0001);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.004);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.0001);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.005);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.0001);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.0002);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.002);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.0001);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.0001);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.0005);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.0001);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.002);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.0002);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.0001);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.0004);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.0001);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.0005);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.0001);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.0002);
            }
            return valueList;
        }

        private List<double> GetXValues()
        {
            List<double> valueList = new List<double>();


            for (int i = iteration * 6000; i < iteration * 6000 + 6000; i++)
            {
                valueList.Add(d += 0.0001);
            }
            return valueList;
        }

        private void btnCangeRange_Click(object sender, RoutedEventArgs e)
        {
            xAxis2.VisibleRange = SciChart.Data.Model.RangeFactory.NewWithMinMax(xAxis2.VisibleRange, (double)xAxis2.VisibleRange.Min + 20, (double)xAxis2.VisibleRange.Max + 20);

        }

        #region Change GridLine visibility
        private void cbDrawMajorGridLinesX_Checked(object sender, RoutedEventArgs e)
        {
            chart3XAxis.DrawMajorGridLines = (bool)cbDrawMajorGridLinesX.IsChecked;
        }

        private void cbDrawMajorGridLinesY_Checked(object sender, RoutedEventArgs e)
        {
            chart3YAxis.DrawMajorGridLines = (bool)cbDrawMajorGridLinesY.IsChecked;

        }

        private void cbDrawMinorGridLinesX_Checked(object sender, RoutedEventArgs e)
        {
            chart3XAxis.DrawMinorGridLines = (bool)cbDrawMinorGridLinesX.IsChecked;
        }

        private void cbDrawMinorGridLinesY_Checked(object sender, RoutedEventArgs e)
        {
            chart3YAxis.DrawMinorGridLines = (bool)cbDrawMinorGridLinesY.IsChecked;
        }
        #endregion

        private void btnChangeColor_Click(object sender, RoutedEventArgs e)
        {
            if (renderableSeriesChart5.Stroke == Colors.Yellow)
                renderableSeriesChart5.Stroke = Colors.Red;
            else
                renderableSeriesChart5.Stroke = Colors.Yellow;
        }

        private void btnStopTimer_Click(object sender, RoutedEventArgs e)
        {
            //if (_timerNewDataUpdate.Enabled)
            {
                _timerNewDataUpdate.Stop();
                //ClearDataSeries();
                //tbCounter.Text = string.Empty;
            }
        }

        private void btnStartTimer_Click(object sender, RoutedEventArgs e)
        {
            //if (!_timerNewDataUpdate.Enabled)
            //{
            //ClearDataSeries();
            xBuffer = new double[_bufferSize];
            yBuffer = new double[_bufferSize];

            iteration = 1;
            _timerNewDataUpdate.Start();
            //}
        }

        private void ChangeRange()
        {
            Dispatcher.BeginInvoke(
                       new System.Threading.ThreadStart(() => UpdateRange()

              //var z = chart7.XAxis
              ));
            // var a = graph2Series.XMax;
            //xAxis2.VisibleRange = SciChart.Data.Model.RangeFactory.NewWithMinMax(xAxis2.VisibleRange, (double)xAxis2.VisibleRange.Min + 20, (double)xAxis2.VisibleRange.Max + 20);

        }

        private void UpdateRange()
        {

            var x = chart7.XAxis as NumericAxis;
            if(DataSeries.XMax.CompareTo(x.VisibleRange.Max)>0)
                 x.VisibleRange = SciChart.Data.Model.RangeFactory.NewWithMinMax(x.VisibleRange, (double)x.VisibleRange.Min + 2, (double)x.VisibleRange.Max + 2);
        }

        private void btnOpenShakingScreen_Click(object sender, RoutedEventArgs e)
        {
            ShakingScreen sScreen = new ShakingScreen();
            //sScreen.Show();
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

        private void btnOpenMultiSeries_Click(object sender, RoutedEventArgs e)
        {
            MultiSeriesWindow mScreen = new MultiSeriesWindow();
            //mScreen.Show();

        }

        private void btnOpenSomeOptions_Click(object sender, RoutedEventArgs e)
        {
            SomeOptionsWindow oScreen = new SomeOptionsWindow();
            //oScreen.Show();

        }
    }
}
