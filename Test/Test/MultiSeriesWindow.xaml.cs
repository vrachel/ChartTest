using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Examples.ExternalDependencies.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for MultiSeriesWindow.xaml
    /// </summary>
    public partial class MultiSeriesWindow : UserControl
    {
        // Data Sample Rate (sec)  - 20 Hz
        private const double dt = 0.05;

        // FIFO Size is 100 samples, meaning after 100 samples have been appended, each new sample appended
        // results in one sample being discarded
        private const int FifoSize = 100;

        // Timer to process updates
        private readonly Timer _timerNewDataUpdate;

        // The current time
        private double t;

        // The dataseries to fill
        private IXyDataSeries<double, double> _series0;
        private IXyDataSeries<double, double> _series1;
        private IXyDataSeries<double, double> _series2;

        //private IXyDataSeries<double, double> _series;

        private ObservableCollection<IRenderableSeries> renderableSeries;

        public ObservableCollection<IRenderableSeries> RenderableSeries { get => renderableSeries; set => renderableSeries = value; }

        public MultiSeriesWindow()
        {
            InitializeComponent();
            _timerNewDataUpdate = new Timer(dt * 1000);
            _timerNewDataUpdate.AutoReset = true;
            _timerNewDataUpdate.Elapsed += OnNewData;

            // Create new Dataseries of type X=double, Y=double
            _series0 = new XyDataSeries<double, double>() { FifoCapacity = FifoSize, SeriesName = "Orange Series" };
            _series1 = new XyDataSeries<double, double>() { FifoCapacity = FifoSize, SeriesName = "Blue Series" };
            _series2 = new XyDataSeries<double, double>() { FifoCapacity = FifoSize, SeriesName = "Green Series" };

            //_series = new XyDataSeries<double, double>() { FifoCapacity = FifoSize, SeriesName = "Red Series" };

            // Set the dataseries on the chart's RenderableSeries
            renderableSeries0.DataSeries = _series0;
            renderableSeries1.DataSeries = _series1;
            renderableSeries2.DataSeries = _series2;

            renderableSeries = new ObservableCollection<IRenderableSeries>();
            DataContext = this;
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
            }

            //_series.Clear();
        }

        private void OnNewData(object sender, EventArgs e)
        {
            // Compute our three series values
            double y1 = 3.0 * Math.Sin(((2 * Math.PI) * 1.4) * t * 0.02);
            double y2 = 2.0 * Math.Cos(((2 * Math.PI) * 0.8) * t * 0.02);
            double y3 = 1.0 * Math.Sin(((2 * Math.PI) * 2.2) * t * 0.02);

            // Suspending updates is optional, and ensures we only get one redraw
            // once all three dataseries have been appended to
            using (sciChartSurface.SuspendUpdates())
            {
                // Append x,y data to previously created series
                _series0.Append(t, y1);
                _series1.Append(t, y2);
                _series2.Append(t, y3);
            }
            //_series.Append(t, y1);
            // Increment current time
            t += dt;

        }

        //private void OnExampleLoaded(object sender, RoutedEventArgs e)
        //{
        //    ClearDataSeries();

        //    _timerNewDataUpdate.Start();
        //}

        //private void OnExampleUnloaded(object sender, RoutedEventArgs e)
        //{
        //    if (_timerNewDataUpdate != null)
        //    {
        //        _timerNewDataUpdate.Stop();
        //    }
        //}

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            ClearDataSeries();

            _timerNewDataUpdate.Start();

        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            if (_timerNewDataUpdate != null)
            {
                _timerNewDataUpdate.Stop();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Create a DataSeries and append some data
            var dataSeries = new XyDataSeries<double, double>();
            var doubleData = DataManager.Instance.GetRandomDoubleSeries(100);
            dataSeries.Append(doubleData.XData, doubleData.YData);

            // Create a RenderableSeries and ensure DataSeries is set
            var renderSeries = new FastLineRenderableSeries
            {
                Stroke = DataManager.Instance.GetRandomColor(),
                DataSeries = dataSeries,
                StrokeThickness = 2,
            };

            // Add the new RenderableSeries
            RenderableSeries.Add(renderSeries);
            
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            var rSeries = RenderableSeries.FirstOrDefault(s=>s.IsSelected);
            if (rSeries == null || rSeries.DataSeries == null)
                return;

            RenderableSeries.Remove(rSeries);

            
        }
    }
}
