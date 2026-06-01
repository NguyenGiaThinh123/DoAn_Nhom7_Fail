using QuanLyCaPheApp.Helpers;
using QuanLyCaPheApp.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace QuanLyCaPheApp.Views
{
    public partial class DangNhapView : Window
    {
        private DangNhapViewModel _vm;

        public DangNhapView()
        {
            InitializeComponent();
            _vm = new DangNhapViewModel();
            DataContext = _vm;

            // Callback khi đăng nhập thành công
            _vm.DangNhapThanhCong = () =>
            {
                try
                {
                    var mainWin = new MainWindow();

                    // *** FIX CHÍNH: Đặt MainWindow là cửa sổ chính ***
                    // Điều này ngăn app tắt khi cửa sổ đăng nhập đóng
                    Application.Current.MainWindow = mainWin;

                    // Khi MainWindow đóng -> shutdown app
                    mainWin.Closed += (s, e) => Application.Current.Shutdown();

                    mainWin.Show();
                    mainWin.Activate();  // Đảm bảo MainWindow focus
                    Close();             // Đóng cửa sổ đăng nhập
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không thể mở giao diện chính:\n{ex.Message}",
                        "Lỗi Khởi Động", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };
        }

        private void btnDangNhap_Click(object sender, RoutedEventArgs e)
        {
            _vm.DangNhapCommand.Execute(pbPass.Password);
        }

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                _vm.DangNhapCommand.Execute(pbPass.Password);
        }
    }
}
