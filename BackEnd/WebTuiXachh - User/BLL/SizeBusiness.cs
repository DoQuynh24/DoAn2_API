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

      
        public List<SizeModel> GetAllSizes() => _sizeRepository.GetDataAll();
       
        public List<SizeModel> SearchSizes(int pageIndex, int pageSize, out long total, string searchCriteria)
        {
            return _sizeRepository.Search(pageIndex, pageSize, out total, searchCriteria);
        }
    }
}
