
USE QuanLyQuanCaPhe
GO

-- ============================================================
-- 1. VAI TRÒ
-- ============================================================
INSERT INTO VaiTro (TenVaiTro, MoTa,
    QuyenQuanLyNguoiDung, QuyenQuanLySanPham, QuyenDatBan, QuyenGoiMon,
    QuyenThanhToan, QuyenQuanLyKhachHang, QuyenThongKe, QuyenXuatBaoCao)
VALUES
(N'Admin',    N'Quản trị viên toàn quyền',               1,1,1,1,1,1,1,1),
(N'PhaChe',   N'Nhân viên pha chế - xem và cập nhật món', 0,1,0,0,0,0,0,0),
(N'NhanVien', N'Nhân viên - đặt bàn, gọi món, thanh toán',0,0,1,1,1,1,0,0);
GO

-- ============================================================
-- 2. NGƯỜI DÙNG (mật khẩu: 123456 → SHA256)
--    SHA256("123456") = 8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92
-- ============================================================
INSERT INTO NguoiDung (TenDangNhap, MatKhauHash, HoTen, Email, SoDienThoai, MaVaiTro) VALUES
(N'admin',      N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'Nguyễn Văn An',   N'admin@cafe.vn',    N'0901234567', 1),
(N'phache01',   N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'Trần Thị Bình',   N'binh@cafe.vn',     N'0902345678', 2),
(N'phache02',   N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'Lê Văn Cường',    N'cuong@cafe.vn',    N'0903456789', 2),
(N'nhanvien01', N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'Phạm Thị Dung',   N'dung@cafe.vn',     N'0904567890', 3),
(N'nhanvien02', N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'Hoàng Văn Em',    N'em@cafe.vn',       N'0905678901', 3),
(N'nhanvien03', N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'Vũ Thị Fa',       N'fa@cafe.vn',       N'0906789012', 3);
GO

-- ============================================================
-- 3. KHÁCH HÀNG THÂN THIẾT
-- ============================================================
INSERT INTO KhachHang (HoTen, SoDienThoai, Email, DiaChi, NgaySinh, TongDiemTichLuy, TongChiTieu, HangKhachHang) VALUES
(N'Nguyễn Thị Hoa',  N'0911111111', N'hoa@gmail.com',  N'12 Lê Lợi, Q1, HCM',          N'1990-05-15', 350,  4500000,  N'Bạc'),
(N'Trần Văn Minh',   N'0922222222', N'minh@gmail.com', N'45 Nguyễn Huệ, Q1, HCM',      N'1988-08-20', 1200, 15600000, N'Vàng'),
(N'Lê Thị Lan',      N'0933333333', N'lan@gmail.com',  N'78 Đồng Khởi, Q1, HCM',       N'1995-12-01', 80,   1050000,  N'Thường'),
(N'Phạm Quốc Tuấn',  N'0944444444', N'tuan@gmail.com', N'23 Hai Bà Trưng, Q3, HCM',    N'1992-03-25', 560,  7200000,  N'Bạc'),
(N'Hoàng Thị Mai',   N'0955555555', N'mai@gmail.com',  N'9 Pasteur, Q3, HCM',           N'1997-07-10', 2100, 28500000, N'Kim cương'),
(N'Đặng Văn Long',   N'0966666666', N'long@gmail.com', N'56 Đinh Tiên Hoàng, Bình Thạnh',N'1985-11-30',150,  2000000,  N'Thường'),
(N'Bùi Thị Nhung',   N'0977777777', N'nhung@gmail.com',N'34 Xô Viết Nghệ Tĩnh, BT',    N'1993-04-18', 890,  11500000, N'Vàng'),
(N'Ngô Văn Đức',     N'0988888888', N'duc@gmail.com',  N'67 Phan Đình Phùng, PN',       N'1989-09-05', 420,  5400000,  N'Bạc'),
(N'Vũ Thị Thảo',     N'0999999999', N'thao@gmail.com', N'15 Cộng Hòa, Tân Bình, HCM',  N'1996-02-22', 30,   400000,   N'Thường'),
(N'Đinh Quang Huy',  N'0800000000', N'huy@gmail.com',  N'88 Trường Chinh, Tân Phú, HCM',N'1991-06-14', 680,  8800000,  N'Vàng');
GO

