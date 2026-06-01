-- ============================================================

--         HỆ THỐNG QUẢN LÍ QUÁN CÀ PHÊ

-- ============================================================

create database QuanLyQuanCaPhe

USE QuanLyQuanCaPhe
GO


-- ============================================================
-- BẢNG 1: VaiTro - Phân quyền hệ thống
-- ============================================================
CREATE TABLE VaiTro (
    MaVaiTro    INT             PRIMARY KEY IDENTITY(1,1),
    TenVaiTro   NVARCHAR(50)    NOT NULL UNIQUE,   -- Admin, PhaChe, NhanVien
    MoTa        NVARCHAR(200)   NULL,
    -- Quyền hạn (1=có, 0=không)
    QuyenQuanLyNguoiDung    BIT NOT NULL DEFAULT 0,
    QuyenQuanLySanPham      BIT NOT NULL DEFAULT 0,
    QuyenDatBan             BIT NOT NULL DEFAULT 0,
    QuyenGoiMon             BIT NOT NULL DEFAULT 0,
    QuyenThanhToan          BIT NOT NULL DEFAULT 0,
    QuyenQuanLyKhachHang    BIT NOT NULL DEFAULT 0,
    QuyenThongKe            BIT NOT NULL DEFAULT 0,
    QuyenXuatBaoCao         BIT NOT NULL DEFAULT 0,
    NgayTao     DATETIME        NOT NULL DEFAULT GETDATE()
)
GO

-- ============================================================
-- BẢNG 2: NguoiDung - Tài khoản người dùng
-- ============================================================
CREATE TABLE NguoiDung (
    MaNguoiDung     INT             PRIMARY KEY IDENTITY(1,1),
    TenDangNhap     NVARCHAR(50)    NOT NULL UNIQUE,
    MatKhauHash     NVARCHAR(256)   NOT NULL,          -- SHA256
    HoTen           NVARCHAR(100)   NOT NULL,
    Email           NVARCHAR(150)   NULL,
    SoDienThoai     NVARCHAR(20)    NULL,
    MaVaiTro        INT             NOT NULL,
    TrangThai       BIT             NOT NULL DEFAULT 1, -- 1=Hoạt động, 0=Khóa
    NgayTao         DATETIME        NOT NULL DEFAULT GETDATE(),
    NgayCapNhat     DATETIME        NULL,
    FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro)
)
GO

-- ============================================================
-- BẢNG 3: KhachHang - Thông tin khách hàng
-- ============================================================
CREATE TABLE KhachHang (
    MaKhachHang     INT             PRIMARY KEY IDENTITY(1,1),
    HoTen           NVARCHAR(100)   NOT NULL,
    SoDienThoai     NVARCHAR(20)    NOT NULL UNIQUE,
    Email           NVARCHAR(150)   NULL,
    DiaChi          NVARCHAR(300)   NULL,
    NgaySinh        DATE            NULL,
    TongDiemTichLuy INT             NOT NULL DEFAULT 0,
    TongChiTieu     DECIMAL(18,0)   NOT NULL DEFAULT 0,
    HangKhachHang   NVARCHAR(30)    NOT NULL DEFAULT N'Thường', -- Thường, Bạc, Vàng, Kim cương
    GhiChu          NVARCHAR(500)   NULL,
    NgayDangKy      DATETIME        NOT NULL DEFAULT GETDATE(),
    NgayCapNhat     DATETIME        NULL
)
GO

-- ============================================================
-- BẢNG 4: KhuVuc - Khu vực trong quán
-- ============================================================
CREATE TABLE KhuVuc (
    MaKhuVuc    INT             PRIMARY KEY IDENTITY(1,1),
    TenKhuVuc   NVARCHAR(100)   NOT NULL,  -- Tầng 1, Tầng 2, Sân thượng, Ngoài trời
    MoTa        NVARCHAR(300)   NULL,
    TrangThai   BIT             NOT NULL DEFAULT 1
)
GO

-- ============================================================
-- BẢNG 5: Ban - Bàn trong quán
-- ============================================================
CREATE TABLE Ban (
    MaBan           INT             PRIMARY KEY IDENTITY(1,1),
    TenBan          NVARCHAR(50)    NOT NULL,    -- Bàn 01, Bàn 02...
    MaKhuVuc        INT             NOT NULL,
    SoGhe           INT             NOT NULL DEFAULT 4,
    ViTriHang       INT             NOT NULL DEFAULT 1,  -- Vị trí hàng trong sơ đồ
    ViTriCot        INT             NOT NULL DEFAULT 1,  -- Vị trí cột trong sơ đồ
    TrangThai       NVARCHAR(20)    NOT NULL DEFAULT N'Trống', -- Trống, Đang dùng, Đã đặt, Bảo trì
    GhiChu          NVARCHAR(300)   NULL,
    NgayCapNhat     DATETIME        NULL,
    FOREIGN KEY (MaKhuVuc) REFERENCES KhuVuc(MaKhuVuc)
)
GO

-- ============================================================
-- BẢNG 6: LoaiSanPham - Danh mục sản phẩm
-- ============================================================
CREATE TABLE LoaiSanPham (
    MaLoai      INT             PRIMARY KEY IDENTITY(1,1),
    TenLoai     NVARCHAR(100)   NOT NULL UNIQUE,
    MoTa        NVARCHAR(300)   NULL,
    ThuTuHienThi INT            NOT NULL DEFAULT 0,
    TrangThai   BIT             NOT NULL DEFAULT 1,
    NgayTao     DATETIME        NOT NULL DEFAULT GETDATE()
)
GO

