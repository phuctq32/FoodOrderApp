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
                //kiểm tra xem value = discount mà lớn hơn 0 thì cho stack chứa giá giảm visible
                return (Double.Parse(value.ToString().Replace("%", " ")) > 0) ? Visibility.Visible : Visibility.Collapsed;

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