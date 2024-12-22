CREATE DATABASE WebTuiXach
USE WebTuiXach
GO

-- Bảng Tài Khoản (Quản lý người dùng)
CREATE TABLE Users
(
    PerID INT IDENTITY(1,1) PRIMARY KEY, -- PerID tự động tăng
    TaiKhoan NVARCHAR(50) NOT NULL,
    MatKhau NVARCHAR(256) NOT NULL, 
    HoTen NVARCHAR(100) NULL,
	NgaySinh DATE ,
	GioiTinh NVARCHAR(10) NULL DEFAULT 'Chưa cập nhật',
    Email NVARCHAR(100) NULL,
    SDT NVARCHAR(20) NULL, 
    DiaChi NVARCHAR(255) NULL,
    Role NVARCHAR(50) NOT NULL  DEFAULT 'Khách hàng', -- Phân biệt khách hàng và quản trị viên
	Avatar VARCHAR(MAX) 
);



EXEC sp_user_search 
    @page_index = 1, 
    @page_size = 8, 
    @hoten = N'Đỗ Quỳnh', 
    @email = '', 
    @sdt = '', 
    @diachi = '';


	-- Thêm tài khoản mặc định cho admin
	INSERT INTO Users (TaiKhoan, MatKhau, HoTen, NgaySinh, GioiTinh, Email, SDT, DiaChi, Role,Avatar) 
	VALUES ('admin', '12345', N'Đỗ Quỳnh','2004-06-02',N'Nữ', 'quynhquynh@gmail.com', '0364554001', 'Hưng Yên', 'Admin', 'avt.jpg');
	INSERT INTO Users (TaiKhoan, MatKhau, HoTen) 
	VALUES ('03546632', '12345', N'Quỳnh ');


	ALTER TABLE Users
	ALTER COLUMN Avatar varchar(MAX) ;

	Select * from Users
	DELETE FROM Users WHERE PerID = 14;

	-- Thêm người dùng (có thể là admin hoặc khách hàng)
	CREATE PROCEDURE sp_user_create
    @taikhoan NVARCHAR(50), 
    @matkhau NVARCHAR(256),
    @hoten NVARCHAR(100) = NULL,
    @ngaysinh DATE = NULL,  -- Ngày sinh có thể là NULL, nếu không có giá trị thì mặc định là 'Chưa cập nhật'
    @gioitinh NVARCHAR(10) = NULL,
    @email NVARCHAR(100) = NULL,
    @sdt NVARCHAR(20) = NULL,
    @diachi NVARCHAR(255) = NULL,
    @role NVARCHAR(50) = NULL,  -- Role có thể là NULL
    @avatar VARCHAR(MAX) = NULL -- Avatar có thể là NULL
AS
BEGIN
    -- Nếu Role là NULL hoặc rỗng, gán mặc định là "Khách hàng"
    IF @role IS NULL OR LTRIM(RTRIM(@role)) = ''
    BEGIN
        SET @role = 'Khách hàng';
    END

    -- Nếu Ngày sinh là NULL, gán giá trị mặc định là 'Chưa cập nhật'
    IF @ngaysinh IS NULL
    BEGIN
        SET @ngaysinh = 'Null'; -- Hoặc bạn có thể dùng một giá trị đặc biệt khác nếu cần
    END

    -- Nếu Avatar là NULL, gán giá trị mặc định là 'Chưa cập nhật'
    IF @avatar IS NULL OR LTRIM(RTRIM(@avatar)) = ''
    BEGIN
        SET @avatar = 'Chưa cập nhật';
    END

    -- Nếu các trường còn lại là NULL hoặc rỗng, gán giá trị mặc định là 'Chưa cập nhật'
    IF @hoten IS NULL OR LTRIM(RTRIM(@hoten)) = ''
    BEGIN
        SET @hoten = 'Chưa cập nhật';
    END

    IF @gioitinh IS NULL OR LTRIM(RTRIM(@gioitinh)) = ''
    BEGIN
        SET @gioitinh = 'Chưa cập nhật';
    END

    IF @email IS NULL OR LTRIM(RTRIM(@email)) = ''
    BEGIN
        SET @email = 'Chưa cập nhật';
    END

    IF @sdt IS NULL OR LTRIM(RTRIM(@sdt)) = ''
    BEGIN
        SET @sdt = 'Chưa cập nhật';
    END

    IF @diachi IS NULL OR LTRIM(RTRIM(@diachi)) = ''
    BEGIN
        SET @diachi = 'Chưa cập nhật';
    END

    -- Thực hiện insert dữ liệu vào bảng Users
    INSERT INTO Users (TaiKhoan, MatKhau, HoTen, NgaySinh, GioiTinh, Email, SDT, DiaChi, Role, Avatar)
    VALUES (@taikhoan, @matkhau, @hoten, @ngaysinh, @gioitinh, @email, @sdt, @diachi, @role, @avatar);

    -- Trả về ID của bản ghi mới được thêm vào
    SELECT SCOPE_IDENTITY() AS NewPerID;
