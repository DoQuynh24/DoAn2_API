using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DAL.Helper;
using Model;

namespace DAL
{
    public partial class DonHangNhapRepository : IDonHangNhapRepository 
    {
        private IDatabaseHelper _dbHelper;

        public DonHangNhapRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }


        public int Create(DonHangNhapModel model)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_don_hang_nhap_create",
                    "@ten_ncc", model.TenNCC);



                if ((result == null || string.IsNullOrEmpty(result.ToString())) && !string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(msgError);
                }


                int maDHN = Convert.ToInt32(result);

                if (model.ChiTietDonHangNhaps != null && model.ChiTietDonHangNhaps.Count > 0)
                {
                    foreach (var chiTiet in model.ChiTietDonHangNhaps)
                    {
                        var detailResult = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_chi_tiet_don_hang_nhap_create",
                            "@ma_dhn", maDHN,
                            "@ma_sp", chiTiet.MaSp,
                            "@ten_sp", chiTiet.TenSp,
                            "@gia_nhap", chiTiet.GiaNhap,
                            "@so_luong", chiTiet.SoLuong);
                   
                        if (!string.IsNullOrEmpty(msgError))
                        {
                            throw new Exception(msgError);
                        }
                    }
                }

                return maDHN; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public bool Update(DonHangNhapModel model)
        {
            string msgError = "";
            try
            {
                // Cập nhật thông tin Đơn Hàng Nhập
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_don_hang_nhap_update",
                    "@ma_dhn", model.MaDHN,
                    "@ten_ncc", model.TenNCC,
                    "@ngay_nhap", model.NgayNhap);

                if (!string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(msgError); // Ném lỗi nếu stored procedure trả về lỗi
                }

                // Cập nhật hoặc thêm mới các Chi Tiết Đơn Hàng Nhập
                if (model.ChiTietDonHangNhaps != null && model.ChiTietDonHangNhaps.Count > 0)
                {
                    foreach (var chiTiet in model.ChiTietDonHangNhaps)
                    {
                        // Kiểm tra nếu chi tiết đã tồn tại thì cập nhật, nếu không thì thêm mới
                        var detailResult = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_chi_tiet_don_hang_nhap_update",
                            "@ma_dhn", model.MaDHN,
                            "@ma_sp", chiTiet.MaSp,
                            "@ten_sp", chiTiet.TenSp,
                            "@gia_nhap", chiTiet.GiaNhap,
                            "@so_luong", chiTiet.SoLuong);

                        if (!string.IsNullOrEmpty(msgError))
                        {
                            throw new Exception(msgError); // Ném lỗi nếu chi tiết có vấn đề
                        }
                    }
                }

                // Xóa các Chi Tiết Đơn Hàng Nhập nếu có trong danh sách ChiTietDonHangNhapsToDelete
                if (model.ChiTietDonHangNhapsToDelete != null && model.ChiTietDonHangNhapsToDelete.Count > 0)
                {
                    foreach (var maSp in model.ChiTietDonHangNhapsToDelete)
                    {
                        var deleteResult = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_chi_tiet_don_hang_nhap_delete",
                            "@ma_dhn", model.MaDHN,
                            "@ma_sp", maSp);

                        if (!string.IsNullOrEmpty(msgError))
                        {
                            throw new Exception(msgError); // Ném lỗi nếu có vấn đề khi xóa chi tiết
                        }
                    }
                }

                return true; // Cập nhật thành công
            }
            catch (Exception ex)
            {
                // Ném lỗi với thông tin chi tiết
                throw new Exception("Lỗi khi cập nhật Đơn Hàng Nhập hoặc Chi Tiết Đơn Hàng Nhập.", ex);
            }
        }



        public List<DonHangNhapModel> GetAll()
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_don_hang_nhap_get_all");
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                return dt.ConvertTo<DonHangNhapModel>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DonHangNhapModel GetDatabyIDDHN(int maDHN)
        {
            string msgError = "";
            try
            {
                // Lấy thông tin đơn hàng nhập
                var dtDonHang = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_don_hang_nhap_get_by_id", "@ma_dhn", maDHN);
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                var donHangNhap = dtDonHang.ConvertTo<DonHangNhapModel>().FirstOrDefault();
                if (donHangNhap == null) return null;

                // Lấy danh sách chi tiết đơn hàng nhập
                var dtChiTiet = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_ct_don_hang_nhap_get_by_ma_dhn", "@ma_dhn", maDHN);
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                donHangNhap.ChiTietDonHangNhaps = dtChiTiet.ConvertTo<ChiTietDonHangNhapModel>().ToList();
                return donHangNhap;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //public ChiTietDonHangNhapModel GetDatabyID(int maDHN)
        //{
        //    string msgError = "";
        //    try
        //    {
        //        var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_ct_don_hang_nhap_get_by_ma_dhn",
        //            "@ma_dhn", maDHN);


        //        if (!string.IsNullOrEmpty(msgError))
        //            throw new Exception(msgError);

        //        return dt.ConvertTo<ChiTietDonHangNhapModel>().FirstOrDefault();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
