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
namespace API.Controllers
{
    //[Authorize] // Bảo vệ tất cả các API trong controller này
    [ApiController]
    [Route("api/[controller]")]
    public class MauSacController : ControllerBase
    {
        private readonly IMauSacBusiness _mauSacBusiness;

       
        public MauSacController(IMauSacBusiness mauSacBusiness)
        {
            _mauSacBusiness = mauSacBusiness;
        }

        
        [HttpPost("create")]

        public IActionResult Create([FromForm] MauSacModel model)
        {
            try
            {
               
                if (_mauSacBusiness.CreateMauSac(model))
                {
                    return Ok(new { Success = true, Message = "Màu sắc đã được thêm thành công!" });
                }
                return BadRequest("Không thể thêm màu sắc");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex); // Lỗi server
            }
        }

     
        [HttpPut("update")]

        public IActionResult Update([FromForm] MauSacModel model)
        {
            try
            {
                if (_mauSacBusiness.UpdateMauSac(model))
                {
                    return Ok(new { Success = true, Message = "Màu sắc đã được cập nhật thành công!" });
                }
                return BadRequest("Không thể cập nhật màu sắc.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex); // Lỗi server
            }
        }

        [HttpDelete("delete/{tenMau}")]

        public IActionResult Delete(string tenMau)
        {
            try
            {
                if (_mauSacBusiness.DeleteMauSac(tenMau))
                {
                    return Ok(new { Success = true, Message = "Màu sắc đã được xóa thành công!" });
                }
                return BadRequest("Không thể xóa màu sắc.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex); // Lỗi server
            }
        }

     
      
        [HttpGet("all")]

        public IActionResult GetAll()
        {
            try
            {
                var tenMaus = _mauSacBusiness.GetAllMauSac();
                return Ok(tenMaus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex); // Lỗi server
            }
        }
       
        [HttpGet("search")]
        public IActionResult Search(int pageIndex = 1, int pageSize = 10, string tenMau = "")
        {
            try
            {
                long total;
                var mauSacs = _mauSacBusiness.SearchMauSacs(pageIndex, pageSize, out total, tenMau);

                if (mauSacs != null && mauSacs.Count > 0)
                {
                    return Ok(new { Total = total, Data = mauSacs });
                }
                return NotFound(new { message = "Không tìm thấy danh mục sản phẩm phù hợp" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

    }
}
