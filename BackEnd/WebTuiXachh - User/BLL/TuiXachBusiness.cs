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

     
        public TuiXachModel GetTuiXachById(string maSp) => _tuiXachRepository.GetTuiXachById(maSp);
        public List<TuiXachModel> GetDataAll() => _tuiXachRepository.GetDataAll();
        public TuiXachModel GetByDanhMuc(string maDanhMuc) => _tuiXachRepository.GetByDanhMuc(maDanhMuc);
     
        public TuiXachModel GetByMauSac(string tenMau) => _tuiXachRepository.GetByMauSac(tenMau);
       

        public TuiXachModel GetBySize(string maSize) => _tuiXachRepository.GetBySize(maSize);
      
        public List<TuiXachModel> SearchTuiXachs(int pageIndex, int pageSize, out long total, string searchTerm = "", string color = "", string size = "", decimal? minPrice = null, decimal? maxPrice = null)
      => _tuiXachRepository.Search(pageIndex, pageSize, out total, searchTerm, color, size, minPrice, maxPrice);

        
       
    }
}
