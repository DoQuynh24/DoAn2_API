using Model;
using System;
using System.Collections.Generic;

namespace BLL
{
    public interface IHoaDonBusiness
    {
        int CreateHoaDon(HoaDonModel model);
        bool UpdateTrangThaiHoaDon(HoaDonModel model);
        List<HoaDonModel> GetByTrangThai(string trangThai);
        HoaDonModel GetHoaDonById(int maHD);
   


    }
}
