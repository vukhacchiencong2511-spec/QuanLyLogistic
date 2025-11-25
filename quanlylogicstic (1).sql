create database quanlylogicstic
use quanlylogicstic

CREATE TABLE VaiTro (
    MaVaiTro INT PRIMARY KEY,
    TenVaiTro NVARCHAR(50) UNIQUE NOT NULL -- 'Admin','DieuPhoi','TaiXe','KhachHang'
);

CREATE TABLE NguoiDung (
    MaNguoiDung NVARCHAR(100) PRIMARY KEY,
    TenDangNhap NVARCHAR(100) UNIQUE NOT NULL,
    MatKhau NVARCHAR(300),
    HoTen NVARCHAR(200),
    MaVaiTro INT NOT NULL FOREIGN KEY REFERENCES VaiTro(MaVaiTro),
    NgayTao DATETIME2 DEFAULT SYSDATETIME()
);

CREATE TABLE KhachHang (
    MaKhachHang NVARCHAR(100) PRIMARY KEY,
    TenKhachHang NVARCHAR(200) NOT NULL,
    SoDienThoai NVARCHAR(30),
    Email NVARCHAR(200),
    NgayTao DATETIME2 DEFAULT SYSDATETIME()
);

CREATE TABLE DiaChi (
    MaDiaChi NVARCHAR(100) PRIMARY KEY,
    MaKhachHang NVARCHAR(100) FOREIGN KEY REFERENCES KhachHang(MaKhachHang) ON DELETE CASCADE,
    DiaChiChiTiet NVARCHAR(300),
    ThanhPho NVARCHAR(100),
    QuanHuyen NVARCHAR(100),
    MaBuuDien NVARCHAR(20)
);

CREATE TABLE DonVanChuyen (
    MaDon NVARCHAR(100) PRIMARY KEY,
    MaDonCode NVARCHAR(50) UNIQUE NOT NULL,
    MaVanDon NVARCHAR(100), -- FK tới VanDon
    MaKhachGui NVARCHAR(100) FOREIGN KEY REFERENCES KhachHang(MaKhachHang) ON DELETE CASCADE,
    MaKhachNhan NVARCHAR(100) FOREIGN KEY REFERENCES KhachHang(MaKhachHang),
    MaDiaChiLay NVARCHAR(100) FOREIGN KEY REFERENCES DiaChi(MaDiaChi),
    MaDiaChiGiao NVARCHAR(100) FOREIGN KEY REFERENCES DiaChi(MaDiaChi),
    LoaiHang NVARCHAR(50),
    KhoiLuong DECIMAL(8,3),
    GiaTriKhaiBao DECIMAL(14,2),
    NguoiTao NVARCHAR(100) FOREIGN KEY REFERENCES NguoiDung(MaNguoiDung),
    NgayTao DATETIME2 DEFAULT SYSDATETIME(),
    MaTuyen NVARCHAR(100) NULL, -- FK tới TuyenDuong
    TrangThai NVARCHAR(50) DEFAULT N'Khởi tạo'
);

CREATE TABLE VanDon (
    MaVanDon NVARCHAR(100) PRIMARY KEY,
    SoVanDon NVARCHAR(100) UNIQUE NOT NULL,
    MaDon NVARCHAR(100) UNIQUE FOREIGN KEY REFERENCES DonVanChuyen(MaDon) ON DELETE CASCADE,
    NgayPhatHanh DATETIME2,
    ThongTinNhaXe NVARCHAR(MAX)
);

CREATE TABLE TuyenDuong (
    MaTuyen NVARCHAR(100) PRIMARY KEY,
    MaTuyenCode NVARCHAR(100) UNIQUE,
    MaTaiXe NVARCHAR(100) FOREIGN KEY REFERENCES NguoiDung(MaNguoiDung),
    PhuongTien NVARCHAR(100),
    ThoiGianBatDau DATETIME2,
    ThoiGianKetThuc DATETIME2,
    MaKhuVuc NVARCHAR(50),
    DoanhThuUocTinh DECIMAL(14,2),
    NgayTao DATETIME2 DEFAULT SYSDATETIME()
);

