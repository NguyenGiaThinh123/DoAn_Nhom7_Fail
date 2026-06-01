using Microsoft.Data.SqlClient;
using QuanLyCaPheApp.Helpers;
using QuanLyCaPheApp.Models;

namespace QuanLyCaPheApp.Repositories
{
    public class HoaDonRepository : BaseRepository
    {
        private HoaDon MapHD(SqlDataReader r) => new()
        {
            MaHoaDon      = I(r, "MaHoaDon"),
            MaBan         = I(r, "MaBan"),
            MaKhachHang   = IN(r, "MaKhachHang"),
            MaNguoiTao    = I(r, "MaNguoiTao"),
            MaKhuyenMai   = IN(r, "MaKhuyenMai"),
            LoaiHoaDon    = S(r, "LoaiHoaDon"),
            TrangThai     = S(r, "TrangThai"),
            TongTamTinh   = D(r, "TongTamTinh"),
            SoTienGiam    = D(r, "SoTienGiam"),
            DiemDung      = I(r, "DiemDung"),
            TongThanhToan = D(r, "TongThanhToan"),
            TienKhachDua  = DN(r, "TienKhachDua"),
            TienThua      = DN(r, "TienThua"),
            PhuongThucTT  = S(r, "PhuongThucTT"),
            GhiChu        = S(r, "GhiChu"),
            NgayTao       = DT(r, "NgayTao"),
            NgayThanhToan = DTN(r, "NgayThanhToan"),
            TenBan        = S(r, "TenBan"),
            TenKhachHang  = S(r, "TenKhachHang"),
            TenNguoiTao   = S(r, "TenNguoiTao"),
        };

        private ChiTietHoaDon MapCT(SqlDataReader r) => new()
        {
            MaChiTiet    = I(r, "MaChiTiet"),
            MaHoaDon     = I(r, "MaHoaDon"),
            MaSanPham    = I(r, "MaSanPham"),
            TenSanPham   = S(r, "TenSanPham"),
            DonGia       = D(r, "DonGia"),
            SoLuong      = I(r, "SoLuong"),
            PhanTramGiam = D(r, "PhanTramGiam"),
            ThanhTien    = D(r, "ThanhTien"),
            TrangThaiMon = S(r, "TrangThaiMon"),
            GhiChuMon    = S(r, "GhiChuMon"),
            NgayTao      = DT(r, "NgayTao"),
        };

        private const string HdSelectSql = @"
            SELECT hd.*,
                b.TenBan,
                ISNULL(kh.HoTen,'') AS TenKhachHang,
                nd.HoTen            AS TenNguoiTao
            FROM HoaDon hd
            JOIN Ban b ON hd.MaBan = b.MaBan
            JOIN NguoiDung nd ON hd.MaNguoiTao = nd.MaNguoiDung
            LEFT JOIN KhachHang kh ON hd.MaKhachHang = kh.MaKhachHang";

        public List<HoaDon> GetAll(string? trangThai = null)
        {
            var sql = HdSelectSql;
            if (!string.IsNullOrEmpty(trangThai)) sql += " WHERE hd.TrangThai=@t";
            sql += " ORDER BY hd.NgayTao DESC";
            var p = string.IsNullOrEmpty(trangThai) ? null : new SqlParameter[] { new("@t", trangThai) };
            return DatabaseHelper.ExecuteQuery(sql, MapHD, p);
        }

        public HoaDon? GetById(int id)
        {
            var hd = DatabaseHelper.ExecuteQuerySingle(HdSelectSql + " WHERE hd.MaHoaDon=@id", MapHD, [new("@id", id)]);
            if (hd != null) hd.ChiTiet = GetChiTiet(id);
            return hd;
        }

        public HoaDon? GetDangGoiByBan(int maBan)
        {
            var hd = DatabaseHelper.ExecuteQuerySingle(
                HdSelectSql + " WHERE hd.MaBan=@b AND hd.TrangThai='DangGoi'", MapHD, [new("@b", maBan)]);
            if (hd != null) hd.ChiTiet = GetChiTiet(hd.MaHoaDon);
            return hd;
        }

        public List<ChiTietHoaDon> GetChiTiet(int maHoaDon)
            => DatabaseHelper.ExecuteQuery(
                "SELECT * FROM ChiTietHoaDon WHERE MaHoaDon=@id ORDER BY NgayTao",
                MapCT, [new("@id", maHoaDon)]);

