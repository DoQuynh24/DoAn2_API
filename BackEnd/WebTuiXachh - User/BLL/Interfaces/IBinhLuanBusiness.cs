using Model;
using System.Collections.Generic;

namespace BLL
{
    public interface IBinhLuanBusiness
    {
        bool CreateBinhLuan(BinhLuanModel model);
        bool UpdateBinhLuan(BinhLuanModel model);
        bool DeleteBinhLuan(int id);
        BinhLuanModel GetBinhLuanById(int id);
        List<BinhLuanModel> GetAllBinhLuan();
        List<BinhLuanModel> Search(int pageIndex, int pageSize, out long total, string maSP, int perID);
    }
}
