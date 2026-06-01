namespace QuanLyCaPheApp.Models
{
    public class KhuyenMai
    {
        public int MaKhuyenMai { get; set; }
        public string TenKhuyenMai { get; set; } = "";
        public string? MaCode { get; set; }
        public string LoaiGiam { get; set; } = "PhanTram";
        public decimal GiaTriGiam { get; set; }
        public decimal GiaTriDonToiThieu { get; set; }
        public decimal? GiamToiDa { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public int? SoLanSuDung { get; set; }
        public int DaSuDung { get; set; }
        public bool TrangThai { get; set; } = true;
        public DateTime NgayTao { get; set; }

        public bool ConHieuLuc =>
            TrangThai &&
            DateTime.Now.Date >= NgayBatDau &&
            DateTime.Now.Date <= NgayKetThuc &&
            (SoLanSuDung == null || DaSuDung < SoLanSuDung);

        public string GiaTriText => LoaiGiam == "PhanTram"
            ? $"-{GiaTriGiam}%"
            : $"-{GiaTriGiam:N0} đ";

        public string ThongTinKM => $"{TenKhuyenMai} ({GiaTriText})";
    }
}
