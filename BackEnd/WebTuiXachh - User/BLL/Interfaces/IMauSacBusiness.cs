using Model;
using System.Collections.Generic;


namespace BLL
{
    public interface IMauSacBusiness
    {
        List<MauSacModel> GetAllMauSac();
        List<MauSacModel> SearchMauSacs(int pageIndex, int pageSize, out long total, string tenMau);
    }
}
