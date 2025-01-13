using DAL.Helper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public partial class BinhLuanRepository : IBinhLuanRepository
    {
        private IDatabaseHelper _dbHelper;

        public BinhLuanRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public bool Create(BinhLuanModel model)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_binh_luan_create",
                    "@per_id", model.PerID,
                    "@ma_sp", model.MaSp,
                    "@noi_dung", model.NoiDung);
                 

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

        public BinhLuanModel GetDatabyID(int maBinhLuan)
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_binh_luan_get_by_id",
                    "@ma_binh_luan", maBinhLuan);

                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                return dt.ConvertTo<BinhLuanModel>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Phương thức xóa bình luận theo mã bình luận
        public bool Delete(int maBinhLuan)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_binh_luan_delete",
                    "@ma_binh_luan", maBinhLuan);

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

        // Phương thức cập nhật thông tin bình luận
        public bool Update(BinhLuanModel model)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_binh_luan_update",
                    "@ma_binh_luan", model.MaBinhLuan,
                    "@per_id", model.PerID,
                    "@ma_sp", model.MaSp,
                    "@noi_dung", model.NoiDung,
                    "@ngay_binh_luan", model.NgayBinhLuan);

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

        // Phương thức tìm kiếm bình luận với các tiêu chí
        public List<BinhLuanModel> Search(int pageIndex, int pageSize, out long total, string maSP, int perID)
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_binh_luan_search",
                    "@page_index", pageIndex,
                    "@page_size", pageSize,
                    "@ma_sp", maSP, 
                    "@per_id", perID); 

                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                total = dt.Rows.Count;

                return dt.ConvertTo<BinhLuanModel>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BinhLuanModel> GetDataAll()
        {
            string msgError = "";
            try
            {
               
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_binh_luan_get_all");

                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                return dt.ConvertTo<BinhLuanModel>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
