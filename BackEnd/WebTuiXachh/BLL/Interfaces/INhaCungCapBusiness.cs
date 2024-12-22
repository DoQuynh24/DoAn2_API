using Model;
using System.Collections.Generic;

namespace BLL
{
    public interface INhaCungCapBusiness
    {
        bool CreateNCC(NhaCungCapModel model);
        bool UpdateNCC(NhaCungCapModel model);
        bool DeleteNCC(string tenNCC);
        NhaCungCapModel GetNCCByName(string tenNCC);
        List<NhaCungCapModel> GetAllNCC();

    }
}
