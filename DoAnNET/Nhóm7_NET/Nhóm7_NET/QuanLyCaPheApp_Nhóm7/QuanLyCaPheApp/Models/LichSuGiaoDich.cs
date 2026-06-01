namespace QuanLyCaPheApp.Models
{
    public class LichSuGiaoDich
    {
        public int MaGiaoDich { get; set; }
        public string LoaiGiaoDich { get; set; } = "";
        public string MoTa { get; set; } = "";
        public int? MaNguoiThucHien { get; set; }
        public string? ThamSo { get; set; }
        public DateTime NgayGiaoDich { get; set; }
        public string TenNguoiThucHien { get; set; } = "";
    }
}
