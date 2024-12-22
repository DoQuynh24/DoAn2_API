using Model;
using System.Collections.Generic;

namespace BLL
{
    public interface ISizeBusiness
    {
        bool CreateSize(SizeModel model);
        bool UpdateSize(SizeModel model);
        bool DeleteSize(string maSize);
        SizeModel GetSizeById(string maSize);
        List<SizeModel> GetAllSizes();
        List<SizeModel> SearchSizes(int pageIndex, int pageSize, out long total, string searchCriteria);
    }
}
