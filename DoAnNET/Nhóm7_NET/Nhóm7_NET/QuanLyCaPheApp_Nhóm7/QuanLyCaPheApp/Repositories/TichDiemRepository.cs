using QuanLyCaPheApp.Helpers;
using QuanLyCaPheApp.Models;

namespace QuanLyCaPheApp.Repositories
{
    public class TichDiemRepository : BaseRepository
    {
        public List<TichDiem> GetByKhachHang(int maKH)
            => DatabaseHelper.ExecuteQuery(
                @"SELECT td.*, kh.HoTen AS TenKhachHang
                  FROM TichDiem td JOIN KhachHang kh ON td.MaKhachHang=kh.MaKhachHang
                  WHERE td.MaKhachHang=@id ORDER BY td.NgayGiaoDich DESC",
                r => new TichDiem
                {
                    MaTichDiem      = I(r, "MaTichDiem"),
                    MaKhachHang     = I(r, "MaKhachHang"),
                    MaHoaDon        = IN(r,"MaHoaDon"),
                    LoaiGiaoDich    = S(r, "LoaiGiaoDich"),
                    SoDiem          = I(r, "SoDiem"),
                    SoDiemSauGD     = I(r, "SoDiemSauGD"),
                    MoTa            = S(r, "MoTa"),
                    NgayGiaoDich    = DT(r,"NgayGiaoDich"),
                    MaNguoiThucHien = IN(r,"MaNguoiThucHien"),
                    TenKhachHang    = S(r, "TenKhachHang"),
                }, [new("@id", maKH)]);

        public void GhiTichDiem(int maKH, int? maHD, int soDiem, int soSau, string moTa, int? maNguoiTH)
            => DatabaseHelper.ExecuteNonQuery(
                @"INSERT INTO TichDiem (MaKhachHang,MaHoaDon,LoaiGiaoDich,SoDiem,SoDiemSauGD,MoTa,MaNguoiThucHien)
                  VALUES (@kh,@hd,'TichDiem',@sd,@ss,@m,@ntl)",
                [new("@kh",  maKH),
                 new("@hd",  (object?)maHD ?? System.DBNull.Value),
                 new("@sd",  soDiem),    new("@ss",  soSau),
                 new("@m",   moTa),
                 new("@ntl", (object?)maNguoiTH ?? System.DBNull.Value)]);

        public void GhiDoiDiem(int maKH, int? maHD, int soDiemDoi, int soSau, int? maNguoiTH)
            => DatabaseHelper.ExecuteNonQuery(
                @"INSERT INTO TichDiem (MaKhachHang,MaHoaDon,LoaiGiaoDich,SoDiem,SoDiemSauGD,MoTa,MaNguoiThucHien)
                  VALUES (@kh,@hd,'DoiDiem',@sd,@ss,N'Doi diem lay giam gia',@ntl)",
                [new("@kh",  maKH),
                 new("@hd",  (object?)maHD ?? System.DBNull.Value),
                 new("@sd",  -soDiemDoi),  new("@ss", soSau),
                 new("@ntl", (object?)maNguoiTH ?? System.DBNull.Value)]);
    }
}
