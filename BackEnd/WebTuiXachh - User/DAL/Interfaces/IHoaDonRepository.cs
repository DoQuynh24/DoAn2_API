using Model;
using System;
using System.Collections.Generic;

namespace DAL
{
    public interface IHoaDonRepository
    {
        int Create(HoaDonModel model);
        bool UpdateTrangThai(HoaDonModel model);
        HoaDonModel GetDatabyIDHD(int maHD);
        List<HoaDonModel> GetByTrangThai(string trangThai);

    }
}
