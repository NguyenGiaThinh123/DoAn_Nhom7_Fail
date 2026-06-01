namespace QuanLyCaPheApp.Models
{
    public class Ban
    {
        public int MaBan { get; set; }
        public string TenBan { get; set; } = "";
        public int MaKhuVuc { get; set; }
        public int SoGhe { get; set; } = 4;
        public int ViTriHang { get; set; } = 1;
        public int ViTriCot { get; set; } = 1;
        public string TrangThai { get; set; } = "Trống";
        public string? GhiChu { get; set; }
        public DateTime? NgayCapNhat { get; set; }

        // Navigation
        public KhuVuc? KhuVuc { get; set; }
        public string TenKhuVuc => KhuVuc?.TenKhuVuc ?? "";

        // Color for UI display
        public string MauTrangThai => TrangThai switch
        {
            "Trống"     => "#4CAF50",
            "Đang dùng" => "#F44336",
            "Đã đặt"    => "#FFC107",
            "Bảo trì"   => "#9E9E9E",
            _           => "#4CAF50"
        };

        public string MauChuTrangThai => TrangThai == "Đã đặt" ? "#1A1A1A" : "#FFFFFF";
    }
}
