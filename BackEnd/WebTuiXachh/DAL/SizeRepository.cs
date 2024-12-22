using DAL.Helper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public partial class SizeRepository : ISizeRepository
    {
        private IDatabaseHelper _dbHelper;

        public SizeRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public bool Create(SizeModel model)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_size_create",
                    "@ma_size", model.MaSize);

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
        // Phương thức Delete
        public bool Delete(string maSize)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_size_delete",
                    "@ma_size", maSize);

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
        public SizeModel GetDatabyID(string maSize)
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_size_get_by_id",
                    "@ma_size", maSize);

                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                return dt.ConvertTo<SizeModel>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Phương thức lấy tất cả kích thước
        public List<SizeModel> GetDataAll()
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_size_get_all");

                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                return dt.ConvertTo<SizeModel>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Phương thức tìm kiếm kích thước với phân trang và các tiêu chí tìm kiếm
        public List<SizeModel> Search(int pageIndex, int pageSize, out long total, string searchCriteria)
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_size_search",
                    "@page_index", pageIndex,
                    "@page_size", pageSize,
                    "@search_criteria", searchCriteria);

                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                total = Convert.ToInt64(dt.Rows[0]["TotalCount"]); // Assuming total count is returned in a column named "TotalCount"

                return dt.ConvertTo<SizeModel>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Phương thức cập nhật kích thước
        public bool Update(SizeModel model)
        {
            string msgError = "";
            try
            {
                // Sửa lại thủ tục lưu trữ để chỉ sử dụng tham số MaSize
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_size_update",
                    "@ma_size", model.MaSize); // Chỉ cần tham số MaSize

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

    }
}
