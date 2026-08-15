-- Tạo CSDL 
CREATE DATABASE PolyCafe;
GO
USE PolyCafe;
GO

--Bảng Khách Hàng
CREATE TABLE KhachHang(
MaKhachHang NVARCHAR(50) PRIMARY KEY,
HoTen NVARCHAR(50),
Email NVARCHAR(50),
SoDienThoai NVARCHAR(50),
MatKhau NVARCHAR(50),
DiaChi NVARCHAR(50),
TrangThai BIT
);
GO

-- Bảng Thẻ Lưu Động
CREATE TABLE TheLuuDong (
    MaThe CHAR(6) PRIMARY KEY,  
    ChuSoHuu NVARCHAR(100) NOT NULL,  
    TrangThai BIT NOT NULL DEFAULT 1  
);
GO

-- Bảng Nhân Viên
CREATE TABLE NhanVien (
    MaNhanVien CHAR(6) PRIMARY KEY, 
    HoTen NVARCHAR(100) NOT NULL, 
    Email NVARCHAR(255) NOT NULL UNIQUE,  
    MatKhau NVARCHAR(255) NOT NULL,
    VaiTro BIT NOT NULL,  
    TrangThai BIT NOT NULL DEFAULT 1  
);
GO

-- Bảng Loại Sản Phẩm
CREATE TABLE LoaiSanPham (
    MaLoai CHAR(6) PRIMARY KEY, 
    TenLoai NVARCHAR(100) NOT NULL UNIQUE,  
    GhiChu NVARCHAR(MAX) NULL  
);
GO

-- Bảng Sản Phẩm
CREATE TABLE SanPham (
    MaSanPham CHAR(6) PRIMARY KEY,
    TenSanPham NVARCHAR(100) NOT NULL, 
    DonGia DECIMAL(10,0) NOT NULL CHECK (DonGia >= 0),  
    MaLoai CHAR(6) NOT NULL, 
    HinhAnh NVARCHAR(MAX) NULL, 
    TrangThai BIT NOT NULL DEFAULT 1,  
    CONSTRAINT FK_SanPham_Loai FOREIGN KEY (MaLoai) REFERENCES LoaiSanPham(MaLoai) 
        ON DELETE CASCADE ON UPDATE CASCADE
);
GO

-- Bảng Phiếu Bán Hàng
CREATE TABLE PhieuBanHang (
    MaPhieu CHAR(6) PRIMARY KEY, 
	MaKhachHang NVARCHAR(50) NULL,
    MaThe CHAR(6) NULL,  
    MaNhanVien CHAR(6) NULL,  
    NgayTao DATETIME NOT NULL DEFAULT GETDATE(),  
    TrangThai BIT NOT NULL DEFAULT 0,  
    CONSTRAINT FK_PhieuBanHang_The FOREIGN KEY (MaThe) REFERENCES TheLuuDong(MaThe) 
        ON DELETE SET NULL ON UPDATE CASCADE,
    CONSTRAINT FK_PhieuBanHang_NhanVien FOREIGN KEY (MaNhanVien) REFERENCES NhanVien(MaNhanVien) 
        ON DELETE SET NULL ON UPDATE CASCADE,
	CONSTRAINT FK_PhieuBanHang_KhachHang   -- THÊM KHÓA NGOẠI
        FOREIGN KEY (MaKhachHang)
        REFERENCES KhachHang(MaKhachHang)
        ON DELETE SET NULL ON UPDATE CASCADE
);
GO

-- Bảng Chi Tiết Phiếu
CREATE TABLE ChiTietPhieu (
    Id INT IDENTITY(1,1) PRIMARY KEY,  
    MaPhieu CHAR(6) NOT NULL,  
    MaSanPham CHAR(6) NOT NULL,  
    SoLuong INT NOT NULL CHECK (SoLuong > 0),  
    DonGia DECIMAL(10,0) NOT NULL CHECK (DonGia >= 0),  
    CONSTRAINT FK_ChiTietPhieu_Phieu FOREIGN KEY (MaPhieu) REFERENCES PhieuBanHang(MaPhieu) 
        ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT FK_ChiTietPhieu_SanPham FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSanPham) 
        ON DELETE CASCADE ON UPDATE CASCADE
);
GO

