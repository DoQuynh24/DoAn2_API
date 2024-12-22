using DAL;
using Model;
using System.Collections.Generic;

namespace BLL
{
    public class NhaCungCapBusiness : INhaCungCapBusiness
    {
        private readonly INhaCungCapRepository _nhaCungCapRepository;

        public NhaCungCapBusiness(INhaCungCapRepository nhaCungCapRepository)
        {
            _nhaCungCapRepository = nhaCungCapRepository;
        }

        public bool CreateNCC(NhaCungCapModel model) => _nhaCungCapRepository.Create(model);
        public bool UpdateNCC(NhaCungCapModel model) => _nhaCungCapRepository.Update(model);
        public bool DeleteNCC(string tenNCC) => _nhaCungCapRepository.Delete(tenNCC);
        public NhaCungCapModel GetNCCByName(string tenNCC) => _nhaCungCapRepository.GetDatabyName(tenNCC);
        public List<NhaCungCapModel> GetAllNCC() => _nhaCungCapRepository.GetDataAll();
        

    }
}