-- ============================================================
-- 4. KHU VỰC
-- ============================================================
INSERT INTO KhuVuc (TenKhuVuc, MoTa) VALUES
(N'Tầng 1',     N'Khu vực chính, máy lạnh'),
(N'Tầng 2',     N'Khu vực thoáng, view đẹp'),
(N'Sân thượng', N'Ngoài trời, cây xanh'),
(N'Phòng VIP',  N'Phòng riêng, yên tĩnh');
GO

-- ============================================================
-- 5. BÀN (20 bàn)
-- ============================================================
INSERT INTO Ban (TenBan, MaKhuVuc, SoGhe, ViTriHang, ViTriCot, TrangThai) VALUES
(N'Bàn 01', 1, 4, 1, 1, N'Trống'),  (N'Bàn 02', 1, 4, 1, 2, N'Trống'),
(N'Bàn 03', 1, 4, 1, 3, N'Trống'),  (N'Bàn 04', 1, 4, 1, 4, N'Trống'),
(N'Bàn 05', 1, 6, 2, 1, N'Trống'),  (N'Bàn 06', 1, 6, 2, 2, N'Trống'),
(N'Bàn 07', 1, 2, 2, 3, N'Trống'),  (N'Bàn 08', 1, 2, 2, 4, N'Trống'),
(N'Bàn 09', 2, 4, 3, 1, N'Trống'),  (N'Bàn 10', 2, 4, 3, 2, N'Trống'),
(N'Bàn 11', 2, 4, 3, 3, N'Trống'),  (N'Bàn 12', 2, 6, 3, 4, N'Trống'),
(N'Bàn 13', 2, 6, 4, 1, N'Trống'),  (N'Bàn 14', 2, 4, 4, 2, N'Trống'),
(N'Bàn 15', 3, 4, 5, 1, N'Trống'),  (N'Bàn 16', 3, 4, 5, 2, N'Trống'),
(N'Bàn 17', 3, 6, 5, 3, N'Trống'),  (N'Bàn 18', 3, 6, 5, 4, N'Trống'),
(N'VIP 01',  4, 8, 6, 1, N'Trống'),  (N'VIP 02',  4, 8, 6, 2, N'Trống');
GO

-- ============================================================
-- 6. LOẠI SẢN PHẨM
-- ============================================================
INSERT INTO LoaiSanPham (TenLoai, MoTa, ThuTuHienThi) VALUES
(N'Cà Phê',            N'Các loại cà phê truyền thống và hiện đại', 1),
(N'Trà & Trà Sữa',     N'Trà truyền thống, trà sữa Đài Loan',      2),
(N'Nước Ép & Sinh Tố', N'Nước ép trái cây tươi, sinh tố',          3),
(N'Đồ Uống Đặc Biệt',  N'Soda, chanh muối, đồ uống sáng tạo',     4),
(N'Bánh & Snack',      N'Bánh ngọt, snack nhẹ kèm đồ uống',        5),
(N'Ăn Nhẹ',           N'Bánh mì, sandwich nhẹ nhàng',              6);
GO

-- ============================================================
-- 7. SẢN PHẨM - 30 MÓN
-- ============================================================
-- [Loại 1] Cà Phê
INSERT INTO SanPham (TenSanPham, MaLoai, GiaBan, GiaGoc, PhanTramGiam, MoTa, DonVi, TichDiemNhan) VALUES
(N'Cà Phê Đen Đá',        1, 25000, 8000,  0,  N'Cà phê robusta pha phin truyền thống, uống với đá.',            N'Ly', 3),
(N'Cà Phê Sữa Đá',        1, 30000, 9000,  0,  N'Cà phê đen pha với sữa đặc Ông Thọ, vị ngọt béo đặc trưng.',  N'Ly', 3),
(N'Cà Phê Trứng',         1, 45000, 15000, 0,  N'Cà phê trứng Hà Nội - lòng đỏ đánh bông trên nền cà phê.',    N'Ly', 5),
(N'Cappuccino',            1, 55000, 18000, 0,  N'Espresso kết hợp sữa nóng và bọt sữa mịn kiểu Ý.',            N'Ly', 6),
(N'Latte',                 1, 55000, 18000, 0,  N'Espresso với lượng lớn sữa nóng, nhẹ nhàng và béo ngậy.',      N'Ly', 6),
(N'Americano',             1, 45000, 14000, 0,  N'Espresso pha loãng với nước nóng, thanh vị.',                  N'Ly', 5),
(N'Mocha',                 1, 60000, 20000, 5,  N'Espresso + sô cô la + sữa nóng, phủ kem tươi.',                N'Ly', 7),
(N'Cold Brew',             1, 65000, 22000, 0,  N'Cà phê ủ lạnh 12 giờ - vị mượt mà, ít đắng.',               N'Ly', 7);
GO