-- ============================================================
-- BẢNG 7: SanPham - Menu sản phẩm (30 sản phẩm)
-- ============================================================
CREATE TABLE SanPham (
    MaSanPham       INT             PRIMARY KEY IDENTITY(1,1),
    TenSanPham      NVARCHAR(150)   NOT NULL,
    MaLoai          INT             NOT NULL,
    GiaBan          DECIMAL(18,0)   NOT NULL,
    GiaGoc          DECIMAL(18,0)   NULL,           -- Giá nhập (tham khảo)
    PhanTramGiam    DECIMAL(5,2)    NOT NULL DEFAULT 0,  -- % giảm giá thường xuyên
    MoTa            NVARCHAR(500)   NULL,
    DonVi           NVARCHAR(30)    NOT NULL DEFAULT N'Ly',
    TichDiemNhan    INT             NOT NULL DEFAULT 1,  -- Số điểm nhận được khi mua
    HinhAnh         NVARCHAR(300)   NULL,
    TrangThai       BIT             NOT NULL DEFAULT 1,
    NgayTao         DATETIME        NOT NULL DEFAULT GETDATE(),
    NgayCapNhat     DATETIME        NULL,
    FOREIGN KEY (MaLoai) REFERENCES LoaiSanPham(MaLoai)
)
GO

-- ============================================================
-- BẢNG 8: KhuyenMai - Chương trình khuyến mãi
-- ============================================================
CREATE TABLE KhuyenMai (
    MaKhuyenMai     INT             PRIMARY KEY IDENTITY(1,1),
    TenKhuyenMai    NVARCHAR(200)   NOT NULL,
    MaCode          NVARCHAR(50)    NULL UNIQUE,         -- Mã voucher
    LoaiGiam        NVARCHAR(20)    NOT NULL DEFAULT N'PhanTram', -- PhanTram, SoTien
    GiaTriGiam      DECIMAL(18,2)   NOT NULL,
    GiaTriDonToiThieu DECIMAL(18,0) NOT NULL DEFAULT 0,
    GiamToiDa      DECIMAL(18,0)   NULL,                -- Giảm tối đa (áp dụng cho loại %)
    NgayBatDau      DATE            NOT NULL,
    NgayKetThuc     DATE            NOT NULL,
    SoLanSuDung     INT             NULL,                -- NULL = không giới hạn
    DaSuDung        INT             NOT NULL DEFAULT 0,
    TrangThai       BIT             NOT NULL DEFAULT 1,
    NgayTao         DATETIME        NOT NULL DEFAULT GETDATE()
)
GO

-- ============================================================
-- BẢNG 9: HoaDon - Hóa đơn chính
-- ============================================================
CREATE TABLE HoaDon (
    MaHoaDon        INT             PRIMARY KEY IDENTITY(1,1),
    MaBan           INT             NOT NULL,
    MaKhachHang     INT             NULL,               -- Khách lẻ không cần thiết
    MaNguoiTao      INT             NOT NULL,
    MaNguoiCapNhat  INT             NULL,
    MaKhuyenMai     INT             NULL,
    LoaiHoaDon      NVARCHAR(20)    NOT NULL DEFAULT N'TraSau', -- TraTruoc, TraSau
    TrangThai       NVARCHAR(30)    NOT NULL DEFAULT N'DangGoi', 
    -- DangGoi, DaThanhToan, HuyBo, ChoXacNhan
    TongTamTinh     DECIMAL(18,0)   NOT NULL DEFAULT 0,
    SoTienGiam      DECIMAL(18,0)   NOT NULL DEFAULT 0,
    DiemDung        INT             NOT NULL DEFAULT 0,  -- Điểm quy đổi (dùng để giảm)
    TongThanhToan   DECIMAL(18,0)   NOT NULL DEFAULT 0,
    TienKhachDua    DECIMAL(18,0)   NULL,
    TienThua        DECIMAL(18,0)   NULL,
    PhuongThucTT    NVARCHAR(50)    NOT NULL DEFAULT N'TienMat', -- TienMat, ChuyenKhoan, TheNganHang
    GhiChu          NVARCHAR(500)   NULL,
    NgayTao         DATETIME        NOT NULL DEFAULT GETDATE(),
    NgayThanhToan   DATETIME        NULL,
    FOREIGN KEY (MaBan)          REFERENCES Ban(MaBan),
    FOREIGN KEY (MaKhachHang)   REFERENCES KhachHang(MaKhachHang),
    FOREIGN KEY (MaNguoiTao)    REFERENCES NguoiDung(MaNguoiDung),
    FOREIGN KEY (MaKhuyenMai)   REFERENCES KhuyenMai(MaKhuyenMai)
)
GO