END;


-- Cập nhật thông tin người dùng
	CREATE PROCEDURE [dbo].[sp_user_update]
		@per_id INT,                     -- ID người dùng cần cập nhật
		@taikhoan NVARCHAR(50),          -- Tài khoản
		@matkhau NVARCHAR(256) = NULL,   -- Mật khẩu
		@hoten NVARCHAR(100) = NULL,     -- Họ tên
		@ngaysinh DATE = NULL,           -- Ngày sinh
		@gioitinh NVARCHAR(10) = NULL,   -- Giới tính
		@email NVARCHAR(100) = NULL,     -- Email
		@sdt NVARCHAR(20) = NULL,        -- Số điện thoại
		@diachi NVARCHAR(255) = NULL,    -- Địa chỉ
		@role NVARCHAR(50) = NULL,       -- Vai trò (chỉ Admin được thay đổi)
		@avatar VARCHAR(MAX) = NULL,     -- Avatar
		@current_role NVARCHAR(50)       -- Vai trò của người thực hiện thao tác (Admin hoặc Khách hàng)
	AS
	BEGIN
		-- Kiểm tra nếu không phải Admin mà cố thay đổi Role
		IF @role IS NOT NULL AND @current_role != 'Admin'
		BEGIN
			RAISERROR('Bạn không có quyền thay đổi vai trò người dùng.', 16, 1);
			RETURN;
		END

		-- Thực hiện cập nhật
		UPDATE Users
		SET TaiKhoan = COALESCE(@taikhoan, TaiKhoan),   -- Nếu không có giá trị mới, giữ nguyên
			MatKhau = COALESCE(@matkhau, MatKhau),
			HoTen = COALESCE(@hoten, HoTen),
			NgaySinh = COALESCE(@ngaysinh, NgaySinh),
			GioiTinh = COALESCE(@gioitinh, GioiTinh),
			Email = COALESCE(@email, Email),
			SDT = COALESCE(@sdt, SDT),
			DiaChi = COALESCE(@diachi, DiaChi),
			Role = COALESCE(@role, Role),              -- Chỉ Admin mới được cập nhật
			Avatar = COALESCE(@avatar, Avatar)
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
		@hoten NVARCHAR(100) = '',
		@email NVARCHAR(100) = '',
		@sdt NVARCHAR(20) = '',
		@diachi NVARCHAR(255) = ''
	AS
	BEGIN
		DECLARE @startRow INT;
		SET @startRow = (@page_index - 1) * @page_size;

		IF @page_index < 1 OR @page_size < 1
		BEGIN
			RAISERROR('page_index và page_size phải lớn hơn 0', 16, 1);
			RETURN;
		END

		-- Tổng số bản ghi
		DECLARE @totalRecords INT;
		SELECT @totalRecords = COUNT(*)
		FROM Users
		WHERE 
			(@hoten = '' OR HoTen LIKE '%' + @hoten + '%')
			AND (@email = '' OR Email LIKE '%' + @email + '%')
			AND (@sdt = '' OR SDT LIKE '%' + @sdt + '%')
			AND (@diachi = '' OR DiaChi LIKE '%' + @diachi + '%');

		-- Lấy danh sách người dùng với phân trang và thêm tổng số bản ghi vào mỗi dòng
		SELECT 
			PerID, 
			TaiKhoan, 
			HoTen, 
			Email, 
			SDT, 
			DiaChi, 
			Role, 
			@totalRecords AS TotalRecords
		FROM Users
		WHERE 
			(@hoten = '' OR HoTen LIKE '%' + @hoten + '%')
			AND (@email = '' OR Email LIKE '%' + @email + '%')
			AND (@sdt = '' OR SDT LIKE '%' + @sdt + '%')
			AND (@diachi = '' OR DiaChi LIKE '%' + @diachi + '%')
		ORDER BY PerID
		OFFSET @startRow ROWS FETCH NEXT @page_size ROWS ONLY;
	END;



