using Microsoft.Data.SqlClient;
using QuanLyCaPheApp.Helpers;
using QuanLyCaPheApp.Models;

namespace QuanLyCaPheApp.Repositories
{
    public class KhachHangRepository : BaseRepository
    {
        private KhachHang Map(SqlDataReader r) => new()
        {
            MaKhachHang    = I(r, "MaKhachHang"),
            HoTen          = S(r, "HoTen"),
            SoDienThoai    = S(r, "SoDienThoai"),
            Email          = S(r, "Email"),
            DiaChi         = S(r, "DiaChi"),
            NgaySinh       = DTN(r, "NgaySinh"),
            TongDiemTichLuy= I(r, "TongDiemTichLuy"),
            TongChiTieu    = D(r, "TongChiTieu"),
            HangKhachHang  = S(r, "HangKhachHang"),
            GhiChu         = S(r, "GhiChu"),
            NgayDangKy     = DT(r, "NgayDangKy"),
            NgayCapNhat    = DTN(r, "NgayCapNhat"),
        };

        public List<KhachHang> GetAll(string keyword = "")
        {
            var sql = "SELECT * FROM KhachHang";
            if (!string.IsNullOrWhiteSpace(keyword))
                sql += " WHERE HoTen LIKE @kw OR SoDienThoai LIKE @kw OR Email LIKE @kw";
            sql += " ORDER BY HoTen";
            var p = string.IsNullOrWhiteSpace(keyword) ? null
                : new SqlParameter[] { new("@kw", $"%{keyword}%") };
            return DatabaseHelper.ExecuteQuery(sql, Map, p);
        }

        public KhachHang? GetById(int id)
            => DatabaseHelper.ExecuteQuerySingle(
                "SELECT * FROM KhachHang WHERE MaKhachHang=@id", Map, [new("@id", id)]);

        public KhachHang? GetBySoDienThoai(string sdt)
            => DatabaseHelper.ExecuteQuerySingle(
                "SELECT * FROM KhachHang WHERE SoDienThoai=@sdt", Map, [new("@sdt", sdt)]);

        public int Add(KhachHang kh)
        {
            var sql = @"INSERT INTO KhachHang (HoTen,SoDienThoai,Email,DiaChi,NgaySinh,GhiChu)
                        VALUES (@h,@s,@e,@d,@n,@g); SELECT SCOPE_IDENTITY();";
            var r = DatabaseHelper.ExecuteScalar(sql, [
                new("@h", kh.HoTen), new("@s", kh.SoDienThoai),
                new("@e", (object?)kh.Email   ?? DBNull.Value),
                new("@d", (object?)kh.DiaChi  ?? DBNull.Value),
                new("@n", (object?)kh.NgaySinh ?? DBNull.Value),
                new("@g", (object?)kh.GhiChu  ?? DBNull.Value)]);
            return System.Convert.ToInt32(r);
        }

        public bool Update(KhachHang kh)
        {
            var sql = @"UPDATE KhachHang SET HoTen=@h, Email=@e, DiaChi=@d,
                        NgaySinh=@n, GhiChu=@g, NgayCapNhat=GETDATE()
                        WHERE MaKhachHang=@id";
            return DatabaseHelper.ExecuteNonQuery(sql, [
                new("@h", kh.HoTen), new("@e", (object?)kh.Email ?? DBNull.Value),
                new("@d", (object?)kh.DiaChi ?? DBNull.Value),
                new("@n", (object?)kh.NgaySinh ?? DBNull.Value),
                new("@g", (object?)kh.GhiChu  ?? DBNull.Value),
                new("@id", kh.MaKhachHang)]) > 0;
        }

        public bool Delete(int id)
            => DatabaseHelper.ExecuteNonQuery(
                "DELETE FROM KhachHang WHERE MaKhachHang=@id", [new("@id", id)]) > 0;

        public void CapNhatTichLuy(int maKH, decimal soTienMua, int diemTich)
        {
            DatabaseHelper.ExecuteNonQuery(
                @"UPDATE KhachHang SET 
                  TongChiTieu    = TongChiTieu + @tien,
                  TongDiemTichLuy = TongDiemTichLuy + @diem,
                  NgayCapNhat    = GETDATE()
                  WHERE MaKhachHang = @id;
                  EXEC sp_CapNhatHangKhachHang @id;",
                [new("@tien", soTienMua), new("@diem", diemTich), new("@id", maKH)]);
        }

        public void TruDiem(int maKH, int soTiem)
        {
            DatabaseHelper.ExecuteNonQuery(
                "UPDATE KhachHang SET TongDiemTichLuy = TongDiemTichLuy - @d, NgayCapNhat=GETDATE() WHERE MaKhachHang=@id",
                [new("@d", soTiem), new("@id", maKH)]);
        }
    }
}
