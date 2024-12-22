using Model;
using System.Collections.Generic;

namespace BLL
{
    public interface ISizeBusiness
    {
        List<SizeModel> GetAllSizes();
        List<SizeModel> SearchSizes(int pageIndex, int pageSize, out long total, string searchCriteria);
    }
}