-- Bảng DanhMucSanPham (Quản lý danh mục sản phẩm)
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
	--lấy dữ liệu danh mục theo mã
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
	--tìm kiếm danh mục với phân trang
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

-- Bảng Size (Quản lý kích thước túi xách)
CREATE TABLE Size
(
    MaSize NVARCHAR(30) PRIMARY KEY -- Trực tiếp lưu các giá trị "Nhỏ", "Vừa", "Lớn"
);
	--thêm size mới
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
		WHERE MaSize = @ma_size; -- Trong trường hợp cập nhật chính giá trị MaSize, điều này có thể dùng để kiểm tra xem giá trị có thay đổi hay không.
	END;
	--xóa size
	CREATE PROCEDURE sp_size_delete
    @ma_size NVARCHAR(30)
	AS
	BEGIN
		DELETE FROM Size
		WHERE MaSize = @ma_size;
	END;
	--lấy dữ liệu kích thước
	CREATE PROCEDURE sp_size_get_by_id
    @ma_size NVARCHAR(30)
	AS
	BEGIN
		SELECT MaSize
		FROM Size
		WHERE MaSize = @ma_size;
	END;
	--lấy thất cả bảng size
	CREATE PROCEDURE sp_size_get_all
	AS
	BEGIN
		SELECT MaSize
		FROM Size;
	END;
	--tìm kiếm với phân trang
	CREATE PROCEDURE sp_size_search
    @page_index INT,
    @page_size INT,
    @search_criteria NVARCHAR(30)
	AS
	BEGIN
		DECLARE @startRow INT;
		SET @startRow = (@page_index - 1) * @page_size;

		-- Tìm kiếm kích thước dựa trên tiêu chí (searchCriteria có thể là một phần của MaSize)
		SELECT MaSize
		FROM Size
		WHERE MaSize LIKE '%' + @search_criteria + '%'
		ORDER BY MaSize
		OFFSET @startRow ROWS FETCH NEXT @page_size ROWS ONLY;

		-- Đếm tổng số kết quả để trả về cho phân trang
		SELECT COUNT(*) AS TotalCount
		FROM Size
		WHERE MaSize LIKE '%' + @search_criteria + '%';
	END;

-- Thêm dữ liệu mặc định cho bảng Size
INSERT INTO Size (MaSize)
VALUES 
(N'Nhỏ'),
(N'Vừa'),
(N'Lớn');
select*from Size
--bảng MauSac 
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
	--tìm kiếm màu sắc, phân trang
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





-- Bảng TuiXach (Quản lý mặt hàng túi xách)
CREATE TABLE TuiXach
(
	MaDanhMuc CHAR(10),
    MaSp CHAR(10) PRIMARY KEY,
    TenSp NVARCHAR(255) NOT NULL,
    GiaSp DECIMAL(10,2) NOT NULL,
    KhuyenMai DECIMAL(18,2),
    TonKho INT,
    TenMau NVARCHAR(30),
    MaSize NVARCHAR(30),
    MoTa NVARCHAR(MAX),
    HinhAnh VARCHAR(MAX),
    SoLuotDanhGia INT DEFAULT 0, -- Số lượt đánh giá
    FOREIGN KEY (MaSize) REFERENCES Size(MaSize),
	FOREIGN KEY (TenMau) REFERENCES MauSac(TenMau),
    FOREIGN KEY (MaDanhMuc) REFERENCES DanhMucSanPham(MaDanhMuc) -- Thiết lập khóa ngoại
);




