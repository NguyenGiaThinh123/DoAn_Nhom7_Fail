namespace QuanLyCaPheApp.Models
{
    public class LoaiSanPham
    {
        public int MaLoai { get; set; }
        public string TenLoai { get; set; } = "";
        public string? MoTa { get; set; }
        public int ThuTuHienThi { get; set; }
        public bool TrangThai { get; set; } = true;
        public DateTime NgayTao { get; set; }
        public string TrangThaiText => TrangThai ? "Hoạt động" : "Ẩn";
    }
}
