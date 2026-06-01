using System.Globalization;
using System.Windows.Data;

namespace QuanLyCaPheApp.Converters
{
    public class NumberFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal d) return $"{d:N0} đ";
            if (value is int    i) return $"{i:N0}";
            if (value is double db) return $"{db:N0} đ";
            return value?.ToString() ?? "";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                s = s.Replace(" đ", "").Replace(",", "").Trim();
                if (decimal.TryParse(s, out var d)) return d;
            }
            return 0m;
        }
    }
}
