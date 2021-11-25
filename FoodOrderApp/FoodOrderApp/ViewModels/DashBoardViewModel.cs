using FoodOrderApp.Models;
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

        private List<RECEIPT> receipts;

        public DashBoardViewModel()
        {
            LoadedCommand = new RelayCommand<DashBoardUC>((parameter) => true, (paramater) => loaded(paramater));
        }

        private void loaded(DashBoardUC paramater)
        {
            // get receipt data from db
            receipts = Data.Ins.DB.RECEIPTs.Where(x => x.USER.USERNAME_ == CurrentAccount.Username).ToList();
            // set up X axis, display 5 column
            DateTime now = DateTime.Now;
            DateTime _1DayBefore = DateTime.Now.Date.AddDays(-1);
            DateTime _2DayBefore = DateTime.Now.Date.AddDays(-2);
            DateTime _3DayBefore = DateTime.Now.Date.AddDays(-3);
            DateTime _4DayBefore = DateTime.Now.Date.AddDays(-4);

            Labels = new[] {_4DayBefore.ToString("dd.MM"),
                            _3DayBefore.ToString("dd.MM"),
                            _2DayBefore.ToString("dd.MM"),
                            _1DayBefore.ToString("dd.MM"),
                            now.ToString("dd.MM")};
            // calculate sales for each day
            int salesNow = calculateSales(out salesNow, now);
            int sales1DayBefore = calculateSales(out salesNow, _1DayBefore);
            int sales2DayBefore = calculateSales(out salesNow, _2DayBefore);
            int sales3DayBefore = calculateSales(out salesNow, _3DayBefore);
            int sales4DayBefore = calculateSales(out salesNow, _4DayBefore);
            // create a line and add data into chart
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "2021",
                    Values = new ChartValues<double> { sales4DayBefore, sales3DayBefore, sales2DayBefore, sales1DayBefore, salesNow },
                    PointGeometry = DefaultGeometries.Circle,
                    PointGeometrySize = 15
                }
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
            //SeriesCollection[0].Values.
        }

        private int calculateSales(out int sales, DateTime dateTime)
        {
            sales = 0;
            foreach (var receipt in receipts)
            {
                if (receipt.DATE_ == dateTime)
                    sales += receipt.VALUE_;
            }
            return sales;
        }
    }
}