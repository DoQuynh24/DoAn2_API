using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL;
using Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;

namespace WebTuiXachh.Controllers
{
    //[Authorize] // Bảo vệ tất cả các API trong controller này
    [ApiController]
    [Route("api/[controller]")]
    public class HoaDonController : ControllerBase
    {
        private readonly IHoaDonBusiness _hoaDonBusiness;

        public HoaDonController(IHoaDonBusiness hoaDonBusiness)
        {
            _hoaDonBusiness = hoaDonBusiness;
        }

        // API: POST /api/hoadon (Tạo mới hóa đơn)
        [HttpPost("create")]
        public IActionResult Create([FromBody] HoaDonModel model)
        {
            try
            {
                // Nếu không có NgayDatHang, sử dụng ngày mặc định được tự động gán từ SQL Server
                if (model.NgayDatHang == null)
                {
                    model.NgayDatHang = DateTime.Now; // Nếu không có giá trị, tự động lấy ngày hiện tại
                }
                if (string.IsNullOrEmpty(model.TrangThai))
                {
                    model.TrangThai = "Đang xử lý";
                }


                // Gọi business layer để xử lý tạo hóa đơn
                var maHD = _hoaDonBusiness.CreateHoaDon(model);
                return Ok(new { Message = "Hóa đơn đã được tạo thành công.", MaHD = maHD });
            }
            catch (Exception ex)
            {
                // Trả về lỗi khi tạo hóa đơn
                return StatusCode(500, new { Message = "Lỗi khi tạo hóa đơn.", Error = ex.Message });
            }
        }


        // API: PUT /api/hoadon/update (Cập nhật thông tin hóa đơn)
        [HttpPut("update")]
        public IActionResult Update([FromBody] HoaDonModel model)
        {
            try
            {
                // Kiểm tra nếu model không hợp lệ
                if (model == null || model.MaHD == 0)
                {
                    return BadRequest(new { Message = "Dữ liệu không hợp lệ." });
                }

                // Gọi phương thức cập nhật hóa đơn từ Business Layer
                bool isUpdated = _hoaDonBusiness.UpdateHoaDon(model);

                if (!isUpdated)
                {
                    return NotFound(new { Message = "Hóa đơn không tồn tại hoặc không thể cập nhật." });
                }

                return Ok(new { Message = "Cập nhật hóa đơn thành công." });
            }
            catch (Exception ex)
            {
                // Trả về lỗi khi cập nhật hóa đơn
                return StatusCode(500, new { Message = "Lỗi khi cập nhật hóa đơn.", detail = ex.Message });
            }
        }
        [HttpPut("update-trang-thai/{maHD}")]
        public IActionResult UpdateTrangThai(int maHD, [FromBody] string trangThai)
        {
            try
            {
                // Tạo model từ maHD và trangThai
                var model = new HoaDonModel
                {
                    MaHD = maHD,
                    TrangThai = trangThai
                };

                // Cập nhật trạng thái hóa đơn
                var isUpdated = _hoaDonBusiness.UpdateTrangThaiHoaDon(model);
                if (isUpdated)
                {
                    // Lấy lại thông tin hóa đơn mới sau khi cập nhật trạng thái
                    var updatedHoaDon = _hoaDonBusiness.GetHoaDonById(maHD);

           
                    // Trả lại thông tin hóa đơn đã cập nhật
                    return Ok(new { message = "Cập nhật trạng thái thành công!", hoaDon = updatedHoaDon });
                }

                return BadRequest(new { message = "Không thể cập nhật trạng thái!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("byTrangThai/{trangThai}")]
        public ActionResult<List<HoaDonModel>> GetByTrangThai(string trangThai)
        {
            var hoaDons = _hoaDonBusiness.GetByTrangThai(trangThai);
            return Ok(hoaDons);
        }

        //// API: DELETE /api/hoadon/delete/{maHD} (Xóa hóa đơn)
        //[HttpDelete("delete/{maHD}")]
        //public IActionResult Delete(int maHD)
        //{
        //    try
        //    {
        //        if (_hoaDonBusiness.DeleteHoaDon(maHD))
        //        {
        //            return Ok(new { Success = true, Message = "Hóa đơn đã được xóa thành công!" });
        //        }
        //        return BadRequest("Không thể xóa hóa đơn.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex);
        //    }
        //}

        //// API: GET /api/hoadon/{maHD} (Lấy thông tin hóa đơn theo mã hóa đơn)
        //[HttpGet("{maHD}")]
        //public IActionResult Get(int maHD)
        //{
        //    try
        //    {
        //        var hoaDon = _hoaDonBusiness.GetHoaDonById(maHD);
        //        if (hoaDon != null)
        //        {
        //            return Ok(hoaDon);
        //        }
        //        return NotFound();
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex);
        //    }
        //}



        // API: GET /api/hoadon/all (lấy tất cả hóa đơn)
        [HttpGet("get-all")]
        public IActionResult GetAllHoaDon()
        {
            try
            {
                var result = _hoaDonBusiness.GetAllHoaDon();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("get/{MaHD}")]
        public IActionResult GetById(int MaHD)
        {
            try
            {
                var donHangNhap = _hoaDonBusiness.GetHoaDonById(MaHD);

                if (donHangNhap != null)
                {
                    return Ok(donHangNhap);
                }
                else
                {
                    return NotFound("Không tìm thấy hóa đơn.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi khi tải hóa đơn.", Error = ex.Message });
            }
        }

      


        //// API: GET /api/hoadon/search (Tìm kiếm hóa đơn)
        //[HttpGet("search")]
        //public IActionResult Search(int pageIndex, int pageSize, int? maHD = null, int perid = 0, DateTime? ngayBan = null, int? trangThai = null)
        //{
        //    try
        //    {
        //        long total;

        //        // Gọi phương thức Search từ Business
        //        var hoaDons = _hoaDonBusiness.Search(pageIndex, pageSize, out total, maHD, perid, ngayBan, trangThai);

        //        return Ok(new { Total = total, HoaDons = hoaDons });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}
    }
}
