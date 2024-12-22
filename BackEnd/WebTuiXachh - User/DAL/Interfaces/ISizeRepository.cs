using Model;
using System;
using System.Collections.Generic;

namespace DAL
{
    public interface ISizeRepository
    {
       
        List<SizeModel> GetDataAll();
        List<SizeModel> Search(int pageIndex, int pageSize, out long total, string maSize);
    }
}