-- [Loại 2] Trà & Trà Sữa
INSERT INTO SanPham (TenSanPham, MaLoai, GiaBan, GiaGoc, PhanTramGiam, MoTa, DonVi, TichDiemNhan) VALUES
(N'Trà Đào Cam Sả',         2, 45000, 14000, 0,  N'Trà xanh pha với đào tươi, cam vắt và sả thơm.',              N'Ly', 5),
(N'Trà Sữa Trân Châu Đen',  2, 50000, 16000, 0,  N'Trà sữa Đài Loan truyền thống với trân châu đen dai.',        N'Ly', 5),
(N'Trà Sữa Trân Châu Trắng',2, 50000, 16000, 0,  N'Trà sữa với trân châu bạch tuộc trắng mềm dai.',              N'Ly', 5),
(N'Trà Sữa Matcha',         2, 55000, 18000, 0,  N'Bột matcha Nhật hòa quyện với sữa tươi và trà đen.',          N'Ly', 6),
(N'Trà Sữa Oolong Sữa Muối',2, 60000, 20000, 0,  N'Trà oolong đặc biệt, phủ tầng kem sữa muối chuẩn Đài Loan.', N'Ly', 7),
(N'Trà Chanh Leo',           2, 40000, 12000, 0,  N'Trà xanh với chanh leo tươi và mật ong nguyên chất.',         N'Ly', 4),
(N'Hồng Trà Sữa Tươi',      2, 55000, 17000, 10, N'Hồng trà đậm vị pha với sữa tươi nguyên kem.',               N'Ly', 6);
GO

-- [Loại 3] Nước Ép & Sinh Tố
INSERT INTO SanPham (TenSanPham, MaLoai, GiaBan, GiaGoc, PhanTramGiam, MoTa, DonVi, TichDiemNhan) VALUES
(N'Nước Ép Cam Tươi',        3, 45000, 15000, 0, N'Cam tươi ép nguyên chất, giàu vitamin C.',                    N'Ly', 5),
(N'Nước Ép Dưa Hấu',         3, 35000, 10000, 0, N'Dưa hấu đỏ mát lạnh ép tươi, thanh mát giải nhiệt.',        N'Ly', 4),
(N'Sinh Tố Bơ Sữa Đặc',      3, 55000, 18000, 0, N'Bơ chín kết hợp sữa đặc và đá xay - béo ngậy, bổ dưỡng.',   N'Ly', 6),
(N'Sinh Tố Xoài Dứa',        3, 50000, 16000, 0, N'Xoài cát Hòa Lộc + dứa tươi xay mịn.',                      N'Ly', 5),
(N'Sinh Tố Dâu Tây Sữa',     3, 60000, 20000, 5, N'Dâu tây Đà Lạt tươi kết hợp sữa tươi và đá xay.',          N'Ly', 7),
(N'Nước Ép Cần Tây Táo Gừng',3, 55000, 18000, 0, N'Combo detox: cần tây + táo xanh + gừng tươi.',              N'Ly', 6);
GO

-- [Loại 4] Đồ Uống Đặc Biệt
INSERT INTO SanPham (TenSanPham, MaLoai, GiaBan, GiaGoc, PhanTramGiam, MoTa, DonVi, TichDiemNhan) VALUES
(N'Chanh Muối Soda',         4, 35000, 10000, 0, N'Chanh muối chua cay + soda lạnh. Cực kỳ giải nhiệt.',         N'Ly', 4),
(N'Soda Việt Quất',          4, 45000, 14000, 0, N'Soda gas + si rô việt quất + chanh vắt.',                     N'Ly', 5),
(N'Soda Hoa Đậu Biếc',       4, 50000, 16000, 0, N'Nước hoa đậu biếc pha soda - màu xanh đổi tím khi thêm chanh.',N'Ly', 5),
(N'Chocolate Đá Xay',        4, 65000, 22000, 0, N'Bột cacao Bỉ + sữa + đá xay + kem tươi.',                    N'Ly', 7),
(N'Matcha Latte Đá',         4, 65000, 22000, 5, N'Matcha grade A Nhật + sữa tươi + đá - vị đắng nhẹ.',         N'Ly', 7);
GO

