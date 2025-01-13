using DAL;
using Model;
using System.Collections.Generic;

namespace BLL
{
    public class BinhLuanBusiness : IBinhLuanBusiness
    {
        private readonly IBinhLuanRepository _binhLuanRepository;

        public BinhLuanBusiness(IBinhLuanRepository binhLuanRepository)
        {
            _binhLuanRepository = binhLuanRepository;
        }

        public bool CreateBinhLuan(BinhLuanModel model) => _binhLuanRepository.Create(model);
        public bool UpdateBinhLuan(BinhLuanModel model) => _binhLuanRepository.Update(model);
        public bool DeleteBinhLuan(int id) => _binhLuanRepository.Delete(id);
        public BinhLuanModel GetBinhLuanById(int id) => _binhLuanRepository.GetDatabyID(id);
        public List<BinhLuanModel> GetAllBinhLuan() => _binhLuanRepository.GetDataAll();
        public List<BinhLuanModel> Search(int pageIndex, int pageSize, out long total, string maSP, int perID)
        {
            return _binhLuanRepository.Search(pageIndex, pageSize, out total, maSP, perID);
        }
    }
}
