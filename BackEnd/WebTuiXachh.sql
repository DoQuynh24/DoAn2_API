CREATE DATABASE WebTuiXachh
USE WebTuiXachh
GO

CREATE TABLE Users
(
    PerID INT IDENTITY(1,1) PRIMARY KEY, 
    TaiKhoan NVARCHAR(50) NOT NULL,
    MatKhau NVARCHAR(256) NOT NULL, 
    HoTen NVARCHAR(100) NULL,
	NgaySinh DATE ,
	GioiTinh NVARCHAR(20) NULL DEFAULT N'Chưa cập nhật',
    DiaChi NVARCHAR(255) NULL DEFAULT N'Chưa cập nhật',
    Role NVARCHAR(50) NOT NULL  DEFAULT 'Khách hàng'
	CONSTRAINT UQ_TaiKhoan UNIQUE (TaiKhoan) 
);
EXEC sp_user_search 
    @page_index = 1, 
    @page_size = 6, 
	@taikhoan ='',
    @hoten = N'Đỗ Quỳnh', 
    @diachi = '';


INSERT INTO Users (TaiKhoan, MatKhau, HoTen, NgaySinh, GioiTinh, DiaChi, Role) 
	VALUES ('0364554001', '12345', N'Đỗ Quỳnh','2004-06-02',N'Nữ', 'Hưng Yên', 'Admin');
INSERT INTO Users(TaiKhoan, MatKhau, HoTen) 
	VALUES ('093273733', 'abcd ', N'Thu Trang ');
	DELETE FROM Users
		WHERE PerID = 9;
	select* from Users
--thêm ng dùng
CREATE PROCEDURE sp_user_create
    @taikhoan NVARCHAR(50), 
    @matkhau NVARCHAR(256),
    @hoten NVARCHAR(100) = NULL,
    @ngaysinh DATE = NULL,  
    @gioitinh NVARCHAR(20) = NULL,
    @diachi NVARCHAR(255) = NULL,
    @role NVARCHAR(50) = NULL
AS
BEGIN
    BEGIN TRY
        IF @role IS NULL OR LTRIM(RTRIM(@role)) = ''
        BEGIN
            SET @role = N'Khách hàng';
        END

        IF @hoten IS NULL OR LTRIM(RTRIM(@hoten)) = ''
        BEGIN
            SET @hoten = N'Chưa cập nhật';
        END
        IF @gioitinh IS NULL OR LTRIM(RTRIM(@gioitinh)) = ''
        BEGIN
            SET @gioitinh = N'Chưa cập nhật';
        END
        IF @diachi IS NULL OR LTRIM(RTRIM(@diachi)) = ''
        BEGIN
            SET @diachi = N'Chưa cập nhật';
        END
        INSERT INTO Users (TaiKhoan, MatKhau, HoTen, NgaySinh, GioiTinh, DiaChi, Role)
        VALUES (@taikhoan, @matkhau, @hoten, @ngaysinh, @gioitinh, @diachi, @role);

        SELECT SCOPE_IDENTITY() AS NewPerID;
    END TRY
    BEGIN CATCH

        IF ERROR_NUMBER() = 2627 
        BEGIN
            RAISERROR (N'Tài khoản đã tồn tại', 16, 1);
            RETURN;
        END
        ELSE
        BEGIN
            THROW;
        END
    END CATCH
END;
	--cập nhật ng dùng
	CREATE PROCEDURE [dbo].[sp_user_update]
		@per_id INT,                     
		@taikhoan NVARCHAR(50),         
		@matkhau NVARCHAR(256) = NULL,   
		@hoten NVARCHAR(100) = NULL,     
		@ngaysinh DATE = NULL,          
		@gioitinh NVARCHAR(20) = NULL,   
		@diachi NVARCHAR(255) = NULL,   
		@role NVARCHAR(50) = NULL           
	AS
	BEGIN

		UPDATE Users
		SET TaiKhoan = COALESCE(@taikhoan, TaiKhoan),   
			MatKhau = COALESCE(@matkhau, MatKhau),
			HoTen = COALESCE(@hoten, HoTen),
			NgaySinh = COALESCE(@ngaysinh, NgaySinh),
			GioiTinh = COALESCE(@gioitinh, GioiTinh),
			DiaChi = COALESCE(@diachi, DiaChi),
			Role = COALESCE(@role, Role)           
		WHERE PerID = @per_id;
	END;


	--Xóa người dùng
	CREATE PROCEDURE sp_user_delete
    @per_id INT
	AS
	BEGIN
	DELETE FROM Users
		WHERE PerID = @per_id;
	END;
	
	--Lấy thông tin người dùng
	CREATE PROCEDURE sp_user_get_by_id
    @per_id INT
	AS
	BEGIN
		SELECT * FROM Users
		WHERE PerID = @per_id;
	END;


	--Lấy tất cả người dùng
	CREATE PROCEDURE sp_user_get_all
	AS
	BEGIN
		SELECT * FROM Users;
	END;

	--Tìm kiếm người dùng
	create PROCEDURE sp_user_search 
		@page_index INT,
		@page_size INT = 6, 
		@taikhoan NVARCHAR(50) ='',
		@hoten NVARCHAR(100) = '',
		@diachi NVARCHAR(255) = ''
	AS
	BEGIN
		DECLARE @startRow INT;
		SET @startRow = (@page_index - 1) * @page_size;

		IF @page_index < 1 OR @page_size < 1
		BEGIN
			RAISERROR('page_index và page_size ph?i l?n h?n 0', 16, 1);
			RETURN;
		END

		DECLARE @totalRecords INT;
		SELECT @totalRecords = COUNT(*)
		FROM Users
		WHERE 
			(@taikhoan = '' OR TaiKhoan LIKE '%' + @taikhoan + '%')
			AND (@hoten = '' OR HoTen LIKE '%' + @hoten + '%')
			AND (@diachi = '' OR DiaChi LIKE '%' + @diachi + '%');

		SELECT 
			PerID, 
			TaiKhoan, 
			HoTen, 
			DiaChi, 
			Role, 
			@totalRecords AS TotalRecords
		FROM Users
		WHERE 
			(@taikhoan = '' OR TaiKhoan LIKE '%' + @taikhoan + '%')
			AND (@hoten = '' OR HoTen LIKE '%' + @hoten + '%')
			AND (@diachi = '' OR DiaChi LIKE '%' + @diachi + '%')
		ORDER BY PerID
		OFFSET @startRow ROWS FETCH NEXT @page_size ROWS ONLY;
	END;

	
	CREATE PROCEDURE sp_user_authenticate
		@taikhoan NVARCHAR(50),
		@matkhau NVARCHAR(256)
	AS
	BEGIN
		SELECT PerID, TaiKhoan, HoTen, DiaChi, Role
		FROM Users
		WHERE TaiKhoan = @taikhoan AND MatKhau = @matkhau;
	END;

