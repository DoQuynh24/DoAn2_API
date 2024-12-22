using DAL;
using Model;
using System.Collections.Generic;

namespace BLL
{
    public class DanhMucSanPhamBusiness : IDanhMucSanPhamBusiness
    {
        private readonly IDanhMucSanPhamRepository _danhMucSanPhamRepository;

        public DanhMucSanPhamBusiness(IDanhMucSanPhamRepository danhMucSanPhamRepository)
        {
            _danhMucSanPhamRepository = danhMucSanPhamRepository;
        }

     
        public DanhMucSanPhamModel GetDanhMucById(string MaDanhMuc) => _danhMucSanPhamRepository.GetDatabyID(MaDanhMuc);
        public List<DanhMucSanPhamModel> GetAllDanhMuc() => _danhMucSanPhamRepository.GetDataAll();
    

    }
}