        public int TaoHoaDon(HoaDon hd)
        {
            var sql = @"INSERT INTO HoaDon (MaBan,MaKhachHang,MaNguoiTao,MaKhuyenMai,LoaiHoaDon,
                        TrangThai,TongTamTinh,SoTienGiam,DiemDung,TongThanhToan,
                        TienKhachDua,TienThua,PhuongThucTT,GhiChu,NgayThanhToan)
                        VALUES (@b,@kh,@nd,@km,@loai,@tt,@ttt,@stg,@dd,@tht,@tkd,@t,@pttt,@g,@ntt);
                        SELECT SCOPE_IDENTITY();";
            var r = DatabaseHelper.ExecuteScalar(sql, [
                new("@b",    hd.MaBan),
                new("@kh",   (object?)hd.MaKhachHang   ?? DBNull.Value),
                new("@nd",   hd.MaNguoiTao),
                new("@km",   (object?)hd.MaKhuyenMai   ?? DBNull.Value),
                new("@loai", hd.LoaiHoaDon),
                new("@tt",   hd.TrangThai),
                new("@ttt",  hd.TongTamTinh),
                new("@stg",  hd.SoTienGiam),
                new("@dd",   hd.DiemDung),
                new("@tht",  hd.TongThanhToan),
                new("@tkd",  (object?)hd.TienKhachDua  ?? DBNull.Value),
                new("@t",    (object?)hd.TienThua       ?? DBNull.Value),
                new("@pttt", hd.PhuongThucTT),
                new("@g",    (object?)hd.GhiChu         ?? DBNull.Value),
                new("@ntt",  (object?)hd.NgayThanhToan  ?? DBNull.Value)]);
            return System.Convert.ToInt32(r);
        }

        public int ThemChiTiet(ChiTietHoaDon ct)
        {
            var r = DatabaseHelper.ExecuteScalar(@"
                INSERT INTO ChiTietHoaDon (MaHoaDon,MaSanPham,TenSanPham,DonGia,SoLuong,PhanTramGiam,ThanhTien,TrangThaiMon,GhiChuMon)
                VALUES (@hd,@sp,@t,@dg,@sl,@ptg,@tt,@ts,@g); SELECT SCOPE_IDENTITY();",
                [new("@hd",  ct.MaHoaDon),
                 new("@sp",  ct.MaSanPham),
                 new("@t",   ct.TenSanPham),
                 new("@dg",  ct.DonGia),
                 new("@sl",  ct.SoLuong),
                 new("@ptg", ct.PhanTramGiam),
                 new("@tt",  ct.ThanhTien),
                 new("@ts",  ct.TrangThaiMon),
                 new("@g",   (object?)ct.GhiChuMon ?? DBNull.Value)]);
            return System.Convert.ToInt32(r);
        }

        public bool ThanhToan(int maHoaDon, decimal tongTT, decimal soTienGiam,
            decimal tienKhDua, decimal tienThua, string phuongThuc, int diemDung = 0)
            => DatabaseHelper.ExecuteNonQuery(@"
                UPDATE HoaDon SET TrangThai='DaThanhToan',
                    TongThanhToan=@tt, SoTienGiam=@stg, DiemDung=@dd,
                    TienKhachDua=@tkd, TienThua=@t, PhuongThucTT=@p,
                    NgayThanhToan=GETDATE()
                WHERE MaHoaDon=@id",
                [new("@tt",  tongTT), new("@stg", soTienGiam), new("@dd", diemDung),
                 new("@tkd", tienKhDua), new("@t", tienThua),
                 new("@p",   phuongThuc), new("@id", maHoaDon)]) > 0;

        public bool HuyHoaDon(int maHoaDon)
            => DatabaseHelper.ExecuteNonQuery(
                "UPDATE HoaDon SET TrangThai='HuyBo' WHERE MaHoaDon=@id", [new("@id", maHoaDon)]) > 0;

        public bool CapNhatTongTien(int maHoaDon, decimal tongTamTinh, decimal tongThanhToan)
            => DatabaseHelper.ExecuteNonQuery(
                "UPDATE HoaDon SET TongTamTinh=@ttt, TongThanhToan=@tht WHERE MaHoaDon=@id",
                [new("@ttt", tongTamTinh), new("@tht", tongThanhToan), new("@id", maHoaDon)]) > 0;

        public bool XoaChiTiet(int maChiTiet)
            => DatabaseHelper.ExecuteNonQuery(
                "DELETE FROM ChiTietHoaDon WHERE MaChiTiet=@id", [new("@id", maChiTiet)]) > 0;

        public bool CapNhatSoLuong(int maChiTiet, int soLuong, decimal thanhTien)
            => DatabaseHelper.ExecuteNonQuery(
                "UPDATE ChiTietHoaDon SET SoLuong=@sl, ThanhTien=@tt WHERE MaChiTiet=@id",
                [new("@sl", soLuong), new("@tt", thanhTien), new("@id", maChiTiet)]) > 0;