--Bảng danh mục sản phẩm
CREATE TABLE DanhMucSanPham
(
    MaDanhMuc CHAR(10) PRIMARY KEY,
    TenDanhMuc NVARCHAR(255) NOT NULL
);
select*from DanhMucSanPham
	
	--thêm danh mục
	CREATE PROCEDURE sp_danh_muc_san_pham_create
    @ma_danh_muc CHAR(10),
    @ten_danh_muc NVARCHAR(255)
	AS
	BEGIN
		INSERT INTO DanhMucSanPham (MaDanhMuc, TenDanhMuc)
		VALUES (@ma_danh_muc, @ten_danh_muc);
	END;
	--cập nhật danh mục
	CREATE PROCEDURE sp_danh_muc_san_pham_update
    @ma_danh_muc CHAR(10),
    @ten_danh_muc NVARCHAR(255)
	AS
	BEGIN
		UPDATE DanhMucSanPham
		SET TenDanhMuc = @ten_danh_muc
		WHERE MaDanhMuc = @ma_danh_muc;
	END;
	--xóa danh mục
	CREATE PROCEDURE sp_danh_muc_san_pham_delete
    @ma_danh_muc CHAR(10)
	AS
	BEGIN
		DELETE FROM DanhMucSanPham
		WHERE MaDanhMuc = @ma_danh_muc;
	END;
	--lấy dữ liệu theo mã danh muc
	CREATE PROCEDURE sp_danh_muc_san_pham_get_by_id
    @ma_danh_muc CHAR(10)
	AS
	BEGIN
		SELECT MaDanhMuc, TenDanhMuc
		FROM DanhMucSanPham
		WHERE MaDanhMuc = @ma_danh_muc;
	END;
	--lấy tất cả danh mục
	CREATE PROCEDURE sp_danh_muc_san_pham_get_all
	AS
	BEGIN
		SELECT MaDanhMuc, TenDanhMuc
		FROM DanhMucSanPham;
	END;
	--tìm kiếm danh mục 
	CREATE PROCEDURE sp_danh_muc_san_pham_search
    @page_index INT,
    @page_size INT,
    @ma_danh_muc CHAR(10),
    @ten_danh_muc NVARCHAR(255)
	AS
	BEGIN
		DECLARE @startRow INT;
		SET @startRow = (@page_index - 1) * @page_size;

		SELECT MaDanhMuc, TenDanhMuc
		FROM DanhMucSanPham
		WHERE (MaDanhMuc LIKE '%' + @ma_danh_muc + '%' OR @ma_danh_muc = '')
		AND (TenDanhMuc LIKE '%' + @ten_danh_muc + '%' OR @ten_danh_muc = '')
		ORDER BY MaDanhMuc
		OFFSET @startRow ROWS FETCH NEXT @page_size ROWS ONLY;
	END;

--Bảng size
CREATE TABLE Size
(
    MaSize NVARCHAR(30) PRIMARY KEY 
);
	--thêm size
	CREATE PROCEDURE sp_size_create
    @ma_size NVARCHAR(30)
	AS
	BEGIN
		INSERT INTO Size (MaSize)
		VALUES (@ma_size);
	END;
	--cập nhật
	CREATE PROCEDURE sp_size_update
    @ma_size NVARCHAR(30)
	AS
	BEGIN
		UPDATE Size
		SET MaSize = @ma_size
		WHERE MaSize = @ma_size; 
	END;
	--xóa size
	CREATE PROCEDURE sp_size_delete
    @ma_size NVARCHAR(30)
	AS
	BEGIN
		DELETE FROM Size
		WHERE MaSize = @ma_size;
	END;
	--liếu dữ liệu kích thước
	CREATE PROCEDURE sp_size_get_by_id
    @ma_size NVARCHAR(30)
	AS
	BEGIN
		SELECT MaSize
		FROM Size
		WHERE MaSize = @ma_size;
	END;
	--lấy tất cả bảng size
	CREATE PROCEDURE sp_size_get_all
	AS
	BEGIN
		SELECT MaSize
		FROM Size;
	END;
	--tìm kiếm
	CREATE PROCEDURE sp_size_search
    @page_index INT,
    @page_size INT,
    @search_criteria NVARCHAR(30)
	AS
	BEGIN
		DECLARE @startRow INT;
		SET @startRow = (@page_index - 1) * @page_size;

		SELECT MaSize
		FROM Size
		WHERE MaSize LIKE '%' + @search_criteria + '%'
		ORDER BY MaSize
		OFFSET @startRow ROWS FETCH NEXT @page_size ROWS ONLY;

		SELECT COUNT(*) AS TotalCount
		FROM Size
		WHERE MaSize LIKE '%' + @search_criteria + '%';
	END;

