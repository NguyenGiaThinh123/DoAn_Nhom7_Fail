namespace QuanLyCaPheApp.Models
{
    public class KhuVuc
    {
        public int MaKhuVuc { get; set; }
        public string TenKhuVuc { get; set; } = "";
        public string? MoTa { get; set; }
        public bool TrangThai { get; set; } = true;
    }
}
