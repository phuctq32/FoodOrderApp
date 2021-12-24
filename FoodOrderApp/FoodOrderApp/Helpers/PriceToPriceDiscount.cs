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
    internal class PriceToPriceDiscount : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType,
               object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (values[1].ToString().Contains("%"))
                {
                    return
                    (Int32.Parse(values[0].ToString().Replace(",", "")) * (1 - Double.Parse(values[1].ToString().Replace("%", "")) / 100)).ToString("N0");
                }
            }
            catch
            {
                return "";
            }

            return "";
        }

        public object[] ConvertBack(object value, Type[] targetTypes,
               object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }
    }
}