using Microsoft.Data.SqlClient;
using QuanLyCaPheApp.Helpers;
using QuanLyCaPheApp.Models;

namespace QuanLyCaPheApp.Repositories
{
    public class NguoiDungRepository : BaseRepository
    {
        private NguoiDung Map(SqlDataReader r) => new()
        {
            MaNguoiDung = I(r, "MaNguoiDung"),
            TenDangNhap = S(r, "TenDangNhap"),
            MatKhauHash = S(r, "MatKhauHash"),
            HoTen       = S(r, "HoTen"),
            Email       = S(r, "Email"),
            SoDienThoai = S(r, "SoDienThoai"),
            MaVaiTro    = I(r, "MaVaiTro"),
            TrangThai   = B(r, "TrangThai"),
            NgayTao     = DT(r, "NgayTao"),
            NgayCapNhat = DTN(r, "NgayCapNhat"),
            VaiTro = new VaiTro
            {
                MaVaiTro  = I(r, "MaVaiTro"),
                TenVaiTro = S(r, "TenVaiTro"),
                QuyenQuanLyNguoiDung = B(r, "QuyenQuanLyNguoiDung"),
                QuyenQuanLySanPham   = B(r, "QuyenQuanLySanPham"),
                QuyenDatBan          = B(r, "QuyenDatBan"),
                QuyenGoiMon          = B(r, "QuyenGoiMon"),
                QuyenThanhToan       = B(r, "QuyenThanhToan"),
                QuyenQuanLyKhachHang = B(r, "QuyenQuanLyKhachHang"),
                QuyenThongKe         = B(r, "QuyenThongKe"),
                QuyenXuatBaoCao      = B(r, "QuyenXuatBaoCao"),
            }
        };

        private const string SelectSql = @"
            SELECT nd.*, vt.TenVaiTro,
                vt.QuyenQuanLyNguoiDung, vt.QuyenQuanLySanPham, vt.QuyenDatBan,
                vt.QuyenGoiMon, vt.QuyenThanhToan, vt.QuyenQuanLyKhachHang,
                vt.QuyenThongKe, vt.QuyenXuatBaoCao
            FROM NguoiDung nd JOIN VaiTro vt ON nd.MaVaiTro = vt.MaVaiTro";

        public NguoiDung? DangNhap(string tenDangNhap, string matKhauHash)
            => DatabaseHelper.ExecuteQuerySingle(
                SelectSql + " WHERE nd.TenDangNhap = @u AND nd.MatKhauHash = @p AND nd.TrangThai = 1",
                Map,
                [new("@u", tenDangNhap), new("@p", matKhauHash)]);

        public List<NguoiDung> GetAll(string keyword = "")
        {
            var sql = SelectSql;
            if (!string.IsNullOrEmpty(keyword))
                sql += " WHERE nd.HoTen LIKE @kw OR nd.TenDangNhap LIKE @kw";
            sql += " ORDER BY nd.HoTen";
            var p = string.IsNullOrEmpty(keyword) ? null
                : new SqlParameter[] { new("@kw", $"%{keyword}%") };
            return DatabaseHelper.ExecuteQuery(sql, Map, p);
        }

        public NguoiDung? GetById(int id)
            => DatabaseHelper.ExecuteQuerySingle(SelectSql + " WHERE nd.MaNguoiDung = @id",
                Map, [new("@id", id)]);

        public int Add(NguoiDung nd)
        {
            var sql = @"INSERT INTO NguoiDung (TenDangNhap,MatKhauHash,HoTen,Email,SoDienThoai,MaVaiTro,TrangThai)
                        VALUES (@u,@p,@h,@e,@s,@r,@t);
                        SELECT SCOPE_IDENTITY();";
            var result = DatabaseHelper.ExecuteScalar(sql, [
                new("@u", nd.TenDangNhap), new("@p", nd.MatKhauHash),
                new("@h", nd.HoTen),       new("@e", (object?)nd.Email ?? DBNull.Value),
                new("@s", (object?)nd.SoDienThoai ?? DBNull.Value),
                new("@r", nd.MaVaiTro),    new("@t", nd.TrangThai)]);
            return System.Convert.ToInt32(result);
        }

        public bool Update(NguoiDung nd)
        {
            var sql = @"UPDATE NguoiDung SET HoTen=@h, Email=@e, SoDienThoai=@s,
                        MaVaiTro=@r, TrangThai=@t, NgayCapNhat=GETDATE()
                        WHERE MaNguoiDung=@id";
            return DatabaseHelper.ExecuteNonQuery(sql, [
                new("@h", nd.HoTen), new("@e", (object?)nd.Email ?? DBNull.Value),
                new("@s", (object?)nd.SoDienThoai ?? DBNull.Value),
                new("@r", nd.MaVaiTro), new("@t", nd.TrangThai),
                new("@id", nd.MaNguoiDung)]) > 0;
        }

        public bool DoiMatKhau(int maNguoiDung, string matKhauMoiHash)
        {
            return DatabaseHelper.ExecuteNonQuery(
                "UPDATE NguoiDung SET MatKhauHash=@p, NgayCapNhat=GETDATE() WHERE MaNguoiDung=@id",
                [new("@p", matKhauMoiHash), new("@id", maNguoiDung)]) > 0;
        }

        public bool KhoaTK(int maNguoiDung, bool trangThai)
            => DatabaseHelper.ExecuteNonQuery(
                "UPDATE NguoiDung SET TrangThai=@t WHERE MaNguoiDung=@id",
                [new("@t", trangThai), new("@id", maNguoiDung)]) > 0;

        public bool CheckTrungTenDangNhap(string ten, int excludeId = 0)
        {
            var r = DatabaseHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM NguoiDung WHERE TenDangNhap=@u AND MaNguoiDung<>@id",
                [new("@u", ten), new("@id", excludeId)]);
            return System.Convert.ToInt32(r) > 0;
        }

        public List<VaiTro> GetAllVaiTro()
            => DatabaseHelper.ExecuteQuery(
                "SELECT * FROM VaiTro ORDER BY MaVaiTro",
                r => new VaiTro
                {
                    MaVaiTro  = I(r, "MaVaiTro"),
                    TenVaiTro = S(r, "TenVaiTro"),
                    MoTa      = S(r, "MoTa"),
                    QuyenQuanLyNguoiDung = B(r, "QuyenQuanLyNguoiDung"),
                    QuyenQuanLySanPham   = B(r, "QuyenQuanLySanPham"),
                    QuyenDatBan          = B(r, "QuyenDatBan"),
                    QuyenGoiMon          = B(r, "QuyenGoiMon"),
                    QuyenThanhToan       = B(r, "QuyenThanhToan"),
                    QuyenQuanLyKhachHang = B(r, "QuyenQuanLyKhachHang"),
                    QuyenThongKe         = B(r, "QuyenThongKe"),
                    QuyenXuatBaoCao      = B(r, "QuyenXuatBaoCao"),
                });
    }
}