-- ============================================================
-- BẢNG 10: ChiTietHoaDon - Chi tiết từng món trong hóa đơn
-- ============================================================
CREATE TABLE ChiTietHoaDon (
    MaChiTiet       INT             PRIMARY KEY IDENTITY(1,1),
    MaHoaDon        INT             NOT NULL,
    MaSanPham       INT             NOT NULL,
    TenSanPham      NVARCHAR(150)   NOT NULL,    -- Lưu lại tên tại thời điểm mua
    DonGia          DECIMAL(18,0)   NOT NULL,    -- Giá tại thời điểm mua
    SoLuong         INT             NOT NULL DEFAULT 1,
    PhanTramGiam    DECIMAL(5,2)    NOT NULL DEFAULT 0,
    ThanhTien       DECIMAL(18,0)   NOT NULL,
    TrangThaiMon    NVARCHAR(30)    NOT NULL DEFAULT N'ChoPha', -- ChoPha, DangPha, DaXong, HuyBo
    GhiChuMon       NVARCHAR(300)   NULL,        -- Ghi chú đặc biệt của khách
    NgayTao         DATETIME        NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (MaHoaDon)   REFERENCES HoaDon(MaHoaDon),
    FOREIGN KEY (MaSanPham)  REFERENCES SanPham(MaSanPham)
)
GO

-- ============================================================
-- BẢNG 11: LichSuDatBan - Lịch sử đặt bàn
-- ============================================================
CREATE TABLE LichSuDatBan (
    MaDatBan        INT             PRIMARY KEY IDENTITY(1,1),
    MaBan           INT             NOT NULL,
    MaKhachHang     INT             NULL,
    TenKhachDat     NVARCHAR(100)   NOT NULL,
    SoDienThoai     NVARCHAR(20)    NOT NULL,
    SoNguoi         INT             NOT NULL DEFAULT 1,
    NgayDat         DATETIME        NOT NULL,
    GioNhanBan      DATETIME        NULL,
    GioRoiBan       DATETIME        NULL,
    MaHoaDon        INT             NULL,
    TrangThai       NVARCHAR(30)    NOT NULL DEFAULT N'DatTruoc',
    -- DatTruoc, DangDen, DaXong, HuyBo
    GhiChu          NVARCHAR(500)   NULL,
    MaNguoiTao      INT             NOT NULL,
    NgayTao         DATETIME        NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (MaBan)         REFERENCES Ban(MaBan),
    FOREIGN KEY (MaKhachHang)   REFERENCES KhachHang(MaKhachHang),
    FOREIGN KEY (MaNguoiTao)    REFERENCES NguoiDung(MaNguoiDung)
)
GO

-- ============================================================
-- BẢNG 12: TichDiem - Lịch sử tích điểm khách hàng
-- ============================================================
CREATE TABLE TichDiem (
    MaTichDiem      INT             PRIMARY KEY IDENTITY(1,1),
    MaKhachHang     INT             NOT NULL,
    MaHoaDon        INT             NULL,
    LoaiGiaoDich    NVARCHAR(30)    NOT NULL, -- TichDiem, DoiDiem, HetHan, DieuChinh
    SoDiem          INT             NOT NULL,            -- Dương = tích, Âm = đổi
    SoDiemSauGD    INT             NOT NULL DEFAULT 0,   -- Số điểm còn lại sau giao dịch
    MoTa            NVARCHAR(300)   NULL,
    NgayGiaoDich    DATETIME        NOT NULL DEFAULT GETDATE(),
    MaNguoiThucHien INT             NULL,
    FOREIGN KEY (MaKhachHang)       REFERENCES KhachHang(MaKhachHang),
    FOREIGN KEY (MaHoaDon)          REFERENCES HoaDon(MaHoaDon),
    FOREIGN KEY (MaNguoiThucHien)   REFERENCES NguoiDung(MaNguoiDung)
)
GO

-- ============================================================
-- INDEX tối ưu truy vấn
-- ============================================================
CREATE INDEX IX_HoaDon_NgayTao          ON HoaDon(NgayTao)
CREATE INDEX IX_HoaDon_TrangThai        ON HoaDon(TrangThai)
CREATE INDEX IX_ChiTietHoaDon_MaHoaDon ON ChiTietHoaDon(MaHoaDon)
CREATE INDEX IX_LichSuDatBan_MaBan     ON LichSuDatBan(MaBan)
CREATE INDEX IX_TichDiem_MaKhachHang   ON TichDiem(MaKhachHang)
CREATE INDEX IX_SanPham_MaLoai         ON SanPham(MaLoai)
GO

PRINT N'=== Tạo thành công ==='
GO
-- ============================================================
-- FILE: 02_DuLieuMau.sql
-- MÔ TẢ: Chèn dữ liệu mẫu vào cơ sở dữ liệu CafeQuanLyDB
--         30 sản phẩm menu, tài khoản, bàn, khách hàng, khuyến mãi
-- ============================================================

USE CafeQuanLyDB;
GO

-- ============================================================
-- 1. VAI TRÒ
-- ============================================================
INSERT INTO VaiTro (TenVaiTro, MoTa,
    QuyenQuanLyNguoiDung, QuyenQuanLySanPham, QuyenDatBan, QuyenGoiMon,
    QuyenThanhToan, QuyenQuanLyKhachHang, QuyenThongKe, QuyenXuatBaoCao)
VALUES
(N'Admin',    N'Quản trị viên toàn quyền hệ thống',     1,1,1,1,1,1,1,1),
(N'PhaChe',  N'Nhân viên pha chế - xem và cập nhật trạng thái món', 0,1,0,0,0,0,0,0),
(N'NhanVien',N'Nhân viên phục vụ - đặt bàn, gọi món, thanh toán',   0,0,1,1,1,1,0,0);
GO

