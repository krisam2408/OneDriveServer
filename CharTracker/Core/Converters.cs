using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace RetiraTracker.Core
{
    public class BoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool val = (bool)value;
            return !val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool val = (bool)value;
            return !val;
        }
    }

    public class EnabledColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool val = (bool)value;
            if (!val)
                return Application.Current.Resources["DarkLight"];

            return Application.Current.Resources["Light"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color val = (Color)value;
            if (val == (Color)Application.Current.Resources["DarkLight"])
                return false;

            return true;
        }
    }
}
