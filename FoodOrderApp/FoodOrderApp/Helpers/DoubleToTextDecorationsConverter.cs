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
            string noneTextDecorations = "None";
            try
            {
                if (value != null)
                {
                    if (value.ToString().Contains("%"))
                        if (Double.Parse(value.ToString().Replace("%", " ")) > 0)
                            return TextDecorations.Strikethrough;
                        else
                            return noneTextDecorations;
                }
            }
            catch
            {
                return noneTextDecorations;
            }
            return noneTextDecorations;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                              CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}