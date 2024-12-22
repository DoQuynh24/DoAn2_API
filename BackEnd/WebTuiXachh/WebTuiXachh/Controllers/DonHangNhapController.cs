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
    [ApiController]
    [Route("api/[controller]")]
    public class DonHangNhapController : ControllerBase
    {
        private readonly IDonHangNhapBusiness _donHangNhapBusiness;

        public DonHangNhapController(IDonHangNhapBusiness donHangNhapBusiness)
        {
            _donHangNhapBusiness = donHangNhapBusiness;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] DonHangNhapModel model)
        {
            try
            {
                if (model.NgayNhap == null)
                {
                    model.NgayNhap = DateTime.Now; 
                }
               
                var maDHN = _donHangNhapBusiness.CreateDonHangNhap(model);
                return Ok(new { Message = "Đơn hàng nhập đã được tạo thành công.", MaDHN = maDHN });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi khi tạo đơn hàng nhập.", Error = ex.Message });
            }
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] DonHangNhapModel model)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (model == null || model.MaDHN == 0)
                {
                    return BadRequest(new { Message = "Dữ liệu không hợp lệ. Mã đơn hàng nhập phải được cung cấp." });
                }

                // Gọi business để cập nhật đơn hàng nhập, bao gồm xóa các chi tiết nếu có
                bool isUpdated = _donHangNhapBusiness.UpdateDonHangNhap(model);

                // Kiểm tra kết quả trả về từ business logic
                if (!isUpdated)
                {
                    return NotFound(new { Message = "Đơn hàng nhập không tồn tại hoặc không thể cập nhật." });
                }

                return Ok(new { Message = "Cập nhật Đơn Hàng Nhập thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi khi cập nhật Đơn Hàng Nhập.", Error = ex.Message });
            }
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




        [HttpGet("get-all")]
        public IActionResult GetAllDonHangNhap()
        {
            try
            {
                var result = _donHangNhapBusiness.GetAllDonHangNhap();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("get/{MaDHN}")]
        public IActionResult GetById(int MaDHN)
        {
            try
            {
                var donHangNhap = _donHangNhapBusiness.GetDonHangNhapById(MaDHN);

                if (donHangNhap != null)
                {
                    return Ok(donHangNhap);
                }
                else
                {
                    return NotFound("Không tìm thấy đơn hàng nhập.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi khi tải đơn hàng nhập.", Error = ex.Message });
            }
        }




    }
}
