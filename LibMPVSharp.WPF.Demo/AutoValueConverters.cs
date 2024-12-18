using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LibMPVSharp.WPF.Demo
{
    public class AutoValueConverters : IValueConverter
    {
        public static readonly AutoValueConverters Instance = new AutoValueConverters();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(double) && value is TimeSpan ts)
            {
                return TimeSpanConverter(ts, (string)parameter);
            }
            else if (targetType == typeof(double) && value is long int64)
            {
                return (double)int64;
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(targetType == typeof(TimeSpan) && value is double val)
            {
                return DoubleConverter(val, (string)parameter);
            }
            else if(targetType == typeof(long) && value is double doubleVal)
            {
                return System.Convert.ToInt64(doubleVal);
            }
            return "";
        }

        private double TimeSpanConverter(TimeSpan ts, string parameter)
        {
            if (string.IsNullOrEmpty(parameter) || parameter == "s")
                return ts.TotalSeconds;
            else if (parameter == "m")
                return ts.TotalMinutes;
            else if (parameter == "h")
                return ts.TotalHours;
            else if (parameter == "d")
                return ts.TotalDays;

            return ts.TotalSeconds;
        }

        private TimeSpan DoubleConverter(double value, string parameter)
        {
            if (string.IsNullOrEmpty(parameter) || parameter == "s")
                return TimeSpan.FromSeconds(value);
            else if (parameter == "m")
                return TimeSpan.FromMinutes(value);
            else if (parameter == "h")
                return TimeSpan.FromHours(value);
            else if (parameter == "d")
                return TimeSpan.FromDays(value);

            return TimeSpan.FromSeconds(value);
        }
    }
}
