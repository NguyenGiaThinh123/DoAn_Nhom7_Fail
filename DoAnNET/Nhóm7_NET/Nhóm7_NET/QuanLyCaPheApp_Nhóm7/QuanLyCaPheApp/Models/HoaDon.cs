namespace QuanLyCaPheApp.Models
{
    public class HoaDon
    {
        public int MaHoaDon { get; set; }
        public int MaBan { get; set; }
        public int? MaKhachHang { get; set; }
        public int MaNguoiTao { get; set; }
        public int? MaNguoiCapNhat { get; set; }
        public int? MaKhuyenMai { get; set; }
        public string LoaiHoaDon { get; set; } = "TraSau";
        public string TrangThai { get; set; } = "DangGoi";
        public decimal TongTamTinh { get; set; }
        public decimal SoTienGiam { get; set; }
        public int DiemDung { get; set; }
        public decimal TongThanhToan { get; set; }
        public decimal? TienKhachDua { get; set; }
        public decimal? TienThua { get; set; }
        public string PhuongThucTT { get; set; } = "TienMat";
        public string? GhiChu { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime? NgayThanhToan { get; set; }

        // Display helpers
        public string TenBan { get; set; } = "";
        public string TenKhachHang { get; set; } = "";
        public string TenNguoiTao { get; set; } = "";
        public List<ChiTietHoaDon> ChiTiet { get; set; } = new();

        public string TrangThaiText => TrangThai switch
        {
            "DangGoi"     => "Đang gọi",
            "DaThanhToan" => "Đã thanh toán",
            "HuyBo"       => "Hủy bỏ",
            "ChoXacNhan"  => "Chờ xác nhận",
            _             => TrangThai
        };
        public string LoaiHoaDonText => LoaiHoaDon == "TraTruoc" ? "Trả trước" : "Trả sau";
        public string TongThanhToanText => $"{TongThanhToan:N0} đ";
        public string PhuongThucText => PhuongThucTT switch
        {
            "TienMat"    => "Tiền mặt",
            "ChuyenKhoan"=> "Chuyển khoản",
            "TheNganHang"=> "Thẻ ngân hàng",
            _            => PhuongThucTT
        };
    }
}
