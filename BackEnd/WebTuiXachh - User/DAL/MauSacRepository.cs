using DAL.Helper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DAL
{
    public partial class MauSacRepository : IMauSacRepository
    {
        private IDatabaseHelper _dbHelper;

        public MauSacRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
      
        public List<MauSacModel> GetDataAll()
        {
            string msgError = "";
            try
            {

                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_mau_sac_get_all");

                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                return dt.ConvertTo<MauSacModel>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       


        public List<MauSacModel> Search(int pageIndex, int pageSize, out long total, string tenMau)
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_mau_sac_search",
                    "@page_index", pageIndex,
                    "@page_size", pageSize,
                    "@ten_mau", tenMau);

                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                total = dt.Rows.Count;

                return dt.ConvertTo<MauSacModel>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
