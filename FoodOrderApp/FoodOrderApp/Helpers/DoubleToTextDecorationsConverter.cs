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
    internal class DoubleToTextDecorationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              CultureInfo culture)
        {
            if (value != null)
            {
                // kiểm tra xem value = discount > 0 thì gạch ngang giá cũ
                return (Double.Parse(value.ToString().Replace("%", " ")) > 0) ? TextDecorations.Strikethrough : value;
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