CREATE TABLE DiemDung (
    MaDiemDung NVARCHAR(100) PRIMARY KEY,
    MaTuyen NVARCHAR(100) FOREIGN KEY REFERENCES TuyenDuong(MaTuyen) ON DELETE CASCADE,
    ThuTuDung INT NOT NULL,
    MaDon NVARCHAR(100) FOREIGN KEY REFERENCES DonVanChuyen(MaDon) ON DELETE CASCADE,
    DuKienDen DATETIME2,
    ThucTeDen DATETIME2
);

CREATE UNIQUE INDEX UX_DiemDung_ThuTu ON DiemDung(MaTuyen, ThuTuDung);

CREATE TABLE SuKienTrangThai (
    MaSuKien BIGINT IDENTITY(1,1) PRIMARY KEY,
    MaDon NVARCHAR(100) FOREIGN KEY REFERENCES DonVanChuyen(MaDon) ON DELETE CASCADE,
    TrangThai NVARCHAR(60) NOT NULL, -- ví dụ: 'Đã lấy','Đang giao','Đã giao','Thất bại'
    LyDo NVARCHAR(MAX),
    ThoiGian DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    NguoiCapNhat NVARCHAR(100) FOREIGN KEY REFERENCES NguoiDung(MaNguoiDung),
    DuLieuThem NVARCHAR(MAX), -- JSON metadata
    MaSuKienNgoai NVARCHAR(200),
    KhoaIdempotent NVARCHAR(200),
    NgayTao DATETIME2 DEFAULT SYSDATETIME()
);

CREATE UNIQUE INDEX UX_SuKien_External ON SuKienTrangThai(MaDon, MaSuKienNgoai) WHERE MaSuKienNgoai IS NOT NULL;

CREATE UNIQUE INDEX UX_SuKien_Idempotent ON SuKienTrangThai(KhoaIdempotent) WHERE KhoaIdempotent IS NOT NULL;

CREATE TABLE GiaoDichCOD (
    MaGiaoDich BIGINT IDENTITY(1,1) PRIMARY KEY,
    MaDon NVARCHAR(100) FOREIGN KEY REFERENCES DonVanChuyen(MaDon) ON DELETE CASCADE,
    SoTien DECIMAL(14,2) NOT NULL,
    NguoiThu NVARCHAR(100) FOREIGN KEY REFERENCES NguoiDung(MaNguoiDung),
    NgayThu DATETIME2,
    DaDoiSoat BIT DEFAULT 0,
    NgayDoiSoat DATETIME2,
    SoTienThanhToan DECIMAL(14,2), -- số tiền trả lại cho người gửi sau khi trừ phí
    DuLieuThem NVARCHAR(MAX)
);

CREATE TABLE ChungTu (
    MaChungTu BIGINT  IDENTITY(1,1) PRIMARY KEY,
    MaDon NVARCHAR(100) FOREIGN KEY REFERENCES DonVanChuyen(MaDon) ON DELETE CASCADE,
    NguoiUpload NVARCHAR(100) FOREIGN KEY REFERENCES NguoiDung(MaNguoiDung),
    NgayUpload DATETIME2 DEFAULT SYSDATETIME(),
    KyNhan NVARCHAR(MAX) NOT NULL,
    DuongDanThuNho NVARCHAR(MAX),
    LoaiKyNhan NVARCHAR(50) -- 'Anh','ChuKy'
);

CREATE TABLE DonYeuCau (
    MaYeuCau NVARCHAR(20) PRIMARY KEY,
    TenNguoiGui NVARCHAR(200),
    SDTNguoiGui NVARCHAR(50),
    EmailNguoiGui NVARCHAR(200),
    DiaChiGui NVARCHAR(300),

    TenNguoiNhan NVARCHAR(200),
    SDTNguoiNhan NVARCHAR(50),
    EmailNguoiNhan NVARCHAR(200),
    DiaChiNhan NVARCHAR(300),

    LoaiHang NVARCHAR(200),
    KhoiLuong DECIMAL(8,3),
    GiaTriKhaiBao DECIMAL(14,2),

    GhiChu NVARCHAR(300),
    NgayTao DATETIME2 DEFAULT SYSDATETIME()
);