-- Thêm dữ liệu cho bảng size
INSERT INTO Size (MaSize)
VALUES 
(N'Nhỏ'),
(N'Vừa'),
(N'Lớn');
select*from Size

--Bảng màu sắc
CREATE TABLE MauSac (
    TenMau NVARCHAR(30) PRIMARY KEY,  
);
select*from MauSac
INSERT INTO MauSac (TenMau)
		VALUES ('Yellow' );
	--thêm màu 
	CREATE PROCEDURE sp_mau_sac_create
		@ten_mau NVARCHAR(30)
	
	AS
	BEGIN
		INSERT INTO MauSac ( TenMau)
		VALUES (@ten_mau );
	END;
	--sửa màu
	CREATE PROCEDURE sp_mau_sac_update
    @ten_mau NVARCHAR(30)
	AS
	BEGIN
		UPDATE MauSac
		SET TenMau = @ten_mau
		WHERE TenMau = @ten_mau;
	END;
	--xóa màu 
	CREATE PROCEDURE sp_mau_sac_delete
    @ten_mau NVARCHAR(30)
	AS
	BEGIN
		DELETE FROM MauSac
		WHERE TenMau = @ten_mau;
	END;
	--lấy tất cả màu sắc
	CREATE PROCEDURE sp_mau_sac_get_all
	AS
	BEGIN
		SELECT  TenMau
		FROM MauSac;
	END;
	--tìm kiếm màu sắc
	CREATE PROCEDURE sp_mau_sac_search
    @page_index INT,
    @page_size INT,
    @ten_mau NVARCHAR(50) = ''
	AS
	BEGIN
		DECLARE @startRow INT;
		SET @startRow = (@page_index - 1) * @page_size;

		SELECT MaMau, TenMau
		FROM MauSac
		WHERE (TenMau LIKE '%' + @ten_mau + '%' OR @ten_mau = '')
		ORDER BY MaMau
		OFFSET @startRow ROWS FETCH NEXT @page_size ROWS ONLY;
	END;

--Bảng nhà cung cấp
CREATE TABLE NhaCungCap
(      
    TenNCC NVARCHAR(255) PRIMARY KEY,
	DiaChi NVARCHAR(255) NULL DEFAULT N'Chưa cập nhật',                     
    LienHe NVARCHAR(255) NULL DEFAULT N'Chưa cập nhật'    
); 
	select * from NhaCungCap
	--thêm ncc
	CREATE PROCEDURE sp_nha_cung_cap_create
    @ten_ncc NVARCHAR(255),
    @dia_chi NVARCHAR(255) = NULL,
    @lien_he NVARCHAR(255) = NULL
	AS
	BEGIN
		IF EXISTS (SELECT 1 FROM NhaCungCap WHERE TenNCC = @ten_ncc)
		BEGIN
			RAISERROR('Nhà cung cấp đã tồn tại.', 16, 1);
			RETURN;
		END
		 IF @dia_chi IS NULL OR LTRIM(RTRIM(@dia_chi)) = ''
        BEGIN
            SET @dia_chi = N'Chưa cập nhật';
        END
		 IF @lien_he IS NULL OR LTRIM(RTRIM(@lien_he)) = ''
        BEGIN
            SET @lien_he = N'Chưa cập nhật';
        END
		INSERT INTO NhaCungCap (TenNCC, DiaChi, LienHe)
		VALUES (@ten_ncc, @dia_chi, @lien_he);
	END
	GO
	--sửa ncc 
	CREATE PROCEDURE sp_nha_cung_cap_update
    @ten_ncc NVARCHAR(255),
    @dia_chi NVARCHAR(255),
    @lien_he NVARCHAR(255)
	AS
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM NhaCungCap WHERE TenNCC = @ten_ncc)
		BEGIN
			RAISERROR('Nhà cung cấp không tồn tại.', 16, 1);
			RETURN;
		END

		UPDATE NhaCungCap
		SET DiaChi = @dia_chi,
			LienHe = @lien_he
		WHERE TenNCC = @ten_ncc;
	END
	GO
	--xóa ncc
	CREATE PROCEDURE sp_nha_cung_cap_delete
    @ten_ncc NVARCHAR(255)
	AS
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM NhaCungCap WHERE TenNCC = @ten_ncc)
		BEGIN
			RAISERROR('Nhà cung cấp không tồn tại.', 16, 1);
			RETURN;
		END

		DELETE FROM NhaCungCap WHERE TenNCC = @ten_ncc;
	END
	GO
	--lấy tất cả ncc
	CREATE PROCEDURE sp_nha_cung_cap_get_all
	AS
	BEGIN
		SELECT * FROM NhaCungCap;
	END
	GO


