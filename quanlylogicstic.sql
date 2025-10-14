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
    MaKhachHang NVARCHAR(100) FOREIGN KEY REFERENCES KhachHang(MaKhachHang),
    DiaChiChiTiet NVARCHAR(300),
    ThanhPho NVARCHAR(100),
    QuanHuyen NVARCHAR(100),
    MaBuuDien NVARCHAR(20)
);

CREATE TABLE DonVanChuyen (
    MaDon NVARCHAR(100) PRIMARY KEY,
    MaDonCode NVARCHAR(50) UNIQUE NOT NULL,
    MaVanDon NVARCHAR(100), -- FK tới VanDon
    MaKhachGui NVARCHAR(100) FOREIGN KEY REFERENCES KhachHang(MaKhachHang),
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
    MaDon NVARCHAR(100) UNIQUE FOREIGN KEY REFERENCES DonVanChuyen(MaDon),
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
    MaDiemDung INT PRIMARY KEY,
    MaTuyen NVARCHAR(100) FOREIGN KEY REFERENCES TuyenDuong(MaTuyen) ON DELETE CASCADE,
    ThuTuDung INT NOT NULL,
    MaDon NVARCHAR(100) FOREIGN KEY REFERENCES DonVanChuyen(MaDon),
    DuKienDen DATETIME2,
    ThucTeDen DATETIME2
);

CREATE UNIQUE INDEX UX_DiemDung_ThuTu ON DiemDung(MaTuyen, ThuTuDung);

CREATE TABLE SuKienTrangThai (
    MaSuKien BIGINT PRIMARY KEY,
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
    MaGiaoDich BIGINT PRIMARY KEY,
    MaDon NVARCHAR(100) FOREIGN KEY REFERENCES DonVanChuyen(MaDon),
    SoTien DECIMAL(14,2) NOT NULL,
    NguoiThu NVARCHAR(100) FOREIGN KEY REFERENCES NguoiDung(MaNguoiDung),
    NgayThu DATETIME2,
    DaDoiSoat BIT DEFAULT 0,
    NgayDoiSoat DATETIME2,
    SoTienThanhToan DECIMAL(14,2), -- số tiền trả lại cho người gửi sau khi trừ phí
    DuLieuThem NVARCHAR(MAX)
);

CREATE TABLE ChungTu (
    MaChungTu BIGINT PRIMARY KEY,
    MaDon NVARCHAR(100) FOREIGN KEY REFERENCES DonVanChuyen(MaDon) ON DELETE CASCADE,
    NguoiUpload NVARCHAR(100) FOREIGN KEY REFERENCES NguoiDung(MaNguoiDung),
    NgayUpload DATETIME2 DEFAULT SYSDATETIME(),
    KyNhan NVARCHAR(MAX) NOT NULL,
    DuongDanThuNho NVARCHAR(MAX),
    LoaiKyNhan NVARCHAR(50) -- 'Anh','ChuKy'
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





