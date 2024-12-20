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
            else if(targetType == typeof(string) && value is double doubleVal)
            {
                return DoubleToString(doubleVal, (string)parameter);
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
            else if (targetType == typeof(double) && value is string strVal)
            {
                return StringToDouble(strVal, (string)parameter);
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
        
        private string DoubleToString(double value, string parameter)
        {
            if (string.IsNullOrEmpty(parameter)) return value.ToString();
            else if (parameter == "aspect-ratio")
            {
                var n = System.Convert.ToInt32(value);
                var remain = System.Convert.ToInt32(value % n);
                return $"{n + 1}:{remain}";
            }
            return value.ToString();
        }

        private double StringToDouble(string value, string parameter)
        {
            if (string.IsNullOrEmpty(parameter)) return double.Parse(value);
            else if(parameter == "aspect-ratio")
            {
                var sp = value.Split(':');
                return double.Parse(sp[0]) / double.Parse(sp[1]);
            }

            return double.Parse(value);
        }
    }
}
