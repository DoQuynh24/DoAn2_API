using DAL.Helper;
using Microsoft.Identity.Client;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DAL
{
    public partial class HoaDonRepository : IHoaDonRepository
    {
        private IDatabaseHelper _dbHelper;

        public HoaDonRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        // Thêm hóa đơn-chi tiết hóa đơn
        public int Create(HoaDonModel model)
        {
            string msgError = "";
            try
            {
                // Tạo hóa đơn
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_hoa_don_create",
                    "@per_id", model.PerID,
                    "@ho_ten", model.HoTen,
                    "@dia_chi", model.DiaChi, 
                    "@sdt", model.SDT,
                    "@trang_thai", string.IsNullOrEmpty(model.TrangThai) ? "Đang xử lý" : model.TrangThai,
                    "@ngay_nhanhang", model.TrangThai == "Hoàn thành" ? DateTime.Now : (object)DBNull.Value);


                if ((result == null || string.IsNullOrEmpty(result.ToString())) && !string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(msgError);
                }

                // Lấy mã hóa đơn vừa tạo
                int maHD = Convert.ToInt32(result);

                // Thêm danh sách chi tiết hóa đơn
                if (model.ChiTietHoaDons != null && model.ChiTietHoaDons.Count > 0)
                {
                    foreach (var chiTiet in model.ChiTietHoaDons)
                    {
                        var detailResult = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_chi_tiet_hoa_don_create",
                            "@ma_hd", maHD,
                            "@ma_sp", chiTiet.MaSp,
                            "@ten_sp", chiTiet.TenSp,
                            "@ten_mau", chiTiet.TenMau,
                            "@ma_size", chiTiet.MaSize,
                            "@so_luong", chiTiet.SoLuong,
                            "@gia_ban", chiTiet.GiaBan,
                            "@ghi_chu", chiTiet.GhiChu,
                            "@khuyen_mai", chiTiet.KhuyenMai);


                        if (!string.IsNullOrEmpty(msgError))
                        {
                            throw new Exception(msgError);
                        }
                    }
                }

                return maHD; // Trả về mã hóa đơn
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Cập nhật thông tin hóa đơn chỉ cập nhật trạng thái
        public bool UpdateTrangThai(HoaDonModel model)
        {
            string msgError = "";
            try
            {
                // Kiểm tra nếu trạng thái là "Hoàn thành", gán giá trị NgayNhanHang
                DateTime? ngayNhanHang = model.TrangThai == "Hoàn thành" ? DateTime.Now : (DateTime?)null;

                // Cập nhật trạng thái hóa đơn
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_hoa_don_update_trang_thai",
                    "@ma_hd", model.MaHD,
                    "@trang_thai", model.TrangThai);

                if ((result == null || string.IsNullOrEmpty(result.ToString())) && !string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(msgError);
                }

               

                return true;
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                throw new Exception("Lỗi khi cập nhật trạng thái hóa đơn.", ex);
            }
        }

        public HoaDonModel GetDatabyIDHD(int maHD)
        {
            string msgError = "";
            try
            {
                var dtHoaDon = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_hoa_don_get_by_id", "@ma_hd", maHD);
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                var hoaDon = dtHoaDon.ConvertTo<HoaDonModel>().FirstOrDefault();
                if (hoaDon == null) return null;

                var dtChiTiet = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_ct_hoa_don_get_by_ma_hd", "@ma_hd", maHD);
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                hoaDon.ChiTietHoaDons = dtChiTiet.ConvertTo<ChiTietHoaDonModel>().ToList();
                return hoaDon;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
 
        public List<HoaDonModel> GetByTrangThai(string trangThai)
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_hoa_don_get_by_trangthai", "@trang_thai", trangThai);
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                return dt.ConvertTo<HoaDonModel>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
  
    }
}
