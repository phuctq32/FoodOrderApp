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
    public class DoubleToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              CultureInfo culture)
        {
            if (value != null)
            {
                // cái stackpanel chứa giá màu đỏ và discount t đặt tên là stack
                if (parameter.ToString() == "stack")
                    return (Double.Parse(value.ToString().Replace("%", 0.ToString())) > 0) ? Visibility.Visible : Visibility.Collapsed;

                // cái giá bình thường t đặt tên là moneyTxt
                if (parameter.ToString() == "moneyTxt" && Double.Parse(value.ToString().Replace("%", 0.ToString())) > 0)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                              CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}