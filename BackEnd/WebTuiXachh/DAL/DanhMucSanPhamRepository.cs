using DAL.Helper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public partial class DanhMucSanPhamRepository : IDanhMucSanPhamRepository
    {
        private IDatabaseHelper _dbHelper;

        public DanhMucSanPhamRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public bool Create(DanhMucSanPhamModel model)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_danh_muc_san_pham_create",
                    "@ma_danh_muc", model.MaDanhMuc,
                    "@ten_danh_muc", model.TenDanhMuc);

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

        public bool Update(DanhMucSanPhamModel model)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_danh_muc_san_pham_update",
                    "@ma_danh_muc", model.MaDanhMuc,
                    "@ten_danh_muc", model.TenDanhMuc);

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

        public bool Delete(string maDanhMuc)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_danh_muc_san_pham_delete",
                    "@ma_danh_muc", maDanhMuc);

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
        public List<DanhMucSanPhamModel> GetDataAll()
        {
            string msgError = "";
            try
            {
                
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_danh_muc_san_pham_get_all");

                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                return dt.ConvertTo<DanhMucSanPhamModel>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DanhMucSanPhamModel GetDatabyID(string maDanhMuc)
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_danh_muc_san_pham_get_by_id",
                    "@ma_danh_muc", maDanhMuc);

                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                return dt.ConvertTo<DanhMucSanPhamModel>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      
        public List<DanhMucSanPhamModel> Search(int pageIndex, int pageSize, out long total, string maDanhMuc, string tenDanhMuc)
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_danh_muc_san_pham_search",
                    "@page_index", pageIndex,
                    "@page_size", pageSize,
                    "@ma_danh_muc", maDanhMuc,
                    "@ten_danh_muc", tenDanhMuc);

                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                total = dt.Rows.Count; 

                return dt.ConvertTo<DanhMucSanPhamModel>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
