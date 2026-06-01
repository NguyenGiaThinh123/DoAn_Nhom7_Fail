
USE master
GO
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'QuanLyQuanCaPhe')
    DROP DATABASE QuanLyQuanCaPhe
GO
CREATE DATABASE QuanLyQuanCaPhe
GO
USE QuanLyQuanCaPhe
GO

-- ============================================================
-- BẢNG 1: VaiTro
-- ============================================================
CREATE TABLE VaiTro (
    MaVaiTro    INT PRIMARY KEY IDENTITY(1,1),
    TenVaiTro   NVARCHAR(50)  NOT NULL UNIQUE,
    MoTa        NVARCHAR(200) NULL,
    QuyenQuanLyNguoiDung  BIT NOT NULL DEFAULT 0,
    QuyenQuanLySanPham    BIT NOT NULL DEFAULT 0,
    QuyenDatBan           BIT NOT NULL DEFAULT 0,
    QuyenGoiMon           BIT NOT NULL DEFAULT 0,
    QuyenThanhToan        BIT NOT NULL DEFAULT 0,
    QuyenQuanLyKhachHang  BIT NOT NULL DEFAULT 0,
    QuyenThongKe          BIT NOT NULL DEFAULT 0,
    QuyenXuatBaoCao       BIT NOT NULL DEFAULT 0,
    NgayTao DATETIME NOT NULL DEFAULT GETDATE()
)
GO

-- ============================================================
-- BẢNG 2: NguoiDung
-- ============================================================
CREATE TABLE NguoiDung (
    MaNguoiDung  INT PRIMARY KEY IDENTITY(1,1),
    TenDangNhap  NVARCHAR(50)  NOT NULL UNIQUE,
    MatKhauHash  NVARCHAR(256) NOT NULL,
    HoTen        NVARCHAR(100) NOT NULL,
    Email        NVARCHAR(150) NULL,
    SoDienThoai  NVARCHAR(20)  NULL,
    MaVaiTro     INT           NOT NULL,
    TrangThai    BIT           NOT NULL DEFAULT 1,
    NgayTao      DATETIME      NOT NULL DEFAULT GETDATE(),
    NgayCapNhat  DATETIME      NULL,
    FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro)
)
GO

-- ============================================================
-- BẢNG 3: KhachHang
-- ============================================================
CREATE TABLE KhachHang (
    MaKhachHang    INT PRIMARY KEY IDENTITY(1,1),
    HoTen          NVARCHAR(100)  NOT NULL,
    SoDienThoai    NVARCHAR(20)   NOT NULL UNIQUE,
    Email          NVARCHAR(150)  NULL,
    DiaChi         NVARCHAR(300)  NULL,
    NgaySinh       DATE           NULL,
    TongDiemTichLuy INT          NOT NULL DEFAULT 0,
    TongChiTieu    DECIMAL(18,0)  NOT NULL DEFAULT 0,
    HangKhachHang  NVARCHAR(30)   NOT NULL DEFAULT N'Thường',
    GhiChu         NVARCHAR(500)  NULL,
    NgayDangKy     DATETIME       NOT NULL DEFAULT GETDATE(),
    NgayCapNhat    DATETIME       NULL
)
GO

-- ============================================================
-- BẢNG 4: KhuVuc
-- ============================================================
CREATE TABLE KhuVuc (
    MaKhuVuc  INT PRIMARY KEY IDENTITY(1,1),
    TenKhuVuc NVARCHAR(100) NOT NULL,
    MoTa      NVARCHAR(300) NULL,
    TrangThai BIT           NOT NULL DEFAULT 1
)
GO

-- ============================================================
-- BẢNG 5: Ban
-- ============================================================
CREATE TABLE Ban (
    MaBan       INT PRIMARY KEY IDENTITY(1,1),
    TenBan      NVARCHAR(50)  NOT NULL,
    MaKhuVuc    INT           NOT NULL,
    SoGhe       INT           NOT NULL DEFAULT 4,
    ViTriHang   INT           NOT NULL DEFAULT 1,
    ViTriCot    INT           NOT NULL DEFAULT 1,
    TrangThai   NVARCHAR(20)  NOT NULL DEFAULT N'Trống',
    GhiChu      NVARCHAR(300) NULL,
    NgayCapNhat DATETIME      NULL,
    FOREIGN KEY (MaKhuVuc) REFERENCES KhuVuc(MaKhuVuc)
)
GO

-- ============================================================
-- BẢNG 6: LoaiSanPham
-- ============================================================
CREATE TABLE LoaiSanPham (
    MaLoai       INT PRIMARY KEY IDENTITY(1,1),
    TenLoai      NVARCHAR(100) NOT NULL UNIQUE,
    MoTa         NVARCHAR(300) NULL,
    ThuTuHienThi INT           NOT NULL DEFAULT 0,
    TrangThai    BIT           NOT NULL DEFAULT 1,
    NgayTao      DATETIME      NOT NULL DEFAULT GETDATE()
)
GO