go

CREATE TRIGGER trg_UpdateTrangThaiDonVanChuyen
ON SuKienTrangThai
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dvc
    SET dvc.TrangThai = i.TrangThai
    FROM DonVanChuyen dvc
    INNER JOIN inserted i ON dvc.MaDon = i.MaDon;
END;
GO


INSERT INTO VaiTro (MaVaiTro, TenVaiTro)
VALUES 
('1', N'Admin'),
('2', N'Tài xế giao hàng'),
('3', N'Nhân viên'),
('4', N'Khách hàng');


INSERT INTO NguoiDung (MaNguoiDung, TenDangNhap, MatKhau, HoTen, MaVaiTro)
VALUES (N'ad1', N'admin', N'123456', N'Nguyen Van A', 1);


SELECT 
    fk.name AS FK_Name,
    tp.name AS TableName,
    cp.name AS ColumnName,
    tr.name AS ReferencedTable
FROM sys.foreign_keys fk
JOIN sys.tables tp ON fk.parent_object_id = tp.object_id
JOIN sys.tables tr ON fk.referenced_object_id = tr.object_id
JOIN sys.foreign_key_columns fkc ON fkc.constraint_object_id = fk.object_id
JOIN sys.columns cp ON fkc.parent_object_id = cp.object_id AND fkc.parent_column_id = cp.column_id
ORDER BY tp.name;

SELECT * FROM NguoiDung;

--nguoidung

SET NOCOUNT ON;
BEGIN TRAN;

DECLARE @i INT = 1;

WHILE @i <= 100
BEGIN
    DECLARE @id NVARCHAR(20) = 'ND' + RIGHT('000' + CAST(@i AS VARCHAR(3)), 3);
    DECLARE @username NVARCHAR(50) = 'user' + RIGHT('000' + CAST(@i AS VARCHAR(3)), 3);
    DECLARE @password NVARCHAR(200) = 'password' + RIGHT('000' + CAST(@i AS VARCHAR(3)), 3);
    DECLARE @hoten NVARCHAR(200) = N'Người Dùng ' + CAST(@i AS NVARCHAR(10));

    DECLARE @role INT =
    CASE
        WHEN @i <= 3 THEN 1          -- 3 Admin
        WHEN @i <= 8 THEN 3          -- 5 Điều phối (4..8)
        WHEN @i <= 18 THEN 2         -- 10 Tài xế (9..18)
        ELSE 4                       -- Còn lại
    END;

    INSERT INTO NguoiDung (MaNguoiDung, TenDangNhap, MatKhau, HoTen, MaVaiTro)
    VALUES (@id, @username, @password, @hoten, @role);

    SET @i += 1;
END

COMMIT;

-- 2) KhachHang: 100 khách hàng (KH0001..KH0100)
SET NOCOUNT ON;
BEGIN TRAN;
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @ma NVARCHAR(20) = 'KH' + RIGHT('000' + CAST(@i AS VARCHAR(3)), 3);
    DECLARE @ten NVARCHAR(200) = N'Khách Hàng ' + CAST(@i AS NVARCHAR(10));
    DECLARE @phone NVARCHAR(30) = '0905' + RIGHT('000' + CAST(@i AS VARCHAR(3)), 3);
    DECLARE @email NVARCHAR(200) = LOWER(CONCAT('kh', @i, '@example.com'));
    INSERT INTO KhachHang (MaKhachHang, TenKhachHang, SoDienThoai, Email)
    VALUES (@ma, @ten, @phone, @email);
    SET @i += 1;
END
COMMIT;