--Bảng đơn hàng nhập
CREATE TABLE DonHangNhap
(
	MaDHN INT IDENTITY(1,1) PRIMARY KEY, 
	TenNCC NVARCHAR(255) NOT NULL,      
	NgayNhap DATETIME DEFAULT GETDATE(),             
	FOREIGN KEY (TenNCC) REFERENCES NhaCungCap(TenNCC) 
);
select * from DonHangNhap
	--thêm DHN 
	CREATE PROCEDURE sp_don_hang_nhap_create
    @ten_ncc NVARCHAR(255)
	AS
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM NhaCungCap WHERE TenNCC = @ten_ncc)
		BEGIN
			RAISERROR('Nhà cung cấp không tồn tại.', 16, 1);
			RETURN;
		END

		-- Thêm đơn hàng nhập
		INSERT INTO DonHangNhap (TenNCC)
		VALUES (@ten_ncc);

		-- Lấy mã đơn hàng nhập vừa tạo
		DECLARE @new_ma_dhn INT;
		SELECT @new_ma_dhn = SCOPE_IDENTITY();

		SELECT @new_ma_dhn AS MaDHN;
	END
	GO

	--sửa DHN
	CREATE PROCEDURE sp_don_hang_nhap_update
    @ma_dhn INT,
    @ten_ncc NVARCHAR(255),
    @ngay_nhap DATE
	AS
	BEGIN
		-- Kiểm tra xem Đơn Hàng Nhập có tồn tại không
		IF NOT EXISTS (SELECT 1 FROM DonHangNhap WHERE MaDHN = @ma_dhn)
		BEGIN
			RAISERROR('Đơn hàng nhập không tồn tại.', 16, 1);
			RETURN;
		END

		-- Kiểm tra nhà cung cấp
		IF NOT EXISTS (SELECT 1 FROM NhaCungCap WHERE TenNCC = @ten_ncc)
		BEGIN
			RAISERROR('Nhà cung cấp không tồn tại.', 16, 1);
			RETURN;
		END

		-- Cập nhật thông tin Đơn Hàng Nhập
		UPDATE DonHangNhap
		SET TenNCC = @ten_ncc,
			NgayNhap = ISNULL(@ngay_nhap, GETDATE()) 
		WHERE MaDHN = @ma_dhn;
	END
	GO

	--Xóa đơn hàng nhập
	CREATE PROCEDURE sp_don_hang_nhap_delete
    @ma_dhn INT
	AS
	BEGIN
		-- Kiểm tra xem đơn hàng nhập có tồn tại không
		IF NOT EXISTS (SELECT 1 FROM DonHangNhap WHERE MaDHN = @ma_dhn)
		BEGIN
			RAISERROR('Đơn hàng nhập không tồn tại.', 16, 1);
			RETURN;
		END

		-- Xóa đơn hàng nhập
		DELETE FROM DonHangNhap WHERE MaDHN = @ma_dhn;

		-- Thông báo xóa thành công
		PRINT 'Đơn hàng nhập đã được xóa thành công.';
	END
	GO
	-- Xóa đơn hàng nhập
		DELETE FROM DonHangNhap WHERE MaDHN = 6;

	--lấy tất cả DHN
	CREATE PROCEDURE sp_don_hang_nhap_get_all
	AS
	BEGIN
		SELECT *FROM DonHangNhap;
	END
	GO

	--lấy đơn hàng nhập theo mã
	CREATE PROCEDURE sp_don_hang_nhap_get_by_id
    @ma_DHN INT
	AS
	BEGIN
		BEGIN TRY
			SELECT MaDHN, TenNCC, NgayNhap
			FROM DonHangNhap
			WHERE MaDHN = @ma_DHN;
		END TRY
		BEGIN CATCH
			THROW;
		END CATCH
	END
	GO
	
--Bảng chi tiết đơn hàng nhập
CREATE TABLE ChiTietDonHangNhap
(
	MaDHN INT,                        
	MaSp CHAR(10),                      
	TenSp NVARCHAR(256),                
	GiaNhap DECIMAL(10,2) NOT NULL,    
	SoLuong INT NOT NULL,               
	PRIMARY KEY (MaDHN, MaSp),        
	FOREIGN KEY (MaDHN) REFERENCES DonHangNhap(MaDHN) 
);
ALTER TABLE ChiTietDonHangNhap
ADD CONSTRAINT UQ_ChiTietDonHangNhap_MaSp UNIQUE (MaSp);

	--thêm CTDHN
	CREATE PROCEDURE sp_chi_tiet_don_hang_nhap_create
    @ma_dhn INT,
    @ma_sp CHAR(10),
    @ten_sp NVARCHAR(256),
    @gia_nhap DECIMAL(10,2),
    @so_luong INT
	AS
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM DonHangNhap WHERE MaDHN = @ma_dhn)
		BEGIN
			RAISERROR('Đơn hàng nhập không tồn tại.', 16, 1);
			RETURN;
		END

		INSERT INTO ChiTietDonHangNhap (MaDHN, MaSp, TenSp, GiaNhap, SoLuong)
		VALUES (@ma_dhn, @ma_sp, @ten_sp, @gia_nhap, @so_luong);
	END
	GO
	--sửa CTDHN
	CREATE PROCEDURE sp_chi_tiet_don_hang_nhap_update
    @ma_dhn INT,
    @ma_sp CHAR(10),
    @ten_sp NVARCHAR(256),
    @gia_nhap DECIMAL(10,2),
    @so_luong INT
	AS
	BEGIN
		-- Kiểm tra nếu chi tiết tồn tại thì cập nhật, nếu không thì thêm mới
		IF EXISTS (SELECT 1 FROM ChiTietDonHangNhap WHERE MaDHN = @ma_dhn AND MaSp = @ma_sp)
		BEGIN
			UPDATE ChiTietDonHangNhap
			SET TenSp = @ten_sp,
				GiaNhap = @gia_nhap,
				SoLuong = @so_luong
			WHERE MaDHN = @ma_dhn AND MaSp = @ma_sp;
		END
		ELSE
		BEGIN
			INSERT INTO ChiTietDonHangNhap (MaDHN, MaSp, TenSp, GiaNhap, SoLuong)
			VALUES (@ma_dhn, @ma_sp, @ten_sp, @gia_nhap, @so_luong);
		END
	END
	GO

	--lấy CTDHN theo maDHN
	CREATE PROCEDURE sp_ct_don_hang_nhap_get_by_ma_dhn
    @ma_dhn INT
	AS
	BEGIN
		-- Kiểm tra sự tồn tại của đơn hàng nhập
		IF NOT EXISTS (SELECT 1 FROM DonHangNhap WHERE MaDHN = @ma_dhn)
		BEGIN
			RAISERROR('Đơn hàng nhập không tồn tại.', 16, 1);
			RETURN;
		END

		-- Lấy chi tiết đơn hàng nhập theo MaDHN
		SELECT CT.MaDHN, CT.MaSp, CT.TenSp, CT.GiaNhap, CT.SoLuong, 
			   DH.TenNCC, DH.NgayNhap
		FROM ChiTietDonHangNhap CT
		INNER JOIN DonHangNhap DH ON CT.MaDHN = DH.MaDHN
		WHERE CT.MaDHN = @ma_dhn;
	END
	GO

	--lấy tất cả CTDHN
	CREATE PROCEDURE sp_ct_don_hang_nhap_get_all
	AS
	BEGIN
		SELECT * 
		FROM ChiTietDonHangNhap;
	END
	GO

	--xóa mã sp trong chi tiết
	CREATE PROCEDURE sp_chi_tiet_don_hang_nhap_delete
    @ma_dhn INT,
    @ma_sp INT
	AS
	BEGIN
		DELETE FROM ChiTietDonHangNhap
		WHERE MaDHN = @ma_dhn AND MaSp = @ma_sp;
	END

	DELETE FROM ChiTietDonHangNhap
		WHERE MaDHN = 5 AND MaSp = 'Gucci04';

