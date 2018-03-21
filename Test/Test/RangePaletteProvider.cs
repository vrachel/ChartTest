using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Charting.Visuals.RenderableSeries;

namespace Test
{
    public class RangePaletteProvider : DependencyObject,IPaletteProvider
    {
        public double MinRange
        {
            get { return (double)GetValue(MinRangeProperty); }
            set { SetValue(MinRangeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinRange.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinRangeProperty =
            DependencyProperty.Register("MinRange", typeof(double), typeof(RangePaletteProvider), null);



        public double MaxRange
        {
            get { return (double)GetValue(MaxRangeProperty); }
            set { SetValue(MaxRangeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxRange.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxRangeProperty =
            DependencyProperty.Register("MaxRange", typeof(double), typeof(RangePaletteProvider), null);


        public void OnBeginSeriesDraw(IRenderableSeries rSeries)
        {
           
        }

        
               
    }

    public class StrokeRangePaletteProvider:RangePaletteProvider,IStrokePaletteProvider
    {
        public void OnBeginSeriesDraw(IRenderableSeries rSeries)
        {
        }
      

        public Color? OverrideStrokeColor(IRenderableSeries rSeries, int index, IPointMetadata metadata)
        {
            // Note: Since IPointMetadata is now passed to palette provider, we can use this too to affect coloring. 
            // In this case we use only YValue but #justsaying

            var currentValue = (double)rSeries.DataSeries.YValues[index];

            if (currentValue > MaxRange || currentValue < MinRange)
                return Colors.Red;
            else return Colors.Green;
        }
    }

    public class PointRangePaletteProvider : RangePaletteProvider, IPointMarkerPaletteProvider
    {
        PointPaletteInfo pinfo;
        public void OnBeginSeriesDraw(IRenderableSeries rSeries)
        {
            pinfo = new PointPaletteInfo();
        }

        public PointPaletteInfo? OverridePointMarker(IRenderableSeries rSeries, int index, IPointMetadata metadata)
        {
            var currentValue = (double)rSeries.DataSeries.YValues[index];

            if (currentValue > MaxRange || currentValue < MinRange)
                pinfo.Stroke = Colors.Red;
            else pinfo.Stroke = Colors.Green;

            return pinfo;
        }
    }
}
