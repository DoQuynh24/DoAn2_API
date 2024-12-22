using Model;
using System;
using System.Collections.Generic;

namespace DAL
{
    public interface IDonHangNhapRepository
    {
       
        int Create(DonHangNhapModel model);
        bool Update(DonHangNhapModel model);
        List<DonHangNhapModel> GetAll();
        DonHangNhapModel GetDatabyIDDHN(int maDHN);
        //ChiTietDonHangNhapModel GetDatabyID(int maDHN);


    }
}
