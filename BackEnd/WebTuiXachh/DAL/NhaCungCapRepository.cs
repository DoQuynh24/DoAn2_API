using DAL.Helper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public partial class NhaCungCapRepository : INhaCungCapRepository
    {
        private IDatabaseHelper _dbHelper;

        public NhaCungCapRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public bool Create(NhaCungCapModel model)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_nha_cung_cap_create",
                    "@ten_ncc", model.TenNCC,
                    "@dia_chi", string.IsNullOrEmpty(model.DiaChi) ? "Chưa cập nhật" : model.DiaChi,
                    "@lien_he", string.IsNullOrEmpty(model.LienHe) ? "Chưa cập nhật" : model.LienHe);

                if (!string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(msgError);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in CreateNCC: {ex.Message}", ex);
            }
        }

        public bool Update(NhaCungCapModel model)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_nha_cung_cap_update",
                    "@ten_ncc", model.TenNCC,
                    "@dia_chi", string.IsNullOrEmpty(model.DiaChi) ? (object)DBNull.Value : model.DiaChi,
                    "@lien_he", string.IsNullOrEmpty(model.LienHe) ? (object)DBNull.Value : model.LienHe);

                if (!string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(msgError);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in UpdateNCC: {ex.Message}", ex);
            }
        }

        public bool Delete(string tenNCC)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_nha_cung_cap_delete",
                    "@ten_ncc", tenNCC);

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
        public List<NhaCungCapModel> GetDataAll()
        {
            string msgError = "";
            try
            {

                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_nha_cung_cap_get_all");

                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                return dt.ConvertTo<NhaCungCapModel>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NhaCungCapModel GetDatabyName(string tenNCC)
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_danh_muc_san_pham_get_by_id",
                    "@ten_ncc", tenNCC);

                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                return dt.ConvertTo<NhaCungCapModel>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
