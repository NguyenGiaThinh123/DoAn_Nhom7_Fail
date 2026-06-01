namespace QuanLyCaPheApp.Models
{
    public class ChiTietHoaDon
    {
        public int MaChiTiet { get; set; }
        public int MaHoaDon { get; set; }
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; } = "";
        public decimal DonGia { get; set; }
        public int SoLuong { get; set; } = 1;
        public decimal PhanTramGiam { get; set; }
        public decimal ThanhTien { get; set; }
        public string TrangThaiMon { get; set; } = "ChoPha";
        public string? GhiChuMon { get; set; }
        public DateTime NgayTao { get; set; }

        public string TrangThaiText => TrangThaiMon switch
        {
            "ChoPha"  => "Chờ pha",
            "DangPha" => "Đang pha",
            "DaXong"  => "Đã xong",
            "HuyBo"   => "Hủy bỏ",
            _         => TrangThaiMon
        };
        public string ThanhTienText => $"{ThanhTien:N0} đ";
        public string DonGiaText    => $"{DonGia:N0} đ";
    }
}