-- 3) DiaChi: 100 địa chỉ, mỗi địa chỉ thuộc 1 KhachHang tương ứng
SET NOCOUNT ON;
BEGIN TRAN;
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @maDC NVARCHAR(20) = 'DC' + RIGHT('000' + CAST(@i AS VARCHAR(3)), 3);
    DECLARE @maKH NVARCHAR(20) = 'KH' + RIGHT('000' + CAST(@i AS VARCHAR(3)), 3);
    DECLARE @detail NVARCHAR(300) = N'Số ' + CAST(100 + @i AS NVARCHAR(10)) + N', Đường A, Phường B';
    DECLARE @city NVARCHAR(100) = CASE WHEN @i % 5 = 0 THEN N'Đà Nẵng'
                                      WHEN @i % 5 = 1 THEN N'Hà Nội'
                                      WHEN @i % 5 = 2 THEN N'TP HCM'
                                      WHEN @i % 5 = 3 THEN N'Hải Phòng'
                                      ELSE N'Đồng Nai' END;
    DECLARE @district NVARCHAR(100) = N'Quận ' + CAST((@i % 20) + 1 AS NVARCHAR(3));
    DECLARE @postal NVARCHAR(20) = RIGHT('00000' + CAST(10000 + @i AS VARCHAR(5)), 5);
    INSERT INTO DiaChi (MaDiaChi, MaKhachHang, DiaChiChiTiet, ThanhPho, QuanHuyen, MaBuuDien)
    VALUES (@maDC, @maKH, @detail, @city, @district, @postal);
    SET @i += 1;
END
COMMIT;

-- 4) TuyenDuong: 100 tuyến, MaTaiXe chọn từ NguoiDung
SET NOCOUNT ON;
BEGIN TRAN;
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @maTD NVARCHAR(20) = 'TD' + RIGHT('000' + CAST(@i AS VARCHAR(3)), 3);
    DECLARE @code NVARCHAR(100) = 'TDCODE' + RIGHT('000' + CAST(@i AS VARCHAR(3)), 3);
    DECLARE @maTaiXe NVARCHAR(20) = 'ND' + RIGHT('000' + CAST(((@i % 100) + 1) AS VARCHAR(3)), 3);
    DECLARE @pt NVARCHAR(100) = CASE WHEN @i % 3 = 0 THEN N'Trả hàng' ELSE N'Xe tải' END;
    DECLARE @start DATETIME2 = DATEADD(HOUR, @i, SYSUTCDATETIME());
    DECLARE @end DATETIME2 = DATEADD(HOUR, @i + 5, SYSUTCDATETIME());
    DECLARE @khu NVARCHAR(50) = 'KV' + RIGHT('00' + CAST((@i % 10) AS VARCHAR(2)), 2);
    DECLARE @doanh DECIMAL(14,2) = 1000000 + (@i * 12345);
    INSERT INTO TuyenDuong (MaTuyen, MaTuyenCode, MaTaiXe, PhuongTien, ThoiGianBatDau, ThoiGianKetThuc, MaKhuVuc, DoanhThuUocTinh)
    VALUES (@maTD, @code, @maTaiXe, @pt, @start, @end, @khu, @doanh);
    SET @i += 1;
END
COMMIT;

-- 5) DonVanChuyen: 100 đơn
SET NOCOUNT ON;
BEGIN TRAN;
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @maDon NVARCHAR(20) = 'DVC' + RIGHT('000' + CAST(@i AS VARCHAR(3)), 3);
    DECLARE @maCode NVARCHAR(50) = 'DVCODE' + RIGHT('000' + CAST(@i AS VARCHAR(3)), 3);
    DECLARE @maVanDon NVARCHAR(100) = NULL; -- sẽ gán khi tạo VanDon
    DECLARE @maGui NVARCHAR(20) = 'KH' + RIGHT('000' + CAST(@i AS VARCHAR(3)), 3);
    DECLARE @maNhan NVARCHAR(20) = 'KH' + RIGHT('000' + CAST(((@i + 1 - 1) % 100 + 1) AS VARCHAR(3)), 3); -- nhận từ next KH (wrap)
    DECLARE @maDiaLay NVARCHAR(20) = 'DC' + RIGHT('000' + CAST(@i AS VARCHAR(3)), 3);
    DECLARE @maDiaGiao NVARCHAR(20) = 'DC' + RIGHT('000' + CAST(((@i % 100) + 1) AS VARCHAR(3)), 3);
    DECLARE @loai NVARCHAR(50) = CASE WHEN @i % 4 = 0 THEN N'Hàng dễ vỡ' ELSE N'Hàng thường' END;
    DECLARE @kl DECIMAL(8,3) = 0.5 + (@i * 0.1);
    DECLARE @gia DECIMAL(14,2) = 100000 + (@i * 1000);
    DECLARE @nguoiTao NVARCHAR(20) = 'ND' + RIGHT('000' + CAST(((@i % 100) + 1) AS VARCHAR(3)), 3);
    DECLARE @maTuyen NVARCHAR(20) = 'TD' + RIGHT('000' + CAST((( @i % 100) + 1) AS VARCHAR(3)), 3);
    INSERT INTO DonVanChuyen (MaDon, MaDonCode, MaVanDon, MaKhachGui, MaKhachNhan, MaDiaChiLay, MaDiaChiGiao, LoaiHang, KhoiLuong, GiaTriKhaiBao, NguoiTao, MaTuyen, TrangThai)
    VALUES (@maDon, @maCode, @maVanDon, @maGui, @maNhan, @maDiaLay, @maDiaGiao, @loai, @kl, @gia, @nguoiTao, @maTuyen, N'Khởi tạo');
    SET @i += 1;