--Thêm dữ liệu khách hàng
INSERT INTO KhachHang (MaKhachHang, HoTen, Email, SoDienThoai, MatKhau, DiaChi,TrangThai)
VALUES 
('KH001', N'Nguyễn Văn An', 'an.nguyen@email.com', '0912345678', 'pass123', N'123 Nguyễn Huệ, Quận 1, TP.HCM',1),
('KH002', N'Trần Thị Bình', 'binh.tran@email.com', '0987654321', 'binh456', N'456 Lê Lợi, Quận 3, TP.HCM',1),
('KH003', N'Lê Văn Cường', 'cuong.le@email.com', '0909090909', 'cuong789', N'789 Hai Bà Trưng, Quận Tân Bình, TP.HCM', 0),
('KH004', N'Phạm Thị Dung', 'dung.pham@email.com', '0978123456', 'dung000', N'321 Điện Biên Phủ, Quận Bình Thạnh, TP.HCM', 1),
('KH005', N'Hoàng Minh Đức', 'duc.hoang@email.com', '0933222111', 'duc111', N'654 Cách Mạng Tháng 8, Quận 10, TP.HCM',1);

-- Thêm dữ liệu vào bảng TheLuuDong
INSERT INTO TheLuuDong (MaThe, ChuSoHuu, TrangThai) VALUES
('THE001', N'Nguyễn Văn An', 1),
('THE002', N'Trần Thị Bình', 1),
('THE003', N'Phạm Minh Châu', 1),
('THE004', N'Đỗ Thành Công', 1),
('THE005', N'Lê Hải Đăng', 0);
GO

-- Thêm dữ liệu vào bảng NhanVien
INSERT INTO NhanVien (MaNhanVien, HoTen, Email, MatKhau, VaiTro, TrangThai) VALUES
('NV001', N'Nguyễn Thị Hoa', 'hoa.nguyen@cafe.com', 'password1', 1, 1),
('NV002', N'Trần Văn Minh', 'minh.tran@cafe.com', 'password2', 0, 1),
('NV003', N'Hoàng Thị Lan', 'lan.hoang@cafe.com', 'password3', 0, 1),
('NV004', N'Phạm Tuấn Kiệt', 'kiet.pham@cafe.com', 'password4', 0, 1),
('NV005', N'Lê Quốc Bảo', 'bao.le@cafe.com', 'password5', 0, 1);
GO

-- Thêm dữ liệu vào bảng LoaiSanPham
INSERT INTO LoaiSanPham (MaLoai, TenLoai, GhiChu) VALUES
('LSP001', N'Cà phê', N'Các loại cà phê nguyên chất và pha chế'),
('LSP002', N'Trà', N'Các loại trà thảo mộc và trái cây'),
('LSP003', N'Bánh ngọt', N'Bánh ngọt ăn kèm với đồ uống'),
('LSP004', N'Nước ép', N'Nước ép trái cây tươi'),
('LSP005', N'Sinh tố', N'Các loại sinh tố hoa quả');
GO

-- Thêm dữ liệu vào bảng SanPham
INSERT INTO SanPham (MaSanPham, TenSanPham, DonGia, MaLoai, HinhAnh, TrangThai) VALUES
('SP001', N'Cà phê đen', 25000, 'LSP001', 'caphe_den.jpg', 1),
('SP002', N'Cà phê sữa', 30000, 'LSP001', 'caphe_sua.jpg', 1),
('SP003', N'Trà đào cam sả', 35000, 'LSP002', 'tra_dao_cam_sa.jpg', 1),
('SP004', N'Bánh Tiramisu', 40000, 'LSP003', 'banh_tiramisu.jpg', 1),
('SP005', N'Nước ép cam', 45000, 'LSP004', 'nuoc_ep_cam.jpg', 1);
GO

-- Thêm dữ liệu vào bảng PhieuBanHang
INSERT INTO PhieuBanHang (MaPhieu,MaKhachHang, MaThe, MaNhanVien, NgayTao, TrangThai) VALUES
('PBH001','KH001', 'THE001', 'NV002', '2024-03-01 08:00:00', 1),
('PBH002','KH002', 'THE002', 'NV003', '2024-03-01 09:30:00', 1),
('PBH003', 'KH003','THE003', 'NV004', '2024-03-01 10:15:00', 1),
('PBH004', 'KH004','THE004', 'NV005', '2024-03-01 11:00:00', 1),
('PBH005', 'KH005','THE005', 'NV002', '2024-03-01 12:45:00', 1);
GO

-- Thêm dữ liệu vào bảng ChiTietPhieu
INSERT INTO ChiTietPhieu (MaPhieu, MaSanPham, SoLuong, DonGia) VALUES
('PBH001', 'SP001', 2, 25000),
('PBH001', 'SP003', 1, 35000),
('PBH002', 'SP002', 1, 30000),
('PBH002', 'SP005', 1, 45000),
('PBH003', 'SP004', 2, 40000),
('PBH003', 'SP001', 1, 25000);
GO
