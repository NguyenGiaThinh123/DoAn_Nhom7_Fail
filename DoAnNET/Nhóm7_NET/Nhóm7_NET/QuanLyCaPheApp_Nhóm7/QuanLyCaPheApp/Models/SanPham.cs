namespace QuanLyCaPheApp.Models
{
    public class SanPham
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; } = "";
        public int MaLoai { get; set; }
        public decimal GiaBan { get; set; }
        public decimal? GiaGoc { get; set; }
        public decimal PhanTramGiam { get; set; }
        public string? MoTa { get; set; }
        public string DonVi { get; set; } = "Ly";
        public int TichDiemNhan { get; set; } = 1;
        public string? HinhAnh { get; set; }
        public bool TrangThai { get; set; } = true;
        public DateTime NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }

        // Navigation
        public LoaiSanPham? LoaiSanPham { get; set; }
        public string TenLoai => LoaiSanPham?.TenLoai ?? "";

        public decimal GiaSauGiam => PhanTramGiam > 0
            ? Math.Round(GiaBan * (1 - PhanTramGiam / 100), 0)
            : GiaBan;

        public string GiaBanText => $"{GiaSauGiam:N0} đ";
        public string TrangThaiText => TrangThai ? "Đang bán" : "Dừng bán";
        public string PhanTramGiamText => PhanTramGiam > 0 ? $"-{PhanTramGiam}%" : "";
    }
}