select * from TuiXach
	-- Tạo trigger để cập nhật SoLuotDanhGia khi có bình luận mới
	CREATE TRIGGER trg_UpdateSoLuotDanhGia
	ON BinhLuan
	AFTER INSERT
	AS
	BEGIN
		-- Tăng giá trị SoLuotDanhGia trong bảng TuiXach khi có bình luận mới
		UPDATE TuiXach
		SET SoLuotDanhGia = SoLuotDanhGia + 1
		FROM TuiXach
		INNER JOIN inserted i ON TuiXach.MaSp = i.MaSp;
	END
	GO
	-- Thêm sản phẩm
CREATE PROCEDURE sp_tui_xach_create
    @ma_sp CHAR(10),
    @ten_sp NVARCHAR(255),
    @gia_sp DECIMAL(10, 2),
    @khuyen_mai DECIMAL(18, 2) = NULL,  -- Mặc định là NULL nếu không có khuyến mãi
    @ton_kho INT,
    @ten_mau NVARCHAR(30), 
    @ma_size NVARCHAR(30),
    @mo_ta NVARCHAR(MAX),
    @hinh_anh VARCHAR(MAX),
    @ma_danh_muc CHAR(10)
AS
BEGIN
    -- Kiểm tra tên màu có tồn tại không
    IF NOT EXISTS (SELECT 1 FROM MauSac WHERE TenMau = @ten_mau)
    BEGIN
        RAISERROR('Tên màu không tồn tại.', 16, 1);
        RETURN;
    END
	 -- Kiểm tra mã size có tồn tại không
    IF NOT EXISTS (SELECT 1 FROM Size WHERE MaSize = @ma_size)
    BEGIN
        RAISERROR('Mã size không tồn tại.', 16, 1);
        RETURN;
    END

    -- Kiểm tra mã danh mục có tồn tại không
    IF NOT EXISTS (SELECT 1 FROM DanhMucSanPham WHERE MaDanhMuc = @ma_danh_muc)
    BEGIN
        RAISERROR('Mã danh mục không tồn tại.', 16, 1);
        RETURN;
    END
    -- Kiểm tra mã sản phẩm đã tồn tại
    IF EXISTS (SELECT 1 FROM TuiXach WHERE MaSp = @ma_sp)
    BEGIN
        RAISERROR('Mã sản phẩm đã tồn tại.', 16, 1);
        RETURN;
    END

    -- Nếu có giá trị khuyến mãi, tính lại GiaSp sau khi giảm giá
    IF @khuyen_mai IS NOT NULL
    BEGIN
        SET @gia_sp = @gia_sp - (@gia_sp * @khuyen_mai / 100);
    END

    -- Thực hiện thêm sản phẩm mới vào bảng TuiXach
    INSERT INTO TuiXach (MaSp, TenSp, GiaSp, KhuyenMai, TonKho, TenMau, MaSize, MoTa, HinhAnh, MaDanhMuc)
    VALUES (@ma_sp, @ten_sp, @gia_sp, @khuyen_mai, @ton_kho, @ten_mau, @ma_size, @mo_ta, @hinh_anh, @ma_danh_muc);
END
GO

	--cập nhật sản phẩm
CREATE PROCEDURE sp_tui_xach_update
    @ma_sp CHAR(10),
    @ten_sp NVARCHAR(255) = NULL,
    @gia_sp DECIMAL(10, 2) = NULL,
    @khuyen_mai DECIMAL(18, 2) = NULL,
    @ton_kho INT = NULL,
    @ten_mau NVARCHAR(30) = NULL,
    @ma_size NVARCHAR(30) = NULL,
    @mo_ta NVARCHAR(MAX) = NULL,
    @hinh_anh VARCHAR(MAX) = NULL,
    @ma_danh_muc CHAR(10) = NULL
AS
BEGIN
    -- Kiểm tra mã sản phẩm có tồn tại không
    IF NOT EXISTS (SELECT 1 FROM TuiXach WHERE MaSp = @ma_sp)
    BEGIN
        RAISERROR('Mã sản phẩm không tồn tại.', 16, 1);
        RETURN;
    END
	 -- Nếu có giá trị khuyến mãi, tính lại GiaSp sau khi giảm giá
    IF @khuyen_mai IS NOT NULL
    BEGIN
        SET @gia_sp = @gia_sp - (@gia_sp * @khuyen_mai / 100);
    END

    -- Cập nhật các trường nếu được cung cấp giá trị
    UPDATE TuiXach
    SET TenSp = ISNULL(@ten_sp, TenSp),
        GiaSp = ISNULL(@gia_sp, GiaSp),
        KhuyenMai = ISNULL(@khuyen_mai, KhuyenMai),
        TonKho = ISNULL(@ton_kho, TonKho),
        TenMau = ISNULL(@ten_mau, TenMau),
        MaSize = ISNULL(@ma_size, MaSize),
        MoTa = ISNULL(@mo_ta, MoTa),
        HinhAnh = ISNULL(@hinh_anh, HinhAnh),
        MaDanhMuc = ISNULL(@ma_danh_muc, MaDanhMuc)
    WHERE MaSp = @ma_sp;
