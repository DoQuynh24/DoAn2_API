using DAL.Helper;
using Microsoft.AspNetCore.Http;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;


namespace DAL
{
    public partial class TuiXachRepository : ITuiXachRepository
    {
        private IDatabaseHelper _dbHelper;
      


        public TuiXachRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
       
        // Phương thức tạo mới sản phẩm túi xách
        public bool Create(TuiXachModel model, IFormFile hinhAnhFile)
        {
            string msgError = "";
            try
            {
                // Đọc hình ảnh từ IFormFile và lưu tên tệp hoặc đường dẫn
                string hinhAnhFileName = null;

                if (hinhAnhFile != null && hinhAnhFile.Length > 0)
                {
                    // Đặt tên tệp theo định dạng mới hoặc sử dụng tên tệp gốc
                    hinhAnhFileName = Path.GetFileName(hinhAnhFile.FileName);

                    // Lưu ảnh vào thư mục trên server nếu cần thiết, hoặc lưu tên ảnh vào CSDL
                    var filePath = Path.Combine("D:\\API\\WebTuiXachh\\images", hinhAnhFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        hinhAnhFile.CopyTo(stream);
                    }
                }

                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_tui_xach_create",
                    "@ma_danh_muc", model.MaDanhMuc,
                    "@ma_sp", model.MaSp,
                    "@ten_sp", model.TenSp,
                    "@ten_mau", model.TenMau,
                    "@ma_size", model.MaSize,
                    "@gia_ban", model.GiaBan,
                    "@khuyen_mai", model.KhuyenMai,
                    "@ton_kho", model.TonKho,
                    "@mo_ta", model.MoTa,
                    "@hinh_anh", hinhAnhFileName); 
                   

                if ((result != null && !string.IsNullOrEmpty(result.ToString())) || !string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(Convert.ToString(result) + msgError);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Cập nhật phương thức Update
        public bool Update(TuiXachModel model, IFormFile hinhAnhFile)
        {
            string msgError = "";
            try
            {
                string hinhAnhFileName = null;

                // Kiểm tra xem hình ảnh có được cung cấp không
                if (hinhAnhFile != null && hinhAnhFile.Length > 0)
                {
                    hinhAnhFileName = Path.GetFileName(hinhAnhFile.FileName);

                    // Lưu ảnh vào thư mục trên server
                    var filePath = Path.Combine("D:\\API\\WebTuiXachh\\images", hinhAnhFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        hinhAnhFile.CopyTo(stream);
                    }
                }

                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_tui_xach_update",
                    "@ma_danh_muc", string.IsNullOrEmpty(model.MaDanhMuc) ? DBNull.Value : (object)model.MaDanhMuc,
                    "@ma_sp", model.MaSp,
                    "@ten_sp", string.IsNullOrEmpty(model.TenSp) ? DBNull.Value : (object)model.TenSp,
                    "@ten_mau", string.IsNullOrEmpty(model.TenMau) ? DBNull.Value : (object)model.TenMau,
                    "@ma_size", string.IsNullOrEmpty(model.MaSize) ? DBNull.Value : (object)model.MaSize,
                    "@gia_ban", model.GiaBan == 0 ? DBNull.Value : (object)model.GiaBan,
                    "@khuyen_mai", model.KhuyenMai == 0 ? DBNull.Value : (object)model.KhuyenMai,
                    "@ton_kho", model.TonKho == 0 ? DBNull.Value : (object)model.TonKho,
                    "@mo_ta", string.IsNullOrEmpty(model.MoTa) ? DBNull.Value : (object)model.MoTa,
                    "@hinh_anh", string.IsNullOrEmpty(hinhAnhFileName) ? DBNull.Value : (object)hinhAnhFileName);
                   

                if (!string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(msgError);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        // Phương thức xóa sản phẩm túi xách theo mã sản phẩm
        public bool Delete(string maSp)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_tui_xach_delete",
                    "@ma_sp", maSp);

                // Kiểm tra lỗi
                if ((result != null && !string.IsNullOrEmpty(result.ToString())) || !string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(Convert.ToString(result) + msgError);
                }
                return true;  // Xóa thành công
            }
            catch (Exception ex)
            {
                throw ex;  // Ném lại lỗi
            }
        }
        // Phương thức lấy tất cả sản phẩm túi xách
        public List<TuiXachModel> GetDataAll()
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_tui_xach_get_all");

                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                return dt.ConvertTo<TuiXachModel>().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDataAll: {ex.Message}");
                throw;  // Ném lại lỗi để giữ nguyên thông tin
            }
        }


        // Phương thức lấy thông tin túi xách theo mã
        public TuiXachModel GetTuiXachById(string maSp)
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_tui_xach_get_by_id",
                    "@ma_sp", maSp);

                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                return dt.ConvertTo<TuiXachModel>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        // Lấy sản phẩm theo danh mục
        public TuiXachModel GetByDanhMuc(string maDanhMuc)
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_get_tuixach_by_danhmuc",
                    "@ma_danhmuc", maDanhMuc);

                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                return dt.ConvertTo<TuiXachModel>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Lấy sản phẩm theo màu sắc
        public TuiXachModel GetByMauSac(string tenMau)
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_get_tuixach_by_mausac",
                    "@ten_mau", tenMau);

                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                return dt.ConvertTo<TuiXachModel>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        // Lấy sản phẩm theo size

        public TuiXachModel GetBySize(string maSize)
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_get_tuixach_by_size",
                    "@ma_size", maSize);

                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                return dt.ConvertTo<TuiXachModel>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Phương thức tìm kiếm sản phẩm túi xách theo các tiêu chí
        public List<TuiXachModel> Search(int pageIndex, int pageSize, out long total, string name, string color, string size, decimal? minPrice, decimal? maxPrice)
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_tui_xach_search",
                    "@page_index", pageIndex,
                    "@page_size", pageSize,
                    "@name", name ?? string.Empty,
                    "@color", color ?? string.Empty,
                    "@size", size ?? string.Empty,
                    "@min_price", minPrice ?? 0,
                    "@max_price", maxPrice ?? decimal.MaxValue);

                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                total = dt.Rows.Count; // Tổng số kết quả tìm kiếm

                return dt.ConvertTo<TuiXachModel>().ToList();
            }
            catch (Exception ex)
            {
                total = 0;
                throw ex;
            }
        }


        

       
    }
}