-- ============================================================
-- 2. NGƯỜI DÙNG (mật khẩu mặc định: 123456 → SHA256)
--    SHA256("123456") = 8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92
-- ============================================================
INSERT INTO NguoiDung (TenDangNhap, MatKhauHash, HoTen, Email, SoDienThoai, MaVaiTro, TrangThai) VALUES
(N'admin',    N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'Nguyễn Văn An',    N'admin@cafe.vn',     N'0901234567', 1, 1),
(N'phache01', N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'Trần Thị Bình',    N'binhtt@cafe.vn',    N'0902345678', 2, 1),
(N'phache02', N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'Lê Văn Cường',     N'cuonglv@cafe.vn',   N'0903456789', 2, 1),
(N'nhanvien01',N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92',N'Phạm Thị Dung',   N'dungpt@cafe.vn',    N'0904567890', 3, 1),
(N'nhanvien02',N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92',N'Hoàng Văn Em',    N'emhv@cafe.vn',      N'0905678901', 3, 1),
(N'nhanvien03',N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92',N'Vũ Thị Fa',       N'favt@cafe.vn',      N'0906789012', 3, 1);
GO

-- ============================================================
-- 3. KHÁCH HÀNG THÂN THIẾT
-- ============================================================
INSERT INTO KhachHang (HoTen, SoDienThoai, Email, DiaChi, NgaySinh, TongDiemTichLuy, TongChiTieu, HangKhachHang) VALUES
(N'Nguyễn Thị Hoa',    N'0911111111', N'hoa@gmail.com',    N'12 Lê Lợi, Q1, HCM',          N'1990-05-15', 350,  4500000,  N'Bạc'),
(N'Trần Văn Minh',     N'0922222222', N'minh@gmail.com',   N'45 Nguyễn Huệ, Q1, HCM',       N'1988-08-20', 1200, 15600000, N'Vàng'),
(N'Lê Thị Lan',        N'0933333333', N'lan@gmail.com',    N'78 Đồng Khởi, Q1, HCM',        N'1995-12-01', 80,   1050000,  N'Thường'),
(N'Phạm Quốc Tuấn',    N'0944444444', N'tuan@gmail.com',   N'23 Hai Bà Trưng, Q3, HCM',     N'1992-03-25', 560,  7200000,  N'Bạc'),
(N'Hoàng Thị Mai',     N'0955555555', N'mai@gmail.com',    N'9 Pasteur, Q3, HCM',            N'1997-07-10', 2100, 28500000, N'Kim cương'),
(N'Đặng Văn Long',     N'0966666666', N'long@gmail.com',   N'56 Đinh Tiên Hoàng, Bình Thạnh',N'1985-11-30', 150,  2000000,  N'Thường'),
(N'Bùi Thị Nhung',     N'0977777777', N'nhung@gmail.com',  N'34 Xô Viết Nghệ Tĩnh, BT, HCM',N'1993-04-18', 890,  11500000, N'Vàng'),
(N'Ngô Văn Đức',       N'0988888888', N'duc@gmail.com',    N'67 Phan Đình Phùng, PN, HCM',   N'1989-09-05', 420,  5400000,  N'Bạc'),
(N'Vũ Thị Thảo',       N'0999999999', N'thao@gmail.com',   N'15 Cộng Hòa, Tân Bình, HCM',   N'1996-02-22', 30,   400000,   N'Thường'),
(N'Đinh Quang Huy',    N'0800000000', N'huy@gmail.com',    N'88 Trường Chinh, Tân Phú, HCM', N'1991-06-14', 680,  8800000,  N'Vàng');
GO

-- ============================================================
-- 4. KHU VỰC
-- ============================================================
INSERT INTO KhuVuc (TenKhuVuc, MoTa) VALUES
(N'Tầng 1',       N'Khu vực chính, máy lạnh'),
(N'Tầng 2',       N'Khu vực thoáng, view đẹp'),
(N'Sân thượng',   N'Ngoài trời, cây xanh'),
(N'Phòng VIP',    N'Phòng riêng, yên tĩnh');
GO

-- ============================================================
-- 5. BÀN (20 bàn, bố cục 5x4)
-- ============================================================
INSERT INTO Ban (TenBan, MaKhuVuc, SoGhe, ViTriHang, ViTriCot, TrangThai) VALUES
-- Tầng 1: 8 bàn (hàng 1-2)
(N'Bàn 01', 1, 4, 1, 1, N'Trống'),
(N'Bàn 02', 1, 4, 1, 2, N'Trống'),
(N'Bàn 03', 1, 4, 1, 3, N'Trống'),
(N'Bàn 04', 1, 4, 1, 4, N'Trống'),
(N'Bàn 05', 1, 6, 2, 1, N'Trống'),
(N'Bàn 06', 1, 6, 2, 2, N'Trống'),
(N'Bàn 07', 1, 2, 2, 3, N'Trống'),
(N'Bàn 08', 1, 2, 2, 4, N'Trống'),
-- Tầng 2: 6 bàn (hàng 3)
(N'Bàn 09', 2, 4, 3, 1, N'Trống'),
(N'Bàn 10', 2, 4, 3, 2, N'Trống'),
(N'Bàn 11', 2, 4, 3, 3, N'Trống'),
(N'Bàn 12', 2, 6, 3, 4, N'Trống'),
(N'Bàn 13', 2, 6, 4, 1, N'Trống'),
(N'Bàn 14', 2, 4, 4, 2, N'Trống'),
-- Sân thượng: 4 bàn
(N'Bàn 15', 3, 4, 5, 1, N'Trống'),
(N'Bàn 16', 3, 4, 5, 2, N'Trống'),
(N'Bàn 17', 3, 6, 5, 3, N'Trống'),
(N'Bàn 18', 3, 6, 5, 4, N'Trống'),
-- Phòng VIP: 2 phòng
(N'VIP 01',  4, 8, 6, 1, N'Trống'),
(N'VIP 02',  4, 8, 6, 2, N'Trống');
GO

-- ============================================================
-- 6. LOẠI SẢN PHẨM
-- ============================================================
INSERT INTO LoaiSanPham (TenLoai, MoTa, ThuTuHienThi) VALUES
(N'Cà Phê',         N'Các loại cà phê truyền thống và hiện đại',      1),
(N'Trà & Trà Sữa',  N'Trà truyền thống, trà sữa Đài Loan',           2),
(N'Nước Ép & Sinh Tố', N'Nước ép trái cây tươi, sinh tố hoa quả',    3),
(N'Đồ Uống Đặc Biệt', N'Soda, chanh muối, đồ uống sáng tạo',        4),
(N'Bánh & Snack',   N'Bánh ngọt, snack nhẹ kèm đồ uống',            5),
(N'Ăn Nhẹ',        N'Mì, phở, bánh mì, sandwich nhẹ nhàng',         6);
GO

-- ============================================================
-- 7. SẢN PHẨM - 30 MÓN THỰC TẾ
-- ============================================================
-- [LOẠI 1] CÀ PHÊ (8 món)
INSERT INTO SanPham (TenSanPham, MaLoai, GiaBan, GiaGoc, PhanTramGiam, MoTa, DonVi, TichDiemNhan) VALUES
(N'Cà Phê Đen Đá',        1, 25000, 8000,  0,
 N'Cà phê robusta pha phin truyền thống, uống với đá. Đậm đà, thơm ngon.', N'Ly', 3),
(N'Cà Phê Sữa Đá',        1, 30000, 9000,  0,
 N'Cà phê đen pha với sữa đặc Ông Thọ, cho vị ngọt béo đặc trưng.', N'Ly', 3),
(N'Cà Phê Trứng',         1, 45000, 15000, 0,
 N'Cà phê trứng Hà Nội - lòng đỏ trứng đánh bông mịn trên nền cà phê nóng.', N'Ly', 5),
(N'Cappuccino',            1, 55000, 18000, 0,
 N'Espresso kết hợp sữa nóng và bọt sữa mịn kiểu Ý truyền thống.', N'Ly', 6),
(N'Latte',                 1, 55000, 18000, 0,
 N'Espresso với lượng lớn sữa nóng, nhẹ nhàng và béo ngậy.', N'Ly', 6),
(N'Americano',             1, 45000, 14000, 0,
 N'Espresso pha loãng với nước nóng, thanh vị, ít béo.', N'Ly', 5),
(N'Mocha',                 1, 60000, 20000, 5,
 N'Espresso + sô cô la + sữa nóng, phủ kem tươi. Ngọt ngào và thơm.', N'Ly', 7),
(N'Cold Brew Cà Phê',      1, 65000, 22000, 0,
 N'Cà phê ủ lạnh 12 giờ - vị mượt mà, ít đắng, mát lạnh hoàn hảo.', N'Ly', 7);
GO

-- [LOẠI 2] TRÀ & TRÀ SỮA (7 món)
INSERT INTO SanPham (TenSanPham, MaLoai, GiaBan, GiaGoc, PhanTramGiam, MoTa, DonVi, TichDiemNhan) VALUES
(N'Trà Đào Cam Sả',        2, 45000, 14000, 0,
 N'Trà xanh pha với đào tươi, cam vắt và sả thơm. Thanh mát giải khát.', N'Ly', 5),
(N'Trà Sữa Trân Châu Đen', 2, 50000, 16000, 0,
 N'Trà sữa Đài Loan truyền thống với trân châu đen dai ngon.', N'Ly', 5),
(N'Trà Sữa Trân Châu Trắng',2, 50000, 16000, 0,
 N'Trà sữa với trân châu bạch tuộc trắng mềm dai, béo ngậy.', N'Ly', 5),
(N'Trà Sữa Matcha',        2, 55000, 18000, 0,
 N'Bột matcha Nhật hòa quyện với sữa tươi và trà đen. Vị đắng nhẹ đặc trưng.', N'Ly', 6),
(N'Trà Sữa Oolong Sữa Muối',2,60000, 20000, 0,
 N'Trà oolong đặc biệt, phủ tầng kem sữa muối chuẩn Đài Loan.', N'Ly', 7),
(N'Trà Chanh Leo',          2, 40000, 12000, 0,
 N'Trà xanh với chanh leo tươi và mật ong nguyên chất. Chua ngọt dễ chịu.', N'Ly', 4),
(N'Hồng Trà Sữa Tươi',     2, 55000, 17000, 10,
 N'Hồng trà đậm vị pha với sữa tươi nguyên kem. Thơm nồng đặc biệt.', N'Ly', 6);
GO

-- [LOẠI 3] NƯỚC ÉP & SINH TỐ (6 món)
INSERT INTO SanPham (TenSanPham, MaLoai, GiaBan, GiaGoc, PhanTramGiam, MoTa, DonVi, TichDiemNhan) VALUES
(N'Nước Ép Cam Tươi',       3, 45000, 15000, 0,
 N'Cam tươi ép nguyên chất, ngọt tự nhiên, giàu vitamin C.', N'Ly', 5),
(N'Nước Ép Dưa Hấu',        3, 35000, 10000, 0,
 N'Dưa hấu đỏ mát lạnh ép tươi, thanh mát giải nhiệt.', N'Ly', 4),
(N'Sinh Tố Bơ Sữa Đặc',     3, 55000, 18000, 0,
 N'Bơ chín kem kết hợp sữa đặc và đá xay - béo ngậy, bổ dưỡng.', N'Ly', 6),
(N'Sinh Tố Xoài Dứa',       3, 50000, 16000, 0,
 N'Xoài cát Hòa Lộc + dứa tươi xay mịn. Hương thơm nhiệt đới đặc trưng.', N'Ly', 5),
(N'Sinh Tố Dâu Tây Sữa',    3, 60000, 20000, 5,
 N'Dâu tây Đà Lạt tươi kết hợp sữa tươi và đá xay. Vị chua ngọt tự nhiên.', N'Ly', 7),
(N'Nước Ép Cần Tây Táo Gừng',3,55000, 18000, 0,
 N'Combo detox: cần tây + táo xanh + gừng tươi. Tốt cho sức khỏe.', N'Ly', 6);
GO

-- [LOẠI 4] ĐỒ UỐNG ĐẶC BIỆT (5 món)
INSERT INTO SanPham (TenSanPham, MaLoai, GiaBan, GiaGoc, PhanTramGiam, MoTa, DonVi, TichDiemNhan) VALUES
(N'Chanh Muối Soda',         4, 35000, 10000, 0,
 N'Chanh muối chua cay + soda lạnh. Cực kỳ giải nhiệt buổi chiều.', N'Ly', 4),
(N'Soda Việt Quất',          4, 45000, 14000, 0,
 N'Soda gas + si rô việt quất + chanh vắt. Màu tím đẹp, vị chua ngọt.', N'Ly', 5),
(N'Soda Xanh Dương Hoa Đậu', 4, 50000, 16000, 0,
 N'Nước hoa đậu biếc pha soda - màu xanh đổi tím khi thêm chanh. Độc đáo.', N'Ly', 5),
(N'Chocolate Đá Xay',        4, 65000, 22000, 0,
 N'Bột cacao Bỉ + sữa + đá xay + kem tươi. Đậm đà, thơm ngon.', N'Ly', 7),
(N'Matcha Latte Đá',         4, 65000, 22000, 5,
 N'Matcha grade A từ Nhật + sữa tươi + đá - màu xanh đẹp, vị đắng nhẹ.', N'Ly', 7);
GO

-- [LOẠI 5] BÁNH & SNACK (2 món)
INSERT INTO SanPham (TenSanPham, MaLoai, GiaBan, GiaGoc, PhanTramGiam, MoTa, DonVi, TichDiemNhan) VALUES
(N'Bánh Croissant Bơ',        5, 35000, 12000, 0,
 N'Bánh sừng trâu bơ Pháp, giòn lớp ngoài mềm lớp trong. Nhập mỗi sáng.', N'Cái', 4),
(N'Bánh Tiramisu',            5, 55000, 20000, 0,
 N'Bánh Ý truyền thống với mascarpone, espresso và bột cacao. Mềm tan, thơm.', N'Miếng', 6),
(N'Cookie Hạt Macadamia',     5, 40000, 14000, 0,
 N'Cookie bơ giòn với hạt macadamia rang thơm. Đóng gói 3 cái.', N'Gói', 4),
(N'Cheesecake New York',      5, 65000, 22000, 0,
 N'Cheesecake truyền thống New York, mềm mịn, kèm coulis dâu.', N'Miếng', 7);
GO

-- [LOẠI 6] ĂN NHẸ (2 món)
INSERT INTO SanPham (TenSanPham, MaLoai, GiaBan, GiaGoc, PhanTramGiam, MoTa, DonVi, TichDiemNhan) VALUES
(N'Bánh Mì Trứng Phô Mai',    6, 45000, 15000, 0,
 N'Bánh mì nướng giòn, trứng ốp la, phô mai mozzarella chảy. No bữa sáng.', N'Cái', 5),
(N'Sandwich Gà Rau Củ',       6, 55000, 18000, 0,
 N'Bánh mì sandwich với gà ức áp chảo, rau xà lách, cà chua, sốt mayo.', N'Cái', 6);
GO

-- ============================================================
-- 8. KHUYẾN MÃI MẪU
-- ============================================================
INSERT INTO KhuyenMai (TenKhuyenMai, MaCode, LoaiGiam, GiaTriGiam, GiaTriDonToiThieu, GiamToiDa, NgayBatDau, NgayKetThuc, SoLanSuDung) VALUES
(N'Giảm 10% cho đơn từ 100k',  N'GIAM10',  N'PhanTram', 10, 100000, 30000, '2026-01-01', '2026-12-31', NULL),
(N'Giảm 20% cuối tuần',        N'WEEKEND', N'PhanTram', 20, 50000,  50000, '2026-01-01', '2026-12-31', NULL),
(N'Voucher 30.000đ',            N'SAVE30K', N'SoTien',   30000, 80000, NULL,'2026-03-01', '2026-06-30', 100),
(N'Flash Sale - Giảm 15%',     N'FLASH15', N'PhanTram', 15, 60000,  40000, '2026-03-27', '2026-04-30', 50),
(N'Ưu đãi khách hàng mới',     N'NEW2026', N'SoTien',   25000, 50000, NULL,'2026-01-01', '2026-12-31', 200);
GO

-- ============================================================
-- 9. DỮ LIỆU HÓA ĐƠN MẪU (5 hóa đơn hoàn chỉnh)
-- ============================================================

-- Hóa đơn 1: Bàn 01, đã thanh toán
UPDATE Ban SET TrangThai = N'Đang dùng' WHERE MaBan = 2;
UPDATE Ban SET TrangThai = N'Đã đặt'   WHERE MaBan = 5;

INSERT INTO HoaDon (MaBan, MaKhachHang, MaNguoiTao, LoaiHoaDon, TrangThai, TongTamTinh, SoTienGiam, TongThanhToan, TienKhachDua, TienThua, PhuongThucTT, NgayThanhToan)
VALUES (1, 1, 4, N'TraSau', N'DaThanhToan', 130000, 0, 130000, 150000, 20000, N'TienMat', DATEADD(HOUR,-3,GETDATE()));

INSERT INTO ChiTietHoaDon (MaHoaDon, MaSanPham, TenSanPham, DonGia, SoLuong, PhanTramGiam, ThanhTien, TrangThaiMon)
VALUES
(1, 2, N'Cà Phê Sữa Đá',        30000, 2, 0, 60000, N'DaXong'),
(1, 9, N'Trà Đào Cam Sả',        45000, 1, 0, 45000, N'DaXong'),
(1,27, N'Bánh Croissant Bơ',      35000, 1, 0, 35000, N'DaXong');

-- Tích điểm cho hóa đơn 1
INSERT INTO TichDiem (MaKhachHang, MaHoaDon, LoaiGiaoDich, SoDiem, SoDiemSauGD, MoTa)
VALUES (1, 1, N'TichDiem', 13, 363, N'Tích điểm hóa đơn #1');

-- Hóa đơn 2: Bàn 3, đang gọi món
INSERT INTO HoaDon (MaBan, MaKhachHang, MaNguoiTao, LoaiHoaDon, TrangThai, TongTamTinh, SoTienGiam, TongThanhToan, PhuongThucTT)
VALUES (3, 2, 4, N'TraSau', N'DangGoi', 165000, 0, 165000, N'TienMat');

INSERT INTO ChiTietHoaDon (MaHoaDon, MaSanPham, TenSanPham, DonGia, SoLuong, PhanTramGiam, ThanhTien, TrangThaiMon)
VALUES
(2, 4,  N'Cappuccino',              55000, 1, 0,  55000, N'DaXong'),
(2, 11, N'Trà Sữa Trân Châu Đen',  50000, 1, 0,  50000, N'DangPha'),
(2, 19, N'Nước Ép Cam Tươi',       45000, 1, 0,  45000, N'ChoPha'),
(2, 27, N'Bánh Croissant Bơ',      35000, 1, 0,  35000, N'DaXong');

-- Hóa đơn 3: Bàn 4, đặt trước
INSERT INTO HoaDon (MaBan, MaKhachHang, MaNguoiTao, LoaiHoaDon, TrangThai, TongTamTinh, SoTienGiam, TongThanhToan, TienKhachDua, TienThua, PhuongThucTT, NgayThanhToan)
VALUES (4, 5, 5, N'TraTruoc', N'DaThanhToan', 220000, 22000, 198000, 200000, 2000, N'TienMat', DATEADD(HOUR,-1,GETDATE()));

INSERT INTO ChiTietHoaDon (MaHoaDon, MaSanPham, TenSanPham, DonGia, SoLuong, PhanTramGiam, ThanhTien, TrangThaiMon)
VALUES
(3, 7,  N'Mocha',                   60000, 2, 5,  114000, N'DaXong'),
(3, 14, N'Trà Sữa Oolong Sữa Muối',60000, 1, 0,   60000, N'DaXong'),
(3, 23, N'Soda Việt Quất',          45000, 1, 0,   45000, N'DaXong');

INSERT INTO TichDiem (MaKhachHang, MaHoaDon, LoaiGiaoDich, SoDiem, SoDiemSauGD, MoTa)
VALUES (5, 3, N'TichDiem', 20, 2120, N'Tích điểm hóa đơn #3');

-- Hóa đơn 4: Thống kê ngày hôm qua
INSERT INTO HoaDon (MaBan, MaKhachHang, MaNguoiTao, LoaiHoaDon, TrangThai, TongTamTinh, SoTienGiam, TongThanhToan, TienKhachDua, TienThua, PhuongThucTT, NgayTao, NgayThanhToan)
VALUES (6, 4, 6, N'TraSau', N'DaThanhToan', 185000, 18500, 166500, 200000, 33500, N'ChuyenKhoan', DATEADD(DAY,-1, CAST(CAST(GETDATE() AS DATE) AS DATETIME)), DATEADD(DAY,-1, CAST(CAST(GETDATE() AS DATE) AS DATETIME)));

INSERT INTO ChiTietHoaDon (MaHoaDon, MaSanPham, TenSanPham, DonGia, SoLuong, PhanTramGiam, ThanhTien, TrangThaiMon)
VALUES
(4, 8,  N'Cold Brew Cà Phê',       65000, 1, 0,  65000, N'DaXong'),
(4, 22, N'Sinh Tố Dâu Tây Sữa',    60000, 1, 5,  57000, N'DaXong'),
(4, 28, N'Bánh Tiramisu',           55000, 1, 0,  55000, N'DaXong');

-- Hóa đơn 5: Bàn 9 - tháng này
INSERT INTO HoaDon (MaBan, MaKhachHang, MaNguoiTao, MaKhuyenMai, LoaiHoaDon, TrangThai, TongTamTinh, SoTienGiam, DiemDung, TongThanhToan, TienKhachDua, TienThua, PhuongThucTT, NgayThanhToan)
VALUES (9, 7, 4, 1, N'TraSau', N'DaThanhToan', 250000, 25000, 0, 225000, 230000, 5000, N'TienMat', DATEADD(DAY,-2,GETDATE()));

INSERT INTO ChiTietHoaDon (MaHoaDon, MaSanPham, TenSanPham, DonGia, SoLuong, PhanTramGiam, ThanhTien, TrangThaiMon)
VALUES
(5, 5,  N'Latte',                   55000, 2, 0, 110000, N'DaXong'),
(5, 13, N'Trà Sữa Matcha',          55000, 1, 0,  55000, N'DaXong'),
(5, 26, N'Matcha Latte Đá',         65000, 1, 5,  61750, N'DaXong'),
(5, 31, N'Bánh Mì Trứng Phô Mai',   45000, 1, 0,  45000, N'DaXong');

INSERT INTO TichDiem (MaKhachHang, MaHoaDon, LoaiGiaoDich, SoDiem, SoDiemSauGD, MoTa)
VALUES (7, 5, N'TichDiem', 22, 912, N'Tích điểm hóa đơn #5');

-- ============================================================
-- 10. LỊCH SỬ ĐẶT BÀN MẪU
-- ============================================================
INSERT INTO LichSuDatBan (MaBan, MaKhachHang, TenKhachDat, SoDienThoai, SoNguoi, NgayDat, GioNhanBan, GioRoiBan, MaHoaDon, TrangThai, MaNguoiTao) VALUES
-- Dòng 1: GioRoiBan là DATETIME (ví dụ: nhận bàn 3 tiếng trước, rời bàn 1 tiếng trước)
(1, 1, N'Nguyễn Thị Hoa',   N'0911111111', 3, DATEADD(HOUR,-3,GETDATE()), DATEADD(HOUR,-3,GETDATE()), DATEADD(HOUR,-1,GETDATE()), 1, N'DaXong', 4),

-- Dòng 2: Thiếu giá trị MaHoaDon (vì chưa có hóa đơn nên để NULL), sửa lại vị trí TrangThai và MaNguoiTao
(5, NULL, N'Nguyễn Văn X',  N'0912345678', 6, GETDATE(), DATEADD(HOUR,1,GETDATE()), NULL, NULL, N'DatTruoc', 5),

-- Dòng 3: Tương tự dòng 1, sửa lại GioRoiBan thành DATETIME
(9, 7, N'Bùi Thị Nhung',    N'0977777777', 4, DATEADD(DAY,-2,GETDATE()), DATEADD(DAY,-2,GETDATE()), DATEADD(DAY,-2, DATEADD(HOUR, 2, GETDATE())), 5, N'DaXong', 4);
GO

-- ============================================================
-- STORED PROCEDURE: Cập nhật hạng khách hàng
-- ============================================================
CREATE OR ALTER PROCEDURE sp_CapNhatHangKhachHang
    @MaKhachHang INT
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
END;
GO

-- ============================================================
-- STORED PROCEDURE: Thống kê doanh thu
-- ============================================================
CREATE OR ALTER PROCEDURE sp_ThongKeDoanhThu
    @TuNgay DATE,
    @DenNgay DATE
AS
BEGIN
    SELECT
        CAST(NgayThanhToan AS DATE)     AS [Ngày],
        COUNT(*)                         AS [Số hóa đơn],
        SUM(TongTamTinh)                AS [Doanh thu gốc],
        SUM(SoTienGiam)                 AS [Tổng giảm giá],
        SUM(TongThanhToan)              AS [Thực thu],
        COUNT(DISTINCT MaKhachHang)     AS [Số khách hàng]
    FROM HoaDon
    WHERE TrangThai = N'DaThanhToan'
      AND CAST(NgayThanhToan AS DATE) BETWEEN @TuNgay AND @DenNgay
    GROUP BY CAST(NgayThanhToan AS DATE)
    ORDER BY CAST(NgayThanhToan AS DATE);
END;
GO

PRINT N'=== Chèn dữ liệu mẫu thành công! ===';
PRINT N'Tài khoản: admin/123456 | phache01/123456 | nhanvien01/123456';
GO