END
GO


	--xóa sản phẩm 
	CREATE PROCEDURE sp_tui_xach_delete
    @ma_sp CHAR(10)
	AS
	BEGIN
		-- Kiểm tra mã sản phẩm có tồn tại không
		IF NOT EXISTS (SELECT 1 FROM TuiXach WHERE MaSp = @ma_sp)
		BEGIN
			RAISERROR('Mã sản phẩm không tồn tại.', 16, 1);
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

	--lấy sp theo danh mục 
  CREATE PROCEDURE sp_get_tuixach_by_danhmuc
    @ma_DanhMuc CHAR(10)
	AS
	BEGIN
		SELECT * 
		FROM TuiXach
		WHERE MaDanhMuc = @ma_DanhMuc;
	END;

	--lấy sp theo size 
	CREATE PROCEDURE sp_get_tuixach_by_size
    @ma_Size NVARCHAR(30)
	AS
	BEGIN
		SELECT * 
		FROM TuiXach
		WHERE MaSize = @ma_Size;
	END;

	--lấy sp theo màu sắc 
	CREATE PROCEDURE sp_get_tuixach_by_mausac
    @ten_Mau NVARCHAR(30)
	AS
	BEGIN
		SELECT * 
		FROM TuiXach
		WHERE TenMau = @ten_Mau;
	END;

	--lấy all sản phẩm
	CREATE PROCEDURE sp_tui_xach_get_all
	AS
	BEGIN
		SELECT * 
		FROM TuiXach;
	END
	GO
	

	--tìm kiếm sp theo tên, màu, size, giá tiền
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
		-- Sử dụng OFFSET để phân trang
		SELECT * FROM TuiXach
		WHERE (TenSp LIKE '%' + @name + '%' OR @name = '')
		  AND (TenMau LIKE '%' + @color + '%' OR @color = '')
		  AND (MaSize = @size OR @size = '')
		  AND (GiaSp BETWEEN @min_price AND @max_price)
		ORDER BY MaSp
		OFFSET @page_index * @page_size ROWS FETCH NEXT @page_size ROWS ONLY;

		-- Trả về tổng số sản phẩm tìm kiếm được
		SELECT COUNT(*) AS TotalCount
		FROM TuiXach
		WHERE (TenSp LIKE '%' + @name + '%' OR @name = '')
		  AND (MauSac LIKE '%' + @color + '%' OR @color = '')
		  AND (MaSize = @size OR @size = '')
		  AND (GiaSp BETWEEN @min_price AND @max_price);
	END

-- Bảng HoaDon (Quản lý đơn đặt hàng)
CREATE TABLE HoaDon
(
    MaHD INT PRIMARY KEY IDENTITY(1,1),
    PerID INT, 
    NgayDatHang DATETIME DEFAULT GETDATE(), 
    TrangThai NVARCHAR(50) DEFAULT 'Đang xử lý',
    NgayNhanHang DATE Null,
    FOREIGN KEY (PerID) REFERENCES Users(PerID)
);
select * from HoaDon