-- ============================================================
-- BẢNG 7: SanPham
-- ============================================================
CREATE TABLE SanPham (
    MaSanPham    INT PRIMARY KEY IDENTITY(1,1),
    TenSanPham   NVARCHAR(150)  NOT NULL,
    MaLoai       INT            NOT NULL,
    GiaBan       DECIMAL(18,0)  NOT NULL,
    GiaGoc       DECIMAL(18,0)  NULL,
    PhanTramGiam DECIMAL(5,2)   NOT NULL DEFAULT 0,
    MoTa         NVARCHAR(500)  NULL,
    DonVi        NVARCHAR(30)   NOT NULL DEFAULT N'Ly',
    TichDiemNhan INT            NOT NULL DEFAULT 1,
    HinhAnh      NVARCHAR(300)  NULL,
    TrangThai    BIT            NOT NULL DEFAULT 1,
    NgayTao      DATETIME       NOT NULL DEFAULT GETDATE(),
    NgayCapNhat  DATETIME       NULL,
    FOREIGN KEY (MaLoai) REFERENCES LoaiSanPham(MaLoai)
)
GO

-- ============================================================
-- BẢNG 8: KhuyenMai
-- ============================================================
CREATE TABLE KhuyenMai (
    MaKhuyenMai       INT PRIMARY KEY IDENTITY(1,1),
    TenKhuyenMai      NVARCHAR(200)  NOT NULL,
    MaCode            NVARCHAR(50)   NULL UNIQUE,
    LoaiGiam          NVARCHAR(20)   NOT NULL DEFAULT N'PhanTram',
    GiaTriGiam        DECIMAL(18,2)  NOT NULL,
    GiaTriDonToiThieu DECIMAL(18,0)  NOT NULL DEFAULT 0,
    GiamToiDa         DECIMAL(18,0)  NULL,
    NgayBatDau        DATE           NOT NULL,
    NgayKetThuc       DATE           NOT NULL,
    SoLanSuDung       INT            NULL,
    DaSuDung          INT            NOT NULL DEFAULT 0,
    TrangThai         BIT            NOT NULL DEFAULT 1,
    NgayTao           DATETIME       NOT NULL DEFAULT GETDATE()
)
GO

-- ============================================================
-- BẢNG 9: HoaDon
-- ============================================================
CREATE TABLE HoaDon (
    MaHoaDon       INT PRIMARY KEY IDENTITY(1,1),
    MaBan          INT           NOT NULL,
    MaKhachHang    INT           NULL,
    MaNguoiTao     INT           NOT NULL,
    MaNguoiCapNhat INT           NULL,
    MaKhuyenMai    INT           NULL,
    LoaiHoaDon     NVARCHAR(20)  NOT NULL DEFAULT N'TraSau',
    TrangThai      NVARCHAR(30)  NOT NULL DEFAULT N'DangGoi',
    TongTamTinh    DECIMAL(18,0) NOT NULL DEFAULT 0,
    SoTienGiam     DECIMAL(18,0) NOT NULL DEFAULT 0,
    DiemDung       INT           NOT NULL DEFAULT 0,
    TongThanhToan  DECIMAL(18,0) NOT NULL DEFAULT 0,
    TienKhachDua   DECIMAL(18,0) NULL,
    TienThua       DECIMAL(18,0) NULL,
    PhuongThucTT   NVARCHAR(50)  NOT NULL DEFAULT N'TienMat',
    GhiChu         NVARCHAR(500) NULL,
    NgayTao        DATETIME      NOT NULL DEFAULT GETDATE(),
    NgayThanhToan  DATETIME      NULL,
    FOREIGN KEY (MaBan)       REFERENCES Ban(MaBan),
    FOREIGN KEY (MaKhachHang) REFERENCES KhachHang(MaKhachHang),
    FOREIGN KEY (MaNguoiTao)  REFERENCES NguoiDung(MaNguoiDung),
    FOREIGN KEY (MaKhuyenMai) REFERENCES KhuyenMai(MaKhuyenMai)
)
GO

-- ============================================================
-- BẢNG 10: ChiTietHoaDon
-- ============================================================
CREATE TABLE ChiTietHoaDon (
    MaChiTiet    INT PRIMARY KEY IDENTITY(1,1),
    MaHoaDon     INT           NOT NULL,
    MaSanPham    INT           NOT NULL,
    TenSanPham   NVARCHAR(150) NOT NULL,
    DonGia       DECIMAL(18,0) NOT NULL,
    SoLuong      INT           NOT NULL DEFAULT 1,
    PhanTramGiam DECIMAL(5,2)  NOT NULL DEFAULT 0,
    ThanhTien    DECIMAL(18,0) NOT NULL,
    TrangThaiMon NVARCHAR(30)  NOT NULL DEFAULT N'ChoPha',
    GhiChuMon    NVARCHAR(300) NULL,
    NgayTao      DATETIME      NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (MaHoaDon)  REFERENCES HoaDon(MaHoaDon),
    FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSanPham)
)
GO

