using QuanLyCaPheApp.Models;

namespace QuanLyCaPheApp.Helpers
{
    public static class SessionManager
    {
        public static NguoiDung? CurrentUser { get; private set; }
        public static VaiTro?   CurrentRole { get; private set; }

        public static void Login(NguoiDung user, VaiTro role)
        {
            CurrentUser = user;
            CurrentRole = role;
        }

        public static void Logout()
        {
            CurrentUser = null;
            CurrentRole = null;
        }

        public static bool IsLoggedIn => CurrentUser != null;

        public static bool CanManageUsers     => CurrentRole?.QuyenQuanLyNguoiDung ?? false;
        public static bool CanManageProducts  => CurrentRole?.QuyenQuanLySanPham   ?? false;
        public static bool CanBookTable       => CurrentRole?.QuyenDatBan          ?? false;
        public static bool CanOrder           => CurrentRole?.QuyenGoiMon          ?? false;
        public static bool CanCheckout        => CurrentRole?.QuyenThanhToan       ?? false;
        public static bool CanManageCustomers => CurrentRole?.QuyenQuanLyKhachHang ?? false;
        public static bool CanViewStats       => CurrentRole?.QuyenThongKe         ?? false;
        public static bool CanExportReport    => CurrentRole?.QuyenXuatBaoCao      ?? false;

        public static bool IsAdmin     => CurrentRole?.TenVaiTro == "Admin";
        public static bool IsPhaChe    => CurrentRole?.TenVaiTro == "PhaChe";
        public static bool IsNhanVien  => CurrentRole?.TenVaiTro == "NhanVien";
    }
}
