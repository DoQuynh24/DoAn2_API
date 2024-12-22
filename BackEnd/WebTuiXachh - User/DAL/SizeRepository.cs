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

                total = Convert.ToInt64(dt.Rows[0]["TotalCount"]); 

                return dt.ConvertTo<SizeModel>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