-- ============================================================
-- BẢNG 11: LichSuDatBan
-- ============================================================
CREATE TABLE LichSuDatBan (
    MaDatBan    INT PRIMARY KEY IDENTITY(1,1),
    MaBan       INT           NOT NULL,
    MaKhachHang INT           NULL,
    TenKhachDat NVARCHAR(100) NOT NULL,
    SoDienThoai NVARCHAR(20)  NOT NULL,
    SoNguoi     INT           NOT NULL DEFAULT 1,
    NgayDat     DATETIME      NOT NULL,
    GioNhanBan  DATETIME      NULL,
    GioRoiBan   DATETIME      NULL,
    MaHoaDon    INT           NULL,
    TrangThai   NVARCHAR(30)  NOT NULL DEFAULT N'DatTruoc',
    GhiChu      NVARCHAR(500) NULL,
    MaNguoiTao  INT           NOT NULL,
    NgayTao     DATETIME      NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (MaBan)       REFERENCES Ban(MaBan),
    FOREIGN KEY (MaKhachHang) REFERENCES KhachHang(MaKhachHang),
    FOREIGN KEY (MaNguoiTao)  REFERENCES NguoiDung(MaNguoiDung)
)
GO

-- ============================================================
-- BẢNG 12: TichDiem
-- ============================================================
CREATE TABLE TichDiem (
    MaTichDiem      INT PRIMARY KEY IDENTITY(1,1),
    MaKhachHang     INT          NOT NULL,
    MaHoaDon        INT          NULL,
    LoaiGiaoDich    NVARCHAR(30) NOT NULL,
    SoDiem          INT          NOT NULL,
    SoDiemSauGD     INT          NOT NULL DEFAULT 0,
    MoTa            NVARCHAR(300) NULL,
    NgayGiaoDich    DATETIME     NOT NULL DEFAULT GETDATE(),
    MaNguoiThucHien INT          NULL,
    FOREIGN KEY (MaKhachHang)     REFERENCES KhachHang(MaKhachHang),
    FOREIGN KEY (MaHoaDon)        REFERENCES HoaDon(MaHoaDon),
    FOREIGN KEY (MaNguoiThucHien) REFERENCES NguoiDung(MaNguoiDung)
)
GO

-- ============================================================
-- INDEX tối ưu truy vấn
-- ============================================================
CREATE INDEX IX_HoaDon_NgayTao          ON HoaDon(NgayTao)
CREATE INDEX IX_HoaDon_TrangThai        ON HoaDon(TrangThai)
CREATE INDEX IX_ChiTietHoaDon_MaHoaDon ON ChiTietHoaDon(MaHoaDon)
CREATE INDEX IX_LichSuDatBan_MaBan     ON LichSuDatBan(MaBan)
CREATE INDEX IX_TichDiem_MaKhachHang   ON TichDiem(MaKhachHang)
CREATE INDEX IX_SanPham_MaLoai         ON SanPham(MaLoai)
GO

-- ============================================================
-- STORED PROCEDURE: Cập nhật hạng khách hàng
-- ============================================================
CREATE OR ALTER PROCEDURE sp_CapNhatHangKhachHang @MaKhachHang INT
AS
BEGIN
    DECLARE @TongChiTieu DECIMAL(18,0);
    SELECT @TongChiTieu = TongChiTieu FROM KhachHang WHERE MaKhachHang = @MaKhachHang;
    DECLARE @Hang NVARCHAR(30);
    SET @Hang = CASE
        WHEN @TongChiTieu >= 20000000 THEN N'Kim cương'
        WHEN @TongChiTieu >= 10000000 THEN N'Vàng'
        WHEN @TongChiTieu >= 3000000  THEN N'Bạc'
        ELSE N'Thường'
    END;
    UPDATE KhachHang SET HangKhachHang = @Hang, NgayCapNhat = GETDATE()
    WHERE MaKhachHang = @MaKhachHang;
END
GO

-- ============================================================
-- STORED PROCEDURE: Thống kê doanh thu
-- ============================================================
CREATE OR ALTER PROCEDURE sp_ThongKeDoanhThu @TuNgay DATE, @DenNgay DATE
AS
BEGIN
    SELECT
        CAST(NgayThanhToan AS DATE) AS [Ngay],
        COUNT(*)                    AS [SoHoaDon],
        SUM(TongTamTinh)            AS [DoanhThuGoc],
        SUM(SoTienGiam)             AS [TongGiamGia],
        SUM(TongThanhToan)          AS [ThucThu],
        COUNT(DISTINCT MaKhachHang) AS [SoKhachHang]
    FROM HoaDon
    WHERE TrangThai = N'DaThanhToan'
      AND CAST(NgayThanhToan AS DATE) BETWEEN @TuNgay AND @DenNgay
    GROUP BY CAST(NgayThanhToan AS DATE)
    ORDER BY CAST(NgayThanhToan AS DATE);
END
GO

PRINT N'=== Tạo CSDL QuanLyQuanCaPhe thành công! ===';
GO
