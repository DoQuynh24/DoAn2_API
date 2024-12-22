using Model;
using System;
using System.Collections.Generic;

namespace BLL
{
    public interface IDonHangNhapBusiness
    {
        int CreateDonHangNhap(DonHangNhapModel model);
        bool UpdateDonHangNhap(DonHangNhapModel model);
        List<DonHangNhapModel> GetAllDonHangNhap();
        DonHangNhapModel GetDonHangNhapById(int MaDHN);
        //ChiTietDonHangNhapModel GetChiTietDonHangNhapById(int MaDHN);

      
    }
}
