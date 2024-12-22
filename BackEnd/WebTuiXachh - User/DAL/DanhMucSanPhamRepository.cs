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

      
     


    }
}
