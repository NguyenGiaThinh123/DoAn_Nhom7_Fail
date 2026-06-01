using QuanLyCaPheApp.Helpers;
using QuanLyCaPheApp.Views;
using System.Windows;
using System.Windows.Controls;

namespace QuanLyCaPheApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetupUser();
            HideRestrictedMenus();
            // Delay navigate để đảm bảo Window đã render xong
            Loaded += (s, e) => NavigateTo("TrangChu");
        }

        private void SetupUser()
        {
            if (SessionManager.CurrentUser != null)
                txtUserInfo.Text = $"{SessionManager.CurrentUser.HoTen}  |  {SessionManager.CurrentUser.TenVaiTro}";
        }

        private void HideRestrictedMenus()
        {
            if (SessionManager.IsPhaChe)
            {
                btnGoiMon.Visibility    = Visibility.Collapsed;
                btnDatBan.Visibility    = Visibility.Collapsed;
                btnThanhToan.Visibility = Visibility.Collapsed;
                btnKhachHang.Visibility = Visibility.Collapsed;
                btnNguoiDung.Visibility = Visibility.Collapsed;
                btnThongKe.Visibility   = Visibility.Collapsed;
                btnBaoCao.Visibility    = Visibility.Collapsed;
            }
            if (SessionManager.IsNhanVien)
            {
                btnNguoiDung.Visibility = Visibility.Collapsed;
                btnThongKe.Visibility   = Visibility.Collapsed;
                btnBaoCao.Visibility    = Visibility.Collapsed;
                btnSanPham.Visibility   = Visibility.Collapsed;
            }
        }

        private void NavBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string tag)
                NavigateTo(tag);
        }

        private void NavigateTo(string page)
        {
            try
            {
                // Reset style tất cả nav buttons
                foreach (var btn in new[] { btnTrangChu, btnGoiMon, btnDatBan, btnThanhToan,
                                             btnSanPham, btnKhachHang, btnNguoiDung, btnThongKe, btnBaoCao })
                {
                    if (btn != null)
                        btn.Style = (Style)FindResource("NavButton");
                }

                // Đánh dấu button đang active
                Button? activeBtn = page switch
                {
                    "TrangChu"  => btnTrangChu,
                    "GoiMon"    => btnGoiMon,
                    "DatBan"    => btnDatBan,
                    "ThanhToan" => btnThanhToan,
                    "SanPham"   => btnSanPham,
                    "KhachHang" => btnKhachHang,
                    "NguoiDung" => btnNguoiDung,
                    "ThongKe"   => btnThongKe,
                    "BaoCao"    => btnBaoCao,
                    _           => btnTrangChu
                };
                if (activeBtn != null)
                    activeBtn.Style = (Style)FindResource("NavButtonActive");

                // Tạo và điều hướng đến View tương ứng
                object? view = page switch
                {
                    "TrangChu"  => new QuanLyCaPheApp.Views.TrangChuView(),
                    "GoiMon"    => new QuanLyCaPheApp.Views.GoiMonView(),
                    "DatBan"    => new QuanLyCaPheApp.Views.DatBanView(),
                    "ThanhToan" => new QuanLyCaPheApp.Views.ThanhToanView(),
                    "SanPham"   => new QuanLyCaPheApp.Views.QuanLySanPhamView(),
                    "KhachHang" => new QuanLyCaPheApp.Views.QuanLyKhachHangView(),
                    "NguoiDung" => new QuanLyCaPheApp.Views.QuanLyNguoiDungView(),
                    "ThongKe"   => new QuanLyCaPheApp.Views.ThongKeView(),
                    "BaoCao"    => new QuanLyCaPheApp.Views.BaoCaoView(),
                    _           => new QuanLyCaPheApp.Views.TrangChuView()
                };

                MainFrame.Navigate(view);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải trang '{page}':\n{ex.Message}",
                    "Lỗi Navigation", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc muốn đăng xuất?",
                "Xác Nhận Đăng Xuất", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                SessionManager.Logout();
                var loginWin = new QuanLyCaPheApp.Views.DangNhapView();
                Application.Current.MainWindow = loginWin;
                loginWin.Show();
                Close();
            }
        }

        // Cho phép các View con điều hướng ra ngoài
        public void NavigateToPage(string page) => NavigateTo(page);
    }
}