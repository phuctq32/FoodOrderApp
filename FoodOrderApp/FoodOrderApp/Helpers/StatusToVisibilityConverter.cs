using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace FoodOrderApp.Helpers
{
    internal class StatusToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType,
               object parameter, System.Globalization.CultureInfo culture)
        {
            //[0] là status, [1] button
            try
            {
                if (values[0].ToString() == "2" || values[0].ToString() == "3")
                    return Visibility.Collapsed;
                //status 0
                if (values[0].ToString() == "0" && (values[1].ToString() == "confirmBtn" || values[1].ToString() == "cancelBtn" || values[1].ToString() == "stackBtn"))
                    return Visibility.Visible;
                //status 1
                else if (values[0].ToString() == "1" && (values[1].ToString() == "doneBtn" || values[1].ToString() == "cancelBtn" || values[1].ToString() == "stackBtn"))
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            catch
            {
                return Visibility.Collapsed;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes,
               object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }
    }
}