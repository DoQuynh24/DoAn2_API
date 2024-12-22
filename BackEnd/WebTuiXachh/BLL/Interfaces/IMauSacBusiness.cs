using Model;
using System.Collections.Generic;


namespace BLL
{
    public interface IMauSacBusiness
    {
        bool CreateMauSac(MauSacModel model);
        bool UpdateMauSac(MauSacModel model);
        bool DeleteMauSac(string tenMau);
        List<MauSacModel> GetAllMauSac();
        List<MauSacModel> SearchMauSacs(int pageIndex, int pageSize, out long total, string tenMau);
    }
}
