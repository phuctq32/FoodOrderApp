using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FoodOrderApp.Helpers
{
    internal class TotalPriceMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType,
               object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return (Int32.Parse((string)values[0]) * (Int32.Parse(values[1].ToString()))).ToString();
            }
            catch
            {
                CustomMessageBox.Show("Lỗi giá hoặc số lượng bị NULL (TotalPriceMultiConverter)");
                return "";
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes,
               object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }
    }
}