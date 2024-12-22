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

        public bool CreateDanhMuc(DanhMucSanPhamModel model) => _danhMucSanPhamRepository.Create(model);
        public bool UpdateDanhMuc(DanhMucSanPhamModel model) => _danhMucSanPhamRepository.Update(model);
        public bool DeleteDanhMuc(string MaDanhMuc) => _danhMucSanPhamRepository.Delete(MaDanhMuc);
        public DanhMucSanPhamModel GetDanhMucById(string MaDanhMuc) => _danhMucSanPhamRepository.GetDatabyID(MaDanhMuc);
        public List<DanhMucSanPhamModel> GetAllDanhMuc() => _danhMucSanPhamRepository.GetDataAll();
        public List<DanhMucSanPhamModel> SearchDanhMucs(int pageIndex, int pageSize, out long total, string maDanhMuc, string tenDanhMuc)
        {
            return _danhMucSanPhamRepository.Search(pageIndex, pageSize, out total, maDanhMuc, tenDanhMuc);
        }


    }
}
