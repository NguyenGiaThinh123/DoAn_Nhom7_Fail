namespace QuanLyCaPheApp.Models
{
    public class KhachHang
    {
        public int MaKhachHang { get; set; }
        public string HoTen { get; set; } = "";
        public string SoDienThoai { get; set; } = "";
        public string? Email { get; set; }
        public string? DiaChi { get; set; }
        public DateTime? NgaySinh { get; set; }
        public int TongDiemTichLuy { get; set; }
        public decimal TongChiTieu { get; set; }
        public string HangKhachHang { get; set; } = "Thường";
        public string? GhiChu { get; set; }
        public DateTime NgayDangKy { get; set; }
        public DateTime? NgayCapNhat { get; set; }

        public string ThongTin => $"{HoTen} - {SoDienThoai}";
        public string DiemText => $"{TongDiemTichLuy:N0} điểm";
        public string ChiTieuText => $"{TongChiTieu:N0} đ";
    }
}
