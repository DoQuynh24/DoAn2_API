using Model;
using System;
using System.Collections.Generic;

namespace DAL
{
    public interface INhaCungCapRepository
    {
       
        bool Create(NhaCungCapModel model);

        bool Update(NhaCungCapModel model);

        bool Delete(string tenNCC);

        NhaCungCapModel GetDatabyName(string tenNCC);

        List<NhaCungCapModel> GetDataAll();
    }
}
