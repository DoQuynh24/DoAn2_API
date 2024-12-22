using DAL;
using Model;
using System.Collections.Generic;

namespace BLL
{
    public class SizeBusiness : ISizeBusiness
    {
        private readonly ISizeRepository _sizeRepository;

        public SizeBusiness(ISizeRepository sizeRepository)
        {
            _sizeRepository = sizeRepository;
        }

        public bool CreateSize(SizeModel model) => _sizeRepository.Create(model);
        public bool UpdateSize(SizeModel model) => _sizeRepository.Update(model);
        public bool DeleteSize(string maSize) => _sizeRepository.Delete(maSize);
        public SizeModel GetSizeById(string maSize) => _sizeRepository.GetDatabyID(maSize);
        public List<SizeModel> GetAllSizes() => _sizeRepository.GetDataAll();
        // Triển khai phương thức tìm kiếm
        public List<SizeModel> SearchSizes(int pageIndex, int pageSize, out long total, string searchCriteria)
        {
            return _sizeRepository.Search(pageIndex, pageSize, out total, searchCriteria);
        }
    }
}