CREATE TABLE TuiXach
(
    MaDanhMuc CHAR(10),                 
    MaSp CHAR(10),                      
    TenSp NVARCHAR(255) NOT NULL,       
    TenMau NVARCHAR(30),                
    MaSize NVARCHAR(30),                
    GiaBan DECIMAL(10,2) NOT NULL,      
    KhuyenMai DECIMAL(18,2),            
    TonKho INT,
    MoTa NVARCHAR(MAX),                 
    HinhAnh VARCHAR(MAX),               
    SoLuotDanhGia INT DEFAULT 0,        
    PRIMARY KEY (MaSp, TenMau, MaSize), 
    FOREIGN KEY (MaDanhMuc) REFERENCES DanhMucSanPham(MaDanhMuc), 
    FOREIGN KEY (MaSp) REFERENCES ChiTietDonHangNhap(MaSp), 
    FOREIGN KEY (TenMau) REFERENCES MauSac(TenMau),          
    FOREIGN KEY (MaSize) REFERENCES Size(MaSize)            
);


select * from TuiXach
	--trigger
	CREATE TRIGGER trg_UpdateSoLuotDanhGia
	ON BinhLuan
	AFTER INSERT
	AS
	BEGIN
		UPDATE TuiXach
		SET SoLuotDanhGia = SoLuotDanhGia + 1
		FROM TuiXach
		INNER JOIN inserted i ON TuiXach.MaSp = i.MaSp;
	END
	GO

	-- thêm sản phẩm
	CREATE PROCEDURE sp_tui_xach_create
		@ma_danh_muc CHAR(10),
		@ma_sp CHAR(10),
		@ten_sp NVARCHAR(255),
		@ten_mau NVARCHAR(30), 
		@ma_size NVARCHAR(30),
		@gia_ban DECIMAL(10, 2),
		@khuyen_mai DECIMAL(18, 2) = NULL, 
		@ton_kho INT,
		@mo_ta NVARCHAR(MAX),
		@hinh_anh VARCHAR(MAX)
	AS
	BEGIN
		-- Kiểm tra ràng buộc khóa ngoài
		IF NOT EXISTS (SELECT 1 FROM MauSac WHERE TenMau = @ten_mau)
		BEGIN
			RAISERROR('Tên màu không tồn tại.', 16, 1);
			RETURN;
		END
		IF NOT EXISTS (SELECT 1 FROM Size WHERE MaSize = @ma_size)
		BEGIN
			RAISERROR('Mã size không tồn tại.', 16, 1);
			RETURN;
		END
		IF NOT EXISTS (SELECT 1 FROM DanhMucSanPham WHERE MaDanhMuc = @ma_danh_muc)
		BEGIN
			RAISERROR('Mã danh mục không tồn tại.', 16, 1);
			RETURN;
		END
		IF NOT EXISTS (SELECT 1 FROM ChiTietDonHangNhap WHERE MaSp = @ma_sp)
		BEGIN
			RAISERROR('Mã sản phẩm không tồn tại.', 16, 1);
			RETURN;
		END

		-- Tính giá bán sau khuyến mãi
		IF @khuyen_mai IS NOT NULL
		BEGIN
			SET @gia_ban = @gia_ban - (@gia_ban * @khuyen_mai / 100);
		END

		-- Chèn dữ liệu vào bảng TuiXach
		INSERT INTO TuiXach (MaDanhMuc, MaSp, TenSp, TenMau, MaSize, GiaBan, KhuyenMai, TonKho, MoTa, HinhAnh)
		VALUES (@ma_danh_muc, @ma_sp, @ten_sp, @ten_mau, @ma_size, @gia_ban, @khuyen_mai, @ton_kho, @mo_ta, @hinh_anh);
	END
	GO



	--cập nhật sản phẩm
	CREATE PROCEDURE sp_tui_xach_update
	@ma_danh_muc CHAR(10) = NULL,
    @ma_sp CHAR(10),
    @ten_sp NVARCHAR(255) = NULL,
	@ten_mau NVARCHAR(30) = NULL,
    @ma_size NVARCHAR(30) = NULL,
    @gia_ban DECIMAL(10, 2) = NULL,
    @khuyen_mai DECIMAL(18, 2) = NULL,
    @ton_kho INT = NULL,
    @mo_ta NVARCHAR(MAX) = NULL,
    @hinh_anh VARCHAR(MAX) = NULL
	AS
	BEGIN
	   IF NOT EXISTS (SELECT 1 FROM ChiTietDonHangNhap WHERE MaSp = @ma_sp)
			BEGIN
				RAISERROR('Mã sản phẩm không tồn tại.', 16, 1);
				RETURN;
			END
		IF @khuyen_mai IS NOT NULL
		BEGIN
		   SET @gia_ban = @gia_ban - (@gia_ban * @khuyen_mai / 100);
		END

		UPDATE TuiXach
		SET MaDanhMuc = ISNULL(@ma_danh_muc, MaDanhMuc),
			TenSp = ISNULL(@ten_sp, TenSp),
			TenMau = ISNULL(@ten_mau, TenMau),
			MaSize = ISNULL(@ma_size, MaSize),
			GiaBan = ISNULL(@gia_ban, GiaBan ),
			KhuyenMai = ISNULL(@khuyen_mai, KhuyenMai),
			TonKho = ISNULL(@ton_kho, TonKho),
			MoTa = ISNULL(@mo_ta, MoTa),
			HinhAnh = ISNULL(@hinh_anh, HinhAnh)
       
		WHERE MaSp = @ma_sp;
	END
	GO


	--xóa sản phẩm
	CREATE PROCEDURE sp_tui_xach_delete
    @ma_sp CHAR(10)
	AS
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM TuiXach WHERE MaSp = @ma_sp)
		BEGIN
			RAISERROR('Mã sản phẩm không tồn tại', 16, 1);
			RETURN;
		END

		DELETE FROM TuiXach
		WHERE MaSp = @ma_sp;
	END
	GO

	--lấy sp theo id
	CREATE PROCEDURE sp_tui_xach_get_by_id
	@ma_Sp CHAR(10)
	AS
	BEGIN
		SELECT * 
		FROM TuiXach
		WHERE MaSp = @ma_Sp;
	END;

	--lấy sản phẩm theo danh mục
	 CREATE PROCEDURE sp_get_tuixach_by_danhmuc
    @ma_DanhMuc CHAR(10)
	AS
	BEGIN
		SELECT * 
		FROM TuiXach
		WHERE MaDanhMuc = @ma_DanhMuc;
	END;

	--lấy sản phẩm theo size
	CREATE PROCEDURE sp_get_tuixach_by_size
    @ma_Size NVARCHAR(30)
	AS
	BEGIN
		SELECT * 
		FROM TuiXach
		WHERE MaSize = @ma_Size;
	END;

	--lấy sản phẩm theo màu sắc
	CREATE PROCEDURE sp_get_tuixach_by_mausac
	@ten_Mau NVARCHAR(30)
	AS
	BEGIN
		SELECT * 
		FROM TuiXach
		WHERE TenMau = @ten_Mau;
	END;

	--lấy tất cả sản phẩm
	CREATE PROCEDURE sp_tui_xach_get_all
	AS
	BEGIN
		SELECT * 
		FROM TuiXach;
	END
	GO
	

	--tìm kiếm sp
	CREATE PROCEDURE sp_tui_xach_search
    @page_index INT,
    @page_size INT,
    @name NVARCHAR(255),
    @color NVARCHAR(30),
    @size NVARCHAR(30),
    @min_price DECIMAL(10, 2),
		@max_price DECIMAL(10, 2)
	AS
	BEGIN
		SELECT * FROM TuiXach
		WHERE (TenSp LIKE '%' + @name + '%' OR @name = '')
		  AND (TenMau LIKE '%' + @color + '%' OR @color = '')
		  AND (MaSize = @size OR @size = '')
		  AND (GiaSp BETWEEN @min_price AND @max_price)
		ORDER BY MaSp
		OFFSET @page_index * @page_size ROWS FETCH NEXT @page_size ROWS ONLY;

		SELECT COUNT(*) AS TotalCount
		FROM TuiXach
		WHERE (TenSp LIKE '%' + @name + '%' OR @name = '')
		  AND (TenMau LIKE '%' + @color + '%' OR @color = '')
		  AND (MaSize = @size OR @size = '')
		  AND (GiaSp BETWEEN @min_price AND @max_price);
	END
	GO


