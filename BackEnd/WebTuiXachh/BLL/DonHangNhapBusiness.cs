using DAL;
using Model;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class DonHangNhapBusiness : IDonHangNhapBusiness
    {
        private readonly IDonHangNhapRepository _donHangNhapRepository;

        public DonHangNhapBusiness(IDonHangNhapRepository donHangNhapRepository)
        {
            _donHangNhapRepository = donHangNhapRepository;
        }

        public int CreateDonHangNhap(DonHangNhapModel model) => _donHangNhapRepository.Create(model);

        public bool UpdateDonHangNhap(DonHangNhapModel model) => _donHangNhapRepository.Update(model);
        public List<DonHangNhapModel> GetAllDonHangNhap() => _donHangNhapRepository.GetAll();
        public DonHangNhapModel GetDonHangNhapById(int maDHN)
        {
            return _donHangNhapRepository.GetDatabyIDDHN(maDHN);
        }


    }
}