END
COMMIT;

-- 6) VanDon: 100 vận đơn gắn với DonVanChuyen
SET NOCOUNT ON;
BEGIN TRAN;
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @maVD NVARCHAR(20) = 'VD' + RIGHT('000' + CAST(@i AS VARCHAR(3)), 3);
    DECLARE @soVD NVARCHAR(100) = 'SVD' + RIGHT('000' + CAST(@i AS VARCHAR(3)), 3);
    DECLARE @maDon NVARCHAR(20) = 'DVC' + RIGHT('000' + CAST(@i AS VARCHAR(3)), 3);
    DECLARE @ngay DATETIME2 = DATEADD(MINUTE, @i * 10, SYSUTCDATETIME());
    DECLARE @tt NVARCHAR(MAX) = N'Nhà xe ' + CAST((@i % 10) + 1 AS NVARCHAR(5));
    INSERT INTO VanDon (MaVanDon, SoVanDon, MaDon, NgayPhatHanh, ThongTinNhaXe)
    VALUES (@maVD, @soVD, @maDon, @ngay, @tt);
    -- Cập nhật DonVanChuyen.MaVanDon nếu cần (nếu yêu cầu)
    UPDATE DonVanChuyen SET MaVanDon = @maVD WHERE MaDon = @maDon;
    SET @i += 1;
END
COMMIT;

-- 7) DiemDung: 100 điểm dừng
SET NOCOUNT ON;
BEGIN TRAN;
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @maDD NVARCHAR(20) = 'DD' + RIGHT('000' + CAST(@i AS VARCHAR(3)), 3);
    DECLARE @maTuyen NVARCHAR(20) = 'TD' + RIGHT('000' + CAST(((@i % 100) + 1) AS VARCHAR(3)), 3);
    DECLARE @thuTu INT = (@i % 10) + 1;
    DECLARE @maDon NVARCHAR(20) = 'DVC' + RIGHT('000' + CAST(((@i % 100) + 1) AS VARCHAR(3)), 3);
    DECLARE @duKien DATETIME2 = DATEADD(MINUTE, @i * 15, SYSUTCDATETIME());
    DECLARE @thucTe DATETIME2 = DATEADD(MINUTE, @i * 15 + 5, SYSUTCDATETIME());
    INSERT INTO DiemDung (MaDiemDung, MaTuyen, ThuTuDung, MaDon, DuKienDen, ThucTeDen)
    VALUES (@maDD, @maTuyen, @thuTu, @maDon, @duKien, @thucTe);
    SET @i += 1;
END
COMMIT;

