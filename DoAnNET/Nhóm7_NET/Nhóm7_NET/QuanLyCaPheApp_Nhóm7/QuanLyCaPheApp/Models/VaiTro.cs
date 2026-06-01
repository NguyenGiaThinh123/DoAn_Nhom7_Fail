namespace QuanLyCaPheApp.Models
{
    public class VaiTro
    {
        public int MaVaiTro { get; set; }
        public string TenVaiTro { get; set; } = "";
        public string? MoTa { get; set; }
        public bool QuyenQuanLyNguoiDung { get; set; }
        public bool QuyenQuanLySanPham { get; set; }
        public bool QuyenDatBan { get; set; }
        public bool QuyenGoiMon { get; set; }
        public bool QuyenThanhToan { get; set; }
        public bool QuyenQuanLyKhachHang { get; set; }
        public bool QuyenThongKe { get; set; }
        public bool QuyenXuatBaoCao { get; set; }
        public DateTime NgayTao { get; set; }
    }
}
