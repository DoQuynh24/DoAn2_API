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
    public class NhaCungCapController : ControllerBase
    {
        private readonly INhaCungCapBusiness _nhaCungCapBusiness;


        public NhaCungCapController(INhaCungCapBusiness nhaCungCapBusiness)
        {
            _nhaCungCapBusiness = nhaCungCapBusiness;
        }

        [HttpPost("create")]

        public IActionResult Create([FromForm] NhaCungCapModel model)
        {
            try
            {
                model.DiaChi ??= "Chưa cập nhật";
                model.LienHe ??= "Chưa cập nhật";
                if (_nhaCungCapBusiness.CreateNCC(model))
                {
                    return Ok(new { Success = true, Message = "Nhà cung cấp đã được tạo thành công!" });
                }
                return BadRequest("Không thể tạo nhà cung cấp.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex); 
            }
        }

        [HttpPut("update")]

        public IActionResult Update([FromForm] NhaCungCapModel model)
        {
            try
            {
                if (_nhaCungCapBusiness.UpdateNCC(model))
                {
                    return Ok(new { Success = true, Message = "Nhà cung cấp đã được cập nhật thành công!" });
                }
                return BadRequest("Không thể cập nhật nhà cung cấp.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex); 
            }
        }

        [HttpDelete("delete/{tenNCC}")]

        public IActionResult Delete(string tenNCC)
        {
            try
            {
                if (_nhaCungCapBusiness.DeleteNCC(tenNCC))
                {
                    return Ok(new { Success = true, Message = "Nhà cung cấp đã được xóa thành công!" });
                }
                return BadRequest("Không thể xóa nhà cung cấp.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex); 
            }
        }

        
        [HttpGet("{tenNCC}")]
        public IActionResult Get(string tenNCC)
        {
            try
            {
                var nhaCungCap = _nhaCungCapBusiness.GetNCCByName(tenNCC);
                if (nhaCungCap != null)
                {
                    return Ok(nhaCungCap);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex); 
            }
        }

       
        [HttpGet("all")]

        public IActionResult GetAll()
        {
            try
            {
                var nhaCungCaps = _nhaCungCapBusiness.GetAllNCC();
                return Ok(nhaCungCaps);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex); 
            }
        }
        


    }
}
