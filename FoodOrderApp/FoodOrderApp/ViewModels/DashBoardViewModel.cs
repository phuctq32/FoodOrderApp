using FoodOrderApp.Views.UserControls.Admin;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace FoodOrderApp.ViewModels
{
    internal class DashBoardViewModel : BaseViewModel
    {
        public ICommand LoadedCommand;
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public DashBoardViewModel()
        {
            LoadedCommand = new RelayCommand<DashBoardUC>((parameter) => true, (paramater) => loaded(paramater));

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 3", //format tháng/ năm
                    Values = new ChartValues<double> { 4,2,7,2,7 },
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 15
                }
            };
            //Data.Ins.DB.RECEIP(Date_ = date.now).toList() lấy số lượng đơn hàng trong ngày

            //DateTime yesterday = DateTime.Now.Date.AddDays(-1); tìm ngày trước đó
            Labels = new[] { DateTime.Now.Date.AddDays(-4).Day.ToString(),
                DateTime.Now.Date.AddDays(-3).ToShortDateString(),
                DateTime.Now.Date.AddDays(-2).ToShortDateString(),
                DateTime.Now.Date.AddDays(-1).ToShortDateString(),
                DateTime.Now.ToShortDateString(),
            };
            YFormatter = value => value.ToString("C");

            //modifying the series collection will animate and update the chart
            //SeriesCollection.Add(new LineSeries
            //{
            //    Title = "Series 4",
            //    Values = new ChartValues<double> { 5, 3, 2, 4 },
            //    LineSmoothness = 0, //0: straight lines, 1: really smooth lines
            //    PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
            //    PointGeometrySize = 50,
            //    PointForeground = Brushes.Gray
            //});

            //modifying any series values will also animate and update the chart
            SeriesCollection[3].Values.Add(5d);
        }

        private void loaded(DashBoardUC paramater)
        {
        }
    }
}