-- Xóa hóa đơn
DELETE FROM HoaDon
WHERE MaHD =3;
	--thêm hóa đơn
	CREATE PROCEDURE sp_hoa_don_create
    @per_id INT,
    @trang_thai NVARCHAR(50) = 'Đang xử lý',
    @ngay_nhanhang DATE = NULL
	AS
	BEGIN
		SET NOCOUNT ON;

		BEGIN TRY
			-- Thêm hóa đơn (NgayDatHang sẽ tự động lấy giá trị từ DEFAULT)
			INSERT INTO HoaDon (PerID, TrangThai, NgayNhanHang)
			VALUES (@per_id, @trang_thai, @ngay_nhanhang);

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
    @trang_thai NVARCHAR(50),
    @ngay_nhanhang DATETIME = NULL
	AS
	BEGIN
		UPDATE HoaDon
		SET TrangThai = @trang_thai,
			NgayNhanHang = @ngay_nhanhang
		WHERE MaHD = @ma_hd;
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
	--Lấy hóa đơn theo mã
	CREATE PROCEDURE sp_hoa_don_get_by_id
    @ma_hd INT
	AS
	BEGIN
		BEGIN TRY
			SELECT MaHD, PerID, NgayBan, TrangThai
			FROM HoaDon
			WHERE MaHD = @ma_hd;
		END TRY
		BEGIN CATCH
			THROW;
		END CATCH
	END;
	-- Lấy tất cả hóa đơn
	CREATE PROCEDURE sp_hoa_don_get_all
	AS
	BEGIN
		BEGIN TRY
			SELECT MaHD, PerID, NgayDatHang, TrangThai, NgayNhanHang
			FROM HoaDon
			ORDER BY NgayDatHang DESC;
		END TRY
		BEGIN CATCH
			THROW;
		END CATCH
	END;
	-- Lấy tất cả hóa đơn-chi tiết 
	CREATE PROCEDURE sp_hoa_don_get_all_with_details
	AS
	BEGIN
		-- Lấy tất cả hóa đơn
		SELECT * FROM HoaDon;

		-- Lấy tất cả chi tiết hóa đơn
		SELECT * FROM ChiTietHoaDon;
	END
	EXEC sp_hoa_don_get_all_with_details;


	--tìm kiếm hóa đơn
	CREATE PROCEDURE sp_hoa_don_search
    @page_index INT,
    @page_size INT,
    @ma_hd INT = NULL,
    @ngay_ban DATETIME = NULL,
    @trang_thai NVARCHAR(50) = NULL
	AS
	BEGIN
		BEGIN TRY
			SELECT MaHD, PerID, NgayBan, TrangThai
			FROM HoaDon
			WHERE (@ma_hd IS NULL OR MaHD = @ma_hd) 
			  AND (@ngay_ban IS NULL OR NgayBan = @ngay_ban) 
			  AND (@trang_thai IS NULL OR TrangThai = @trang_thai)
			ORDER BY NgayBan DESC
			OFFSET (@page_index - 1) * @page_size ROWS
			FETCH NEXT @page_size ROWS ONLY;
		END TRY
		BEGIN CATCH
			THROW;
		END CATCH
	END;


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
    FOREIGN KEY (MaSp) REFERENCES TuiXach(MaSp),
    FOREIGN KEY (MaSize) REFERENCES Size(MaSize),
    FOREIGN KEY (TenMau) REFERENCES MauSac(TenMau)
);
select * from ChiTietHoaDon
	--thêm chi tiết hóa đơn
	CREATE PROCEDURE sp_chi_tiet_hoa_don_create
    @ma_hd INT,
    @ma_sp CHAR(10),
    @ten_sp NVARCHAR(255),
    @ten_mau NVARCHAR(30),  -- Nhận tên màu thay vì mã màu
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
        IF NOT EXISTS (SELECT 1 FROM TuiXach WHERE MaSp = @ma_sp)
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



	-- Lấy thông tin theo mã chi tiết
	CREATE PROCEDURE sp_chi_tiet_hoa_don_get_by_id
		@ma_hd INT
	AS
	BEGIN
		SET NOCOUNT ON;

		BEGIN TRY
			SELECT *
			FROM ChiTietHoaDon
			WHERE MaHD = @ma_hd;
		END TRY
		BEGIN CATCH
			DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
			DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
			DECLARE @ErrorState INT = ERROR_STATE();
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
		END CATCH
	END

	-- Lấy tất cả chi tiết hóa đơn
	CREATE PROCEDURE sp_chi_tiet_hoa_don_get_all
	AS
	BEGIN
		SET NOCOUNT ON;

		BEGIN TRY
			SELECT *
			FROM ChiTietHoaDon;
		END TRY
		BEGIN CATCH
			DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
			DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
			DECLARE @ErrorState INT = ERROR_STATE();
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
		END CATCH
	END

	--tìm kiếm chi tiết
	CREATE PROCEDURE sp_chi_tiet_hoa_don_search
    @page_index INT,
    @page_size INT,
    @ma_hd INT,
    @ma_sp CHAR(10)
	AS
	BEGIN
		SET NOCOUNT ON;

		DECLARE @offset INT = (@page_index - 1) * @page_size;
		DECLARE @total BIGINT;

		BEGIN TRY
			-- Tính tổng số dòng dựa trên điều kiện tìm kiếm
			SELECT @total = COUNT(*)
			FROM ChiTietHoaDon
			WHERE (@ma_hd IS NULL OR MaHD = @ma_hd)
			  AND (@ma_sp IS NULL OR MaSp = @ma_sp);

			-- Lấy dữ liệu phân trang
			SELECT *, @total AS TotalCount
			FROM ChiTietHoaDon
			WHERE (@ma_hd IS NULL OR MaHD = @ma_hd)
			  AND (@ma_sp IS NULL OR MaSp = @ma_sp)
			ORDER BY MaChiTietHD
			OFFSET @offset ROWS
			FETCH NEXT @page_size ROWS ONLY;
		END TRY
		BEGIN CATCH
			DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
			DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
			DECLARE @ErrorState INT = ERROR_STATE();
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
		END CATCH
	END


