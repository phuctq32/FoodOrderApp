using FoodOrderApp.Models;
using FoodOrderApp.Views.UserControls.Admin;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace FoodOrderApp.ViewModels
{
    internal class DashBoardViewModel : BaseViewModel
    {
        public int TotalProduct { get; set; }
        public int TotalValue { get; set; }
        public int TotalReceipt { get; set; }
        public int TotalCustomer { get; set; }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        private List<RECEIPT> receipts;
        public ICommand LoadedCommand { get; set; }
        public DashBoardViewModel()
        {
            TotalProduct = Data.Ins.DB.PRODUCTs.Where(x => x.ACTIVE_ == 1).Count();
            TotalCustomer = Data.Ins.DB.USERS.Count() - 1;
            TotalReceipt = Data.Ins.DB.RECEIPTs.Where(x => x.STATUS_ == "2").Count();
            receipts = Data.Ins.DB.RECEIPTs.ToList();
            TotalValue = calculateTotalSales();

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
            int sales1DayBefore = calculateSales(out sales1DayBefore, _1DayBefore);
            int sales2DayBefore = calculateSales(out sales2DayBefore, _2DayBefore);
            int sales3DayBefore = calculateSales(out sales3DayBefore, _3DayBefore);
            int sales4DayBefore = calculateSales(out sales4DayBefore, _4DayBefore);
            // create a line and add data into chart
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = now.Year.ToString(),
                    Values = new ChartValues<long> { sales4DayBefore, sales3DayBefore, sales2DayBefore, sales1DayBefore, salesNow },
                    PointGeometry = DefaultGeometries.Circle,
                    PointGeometrySize = 15
                }
            };

            YFormatter = value => value.ToString("N0");
            LoadedCommand = new RelayCommand<DashBoardUC>((parameter) => parameter == null ? false : true, (parameter) => Loaded(parameter));
        }

        private void Loaded(DashBoardUC parameter)
        {
            TotalProduct = Data.Ins.DB.PRODUCTs.Where(x => x.ACTIVE_ == 1).Count();
            TotalCustomer = Data.Ins.DB.USERS.Count() - 1;
            TotalReceipt = Data.Ins.DB.RECEIPTs.Where(x => x.STATUS_ == "2").Count();
            receipts = Data.Ins.DB.RECEIPTs.ToList();
            TotalValue = calculateTotalSales();
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
            int sales1DayBefore = calculateSales(out sales1DayBefore, _1DayBefore);
            int sales2DayBefore = calculateSales(out sales2DayBefore, _2DayBefore);
            int sales3DayBefore = calculateSales(out sales3DayBefore, _3DayBefore);
            int sales4DayBefore = calculateSales(out sales4DayBefore, _4DayBefore);
            // create a line and add data into chart
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = now.Year.ToString(),
                    Values = new ChartValues<long> { sales4DayBefore, sales3DayBefore, sales2DayBefore, sales1DayBefore, salesNow },
                    PointGeometry = DefaultGeometries.Circle,
                    PointGeometrySize = 15
                }
            };

            YFormatter = value => value.ToString("N0");
        }
        private int calculateSales(out int sales, DateTime dateTime)
        {
            sales = 0;
            foreach (var receipt in receipts)
            {
                if (receipt.DATE_.ToShortDateString() == dateTime.ToShortDateString() && receipt.STATUS_ == "2")
                    sales += receipt.VALUE_;
            }
            return sales;
        }

        private int calculateTotalSales()
        {
            if (receipts != null)
            {
                int sales = 0;
                foreach (var receipt in receipts)
                {
                    if (receipt.STATUS_ == "2")
                        sales += receipt.VALUE_;
                }
                return sales;
            }
            return 0;
        }
    }
}