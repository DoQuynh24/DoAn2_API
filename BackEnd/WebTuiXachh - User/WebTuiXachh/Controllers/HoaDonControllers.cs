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

      

    }
}