-- Bảng HoaDon (Quản lý đơn đặt hàng)
CREATE TABLE HoaDon
(
    MaHD INT PRIMARY KEY IDENTITY(1,1),
    PerID INT, 
    NgayDatHang DATETIME DEFAULT GETDATE(), 
    TrangThai NVARCHAR(50) DEFAULT N'Đang xử lý',
    NgayNhanHang DATE Null,
    FOREIGN KEY (PerID) REFERENCES Users(PerID)
);

ALTER TABLE HoaDon
ADD	HoTen NVARCHAR(50) ,
	DiaChi NVARCHAR(225) ,
	SDT NVARCHAR(15) ;

select * from HoaDon

-- Xóa hóa đơn
DELETE FROM HoaDon
WHERE MaHD =2;
	--thêm hóa đơn
CREATE PROCEDURE sp_hoa_don_create
    @per_id INT,
    @ho_ten NVARCHAR(50),
    @dia_chi NVARCHAR(225),
    @sdt NVARCHAR(15),
    @trang_thai NVARCHAR(50) = N'Đang xử lý',
    @ngay_nhanhang DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Thêm hóa đơn (NgayDatHang sẽ tự động lấy giá trị từ DEFAULT)
        INSERT INTO HoaDon (PerID, HoTen, DiaChi, SDT, TrangThai, NgayNhanHang)
        VALUES (@per_id, @ho_ten, @dia_chi, @sdt, @trang_thai, 
                CASE WHEN @trang_thai = N'Hoàn thành' THEN GETDATE() ELSE NULL END);

        -- Trả về mã hóa đơn vừa tạo
        SELECT SCOPE_IDENTITY() AS MaHD;
    END TRY
    BEGIN CATCH
        -- Bắt lỗi và trả về thông báo lỗi
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END;
GO


	--cập nhập hóa đơn
	CREATE PROCEDURE sp_hoa_don_update
    @ma_hd INT,
	@ho_ten NVARCHAR(50) ,
	@dia_chi NVARCHAR(225) ,
	@sdt NVARCHAR(15) ,
    @trang_thai NVARCHAR(50),
    @ngay_nhanhang DATETIME = NULL
	AS
	BEGIN
		UPDATE HoaDon
		SET HoTen = @ho_ten,
			DiaChi = @dia_chi,
			SDT = @sdt,
			TrangThai = @trang_thai,
			NgayNhanHang = @ngay_nhanhang
		WHERE MaHD = @ma_hd;
	END
	GO

	--cập nhậ trạng thái
	CREATE PROCEDURE sp_hoa_don_update_trang_thai
    @ma_hd INT,
    @trang_thai NVARCHAR(50)
	AS
	BEGIN
		UPDATE HoaDon
		SET TrangThai = @trang_thai
		WHERE MaHD = @ma_hd;
	END
	GO
	-- Cập nhật trạng thái và ngày nhận hàng nếu trạng thái là "Hoàn thành"
	CREATE PROCEDURE sp_hoa_don_update_trang_thai
		@ma_hd INT,
		@trang_thai NVARCHAR(50)
	AS
	BEGIN
		-- Kiểm tra nếu trạng thái là "Hoàn thành"
		IF @trang_thai = N'Hoàn thành'
		BEGIN
			-- Cập nhật trạng thái và Ngày nhận hàng
			UPDATE HoaDon
			SET TrangThai = @trang_thai,
				NgayNhanHang = COALESCE(NgayNhanHang, GETDATE()) -- Thay giá trị NULL bằng thời gian hiện tại
			WHERE MaHD = @ma_hd;
		END
		ELSE
		BEGIN
			-- Nếu trạng thái không phải "Hoàn thành", chỉ cập nhật trạng thái
			UPDATE HoaDon
			SET TrangThai = @trang_thai
			WHERE MaHD = @ma_hd;
		END
	END
	GO

	-- xóa hóa đơn
	CREATE PROCEDURE sp_hoa_don_delete
		@ma_hd INT
	AS
	BEGIN
		BEGIN TRY
			DELETE FROM HoaDon
			WHERE MaHD = @ma_hd;
		END TRY
		BEGIN CATCH
			THROW;
		END CATCH
	END
	GO

	--Lấy hóa đơn theo mã
	CREATE PROCEDURE sp_hoa_don_get_by_id
    @ma_hd INT
	AS
	BEGIN
		BEGIN TRY
			SELECT MaHD, PerID, HoTen,  DiaChi, SDT, NgayDatHang, TrangThai, NgayNhanHang
			FROM HoaDon
			WHERE MaHD = @ma_hd;
		END TRY
		BEGIN CATCH
			THROW;
		END CATCH
	END;
	GO

	--lấy hóa đơn theo trạng thái
	CREATE PROCEDURE sp_hoa_don_get_by_trangthai
    @trang_thai NVARCHAR(50)
	AS
	BEGIN
		BEGIN TRY
			SELECT MaHD, PerID, HoTen,  DiaChi, SDT, NgayDatHang, TrangThai, NgayNhanHang
			FROM HoaDon
			WHERE TrangThai = @trang_thai;
		END TRY
		BEGIN CATCH
			THROW;
		END CATCH
	END;
	GO
	
	-- Lấy tất cả hóa đơn
	CREATE PROCEDURE sp_hoa_don_get_all
	AS
	BEGIN
		BEGIN TRY
			SELECT * FROM HoaDon
			ORDER BY NgayDatHang DESC;
		END TRY
		BEGIN CATCH
			THROW;
		END CATCH
	END;
	SELECT MaHD, TrangThai, NgayNhanHang FROM HoaDon WHERE MaHD = 6

	-- Lấy tất cả hóa đơn-chi tiết 
	CREATE PROCEDURE sp_hoa_don_get_all_with_details
	AS
	BEGIN
		-- Lấy tất cả hóa đơn
		SELECT * FROM HoaDon;

		-- Lấy tất cả chi tiết hóa đơn
		SELECT * FROM ChiTietHoaDon;
	END
	GO

	--lấy CTHD theo maHD
	CREATE PROCEDURE sp_ct_hoa_don_get_by_ma_hd
    @ma_hd INT
	AS
	BEGIN
		SET NOCOUNT ON;

		BEGIN TRY
			-- Kiểm tra xem hóa đơn có tồn tại không
			IF NOT EXISTS (SELECT 1 FROM HoaDon WHERE MaHD = @ma_hd)
			BEGIN
				THROW 50001, 'Hóa Đơn không tồn tại.', 1;
			END

			-- Truy vấn chi tiết hóa đơn
			SELECT  CT.MaHD, CT.MaSp, CT.TenSp, CT.TenMau,CT.MaSize, CT.SoLuong, CT.GiaBan,CT.GhiChu,CT.KhuyenMai
			FROM ChiTietHoaDon CT
			INNER JOIN HoaDon HD ON CT.MaHD = HD.MaHD
			WHERE CT.MaHD = @ma_hd;
		END TRY
		BEGIN CATCH
			-- Bắt lỗi và trả về thông báo
			DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
			DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
			DECLARE @ErrorState INT = ERROR_STATE();
			RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
		END CATCH
	END
	GO


