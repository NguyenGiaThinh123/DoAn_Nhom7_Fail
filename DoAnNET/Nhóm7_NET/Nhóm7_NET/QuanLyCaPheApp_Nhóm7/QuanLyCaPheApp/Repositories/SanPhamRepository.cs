using Microsoft.Data.SqlClient;
using QuanLyCaPheApp.Helpers;
using QuanLyCaPheApp.Models;

namespace QuanLyCaPheApp.Repositories
{
    public class SanPhamRepository : BaseRepository
    {
        private SanPham MapSP(SqlDataReader r) => new()
        {
            MaSanPham    = I(r, "MaSanPham"),
            TenSanPham   = S(r, "TenSanPham"),
            MaLoai       = I(r, "MaLoai"),
            GiaBan       = D(r, "GiaBan"),
            GiaGoc       = DN(r, "GiaGoc"),
            PhanTramGiam = D(r, "PhanTramGiam"),
            MoTa         = S(r, "MoTa"),
            DonVi        = S(r, "DonVi"),
            TichDiemNhan = I(r, "TichDiemNhan"),
            HinhAnh      = S(r, "HinhAnh"),
            TrangThai    = B(r, "TrangThai"),
            NgayTao      = DT(r, "NgayTao"),
            NgayCapNhat  = DTN(r, "NgayCapNhat"),
            LoaiSanPham  = new LoaiSanPham { MaLoai = I(r,"MaLoai"), TenLoai = S(r,"TenLoai") }
        };

        private LoaiSanPham MapLoai(SqlDataReader r) => new()
        {
            MaLoai       = I(r, "MaLoai"),
            TenLoai      = S(r, "TenLoai"),
            MoTa         = S(r, "MoTa"),
            ThuTuHienThi = I(r, "ThuTuHienThi"),
            TrangThai    = B(r, "TrangThai"),
            NgayTao      = DT(r, "NgayTao"),
        };

        public List<SanPham> GetAll(string keyword = "", int maLoai = 0, bool? trangThai = null)
        {
            var where = " WHERE 1=1";
            var pList = new List<SqlParameter>();
            if (!string.IsNullOrWhiteSpace(keyword))
            { where += " AND sp.TenSanPham LIKE @kw"; pList.Add(new("@kw", $"%{keyword}%")); }
            if (maLoai > 0)
            { where += " AND sp.MaLoai=@l"; pList.Add(new("@l", maLoai)); }
            if (trangThai.HasValue)
            { where += " AND sp.TrangThai=@t"; pList.Add(new("@t", trangThai.Value)); }

            return DatabaseHelper.ExecuteQuery(
                $"SELECT sp.*,lsp.TenLoai FROM SanPham sp JOIN LoaiSanPham lsp ON sp.MaLoai=lsp.MaLoai{where} ORDER BY lsp.ThuTuHienThi, sp.TenSanPham",
                MapSP, pList.Count > 0 ? pList.ToArray() : null);
        }

        public SanPham? GetById(int id)
            => DatabaseHelper.ExecuteQuerySingle(
                "SELECT sp.*,lsp.TenLoai FROM SanPham sp JOIN LoaiSanPham lsp ON sp.MaLoai=lsp.MaLoai WHERE sp.MaSanPham=@id",
                MapSP, [new("@id", id)]);

        public int Add(SanPham sp)
        {
            var sql = @"INSERT INTO SanPham (TenSanPham,MaLoai,GiaBan,GiaGoc,PhanTramGiam,MoTa,DonVi,TichDiemNhan,TrangThai)
                        VALUES (@n,@l,@g,@gg,@ptg,@m,@dv,@td,@t); SELECT SCOPE_IDENTITY();";
            var r = DatabaseHelper.ExecuteScalar(sql, [
                new("@n", sp.TenSanPham), new("@l", sp.MaLoai),
                new("@g", sp.GiaBan),     new("@gg", (object?)sp.GiaGoc ?? DBNull.Value),
                new("@ptg", sp.PhanTramGiam), new("@m", (object?)sp.MoTa ?? DBNull.Value),
                new("@dv", sp.DonVi),    new("@td", sp.TichDiemNhan),
                new("@t", sp.TrangThai)]);
            return System.Convert.ToInt32(r);
        }

        public bool Update(SanPham sp)
            => DatabaseHelper.ExecuteNonQuery(
                @"UPDATE SanPham SET TenSanPham=@n,MaLoai=@l,GiaBan=@g,GiaGoc=@gg,
                  PhanTramGiam=@ptg,MoTa=@m,DonVi=@dv,TichDiemNhan=@td,
                  TrangThai=@t,NgayCapNhat=GETDATE() WHERE MaSanPham=@id",
                [new("@n", sp.TenSanPham), new("@l", sp.MaLoai),
                 new("@g", sp.GiaBan),     new("@gg", (object?)sp.GiaGoc ?? DBNull.Value),
                 new("@ptg", sp.PhanTramGiam), new("@m", (object?)sp.MoTa ?? DBNull.Value),
                 new("@dv", sp.DonVi),    new("@td", sp.TichDiemNhan),
                 new("@t", sp.TrangThai), new("@id", sp.MaSanPham)]) > 0;

        public bool Delete(int id)
            => DatabaseHelper.ExecuteNonQuery(
                "UPDATE SanPham SET TrangThai=0 WHERE MaSanPham=@id", [new("@id", id)]) > 0;

        // LoaiSanPham CRUD
        public List<LoaiSanPham> GetAllLoai(bool? trangThai = null)
        {
            var sql = "SELECT * FROM LoaiSanPham";
            if (trangThai.HasValue) sql += $" WHERE TrangThai={(trangThai.Value ? 1 : 0)}";
            sql += " ORDER BY ThuTuHienThi, TenLoai";
            return DatabaseHelper.ExecuteQuery(sql, MapLoai);
        }

        public int AddLoai(LoaiSanPham loai)
        {
            var r = DatabaseHelper.ExecuteScalar(
                "INSERT INTO LoaiSanPham(TenLoai,MoTa,ThuTuHienThi,TrangThai) VALUES(@t,@m,@tt,1); SELECT SCOPE_IDENTITY();",
                [new("@t", loai.TenLoai), new("@m", (object?)loai.MoTa ?? DBNull.Value),
                 new("@tt", loai.ThuTuHienThi)]);
            return System.Convert.ToInt32(r);
        }

        public bool UpdateLoai(LoaiSanPham loai)
            => DatabaseHelper.ExecuteNonQuery(
                "UPDATE LoaiSanPham SET TenLoai=@t,MoTa=@m,ThuTuHienThi=@tt,TrangThai=@ts WHERE MaLoai=@id",
                [new("@t", loai.TenLoai), new("@m", (object?)loai.MoTa ?? DBNull.Value),
                 new("@tt", loai.ThuTuHienThi), new("@ts", loai.TrangThai),
                 new("@id", loai.MaLoai)]) > 0;
    }
}
