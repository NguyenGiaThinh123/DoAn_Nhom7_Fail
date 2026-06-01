using QuanLyCaPheApp.Views;
using System.Windows;

namespace QuanLyCaPheApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Xử lý exception toàn cục để app không crash âm thầm
            DispatcherUnhandledException += (s, ex) =>
            {
                MessageBox.Show($"Lỗi không xác định:\n{ex.Exception.Message}",
                    "Lỗi Hệ Thống", MessageBoxButton.OK, MessageBoxImage.Error);
                ex.Handled = true;
            };

            var loginWin = new DangNhapView();
            loginWin.Show();
        }
    }
}