-- Bảng ChiTietHoaDon (Chi tiết đơn đặt hàng)
CREATE TABLE ChiTietHoaDon
(
    MaHD INT,
    MaSp CHAR(10),
    TenSp NVARCHAR(255),
    TenMau NVARCHAR(30),
    MaSize NVARCHAR(30),
    SoLuong INT,
    GiaBan DECIMAL(18,2),
    GhiChu NVARCHAR(255),
    KhuyenMai DECIMAL(18,2),
    PRIMARY KEY (MaHD, MaSp), -- Khóa chính kết hợp
    FOREIGN KEY (MaHD) REFERENCES HoaDon(MaHD),
    FOREIGN KEY (MaSp) REFERENCES ChiTIetDonHangNhap(MaSp),
    FOREIGN KEY (MaSize) REFERENCES Size(MaSize),
    FOREIGN KEY (TenMau) REFERENCES MauSac(TenMau)
);
select * from ChiTietHoaDon
	--thêm chi tiết hóa đơn
	CREATE PROCEDURE sp_chi_tiet_hoa_don_create
    @ma_hd INT,
    @ma_sp CHAR(10),
    @ten_sp NVARCHAR(255),
    @ten_mau NVARCHAR(30),
    @ma_size NVARCHAR(30),
    @so_luong INT,
    @gia_ban DECIMAL(18, 2),
    @ghi_chu NVARCHAR(255),
    @khuyen_mai DECIMAL(18, 2)
	AS
	BEGIN
		SET NOCOUNT ON;

		BEGIN TRY
			-- Kiểm tra xem sản phẩm có tồn tại không
			IF NOT EXISTS (SELECT 1 FROM ChiTietDonHangNhap WHERE MaSp = @ma_sp)
			BEGIN
				THROW 50001, 'Sản phẩm không tồn tại.', 1;
			END

			-- Kiểm tra tồn kho
			DECLARE @ton_kho INT;
			SELECT @ton_kho = TonKho FROM TuiXach WHERE MaSp = @ma_sp;
        
			IF @ton_kho < @so_luong
			BEGIN
				THROW 50002, 'Số lượng tồn kho không đủ.', 1;
			END

			-- Thêm chi tiết hóa đơn
			INSERT INTO ChiTietHoaDon (MaHD, MaSp, TenSp, TenMau, MaSize, SoLuong, GiaBan, GhiChu, KhuyenMai)
			VALUES (@ma_hd, @ma_sp, @ten_sp, @ten_mau, @ma_size, @so_luong, @gia_ban, @ghi_chu, @khuyen_mai);

			-- Cập nhật tồn kho
			UPDATE TuiXach
			SET TonKho = TonKho - @so_luong
			WHERE MaSp = @ma_sp;

			-- Trả về thông báo thành công
			SELECT 'Thêm chi tiết hóa đơn thành công.' AS Message;
		END TRY
		BEGIN CATCH
			-- Bắt lỗi và trả về thông báo
			DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
			DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
			DECLARE @ErrorState INT = ERROR_STATE();
			RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
		END CATCH
	END;
	GO

	--cập nhật
	CREATE PROCEDURE sp_chi_tiet_hoa_don_update
    @ma_hd INT,
    @ma_sp CHAR(10),
    @ten_sp NVARCHAR(255),
    @ten_mau NVARCHAR(30),
    @ma_size NVARCHAR(30),
    @so_luong INT,
    @gia_ban DECIMAL(10, 2),
    @ghi_chu NVARCHAR(255),
    @khuyen_mai DECIMAL(18, 2)
	AS
	BEGIN
		UPDATE ChiTietHoaDon
		SET SoLuong = @so_luong,
			GiaBan = @gia_ban,
			GhiChu = @ghi_chu,
			KhuyenMai = @khuyen_mai
		WHERE MaHD = @ma_hd AND MaSp = @ma_sp AND TenMau = @ten_mau AND MaSize = @ma_size;
	END
	GO


-- Bảng Bình Luận (Bình luận về sản phẩm)
CREATE TABLE BinhLuan
(
    MaBinhLuan INT PRIMARY KEY IDENTITY(1,1),
    PerID INT, 
    MaSp CHAR(10),
    NoiDung NVARCHAR(MAX),
    NgayBinhLuan DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (PerID) REFERENCES Users(PerID), -- Tham chiếu đến bảng Users
    FOREIGN KEY (MaSp) REFERENCES ChiTietDonHangNhap(MaSp)
);