-- Bảng Bình Luận (Bình luận về sản phẩm)
CREATE TABLE BinhLuan
(
    MaBinhLuan INT PRIMARY KEY IDENTITY(1,1),
    PerID INT, -- Sử dụng PerID thay cho MaKH
    MaSp CHAR(10),
    NoiDung NVARCHAR(MAX),
    NgayBinhLuan DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (PerID) REFERENCES Users(PerID), -- Tham chiếu đến bảng Users
    FOREIGN KEY (MaSp) REFERENCES TuiXach(MaSp)
);

	select*from BinhLuan

	-- Thêm bình luận
	CREATE PROCEDURE sp_binh_luan_create
		@ma_binh_luan INT OUTPUT,
		@per_id INT, -- Sử dụng PerID
		@ma_sp CHAR(10),
		@noi_dung NVARCHAR(MAX),
		@ngay_binh_luan DATETIME
	AS
	BEGIN
		SET NOCOUNT ON;

		INSERT INTO BinhLuan (PerID, MaSp, NoiDung, NgayBinhLuan)
		VALUES (@per_id, @ma_sp, @noi_dung, @ngay_binh_luan);

		SET @ma_binh_luan = SCOPE_IDENTITY(); -- Lấy ID bình luận vừa tạo
	END

	-- Cập nhật bình luận
	CREATE PROCEDURE sp_binh_luan_update
		@ma_binh_luan INT,
		@per_id INT, -- Sử dụng PerID
		@ma_sp CHAR(10),
		@noi_dung NVARCHAR(MAX),
		@ngay_binh_luan DATETIME
	AS
	BEGIN
		SET NOCOUNT ON;

		UPDATE BinhLuan
		SET PerID = @per_id,
			MaSp = @ma_sp,
			NoiDung = @noi_dung,
			NgayBinhLuan = @ngay_binh_luan
		WHERE MaBinhLuan = @ma_binh_luan;
	END

	-- Xóa bình luận
	CREATE PROCEDURE sp_binh_luan_delete
		@ma_binh_luan INT
	AS
	BEGIN
		SET NOCOUNT ON;

		DELETE FROM BinhLuan WHERE MaBinhLuan = @ma_binh_luan;
	END

	-- Lấy bình luận theo mã
	CREATE PROCEDURE sp_binh_luan_get_by_id
		@ma_binh_luan INT
	AS
	BEGIN
		SET NOCOUNT ON;

		SELECT * FROM BinhLuan WHERE MaBinhLuan = @ma_binh_luan;
	END

	-- Lấy tất cả bình luận
	CREATE PROCEDURE sp_binh_luan_get_all
	AS
	BEGIN
		SET NOCOUNT ON;

		SELECT * FROM BinhLuan;
	END

	-- Tìm kiếm bình luận
	CREATE PROCEDURE sp_binh_luan_search
		@page_index INT,
		@page_size INT,
		@ma_binh_luan NVARCHAR(20) = NULL,
		@noi_dung NVARCHAR(MAX) = NULL
	AS
	BEGIN
		SET NOCOUNT ON;

		SELECT * FROM BinhLuan
		WHERE (@ma_binh_luan IS NULL OR MaBinhLuan = @ma_binh_luan)
		  AND (@noi_dung IS NULL OR NoiDung LIKE '%' + @noi_dung + '%')
		ORDER BY MaBinhLuan
		OFFSET (@page_index - 1) * @page_size ROWS
		FETCH NEXT @page_size ROWS ONLY;
	END

