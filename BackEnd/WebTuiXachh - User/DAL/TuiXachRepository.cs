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
