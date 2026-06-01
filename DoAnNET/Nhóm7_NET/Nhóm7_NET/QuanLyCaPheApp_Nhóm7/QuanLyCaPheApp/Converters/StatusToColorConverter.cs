using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace QuanLyCaPheApp.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value?.ToString()) switch
            {
                "Trống"       => new SolidColorBrush(Color.FromRgb(76,  175, 80)),
                "Đang dùng"   => new SolidColorBrush(Color.FromRgb(244, 67,  54)),
                "Đã đặt"      => new SolidColorBrush(Color.FromRgb(255, 193, 7)),
                "Bảo trì"     => new SolidColorBrush(Color.FromRgb(158, 158, 158)),
                "DaThanhToan" => new SolidColorBrush(Color.FromRgb(76,  175, 80)),
                "DangGoi"     => new SolidColorBrush(Color.FromRgb(33,  150, 243)),
                "HuyBo"       => new SolidColorBrush(Color.FromRgb(244, 67,  54)),
                "DaXong"      => new SolidColorBrush(Color.FromRgb(76,  175, 80)),
                "DangPha"     => new SolidColorBrush(Color.FromRgb(255, 152, 0)),
                "ChoPha"      => new SolidColorBrush(Color.FromRgb(158, 158, 158)),
                _             => new SolidColorBrush(Colors.Gray)
            };
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
