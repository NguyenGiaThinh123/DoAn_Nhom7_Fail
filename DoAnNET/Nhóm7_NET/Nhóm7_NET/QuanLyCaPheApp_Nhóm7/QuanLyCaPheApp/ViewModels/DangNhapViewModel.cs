using QuanLyCaPheApp.Helpers;
using QuanLyCaPheApp.Repositories;
using System.Windows;
using System.Windows.Input;

namespace QuanLyCaPheApp.ViewModels
{
    public class DangNhapViewModel : BaseViewModel
    {
        private readonly NguoiDungRepository _repo = new();

        private string _tenDangNhap = "";
        private string _thongBaoLoi = "";
        private bool   _dangXuLy;

        public string TenDangNhap
        {
            get => _tenDangNhap;
            set { SetProperty(ref _tenDangNhap, value); ThongBaoLoi = ""; }
        }

        public string ThongBaoLoi
        {
            get => _thongBaoLoi;
            set => SetProperty(ref _thongBaoLoi, value);
        }

        public bool DangXuLy
        {
            get => _dangXuLy;
            set => SetProperty(ref _dangXuLy, value);
            // CommandManager.RequerySuggested sẽ tự re-evaluate CanExecute
        }

        public bool CoThongBaoLoi => !string.IsNullOrEmpty(ThongBaoLoi);

        public Action? DangNhapThanhCong { get; set; }
        public ICommand DangNhapCommand  { get; }

        public DangNhapViewModel()
        {
            DangNhapCommand = new RelayCommand(ExecuteDangNhap, _ => !DangXuLy);
        }

        private void ExecuteDangNhap(object? parameter)
        {
            ThongBaoLoi = "";

            if (string.IsNullOrWhiteSpace(TenDangNhap))
            {
                ThongBaoLoi = "Vui lòng nhập tên đăng nhập.";
                return;
            }

            var matKhau = parameter?.ToString() ?? "";
            if (string.IsNullOrWhiteSpace(matKhau))
            {
                ThongBaoLoi = "Vui lòng nhập mật khẩu.";
                return;
            }

            DangXuLy = true;
            try
            {
                var hash = HashHelper.SHA256Hash(matKhau);
                var user = _repo.DangNhap(TenDangNhap.Trim(), hash);

                if (user == null)
                {
                    ThongBaoLoi = "Tên đăng nhập hoặc mật khẩu không đúng.";
                    return;
                }

                if (user.TrangThai == false)
                {
                    ThongBaoLoi = "Tài khoản đã bị khóa. Vui lòng liên hệ quản trị.";
                    return;
                }

                user.VaiTro = user.VaiTro;  // VaiTro đã được join trong DangNhap query
                if (user.VaiTro == null)
                {
                    ThongBaoLoi = "Không xác định được vai trò người dùng.";
                    return;
                }

                SessionManager.Login(user, user.VaiTro);

                // Gọi callback (trên UI thread)
                DangNhapThanhCong?.Invoke();
            }
            catch (Exception ex)
            {
                ThongBaoLoi = $"Lỗi kết nối CSDL: {ex.Message}";
            }
            finally
            {
                DangXuLy = false;
            }
        }
    }
}
