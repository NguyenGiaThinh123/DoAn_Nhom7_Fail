namespace QuanLyCaPheApp.Models
{
    public class LichSuDatBan
    {
        public int MaDatBan { get; set; }
        public int MaBan { get; set; }
        public int? MaKhachHang { get; set; }
        public string TenKhachDat { get; set; } = "";
        public string SoDienThoai { get; set; } = "";
        public int SoNguoi { get; set; } = 1;
        public DateTime NgayDat { get; set; }
        public DateTime? GioNhanBan { get; set; }
        public DateTime? GioRoiBan { get; set; }
        public int? MaHoaDon { get; set; }
        public string TrangThai { get; set; } = "DatTruoc";
        public string? GhiChu { get; set; }
        public int MaNguoiTao { get; set; }
        public DateTime NgayTao { get; set; }

        // Display helpers
        public string TenBan { get; set; } = "";

        public string TrangThaiText => TrangThai switch
        {
            "DatTruoc" => "Đặt trước",
            "DangDen"  => "Đang đến",
            "DaXong"   => "Đã xong",
            "HuyBo"    => "Hủy bỏ",
            _          => TrangThai
        };
    }
}
