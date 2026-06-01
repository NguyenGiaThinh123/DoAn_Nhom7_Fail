using Microsoft.Data.SqlClient;
using QuanLyCaPheApp.Helpers;

namespace QuanLyCaPheApp.Repositories
{
    public class ThongKeRepository : BaseRepository
    {
        // Thay thế EXEC sp_ThongKeDoanhThu bằng SQL trực tiếp
        public List<ThongKeNgay> GetThongKeTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            const string sql = @"
                SELECT
                    CAST(hd.NgayThanhToan AS DATE) AS Ngay,
                    COUNT(*)                        AS SoHoaDon,
                    SUM(hd.TongTamTinh)             AS DoanhThuGoc,
                    SUM(hd.SoTienGiam)              AS TongGiamGia,
                    SUM(hd.TongThanhToan)           AS ThucThu,
                    COUNT(DISTINCT hd.MaKhachHang)  AS SoKhachHang
                FROM HoaDon hd
                WHERE hd.TrangThai = 'DaThanhToan'
                  AND CAST(hd.NgayThanhToan AS DATE) BETWEEN @TuNgay AND @DenNgay
                GROUP BY CAST(hd.NgayThanhToan AS DATE)
                ORDER BY Ngay";

            return DatabaseHelper.ExecuteQuery(sql,
                r => new ThongKeNgay
                {
                    Ngay        = DT(r, "Ngay"),
                    SoHoaDon    = I(r,  "SoHoaDon"),
                    DoanhThuGoc = D(r,  "DoanhThuGoc"),
                    TongGiamGia = D(r,  "TongGiamGia"),
                    ThucThu     = D(r,  "ThucThu"),
                    SoKhachHang = I(r,  "SoKhachHang"),
                },
                [new SqlParameter("@TuNgay",  tuNgay.ToString("yyyy-MM-dd")),
                 new SqlParameter("@DenNgay", denNgay.ToString("yyyy-MM-dd"))]);
        }

        public List<TopSanPham> GetTopSanPham(DateTime tuNgay, DateTime denNgay, int top = 10)
        {
            const string sql = @"
                SELECT TOP(@top)
                    ct.TenSanPham,
                    SUM(ct.SoLuong)   AS TongSoLuong,
                    SUM(ct.ThanhTien) AS TongDoanhThu
                FROM ChiTietHoaDon ct
                JOIN HoaDon hd ON ct.MaHoaDon = hd.MaHoaDon
                WHERE hd.TrangThai = 'DaThanhToan'
                  AND CAST(hd.NgayThanhToan AS DATE) BETWEEN @tn AND @dn
                GROUP BY ct.TenSanPham
                ORDER BY TongDoanhThu DESC";

            return DatabaseHelper.ExecuteQuery(sql,
                r => new TopSanPham
                {
                    TenSanPham   = S(r, "TenSanPham"),
                    TongSoLuong  = I(r, "TongSoLuong"),
                    TongDoanhThu = D(r, "TongDoanhThu"),
                },
                [new SqlParameter("@top", top),
                 new SqlParameter("@tn",  tuNgay.ToString("yyyy-MM-dd")),
                 new SqlParameter("@dn",  denNgay.ToString("yyyy-MM-dd"))]);
        }

        public decimal GetDoanhThuHomNay()
        {
            var r = DatabaseHelper.ExecuteScalar(@"
                SELECT ISNULL(SUM(TongThanhToan), 0)
                FROM HoaDon
                WHERE TrangThai = 'DaThanhToan'
                  AND CAST(NgayThanhToan AS DATE) = CAST(GETDATE() AS DATE)");
            return r == null ? 0m : System.Convert.ToDecimal(r);
        }

        public int GetSoHoaDonHomNay()
        {
            var r = DatabaseHelper.ExecuteScalar(@"
                SELECT COUNT(*)
                FROM HoaDon
                WHERE TrangThai = 'DaThanhToan'
                  AND CAST(NgayThanhToan AS DATE) = CAST(GETDATE() AS DATE)");
            return r == null ? 0 : System.Convert.ToInt32(r);
        }

        public int GetSoBanDangDung()
        {
            // Kiểm tra cả hai dạng: có dấu và không dấu
            var r = DatabaseHelper.ExecuteScalar(@"
                SELECT COUNT(*)
                FROM Ban
                WHERE TrangThai IN (N'Đang dùng', N'Dang dung')");
            return r == null ? 0 : System.Convert.ToInt32(r);
        }

        public int GetSoKhachHangMoi()
        {
            var r = DatabaseHelper.ExecuteScalar(@"
                SELECT COUNT(*)
                FROM KhachHang
                WHERE CAST(NgayDangKy AS DATE) = CAST(GETDATE() AS DATE)");
            return r == null ? 0 : System.Convert.ToInt32(r);
        }
    }

    // ========= MODEL CLASSES =========
    public class ThongKeNgay
    {
        public DateTime Ngay        { get; set; }
        public int      SoHoaDon    { get; set; }
        public decimal  DoanhThuGoc { get; set; }
        public decimal  TongGiamGia { get; set; }
        public decimal  ThucThu     { get; set; }
        public int      SoKhachHang { get; set; }

        public string NgayText     => Ngay.ToString("dd/MM/yyyy");
        public string ThucThuText  => $"{ThucThu:N0} đ";
    }

    public class TopSanPham
    {
        public string  TenSanPham   { get; set; } = "";
        public int     TongSoLuong  { get; set; }
        public decimal TongDoanhThu { get; set; }

        public string TongDoanhThuText => $"{TongDoanhThu:N0} đ";
    }
}
