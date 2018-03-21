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

namespace Test
{
    /// <summary>
    /// Interaction logic for WarningLineWindow.xaml
    /// </summary>
    public partial class WarningLineWindow : UserControl
    {
        private readonly Timer _timerNewDataUpdate;
        private IXyDataSeries<double, double> dataSeries;
        public IXyDataSeries<double, double> DataSeries
        {
            get { return dataSeries; }
            set { dataSeries = value; }
        }
        private readonly FasterRandom _random = new FasterRandom();
        private int _bufferSize = 6000; // Number of points to append to each channel each timer tick
        private double[] xBuffer;
        private double[] yBuffer;


        public WarningLineWindow()
        {
            InitializeComponent();
            _timerNewDataUpdate = new Timer(1000);
            _timerNewDataUpdate.AutoReset = true;
            _timerNewDataUpdate.Elapsed += OnTick;
            DataContext = this;
            DataSeries = new XyDataSeries<double, double>();
        }

       
        private void OnTick(object sender, EventArgs e)
        {

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
        }
            private List<double> GetYValues()
        {
            List<double> valueList = new List<double>();
            double d = 0.02;
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.02);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.01);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.05);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.01);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.01);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.05);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.01);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.05);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.001);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.002);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.02);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.001);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.04);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.001);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.05);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.001);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.002);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.02);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.001);
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
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.04);
            }
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
                valueList.Add(d += 0.004);
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
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.002);
            }
            return valueList;
        }
            private List<double> GetNiceYValues()
        {
            List<double> valueList = new List<double>();
            double d = 0.02;
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.02);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.01);
            }
            for (int i = 0; i < 300; i++)
            {
                valueList.Add(d += 0.01);
            }

            for (int i = 0; i < 100; i++)
            {
                valueList.Add(d -= 0.01);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.01);
            }
            for (int i = 0; i < 300; i++)
            {
                valueList.Add(d -= 0.01);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.01);
            }
            for (int i = 0; i < 100; i++)
            {
                valueList.Add(d -= 0.01);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.001);
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
                valueList.Add(d -= 0.001);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.01);
            }

            for (int i = 0; i < 300; i++)
            {
                valueList.Add(d -= 0.001);
            }
            for (int i = 0; i < 100; i++)
            {
                valueList.Add(d += 0.01);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.001);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.002);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.002);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.001);
            }

            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d -= 0.001);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.005);
            }

            for (int i = 0; i < 300; i++)
            {
                valueList.Add(d -= 0.001);
            }
            for (int i = 0; i < 100; i++)
            {
                valueList.Add(d -= 0.01);
            }
            for (int i = 0; i < 100; i++)
            {
                valueList.Add(d += 0.002);
            }

            for (int i = 0; i < 300; i++)
            {
                valueList.Add(d -= 0.001);
            }
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.004);
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
            for (int i = 0; i < 200; i++)
            {
                valueList.Add(d += 0.002);
            }
            return valueList;
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
    }
}
