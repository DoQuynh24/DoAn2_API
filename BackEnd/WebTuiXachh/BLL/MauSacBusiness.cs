using DAL;
using Model;
using System.Collections.Generic;


namespace BLL
{
    public class MauSacBusiness : IMauSacBusiness
    {
        private readonly IMauSacRepository _mauSacRepository;

        public MauSacBusiness(IMauSacRepository mauSacRepository)
        {
            _mauSacRepository = mauSacRepository;
        }

        public bool CreateMauSac(MauSacModel model) => _mauSacRepository.Create(model);
        public bool UpdateMauSac(MauSacModel model) => _mauSacRepository.Update(model);
        public bool DeleteMauSac(string tenMau) => _mauSacRepository.Delete(tenMau);
        public List<MauSacModel> GetAllMauSac() => _mauSacRepository.GetDataAll();
        public List<MauSacModel> SearchMauSacs(int pageIndex, int pageSize, out long total, string tenMau)
        {
            return _mauSacRepository.Search(pageIndex, pageSize, out total ,tenMau);
        }
    }
}