-- [Loại 5] Bánh & Snack
INSERT INTO SanPham (TenSanPham, MaLoai, GiaBan, GiaGoc, PhanTramGiam, MoTa, DonVi, TichDiemNhan) VALUES
(N'Bánh Croissant Bơ',       5, 35000, 12000, 0, N'Bánh sừng trâu bơ Pháp, giòn ngoài mềm trong.',              N'Cái',   4),
(N'Bánh Tiramisu',           5, 55000, 20000, 0, N'Bánh Ý với mascarpone, espresso và bột cacao.',               N'Miếng', 6),
(N'Cookie Hạt Macadamia',    5, 40000, 14000, 0, N'Cookie bơ giòn với hạt macadamia rang thơm (3 cái).',         N'Gói',   4),
(N'Cheesecake New York',     5, 65000, 22000, 0, N'Cheesecake mềm mịn kèm coulis dâu.',                          N'Miếng', 7);
GO

-- [Loại 6] Ăn Nhẹ
INSERT INTO SanPham (TenSanPham, MaLoai, GiaBan, GiaGoc, PhanTramGiam, MoTa, DonVi, TichDiemNhan) VALUES
(N'Bánh Mì Trứng Phô Mai',   6, 45000, 15000, 0, N'Bánh mì nướng giòn, trứng ốp la, phô mai mozzarella chảy.', N'Cái', 5),
(N'Sandwich Gà Rau Củ',      6, 55000, 18000, 0, N'Sandwich với gà ức áp chảo, rau xà lách, cà chua, mayo.',   N'Cái', 6);
GO

-- ============================================================
-- 8. KHUYẾN MÃI
-- ============================================================
INSERT INTO KhuyenMai (TenKhuyenMai, MaCode, LoaiGiam, GiaTriGiam, GiaTriDonToiThieu, GiamToiDa, NgayBatDau, NgayKetThuc, SoLanSuDung) VALUES
(N'Giảm 10% đơn từ 100k',  N'GIAM10',  N'PhanTram', 10,  100000, 30000, '2026-01-01', '2026-12-31', NULL),
(N'Giảm 20% cuối tuần',    N'WEEKEND', N'PhanTram', 20,   50000, 50000, '2026-01-01', '2026-12-31', NULL),
(N'Voucher giảm 30.000đ',  N'SAVE30K', N'SoTien',   30000,80000, NULL,  '2026-03-01', '2026-06-30', 100),
(N'Flash Sale giảm 15%',   N'FLASH15', N'PhanTram', 15,   60000, 40000, '2026-03-27', '2026-12-31', 50),
(N'Ưu đãi khách hàng mới', N'NEW2026', N'SoTien',   25000,50000, NULL,  '2026-01-01', '2026-12-31', 200);
GO

-- ============================================================
-- 9. HÓA ĐƠN MẪU
-- ============================================================
UPDATE Ban SET TrangThai = N'Đang dùng' WHERE MaBan = 2;
UPDATE Ban SET TrangThai = N'Đã đặt'   WHERE MaBan = 5;

-- HD1: Đã thanh toán
INSERT INTO HoaDon (MaBan, MaKhachHang, MaNguoiTao, LoaiHoaDon, TrangThai,
    TongTamTinh, SoTienGiam, TongThanhToan, TienKhachDua, TienThua, PhuongThucTT, NgayThanhToan)
VALUES (1, 1, 4, N'TraSau', N'DaThanhToan', 130000, 0, 130000, 150000, 20000, N'TienMat', DATEADD(HOUR,-3,GETDATE()));

INSERT INTO ChiTietHoaDon (MaHoaDon, MaSanPham, TenSanPham, DonGia, SoLuong, PhanTramGiam, ThanhTien, TrangThaiMon) VALUES
(1, 2, N'Cà Phê Sữa Đá',    30000, 2, 0, 60000, N'DaXong'),
(1, 9, N'Trà Đào Cam Sả',   45000, 1, 0, 45000, N'DaXong'),
(1,27, N'Bánh Croissant Bơ',35000, 1, 0, 35000, N'DaXong');

INSERT INTO TichDiem (MaKhachHang, MaHoaDon, LoaiGiaoDich, SoDiem, SoDiemSauGD, MoTa) VALUES
(1, 1, N'TichDiem', 13, 363, N'Tích điểm HD #1');

-- HD2: Đang gọi món - Bàn 3
INSERT INTO HoaDon (MaBan, MaKhachHang, MaNguoiTao, LoaiHoaDon, TrangThai,
    TongTamTinh, SoTienGiam, TongThanhToan, PhuongThucTT)
