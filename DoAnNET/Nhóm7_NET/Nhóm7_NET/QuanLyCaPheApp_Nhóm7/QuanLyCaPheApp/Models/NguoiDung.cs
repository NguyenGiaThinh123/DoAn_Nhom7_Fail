namespace QuanLyCaPheApp.Models
{
    public class NguoiDung
    {
        public int MaNguoiDung { get; set; }
        public string TenDangNhap { get; set; } = "";
        public string MatKhauHash { get; set; } = "";
        public string HoTen { get; set; } = "";
        public string? Email { get; set; }
        public string? SoDienThoai { get; set; }
        public int MaVaiTro { get; set; }
        public bool TrangThai { get; set; } = true;
        public DateTime NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }

        // Navigation
        public VaiTro? VaiTro { get; set; }
        public string TenVaiTro => VaiTro?.TenVaiTro ?? "";
        public string TrangThaiText => TrangThai ? "Hoạt động" : "Bị khóa";
    }
}
