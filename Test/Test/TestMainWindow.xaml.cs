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
using SciChart.Charting.Model.DataSeries;
using SciChart.Core.Helpers;

namespace Test
{
    /// <summary>
    /// Interaction logic for TestMainWindow.xaml
    /// </summary>
    public partial class TestMainWindow : Window
    {
        public TestMainWindow()
        {
            InitializeComponent();
        }

        private void ChangeView(UserControl newView)
        {
            mainCanvas.Children.Clear();
            mainCanvas.Children.Add(newView);
        }

        private void btnOpenMultiSeries_Click(object sender, RoutedEventArgs e)
        {
            MultiSeriesWindow mScreen = new MultiSeriesWindow();
            ChangeView(mScreen);
        }

        private void btnOpenSomeOptions_Click(object sender, RoutedEventArgs e)
        {
            SomeOptionsWindow oScreen = new SomeOptionsWindow();
            ChangeView(oScreen);

        }

        private void btnOpenShakingScreen_Click(object sender, RoutedEventArgs e)
        {
            ShakingScreen sScreen = new ShakingScreen();
            ChangeView(sScreen);
        }

        private void btnOpenChangeRange_Click(object sender, RoutedEventArgs e)
        {
            ChangeRangeWindow rScreen = new ChangeRangeWindow();
            ChangeView(rScreen);
        }

        private void btnOpenWarningLine_Click(object sender, RoutedEventArgs e)
        {
            WarningLineWindow wScreen = new WarningLineWindow();
            ChangeView(wScreen);
        }

        private void btnOpenAllInOne_Click(object sender, RoutedEventArgs e)
        {
            AllInOneWindow oScreen = new AllInOneWindow();
            ChangeView(oScreen);
        }
    }
}