-- 8) SuKienTrangThai: 100 sự kiện trạng thái
SET NOCOUNT ON;
BEGIN TRAN;
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @maDon NVARCHAR(20) = 'DVC' + RIGHT('000' + CAST(((@i % 100) + 1) AS VARCHAR(3)), 3);
    DECLARE @trangThai NVARCHAR(60) = CASE WHEN @i % 4 = 0 THEN N'Đã lấy'
                                          WHEN @i % 4 = 1 THEN N'Đang giao'
                                          WHEN @i % 4 = 2 THEN N'Đã giao'
                                          ELSE N'Thất bại' END;
    DECLARE @lyDo NVARCHAR(MAX) = CASE WHEN @trangThai = N'Thất bại' THEN N'Khách vắng nhà' ELSE NULL END;
    DECLARE @nguoi NVARCHAR(20) = 'ND' + RIGHT('000' + CAST((( (@i * 3) % 100) + 1) AS VARCHAR(3)), 3);
    DECLARE @duLieu NVARCHAR(MAX) = N'{"note":"sự kiện ' + CAST(@i AS NVARCHAR(10)) + N'"}';
    DECLARE @ext NVARCHAR(200) = 'EXT' + RIGHT('000' + CAST(@i AS VARCHAR(3)), 3);
    DECLARE @idempot NVARCHAR(200) = NULL; IF (@i % 10 = 0) SET @idempot = 'IDEMP' + RIGHT('000' + CAST(@i AS VARCHAR(3)), 3);
    INSERT INTO SuKienTrangThai (MaDon, TrangThai, LyDo, ThoiGian, NguoiCapNhat, DuLieuThem, MaSuKienNgoai, KhoaIdempotent)
    VALUES (@maDon, @trangThai, @lyDo, DATEADD(MINUTE, @i * 7, SYSUTCDATETIME()), @nguoi, @duLieu, @ext, @idempot);
    SET @i += 1;
END
COMMIT;

-- 9) GiaoDichCOD: 100 giao dịch COD
SET NOCOUNT ON;
BEGIN TRAN;
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @maDon NVARCHAR(20) = 'DVC' + RIGHT('000' + CAST(((@i % 100) + 1) AS VARCHAR(3)), 3);
    DECLARE @soTien DECIMAL(14,2) = 50000 + (@i * 1000);
    DECLARE @nguoiThu NVARCHAR(20) = 'ND' + RIGHT('000' + CAST((( @i * 7) % 100 + 1) AS VARCHAR(3)), 3);
    DECLARE @ngayThu DATETIME2 = DATEADD(MINUTE, @i * 12, SYSUTCDATETIME());
    DECLARE @daDoiSoat BIT = CASE WHEN @i % 6 = 0 THEN 1 ELSE 0 END;
    DECLARE @ngayDoi DATETIME2 = CASE WHEN @daDoiSoat = 1 THEN DATEADD(DAY,1,@ngayThu) ELSE NULL END;
    DECLARE @soTienTT DECIMAL(14,2) = @soTien * 0.95;
    DECLARE @duLieu NVARCHAR(MAX) = N'{"info":"cod ' + CAST(@i AS NVARCHAR(10)) + N'"}';
    INSERT INTO GiaoDichCOD (MaDon, SoTien, NguoiThu, NgayThu, DaDoiSoat, NgayDoiSoat, SoTienThanhToan, DuLieuThem)
    VALUES (@maDon, @soTien, @nguoiThu, @ngayThu, @daDoiSoat, @ngayDoi, @soTienTT, @duLieu);
    SET @i += 1;
END
COMMIT;

-- 10) ChungTu: 100 chứng từ
SET NOCOUNT ON;
BEGIN TRAN;
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @maDon NVARCHAR(20) = 'DVC' + RIGHT('000' + CAST(((@i % 100) + 1) AS VARCHAR(3)), 3);
    DECLARE @nguoi NVARCHAR(20) = 'ND' + RIGHT('000' + CAST((( @i * 5) % 100 + 1) AS VARCHAR(3)), 3);
    DECLARE @ky NVARCHAR(MAX) = N'Chứng từ ' + CAST(@i AS NVARCHAR(10));
    DECLARE @thumb NVARCHAR(MAX) = '/uploads/thumb' + RIGHT('000' + CAST(@i AS VARCHAR(3)), 3) + '.jpg';
    DECLARE @loai NVARCHAR(50) = CASE WHEN @i % 3 = 0 THEN N'Ảnh' ELSE N'ChuKy' END;
    INSERT INTO ChungTu (MaDon, NguoiUpload, KyNhan, DuongDanThuNho, LoaiKyNhan)
    VALUES (@maDon, @nguoi, @ky, @thumb, @loai);
    SET @i += 1;
END
COMMIT;