VALUES (3, 2, 4, N'TraSau', N'DangGoi', 165000, 0, 165000, N'TienMat');

INSERT INTO ChiTietHoaDon (MaHoaDon, MaSanPham, TenSanPham, DonGia, SoLuong, PhanTramGiam, ThanhTien, TrangThaiMon) VALUES
(2, 4,  N'Cappuccino',           55000, 1, 0, 55000, N'DaXong'),
(2, 10, N'Trà Sữa Trân Châu Đen',50000, 1, 0, 50000, N'DangPha'),
(2, 16, N'Nước Ép Cam Tươi',     45000, 1, 0, 45000, N'ChoPha'),
(2, 27, N'Bánh Croissant Bơ',    35000, 1, 0, 35000, N'DaXong');

-- HD3: Đã thanh toán - hôm qua
INSERT INTO HoaDon (MaBan, MaKhachHang, MaNguoiTao, LoaiHoaDon, TrangThai,
    TongTamTinh, SoTienGiam, TongThanhToan, TienKhachDua, TienThua, PhuongThucTT, NgayTao, NgayThanhToan)
VALUES (6, 4, 5, N'TraSau', N'DaThanhToan', 185000, 18500, 166500, 200000, 33500, N'ChuyenKhoan',
    DATEADD(DAY,-1,GETDATE()), DATEADD(DAY,-1,GETDATE()));

INSERT INTO ChiTietHoaDon (MaHoaDon, MaSanPham, TenSanPham, DonGia, SoLuong, PhanTramGiam, ThanhTien, TrangThaiMon) VALUES
(3, 8,  N'Cold Brew',        65000, 1, 0,  65000, N'DaXong'),
(3, 20, N'Sinh Tố Dâu Tây', 60000, 1, 5, 57000, N'DaXong'),
(3, 28, N'Bánh Tiramisu',   55000, 1, 0,  55000, N'DaXong');

-- HD4: Tháng này
INSERT INTO HoaDon (MaBan, MaKhachHang, MaNguoiTao, MaKhuyenMai, LoaiHoaDon, TrangThai,
    TongTamTinh, SoTienGiam, TongThanhToan, TienKhachDua, TienThua, PhuongThucTT, NgayThanhToan)
VALUES (9, 7, 4, 1, N'TraSau', N'DaThanhToan', 250000, 25000, 225000, 230000, 5000, N'TienMat', DATEADD(DAY,-2,GETDATE()));

INSERT INTO ChiTietHoaDon (MaHoaDon, MaSanPham, TenSanPham, DonGia, SoLuong, PhanTramGiam, ThanhTien, TrangThaiMon) VALUES
(4, 5,  N'Latte',             55000, 2, 0, 110000, N'DaXong'),
(4, 12, N'Trà Sữa Matcha',   55000, 1, 0,  55000, N'DaXong'),
(4, 25, N'Matcha Latte Đá',  65000, 1, 5,  61750, N'DaXong'),
(4, 29, N'Bánh Mì Trứng',   45000, 1, 0,  45000, N'DaXong');

INSERT INTO TichDiem (MaKhachHang, MaHoaDon, LoaiGiaoDich, SoDiem, SoDiemSauGD, MoTa) VALUES
(7, 4, N'TichDiem', 22, 912, N'Tích điểm HD #4');
GO

-- ============================================================
-- 10. LỊCH SỬ ĐẶT BÀN MẪU
-- ============================================================
INSERT INTO LichSuDatBan (MaBan, MaKhachHang, TenKhachDat, SoDienThoai, SoNguoi,
    NgayDat, GioNhanBan, GioRoiBan, MaHoaDon, TrangThai, MaNguoiTao) VALUES
(1, 1, N'Nguyễn Thị Hoa', N'0911111111', 3,
    DATEADD(HOUR,-3,GETDATE()), DATEADD(HOUR,-3,GETDATE()), DATEADD(HOUR,-1,GETDATE()), 1, N'DaXong', 4),
(5, NULL, N'Nguyễn Văn X', N'0912345678', 6,
    GETDATE(), DATEADD(HOUR,1,GETDATE()), NULL, NULL, N'DatTruoc', 5);
GO

PRINT N'=== Dữ liệu mẫu đã được chèn thành công! ===';
PRINT N'Tài khoản: admin/123456 | phache01/123456 | nhanvien01/123456';
GO
