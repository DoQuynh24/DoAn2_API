using DAL;
using Model;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class HoaDonBusiness : IHoaDonBusiness
    {
        private readonly IHoaDonRepository _hoaDonRepository;

        public HoaDonBusiness(IHoaDonRepository hoaDonRepository)
        {
            _hoaDonRepository = hoaDonRepository;
        }

        public int CreateHoaDon(HoaDonModel model) => _hoaDonRepository.Create(model);
        public bool UpdateTrangThaiHoaDon(HoaDonModel model) => _hoaDonRepository.UpdateTrangThai(model);
        public List<HoaDonModel> GetByTrangThai(string trangThai)
        {
            try
            {
                return _hoaDonRepository.GetByTrangThai(trangThai);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy hóa đơn theo trạng thái.", ex);
            }
        }
        public HoaDonModel GetHoaDonById(int maHD)
        {
            return _hoaDonRepository.GetDatabyIDHD(maHD);
        }



    }
}
