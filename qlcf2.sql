CREATE DATABASE btlquancaphe2;
GO
USE btlquancaphe2;
GO



CREATE TABLE vai_tro (
  ma_vt TINYINT IDENTITY(1,1) PRIMARY KEY,
  ten_vt NVARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE nguoi_dung (
  ma_nd INT IDENTITY(1,1) PRIMARY KEY,
  ho_ten NVARCHAR(100) NOT NULL,
  tai_khoan NVARCHAR(50) NOT NULL UNIQUE,
  mat_khau VARBINARY(MAX) NOT NULL,
  ma_vt TINYINT NOT NULL,
  hoat_dong BIT DEFAULT 1,
  tao_luc DATETIME DEFAULT GETDATE(),
  FOREIGN KEY (ma_vt) REFERENCES vai_tro(ma_vt)
);

CREATE TABLE khu_vuc (
  ma_khu INT IDENTITY(1,1) PRIMARY KEY,
  ten_khu NVARCHAR(100) NOT NULL,
  thu_tu INT
);

CREATE TABLE ban (
    ma_ban INT PRIMARY KEY,       
    ma_khu INT NOT NULL,
    ky_hieu NVARCHAR(20),
    suc_chua INT,
    trang_thai NVARCHAR(30),
    FOREIGN KEY (ma_khu) REFERENCES khu_vuc(ma_khu)
);
select * from ban

CREATE TABLE mon (
  ma_mon INT IDENTITY(1,1) PRIMARY KEY,
  ten_mon NVARCHAR(100) NOT NULL,
  loai_mon NVARCHAR(50),
  dang_ban BIT DEFAULT 1
);

CREATE TABLE size_mon (
  ma_size INT IDENTITY(1,1) PRIMARY KEY,
  ma_mon INT NOT NULL,
  ten_size NVARCHAR(20),
  gia DECIMAL(18,2),
  FOREIGN KEY (ma_mon) REFERENCES mon(ma_mon)
);

CREATE TABLE topping (
  ma_topping INT IDENTITY(1,1) PRIMARY KEY,
  ten_topping NVARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE lua_chon_topping (
  ma_lua_chon INT IDENTITY(1,1) PRIMARY KEY,
  ma_topping INT NOT NULL,
  ten_lua_chon NVARCHAR(100),
  gia_them DECIMAL(18,2),
  FOREIGN KEY (ma_topping) REFERENCES topping(ma_topping)
);

CREATE TABLE mon_topping (
  ma_mon INT NOT NULL,
  ma_topping INT NOT NULL,
  PRIMARY KEY (ma_mon, ma_topping),
  FOREIGN KEY (ma_mon) REFERENCES mon(ma_mon),
  FOREIGN KEY (ma_topping) REFERENCES topping(ma_topping)
);

CREATE TABLE don_hang (
  ma_dh INT IDENTITY(1,1) PRIMARY KEY,
  ma_ban INT NULL,
  loai_dh NVARCHAR(30),
  trang_thai NVARCHAR(30),
  mo_boi INT,
  mo_luc DATETIME DEFAULT GETDATE(),
  FOREIGN KEY (ma_ban) REFERENCES ban(ma_ban),
  FOREIGN KEY (mo_boi) REFERENCES nguoi_dung(ma_nd)
);

CREATE TABLE chi_tiet_don (
  ma_ct INT IDENTITY(1,1) PRIMARY KEY,
  ma_dh INT NOT NULL,
  ma_mon INT NOT NULL,
  ma_size INT NOT NULL,
  so_luong INT DEFAULT 1,
  don_gia DECIMAL(18,2),
  da_gui_phache BIT DEFAULT 0,
  FOREIGN KEY (ma_dh) REFERENCES don_hang(ma_dh),
  FOREIGN KEY (ma_mon) REFERENCES mon(ma_mon),
  FOREIGN KEY (ma_size) REFERENCES size_mon(ma_size)
);

CREATE TABLE don_topping (
  ma_ct INT NOT NULL,
  ma_lua_chon INT NOT NULL,
  gia_them DECIMAL(18,2),
  PRIMARY KEY (ma_ct, ma_lua_chon),
  FOREIGN KEY (ma_ct) REFERENCES chi_tiet_don(ma_ct),
  FOREIGN KEY (ma_lua_chon) REFERENCES lua_chon_topping(ma_lua_chon)
);

CREATE TABLE phieu_phache (
  ma_phieu INT IDENTITY(1,1) PRIMARY KEY,
  ma_dh INT NOT NULL,
  trang_thai NVARCHAR(30),
  tao_luc DATETIME DEFAULT GETDATE(),
  tao_boi INT,
  FOREIGN KEY (ma_dh) REFERENCES don_hang(ma_dh),
  FOREIGN KEY (tao_boi) REFERENCES nguoi_dung(ma_nd)
);

CREATE TABLE chi_tiet_phieu_phache (
  ma_phieu INT NOT NULL,
  ma_ct INT NOT NULL,
  so_luong INT,
  trang_thai NVARCHAR(30),
  PRIMARY KEY (ma_phieu, ma_ct),
  FOREIGN KEY (ma_phieu) REFERENCES phieu_phache(ma_phieu),
  FOREIGN KEY (ma_ct) REFERENCES chi_tiet_don(ma_ct)
);

CREATE TABLE hoa_don (
  ma_hd INT IDENTITY(1,1) PRIMARY KEY,
  ma_dh INT NOT NULL,
  trang_thai NVARCHAR(30),
  tam_tinh DECIMAL(18,2),
  giam_gia DECIMAL(18,2),
  thue DECIMAL(18,2),
  tao_luc DATETIME DEFAULT GETDATE(),
  FOREIGN KEY (ma_dh) REFERENCES don_hang(ma_dh)
);

CREATE TABLE chi_tiet_hoa_don (
  ma_hd INT NOT NULL,
  ma_ct INT NOT NULL,
  don_gia DECIMAL(18,2),
  PRIMARY KEY (ma_hd, ma_ct),
  FOREIGN KEY (ma_hd) REFERENCES hoa_don(ma_hd),
  FOREIGN KEY (ma_ct) REFERENCES chi_tiet_don(ma_ct)
);

CREATE TABLE thanh_toan (
  ma_tt INT IDENTITY(1,1) PRIMARY KEY,
  ma_hd INT NOT NULL,
  phuong_thuc NVARCHAR(50),
  so_tien DECIMAL(18,2),
  thu_boi INT,
  tra_luc DATETIME DEFAULT GETDATE(),
  FOREIGN KEY (ma_hd) REFERENCES hoa_don(ma_hd),
  FOREIGN KEY (thu_boi) REFERENCES nguoi_dung(ma_nd)
);

CREATE TABLE nguyen_lieu (
  ma_nl INT IDENTITY(1,1) PRIMARY KEY,
  ten_nl NVARCHAR(100),
  don_vi NVARCHAR(20),
  ton_hien_tai DECIMAL(18,2),
  ton_toi_thieu DECIMAL(18,2),
  trang_thai NVARCHAR(30)
);

CREATE TABLE cong_thuc (
  ma_cong_thuc INT IDENTITY(1,1) PRIMARY KEY,
  ma_mon INT,
  ma_size INT,
  FOREIGN KEY (ma_mon) REFERENCES mon(ma_mon),
  FOREIGN KEY (ma_size) REFERENCES size_mon(ma_size)
);

CREATE TABLE ct_nguyen_lieu (
  ma_ctnl INT IDENTITY(1,1) PRIMARY KEY,
  ma_cong_thuc INT,
  ma_nl INT,
  dinh_muc DECIMAL(18,2),
  FOREIGN KEY (ma_cong_thuc) REFERENCES cong_thuc(ma_cong_thuc),
  FOREIGN KEY (ma_nl) REFERENCES nguyen_lieu(ma_nl)
);

CREATE TABLE phieu_xuat (
  ma_px INT IDENTITY(1,1) PRIMARY KEY,
  ma_ct INT,
  ma_nl INT,
  so_luong DECIMAL(18,2),
  FOREIGN KEY (ma_ct) REFERENCES chi_tiet_don(ma_ct),
  FOREIGN KEY (ma_nl) REFERENCES nguyen_lieu(ma_nl)
);

CREATE TABLE lich_su_kho (
  ma_ls INT IDENTITY(1,1) PRIMARY KEY,
  ma_nl INT,
  loai NVARCHAR(20),
  so_luong DECIMAL(18,2),
  ref_loai NVARCHAR(50),
  ref_id INT,
  tao_boi INT,
  FOREIGN KEY (ma_nl) REFERENCES nguyen_lieu(ma_nl),
  FOREIGN KEY (tao_boi) REFERENCES nguoi_dung(ma_nd)
);

CREATE TABLE khuyen_mai (
  ma_km INT IDENTITY(1,1) PRIMARY KEY,
  ten_km NVARCHAR(100),
  loai NVARCHAR(20),
  gia_tri DECIMAL(18,2),
  bat_dau DATE,
  ket_thuc DATE,
  active BIT
);

CREATE TABLE ct_khuyen_mai (
  ma_ctkm INT IDENTITY(1,1) PRIMARY KEY,
  ma_km INT,
  ma_mon INT NULL,
  dieu_kien_tien DECIMAL(18,2) NULL,
  so_luong_toi_thieu INT NULL,
  la_combo BIT,
  FOREIGN KEY (ma_km) REFERENCES khuyen_mai(ma_km),
  FOREIGN KEY (ma_mon) REFERENCES mon(ma_mon)
);

CREATE TABLE ca_lam (
  ma_ca INT IDENTITY(1,1) PRIMARY KEY,
  mo_boi INT,
  mo_luc DATETIME DEFAULT GETDATE(),
  dong_boi INT NULL,
  dong_luc DATETIME NULL,
  FOREIGN KEY (mo_boi) REFERENCES nguoi_dung(ma_nd),
  FOREIGN KEY (dong_boi) REFERENCES nguoi_dung(ma_nd)
);
GO
-- Vai trò
INSERT INTO vai_tro(ten_vt)
VALUES (N'Admin'), (N'Thu ngân'), (N'Phục vụ'), (N'Pha chế');
GO

-- Người dùng
INSERT INTO nguoi_dung(ho_ten, tai_khoan, mat_khau, ma_vt, hoat_dong, tao_luc)
VALUES
(N'Nguyễn Văn A', 'admin', 0x1234, 1, 1, GETDATE()),
(N'Trần Thị B', 'thungan', 0x1234, 2, 1, GETDATE()),
(N'Lê Văn C', 'phucvu', 0x1234, 3, 1, GETDATE()),
(N'Phạm Thị D', 'phache', 0x1234, 4, 1, GETDATE());
GO

-- Khu vực
INSERT INTO khu_vuc(ten_khu, thu_tu)
VALUES (N'Tầng 1',1), (N'Tầng 2',2);
GO

-- Bàn
INSERT INTO ban(ma_khu, ky_hieu, suc_chua, trang_thai)
VALUES 
(1, N'B1', 2, N'Trống'),
(1, N'B2', 4, N'Trống'),
(2, N'T2-1', 2, N'Trống'),
(2, N'T2-2', 4, N'Trống');
GO

-- Món
INSERT INTO mon(ten_mon, loai_mon, dang_ban)
VALUES 
(N'Cà phê đen', N'Pha chế', 1),
(N'Cà phê sữa', N'Pha chế', 1),
(N'Trà đào cam sả', N'Pha chế', 1),
(N'Latte', N'Pha chế', 1),
(N'Bánh ngọt', N'Đồ ăn', 1);
GO

-- Size món
INSERT INTO size_mon(ma_mon, ten_size, gia)
VALUES
(1, N'S', 20000),(1, N'M', 25000),(1, N'L', 30000),
(2, N'S', 25000),(2, N'M', 30000),(2, N'L', 35000),
(3, N'M', 35000),(3, N'L', 40000),
(4, N'M', 40000),(4, N'L', 45000);
GO

-- Topping
INSERT INTO topping(ten_topping)
VALUES (N'Trân châu'),(N'Thạch đào');
GO

-- Lựa chọn topping
INSERT INTO lua_chon_topping(ma_topping, ten_lua_chon, gia_them)
VALUES 
(1, N'Trân châu đen', 5000),
(1, N'Trân châu trắng', 6000),
(2, N'Thạch đào miếng', 7000);
GO

-- Món – topping
INSERT INTO mon_topping(ma_mon, ma_topping)
VALUES (3,1),(3,2),(4,1);
GO

-- Đơn hàng
INSERT INTO don_hang(ma_ban, loai_dh, trang_thai, mo_boi, mo_luc)
VALUES 
(1, N'Tại bàn', N'Mở', 3, GETDATE()),
(2, N'Tại bàn', N'Mở', 3, GETDATE()),
(NULL, N'Mang đi', N'Mở', 3, GETDATE());
GO

-- Chi tiết đơn
INSERT INTO chi_tiet_don(ma_dh, ma_mon, ma_size, so_luong, don_gia, da_gui_phache)
VALUES
(1, 1, 2, 2, 25000, 1),
(1, 3, 8, 1, 40000, 1),
(2, 4, 9, 1, 45000, 0),
(3, 2, 5, 1, 30000, 0);
GO

-- Đơn topping
INSERT INTO don_topping(ma_ct, ma_lua_chon, gia_them)
VALUES 
(2, 1, 5000),
(2, 2, 6000);
GO

-- Phiếu pha chế
INSERT INTO phieu_phache(ma_dh, trang_thai, tao_luc, tao_boi)
VALUES (1, N'Chờ', GETDATE(), 4),
(2, N'Đang làm', GETDATE(), 4);
GO

-- Chi tiết phiếu pha chế
INSERT INTO chi_tiet_phieu_phache(ma_phieu, ma_ct, so_luong, trang_thai)
VALUES
(1,1,2,N'Đang làm'),
(1,2,1,N'Chờ'),
(2,3,1,N'Đang làm');
GO

-- Hóa đơn
INSERT INTO hoa_don(ma_dh, trang_thai, tam_tinh, giam_gia, thue, tao_luc)
VALUES
(1, N'Đã trả', 70000, 0, 7000, GETDATE()),
(2, N'Chưa trả', 45000, 0, 4500, GETDATE()),
(3, N'Chưa trả', 30000, 0, 3000, GETDATE());
GO

-- Chi tiết hóa đơn
INSERT INTO chi_tiet_hoa_don(ma_hd, ma_ct, don_gia)
VALUES
(1,1,25000),
(1,2,40000),
(2,3,45000),
(3,4,30000);
GO

-- Thanh toán
INSERT INTO thanh_toan(ma_hd, phuong_thuc, so_tien, thu_boi, tra_luc)
VALUES
(1, N'Tiền mặt', 77000, 2, GETDATE()),
(2, N'Tiền mặt', 0, 2, GETDATE());
GO

-- Nguyên liệu
INSERT INTO nguyen_lieu(ten_nl, don_vi, ton_hien_tai, ton_toi_thieu, trang_thai)
VALUES
(N'Cà phê bột', N'gram', 5000, 500, N'Đang dùng'),
(N'Sữa đặc', N'ml', 2000, 300, N'Đang dùng'),
(N'Đường', N'gram', 3000, 300, N'Đang dùng'),
(N'Trà đào gói', N'gram', 4000, 500, N'Đang dùng'),
(N'Đào lát hộp', N'hộp', 20, 5, N'Đang dùng');
GO

-- Công thức
INSERT INTO cong_thuc(ma_mon, ma_size)
VALUES (1,2),(2,5),(3,8),(4,9);
GO

-- Chi tiết nguyên liệu
INSERT INTO ct_nguyen_lieu(ma_cong_thuc, ma_nl, dinh_muc)
VALUES
(1,1,15),(1,3,10),
(2,1,12),(2,2,50),
(3,4,20),(3,5,10),
(4,1,15),(4,2,40);
GO

-- Phiếu xuất
INSERT INTO phieu_xuat(ma_ct, ma_nl, so_luong)
VALUES 
(1,1,30),
(1,3,20);
GO

-- Lịch sử kho
INSERT INTO lich_su_kho(ma_nl, loai, so_luong, ref_loai, ref_id, tao_boi)
VALUES
(1,N'Xuất',-30,N'PHIEU_XUAT',1,1),
(3,N'Xuất',-20,N'PHIEU_XUAT',1,1);
GO

-- Khuyến mãi
INSERT INTO khuyen_mai(ten_km, loai, gia_tri, bat_dau, ket_thuc, active)
VALUES
(N'Giảm 10% hóa đơn >100k', N'Phần trăm', 10, '2025-10-01','2025-12-31',1),
(N'Giảm 15k cho Latte', N'Số tiền', 15000, '2025-10-01','2025-12-31',1);
GO

-- Chi tiết khuyến mãi
INSERT INTO ct_khuyen_mai(ma_km, ma_mon, dieu_kien_tien, so_luong_toi_thieu, la_combo)
VALUES
(1,NULL,100000,NULL,0),
(2,4,NULL,1,0);
GO

-- Ca làm
INSERT INTO ca_lam(mo_boi, mo_luc)
VALUES (2,DATEADD(HOUR,-3,GETDATE()));

INSERT INTO ca_lam(mo_boi, mo_luc, dong_boi, dong_luc)
VALUES (2,DATEADD(HOUR,-8,GETDATE()),2,DATEADD(HOUR,-1,GETDATE()));
GO
SELECT DB_NAME() AS current_db;
EXEC sp_columns chi_tiet_don;
EXEC sp_columns chi_tiet_hoa_don;
SELECT COLUMN_NAME, DATA_TYPE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'chi_tiet_don';

EXEC sp_columns chi_tiet_don;
EXEC sp_columns ct_khuyen_mai;
EXEC sp_columns don_hang;
ALTER TABLE don_hang ADD ghi_chu NVARCHAR(200) NULL;
ALTER TABLE don_hang ADD dong_luc DATETIME NULL;
EXEC sp_columns ca_lam;
ALTER TABLE ca_lam ADD 
    mo_boi INT NULL,
    mo_luc DATETIME NULL,
    dong_boi INT NULL,
    dong_luc DATETIME NULL;
	SELECT * FROM ca_lam;
INSERT INTO ca_lam (ten_ca, thoi_gian_bat_dau, thoi_gian_ket_thuc,
                    ma_nguoi_mo, ma_nguoi_dong, mo_boi, mo_luc, dong_boi, dong_luc)
VALUES
(N'Ca sáng', '07:00', '11:30', 1, 2, 1, GETDATE(), 2, DATEADD(HOUR, 4, GETDATE())),
(N'Ca trưa', '11:30', '16:00', 2, 2, 2, GETDATE(), 2, DATEADD(HOUR, 5, GETDATE())),
(N'Ca tối', '16:00', '22:00', 2, 2, 3, GETDATE(), 4, DATEADD(HOUR, 6, GETDATE()));
ALTER TABLE ca_lam
ALTER COLUMN thoi_gian_bat_dau DATETIME NULL;

select * from chi_tiet_don
ALTER COLUMN thoi_gian_ket_thuc DATETIME NULL;
sp_columns ca_lam
-- chi tiết đơn Không cho xóa, chỉ cập nhật trạng thái
ALTER TABLE chi_tiet_don ADD da_xoa BIT DEFAULT 0;
EXEC sp_columns chi_tiet_don;
UPDATE chi_tiet_don SET da_xoa = 1 WHERE ma_ct = 2;
SELECT * FROM chi_tiet_don WHERE da_xoa = 0;

SELECT * FROM ct_khuyen_mai WHERE ma_km = 6 AND ma_mon = 6;
-- km
INSERT INTO khuyen_mai (ten_km, loai, gia_tri, bat_dau, ket_thuc, active)
VALUES
(N'Giảm 20% cho Trà sữa Matcha', N'Phần trăm', 20, '2025-10-01', '2025-12-31', 1),
(N'Giảm 10k cho Cappuccino', N'Số tiền', 10000, '2025-10-01', '2025-12-31', 1),
(N'Mua 2 tặng 1 Soda Việt Quất', N'Combo', 0, '2025-10-01', '2025-12-31', 1);
GO


-- Giảm 10% cho Cà phê đen (ma_mon = 1)
INSERT INTO ct_khuyen_mai (ma_km, ma_mon, dieu_kien_tien, so_luong_toi_thieu, la_combo)
VALUES (1, 1, NULL, 1, 0);

-- Giảm 15% cho Cà phê sữa (ma_mon = 2)
INSERT INTO ct_khuyen_mai (ma_km, ma_mon, dieu_kien_tien, so_luong_toi_thieu, la_combo)
VALUES (1, 2, NULL, 1, 0);

-- Giảm 20% cho Trà đào cam sả (ma_mon = 3)
INSERT INTO ct_khuyen_mai (ma_km, ma_mon, dieu_kien_tien, so_luong_toi_thieu, la_combo)
VALUES (1, 3, NULL, 1, 0);

-- Giảm 10% cho hóa đơn từ 100.000đ trở lên
INSERT INTO ct_khuyen_mai (ma_km, ma_mon, dieu_kien_tien, so_luong_toi_thieu, la_combo)
VALUES (1, NULL, 100000, NULL, 0);

-- Giảm 20% cho hóa đơn từ 200.000đ trở lên
INSERT INTO ct_khuyen_mai (ma_km, ma_mon, dieu_kien_tien, so_luong_toi_thieu, la_combo)
VALUES (1, NULL, 200000, NULL, 0);


-- Mua 2 Latte (ma_mon = 4) giảm giá combo
INSERT INTO ct_khuyen_mai (ma_km, ma_mon, dieu_kien_tien, so_luong_toi_thieu, la_combo)
VALUES (2, 4, NULL, 2, 1);

-- Mua 3 Bánh ngọt (ma_mon = 5) được combo giảm giá
INSERT INTO ct_khuyen_mai (ma_km, ma_mon, dieu_kien_tien, so_luong_toi_thieu, la_combo)
VALUES (2, 5, NULL, 3, 1);

INSERT INTO ct_khuyen_mai(ma_km, ma_mon, dieu_kien_tien, so_luong_toi_thieu, la_combo)
VALUES (1, 2, 100000, 1, 0);

SELECT * FROM ct_khuyen_mai WHERE ma_km = 1 AND ma_mon = 2;

SELECT * FROM ct_nguyen_lieu WHERE ma_cong_thuc = 3 AND ma_nl = 5;

INSERT INTO phieu_xuat(ma_ct, ma_nl, so_luong)
VALUES 
(2,2,15), -- dùng sữa đặc
(3,4,20), -- trà đào cam sả
(4,1,10); -- cà phê sữa

INSERT INTO lich_su_kho(ma_nl, loai, so_luong, ref_loai, ref_id, tao_boi)
VALUES
(1, N'Nhập', 5000, N'KHOI_TAO', NULL, 1),
(2, N'Nhập', 2000, N'KHOI_TAO', NULL, 1),
(3, N'Nhập', 3000, N'KHOI_TAO', NULL, 1),
(4, N'Nhập', 4000, N'KHOI_TAO', NULL, 1),
(5, N'Nhập', 20, N'KHOI_TAO', NULL, 1);

UPDATE ca_lam SET
    thoi_gian_bat_dau = '07:00',
    thoi_gian_ket_thuc = '11:30'
WHERE ma_ca = 1;

UPDATE ca_lam SET
    thoi_gian_bat_dau = '11:30',
    thoi_gian_ket_thuc = '16:00'
WHERE ma_ca = 2;

UPDATE ca_lam SET
    thoi_gian_bat_dau = '16:00',
    thoi_gian_ket_thuc = '22:00'
WHERE ma_ca = 3;

SELECT * FROM vai_tro;
SELECT * FROM nguoi_dung;
SELECT * FROM khu_vuc;
SELECT * FROM ban;
SELECT * FROM mon;
SELECT * FROM size_mon;
SELECT * FROM topping;
SELECT * FROM lua_chon_topping;
SELECT * FROM mon_topping;
SELECT * FROM don_hang;
SELECT * FROM chi_tiet_don;
SELECT * FROM don_topping;
SELECT * FROM phieu_phache;
SELECT * FROM chi_tiet_phieu_phache;
SELECT * FROM hoa_don;
SELECT * FROM chi_tiet_hoa_don;
SELECT * FROM thanh_toan;
SELECT * FROM nguyen_lieu;
SELECT * FROM cong_thuc;
SELECT * FROM ct_nguyen_lieu;
SELECT * FROM phieu_xuat;
SELECT * FROM lich_su_kho;
SELECT * FROM khuyen_mai;
SELECT * FROM ct_khuyen_mai;
SELECT * FROM ca_lam;


DELETE FROM ct_khuyen_mai WHERE ma_km = 2;
DELETE FROM khuyen_mai WHERE ma_km = 2;

ALTER TABLE lich_su_kho
ADD tao_luc DATETIME DEFAULT GETDATE();


