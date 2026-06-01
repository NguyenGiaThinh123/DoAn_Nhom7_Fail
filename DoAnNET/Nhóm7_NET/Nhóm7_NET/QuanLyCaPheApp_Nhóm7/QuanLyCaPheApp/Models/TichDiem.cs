namespace QuanLyCaPheApp.Models
{
    public class TichDiem
    {
        public int MaTichDiem { get; set; }
        public int MaKhachHang { get; set; }
        public int? MaHoaDon { get; set; }
        public string LoaiGiaoDich { get; set; } = "TichDiem";
        public int SoDiem { get; set; }
        public int SoDiemSauGD { get; set; }
        public string? MoTa { get; set; }
        public DateTime NgayGiaoDich { get; set; }
        public int? MaNguoiThucHien { get; set; }

        public string TenKhachHang { get; set; } = "";

        public string LoaiText => LoaiGiaoDich switch
        {
            "TichDiem"  => "Tích điểm",
            "DoiDiem"   => "Đổi điểm",
            "HetHan"    => "Hết hạn",
            "DieuChinh" => "Điều chỉnh",
            _           => LoaiGiaoDich
        };
        public string SoDiemText => SoDiem >= 0 ? $"+{SoDiem}" : $"{SoDiem}";
    }
}
