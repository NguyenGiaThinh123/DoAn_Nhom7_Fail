using Microsoft.Data.SqlClient;
using QuanLyCaPheApp.Helpers;
using QuanLyCaPheApp.Models;

namespace QuanLyCaPheApp.Repositories
{
    public class BanRepository : BaseRepository
    {
        // Map từ reader sang Ban - an toàn với mọi schema
        private static Ban MapBan(SqlDataReader r) => new()
        {
            MaBan       = I(r, "MaBan"),
            TenBan      = S(r, "TenBan"),
            MaKhuVuc    = IN(r, "MaKhuVuc") ?? 1,
            SoGhe       = I(r, "SoGhe"),
            ViTriHang   = I(r, "ViTriHang"),
            ViTriCot    = I(r, "ViTriCot"),
            TrangThai   = S(r, "TrangThai"),
            GhiChu      = S(r, "GhiChu"),
            NgayCapNhat = DTN(r, "NgayCapNhat"),
            // TenKhuVuc có thể không có nếu không JOIN
            KhuVuc = new KhuVuc
            {
                MaKhuVuc  = IN(r, "MaKhuVuc") ?? 1,
                TenKhuVuc = S(r, "TenKhuVuc")   // SafeString trả "" nếu không có cột
            }
        };

        public List<Ban> GetAll()
        {
            // LEFT JOIN để không crash nếu KhuVuc không tồn tại
            const string sql = @"
                SELECT b.MaBan, b.TenBan, b.MaKhuVuc, b.SoGhe,
                       b.ViTriHang, b.ViTriCot, b.TrangThai, b.GhiChu, b.NgayCapNhat,
                       ISNULL(kv.TenKhuVuc, N'Tầng 1') AS TenKhuVuc
                FROM Ban b
                LEFT JOIN KhuVuc kv ON b.MaKhuVuc = kv.MaKhuVuc
                ORDER BY b.ViTriHang, b.ViTriCot, b.TenBan";
            return DatabaseHelper.ExecuteQuery(sql, MapBan);
        }

        public List<Ban> GetAllKhuVuc()
        {
            // Lấy theo khu vực, an toàn
            return GetAll();
        }

        public Ban? GetById(int maBan)
        {
            const string sql = @"
                SELECT b.MaBan, b.TenBan, b.MaKhuVuc, b.SoGhe,
                       b.ViTriHang, b.ViTriCot, b.TrangThai, b.GhiChu, b.NgayCapNhat,
                       ISNULL(kv.TenKhuVuc, N'Tầng 1') AS TenKhuVuc
                FROM Ban b
                LEFT JOIN KhuVuc kv ON b.MaKhuVuc = kv.MaKhuVuc
                WHERE b.MaBan = @id";
            return DatabaseHelper.ExecuteQuerySingle(sql, MapBan,
                [new SqlParameter("@id", maBan)]);
        }

        public List<string> GetDanhSachKhuVuc()
        {
            // Thử lấy từ KhuVuc table, nếu không có thì trả default
            try
            {
                var result = DatabaseHelper.ExecuteQuery(
                    "SELECT DISTINCT TenKhuVuc FROM KhuVuc ORDER BY TenKhuVuc",
                    r => DatabaseHelper.SafeString(r, "TenKhuVuc"));
                return result.Count > 0 ? result : new List<string> { "Tầng 1" };
            }
            catch
            {
                return new List<string> { "Tầng 1" };
            }
        }

        public void CapNhatTrangThai(int maBan, string trangThai)
        {
            DatabaseHelper.ExecuteNonQuery(
                "UPDATE Ban SET TrangThai=@t, NgayCapNhat=GETDATE() WHERE MaBan=@id",
                [new SqlParameter("@t", trangThai), new SqlParameter("@id", maBan)]);
        }

        public void AddBan(Ban ban)
        {
            DatabaseHelper.ExecuteNonQuery(@"
                INSERT INTO Ban (TenBan, MaKhuVuc, SoGhe, ViTriHang, ViTriCot, TrangThai, GhiChu)
                VALUES (@ten, @kv, @sg, @hr, @co, @tt, @gc)",
                [new SqlParameter("@ten", ban.TenBan),
                 new SqlParameter("@kv",  ban.MaKhuVuc),
                 new SqlParameter("@sg",  ban.SoGhe),
                 new SqlParameter("@hr",  ban.ViTriHang),
                 new SqlParameter("@co",  ban.ViTriCot),
                 new SqlParameter("@tt",  ban.TrangThai),
                 new SqlParameter("@gc",  (object?)ban.GhiChu ?? DBNull.Value)]);
        }

        public void UpdateBan(Ban ban)
        {
            DatabaseHelper.ExecuteNonQuery(@"
                UPDATE Ban SET TenBan=@ten, SoGhe=@sg, TrangThai=@tt,
                               GhiChu=@gc, NgayCapNhat=GETDATE()
                WHERE MaBan=@id",
                [new SqlParameter("@ten", ban.TenBan),
                 new SqlParameter("@sg",  ban.SoGhe),
                 new SqlParameter("@tt",  ban.TrangThai),
                 new SqlParameter("@gc",  (object?)ban.GhiChu ?? DBNull.Value),
                 new SqlParameter("@id",  ban.MaBan)]);
        }
    }
}