        public bool CapNhatTrangThaiMon(int maChiTiet, string trangThai)
            => DatabaseHelper.ExecuteNonQuery(
                "UPDATE ChiTietHoaDon SET TrangThaiMon=@t WHERE MaChiTiet=@id",
                [new("@t", trangThai), new("@id", maChiTiet)]) > 0;

        public List<KhuyenMai> GetKhuyenMaiConHieuLuc()
            => DatabaseHelper.ExecuteQuery(
                @"SELECT * FROM KhuyenMai WHERE TrangThai=1 
                  AND NgayBatDau <= CAST(GETDATE() AS DATE)
                  AND NgayKetThuc >= CAST(GETDATE() AS DATE)
                  AND (SoLanSuDung IS NULL OR DaSuDung < SoLanSuDung)
                  ORDER BY TenKhuyenMai",
                r => new KhuyenMai
                {
                    MaKhuyenMai       = I(r, "MaKhuyenMai"),
                    TenKhuyenMai      = S(r, "TenKhuyenMai"),
                    MaCode            = S(r, "MaCode"),
                    LoaiGiam          = S(r, "LoaiGiam"),
                    GiaTriGiam        = D(r, "GiaTriGiam"),
                    GiaTriDonToiThieu = D(r, "GiaTriDonToiThieu"),
                    GiamToiDa         = DN(r, "GiamToiDa"),
                    NgayBatDau        = DT(r, "NgayBatDau"),
                    NgayKetThuc       = DT(r, "NgayKetThuc"),
                    SoLanSuDung       = IN(r, "SoLanSuDung"),
                    DaSuDung          = I(r, "DaSuDung"),
                    TrangThai         = B(r, "TrangThai"),
                });

        public List<LichSuDatBan> GetLichSuDatBan(int? maBan = null)
        {
            var sql = @"SELECT lsdb.*, b.TenBan FROM LichSuDatBan lsdb
                        JOIN Ban b ON lsdb.MaBan = b.MaBan";
            if (maBan.HasValue) sql += " WHERE lsdb.MaBan=@mb";
            sql += " ORDER BY lsdb.NgayDat DESC";
            var p = maBan.HasValue ? new SqlParameter[] { new("@mb", maBan.Value) } : null;
            return DatabaseHelper.ExecuteQuery(sql, r => new LichSuDatBan
            {
                MaDatBan    = I(r, "MaDatBan"),
                MaBan       = I(r, "MaBan"),
                MaKhachHang = IN(r,"MaKhachHang"),
                TenKhachDat = S(r, "TenKhachDat"),
                SoDienThoai = S(r, "SoDienThoai"),
                SoNguoi     = I(r, "SoNguoi"),
                NgayDat     = DT(r, "NgayDat"),
                GioNhanBan  = DTN(r,"GioNhanBan"),
                GioRoiBan   = DTN(r,"GioRoiBan"),
                MaHoaDon    = IN(r, "MaHoaDon"),
                TrangThai   = S(r, "TrangThai"),
                GhiChu      = S(r, "GhiChu"),
                MaNguoiTao  = I(r, "MaNguoiTao"),
                NgayTao     = DT(r, "NgayTao"),
                TenBan      = S(r, "TenBan"),
            }, p);
        }

        public int TaoDatBan(LichSuDatBan datBan)
        {
            var r = DatabaseHelper.ExecuteScalar(@"
                INSERT INTO LichSuDatBan (MaBan,MaKhachHang,TenKhachDat,SoDienThoai,SoNguoi,NgayDat,GioNhanBan,TrangThai,GhiChu,MaNguoiTao)
                VALUES (@b,@kh,@t,@sdt,@sn,@nd,@gn,@ts,@g,@nd2); SELECT SCOPE_IDENTITY();",
                [new("@b",   datBan.MaBan),
                 new("@kh",  (object?)datBan.MaKhachHang ?? DBNull.Value),
                 new("@t",   datBan.TenKhachDat),
                 new("@sdt", datBan.SoDienThoai),
                 new("@sn",  datBan.SoNguoi),
                 new("@nd",  datBan.NgayDat),
                 new("@gn",  (object?)datBan.GioNhanBan ?? DBNull.Value),
                 new("@ts",  datBan.TrangThai),
                 new("@g",   (object?)datBan.GhiChu ?? DBNull.Value),
                 new("@nd2", datBan.MaNguoiTao)]);
            return System.Convert.ToInt32(r);
        }
    }
}