-- Bảng TinTuc (Quản lý tin tức)
CREATE TABLE TinTuc
(
    MaTinTuc INT PRIMARY KEY IDENTITY(1,1), 
    TieuDe NVARCHAR(255),
    NoiDung NVARCHAR(MAX),
    HinhAnh VARBINARY(MAX),
    NgayDang DATETIME,
    NguoiDang NVARCHAR(100)
);
	-- thêm tin tức
	CREATE PROCEDURE sp_tin_tuc_create
		@ma_tin_tuc INT OUTPUT, 
		@tieu_de NVARCHAR(255),
		@noi_dung NVARCHAR(MAX),
		@hinh_anh VARBINARY(MAX),
		@ngay_dang DATETIME,
		@nguoi_dang NVARCHAR(100)
	AS
	BEGIN
		SET NOCOUNT ON;

		INSERT INTO TinTuc (TieuDe, NoiDung, HinhAnh, NgayDang, NguoiDang)
		VALUES (@tieu_de, @noi_dung, @hinh_anh, @ngay_dang, @nguoi_dang);

		SET @ma_tin_tuc = SCOPE_IDENTITY(); -- Lấy ID tin tức vừa tạo
	END
	-- cập nhật tin tức
	CREATE PROCEDURE sp_tin_tuc_update
		@ma_tin_tuc INT, 
		@tieu_de NVARCHAR(255),
		@noi_dung NVARCHAR(MAX),
		@hinh_anh VARBINARY(MAX),
		@ngay_dang DATETIME,
		@nguoi_dang NVARCHAR(100)
	AS
	BEGIN
		SET NOCOUNT ON;

		UPDATE TinTuc
		SET TieuDe = @tieu_de,
			NoiDung = @noi_dung,
			HinhAnh = @hinh_anh,
			NgayDang = @ngay_dang,
			NguoiDang = @nguoi_dang
		WHERE MaTinTuc = @ma_tin_tuc;
	END
	-- xóa tin tức
	CREATE PROCEDURE sp_tin_tuc_delete
		@ma_tin_tuc INT 
	AS
	BEGIN
		SET NOCOUNT ON;

		DELETE FROM TinTuc WHERE MaTinTuc = @ma_tin_tuc;
	END
	-- lấy tin tức theo mã
	CREATE PROCEDURE sp_tin_tuc_get_by_id
		@ma_tin_tuc INT 
	AS
	BEGIN
		SET NOCOUNT ON;

		SELECT * FROM TinTuc WHERE MaTinTuc = @ma_tin_tuc;
	END
	-- lấy tất cả tin tức
	CREATE PROCEDURE sp_tin_tuc_get_all
	AS
	BEGIN
		SET NOCOUNT ON;

		SELECT * FROM TinTuc;
	END
	-- tìm kiếm tin tức
	CREATE PROCEDURE sp_tin_tuc_search
		@page_index INT,
		@page_size INT,
		@title NVARCHAR(255) = NULL,
		@content NVARCHAR(MAX) = NULL,
		@nguoi_dang NVARCHAR(100) = NULL
	AS
	BEGIN
		SET NOCOUNT ON;

		SELECT *
		FROM TinTuc
		WHERE (@title IS NULL OR TieuDe LIKE '%' + @title + '%')
		  AND (@content IS NULL OR NoiDung LIKE '%' + @content + '%')
		  AND (@nguoi_dang IS NULL OR NguoiDang LIKE '%' + @nguoi_dang + '%')
		ORDER BY NgayDang DESC -- Có thể thay đổi thứ tự sắp xếp theo yêu cầu
		OFFSET (@page_index - 1) * @page_size ROWS
		FETCH NEXT @page_size ROWS ONLY;
	END

