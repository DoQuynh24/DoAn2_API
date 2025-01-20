using DAL;
using Model;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http; // Thêm thư viện này để sử dụng IFormFile

namespace BLL
{
    public class TuiXachBusiness : ITuiXachBusiness
    {
        private readonly ITuiXachRepository _tuiXachRepository;

        public TuiXachBusiness(ITuiXachRepository tuiXachRepository)
        {
            _tuiXachRepository = tuiXachRepository;
        }

        // Thêm túi xách với hình ảnh
        public bool CreateTuiXach(TuiXachModel model, IFormFile hinhAnhFile)
        {
            return _tuiXachRepository.Create(model, hinhAnhFile);
        }

        // Cập nhật túi xách với hình ảnh
        public bool UpdateTuiXach(TuiXachModel model, IFormFile hinhAnhFile)
        {
            return _tuiXachRepository.Update(model, hinhAnhFile);
        }

        public bool DeleteTuiXach(string maSp) => _tuiXachRepository.Delete(maSp);
        public TuiXachModel GetTuiXachById(string maSp) => _tuiXachRepository.GetTuiXachById(maSp);
        public List<TuiXachModel> GetDataAll() => _tuiXachRepository.GetDataAll();
        public TuiXachModel GetByDanhMuc(string maDanhMuc) => _tuiXachRepository.GetByDanhMuc(maDanhMuc);
     
        public TuiXachModel GetByMauSac(string tenMau) => _tuiXachRepository.GetByMauSac(tenMau);
       

        public TuiXachModel GetBySize(string maSize) => _tuiXachRepository.GetBySize(maSize);

        public List<TuiXachModel> SearchTuiXachs(int pageIndex, int pageSize, out long total, string madanhmuc = "", string masp = "", string tensp = "", string tenmau = "", string masize = "", decimal? giabanMin = null, decimal? giabanMax = null)
        {
            return _tuiXachRepository.Search(pageIndex, pageSize, out total, madanhmuc, masp, tensp, tenmau, masize, giabanMin, giabanMax);
        }





    }
}
